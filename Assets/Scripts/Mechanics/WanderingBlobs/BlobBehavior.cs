using System;
using System.Collections;
using ReadOnlyData;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlobBehavior : MonoBehaviour {
	[SerializeField] private float minWanderTime;
	[SerializeField] private float maxWanderTime;
	[SerializeField] private float moveSpeed;
	private Rigidbody _rigidbody;
	private Transform _currentFoodTarget;

	private void Start() {
		_rigidbody = GetComponent<Rigidbody>();
		StartCoroutine(Wander());
	}

	private void Update() {
		//TODO: realign blob to planet gravity
	}

	private IEnumerator Wander() {
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
		if (!other.gameObject.CompareTag(Tags.BLOB_FOOD)) {
			return;
		}
		if (_currentFoodTarget == null) {
			StopAllCoroutines();
			var targetFoodScript = other.GetComponent<FruitLogic>();
			StartCoroutine(GetFood(targetFoodScript));
		}else {
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
		
	}
}
