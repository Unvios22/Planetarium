using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class FruitLogic : MonoBehaviour {
	public enum FruitState {
		Growing,
		Ripe,
		Overripe
	}
	
	[SerializeField] public FruitState fruitState;
	
	[SerializeField] private Color growingColor;
	[SerializeField] private Color ripeColor;
	[SerializeField] private Color overRipeColor;
	
	[SerializeField] private float minRipeningTime;
	[SerializeField] private float maxRipeningTime;
	[SerializeField] private Vector3 ripeStageSizeScale;
	
	[SerializeField] private float minOverRipeningTime;
	[SerializeField] private float maxOverRipeningTime;
	[SerializeField] private Vector3 overRipeStageSizeScale;
	[SerializeField] private float overRipeStageGravityMultiplier;
	[SerializeField] private float foodAmount;

	[SerializeField] private AudioClip[] soundOnPop;

	private Transform _fruitTransform;
	private Rigidbody _rigidbody;
	private Collider _collider;
	private Renderer _renderer;
	private Material _fruitMaterial;
	private FruitSpawner _parentTreeFruitSpawner;
	private AudioSource _audioSource;
	private GravityBody _gravityBodyScript;

	public void BeEaten(float amountEaten) {
		foodAmount -= amountEaten;
		if (foodAmount <= 0) {
			StartCoroutine(DestroyFruit());
		}
	}

	private void Start() {
		//load all references
		_fruitTransform = transform;
		_rigidbody = _fruitTransform.GetComponent<Rigidbody>();
		_collider = _fruitTransform.GetComponent<Collider>();
		_renderer = _fruitTransform.GetComponent<Renderer>();
		_fruitMaterial = _renderer.material;
		_parentTreeFruitSpawner = _fruitTransform.GetComponentInParent<FruitSpawner>();
		_audioSource = gameObject.GetComponent<AudioSource>();
		_gravityBodyScript = gameObject.GetComponent<GravityBody>();
		
		//fruit is growing on tree
		_rigidbody.isKinematic = true;
		_collider.enabled = false;
		_fruitTransform.localScale = Vector3.zero;
		fruitState = FruitState.Growing;
		StartCoroutine(GrowOnTree());
	}

	private IEnumerator GrowOnTree() {
		var timeToFullyGrow = Random.Range(minRipeningTime, maxRipeningTime);
		float timeGrowing = 0;
		while (timeGrowing < timeToFullyGrow) {
			var lerpRatio = timeGrowing / timeToFullyGrow;
			_fruitMaterial.color = Color.Lerp(growingColor, ripeColor, lerpRatio);
			_fruitTransform.localScale = Vector3.Lerp(Vector3.zero, ripeStageSizeScale, lerpRatio);
			timeGrowing += Time.deltaTime;
			yield return null;
		}

		//fruit is fully ripe - clear parent & activate rb so that fruit falls from tree
		_fruitTransform.parent = null;
		_rigidbody.isKinematic = false;
		_collider.enabled = true;
		fruitState = FruitState.Ripe;
		
		//start overripening
		yield return StartCoroutine(OverRipening());
	}

	private IEnumerator OverRipening() {
		var timeToOverRipen = Random.Range(minOverRipeningTime, maxOverRipeningTime);
		float timeOverRipening = 0;
		float baseGravityMultiplier = _gravityBodyScript.gravityMultiplier;
		while (timeOverRipening < timeToOverRipen) {
			var lerpRatio = timeOverRipening / timeToOverRipen;
			_fruitMaterial.color = Color.Lerp(ripeColor, overRipeColor, lerpRatio);
			_fruitTransform.localScale = Vector3.Slerp(ripeStageSizeScale, overRipeStageSizeScale, lerpRatio);
			_gravityBodyScript.gravityMultiplier =
				Mathf.Lerp(baseGravityMultiplier, overRipeStageGravityMultiplier, lerpRatio);
			timeOverRipening += Time.deltaTime;
			yield return null;
		}

		fruitState = FruitState.Overripe;
		StartCoroutine(DestroyFruit());
	}

	private IEnumerator DestroyFruit() {
		//play random sound from provided sounds array, using a random pitch
		_audioSource.clip = soundOnPop[Random.Range(0, soundOnPop.Length)];
		_audioSource.pitch = Random.Range(0.6f, 1.2f);
		_audioSource.Play();
		
		//make object invisible
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.isKinematic = true;
		_renderer.enabled = false;
		_parentTreeFruitSpawner.FruitDestroyed();
		
		//let audio finish playing
		yield return new WaitForSeconds(_audioSource.clip.length);
		Destroy(gameObject);
	}
	
}
