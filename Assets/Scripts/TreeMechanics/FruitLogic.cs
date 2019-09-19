using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitLogic : MonoBehaviour {
	private enum FruitState {
		Growing,
		Ripe,
		Overripe
	}
	
	[SerializeField] private FruitState fruitState;
	
	[SerializeField] private Color growingColor;
	[SerializeField] private Color ripeColor;
	[SerializeField] private Color overRipeColor;
	
	[SerializeField] private float minRipeningTime;
	[SerializeField] private float maxRipeningTime;
	[SerializeField] private Vector3 ripeStageSizeScale;
	
	[SerializeField] private float minOverRipeningTime;
	[SerializeField] private float maxOverRipeningTime;
	[SerializeField] private Vector3 overRipeStageSizeScale;

	private Transform _fruitTransform;
	private Rigidbody _rigidbody;
	private Collider _collider;
	private Material _fruitMaterial;

	private void Awake() {
		_fruitTransform = transform;
		_rigidbody = _fruitTransform.GetComponent<Rigidbody>();
		_collider = _fruitTransform.GetComponent<Collider>();
		_fruitMaterial = _fruitTransform.GetComponent<Renderer>().material;
	}

	private void OnEnable() {
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

		//clear parent & activate rb so that fruit falls from tree
		_fruitTransform.parent = null;
		_rigidbody.isKinematic = false;
		_collider.enabled = true;
		fruitState = FruitState.Ripe;
		yield return StartCoroutine(OverRipening());
	}

	private IEnumerator OverRipening() {
		var timeToOverRipen = Random.Range(minOverRipeningTime, maxOverRipeningTime);
		float timeOverRipening = 0;
		while (timeOverRipening < timeToOverRipen) {
			var lerpRatio = timeOverRipening / timeToOverRipen;
			_fruitMaterial.color = Color.Lerp(ripeColor, overRipeColor, lerpRatio);
			_fruitTransform.localScale = Vector3.Lerp(ripeStageSizeScale, overRipeStageSizeScale, lerpRatio);
			timeOverRipening += Time.deltaTime;
			yield return null;
		}

		fruitState = FruitState.Overripe;
	}
	
}
