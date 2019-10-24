using System;
using System.Collections;
using ReadOnlyData;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlobBehavior : MonoBehaviour {
	[SerializeField] private float minWanderTime;
	[SerializeField] private float maxWanderTime;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float eatingRange;
	[SerializeField] private float eatingSpeed;
	[SerializeField] private BlobState blobState;
	
	private Rigidbody _rigidbody;
	private Transform _currentFoodTarget;
	
	public enum BlobState {
		Wandering,
		MovingToFood,
		Eating
	}

	private void Start() {
		_rigidbody = GetComponent<Rigidbody>();
		StartCoroutine(Wander());
	}

	private void Update() {
		//TODO: realign blob to planet gravity
	}

	private IEnumerator Wander() {
		blobState = BlobState.Wandering;
		//get random direction
		var randomAngle = Random.Range(0f, Mathf.PI * 2f);
		var randomDirection = new Vector2(Mathf.Sin(randomAngle), Mathf.Cos(randomAngle)).normalized;
		
		//wander in random direction for x seconds
		var wanderTime = Random.Range(minWanderTime, maxWanderTime);
		while (wanderTime > 0) {
			_rigidbody.AddForce(randomDirection * moveSpeed, ForceMode.VelocityChange);
			wanderTime -= Time.deltaTime;
			yield return null;
		}
		StartCoroutine(Wander());
	}

	private void OnTriggerEnter(Collider other) {
		//return if found collider isn't food
		if (!other.gameObject.CompareTag(Tags.BLOB_FOOD)) {
			return;
		}
		//return if found food isn't ripe
		if (!other.GetComponent<FruitLogic>().fruitState.Equals(FruitLogic.FruitState.Ripe)) {
			return;
		}
		//Start moving towards found food if no other food is currently pursued
		if (_currentFoodTarget == null) {
			StopAllCoroutines();
			var targetFoodScript = other.GetComponent<FruitLogic>();
			StartCoroutine(GetFood(targetFoodScript));
		}else {
			//if other food is currently pursued, check which one is closer and pursue the closer one
			var blobPosition = transform.position;
			var foundFoodDistance = Vector3.Distance(other.transform.position, blobPosition);
			var currentTargetFoodDistance = Vector3.Distance(_currentFoodTarget.position, blobPosition);
			if (!(foundFoodDistance < currentTargetFoodDistance)) return;
			StopAllCoroutines();
			var targetFoodScript = other.GetComponent<FruitLogic>();
			StartCoroutine(GetFood(targetFoodScript));
		}
	}

	private IEnumerator GetFood(FruitLogic targetFoodScript) {
		blobState = BlobState.MovingToFood;
		// move towards found food
		var blobPositon = transform.position;
		var foodPosition = targetFoodScript.transform.position;

		var distanceToFood = Vector3.Distance(blobPositon, foodPosition);
		var vectorTowardsFood = (blobPositon - foodPosition).normalized;
		
		while (distanceToFood > eatingRange) {
			_rigidbody.AddForce(vectorTowardsFood * moveSpeed, ForceMode.VelocityChange);
			distanceToFood = Vector3.Distance(transform.position, targetFoodScript.transform.position);
			vectorTowardsFood = (blobPositon - foodPosition).normalized;
			yield return null;
		}
		StartCoroutine(EatFood(targetFoodScript));
	}

	private IEnumerator EatFood(FruitLogic targetFoodScript) {
		blobState = BlobState.Eating;
		var distanceToFood = Vector3.Distance(blobPositon, foodPosition);
		while (expression) {
			
		}
	}
}
