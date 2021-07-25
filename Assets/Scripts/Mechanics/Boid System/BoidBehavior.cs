using System.Linq;
using ReadOnlyData;
using UnityEngine;

public class BoidBehavior : MonoBehaviour {
	[SerializeField] private BoidSettings _boidSettings;
	[SerializeField] private bool separationBehavior, cohesionBehavior, alignmentBehavior, moveForwards;
 
	private BoidAgent[] _boids;
	private int boidsLayerMask;

	private void Start() {
		boidsLayerMask = PhysicsLayers.GetLayerMask(new[] {PhysicsLayers.Layers.Boids});
		_boids = FindObjectsOfType<BoidAgent>();
	}

	void Update() {
		foreach (var boidAgent in _boids) {
			ApplyBoidBehavior(boidAgent);
		}
	}

	private void ApplyBoidBehavior(BoidAgent boid) {
		//Todo: optimize the algorithm
		if (separationBehavior) {ApplySeparationBehavior(boid);}
		if (cohesionBehavior) {ApplyCohesionBehavior(boid);}
		if (alignmentBehavior) {ApplyAlignmentBehavior(boid);}
		if (moveForwards) {ApplyForwardsMotion(boid);}
	}

	private void ApplySeparationBehavior(BoidAgent boid) {
		var tooCloseBoids = GetBoidsInRange(boid, _boidSettings.avoidanceRange);
		var avoidanceMove = GetAvoidanceMove(boid.transform.position, tooCloseBoids);
		ApplyAvoidanceMove(boid, avoidanceMove, _boidSettings.moveSpeed);
	}

	private Vector3 GetAvoidanceMove(Vector3 currentBoidPosition,Collider[] boidsTooClose) {
		var avoidanceMove = new Vector3();
		foreach (var boidToAvoid in boidsTooClose) {
			var partialAvoidanceMove = currentBoidPosition - boidToAvoid.transform.position;
			avoidanceMove += partialAvoidanceMove;
			//todo: apply weights to found vectors, so that the closest object is the one generating the most avoidace move
		}
		return avoidanceMove;
	}

	private void ApplyAvoidanceMove(BoidAgent boid, Vector3 direction, float force) {
		boid.Rigidbody.AddForce(direction * (force * Time.deltaTime), ForceMode.Impulse);
	}

	private void ApplyCohesionBehavior(BoidAgent boid) {
		var neighbouringBoids = GetBoidsInRange(boid, _boidSettings.neighborsRange);
		var cohesionMoveVector = GetCohesionMove(boid.transform.position, neighbouringBoids);
		Debug.DrawRay(boid.transform.position, cohesionMoveVector);
		ApplyCohesionMove(boid, cohesionMoveVector, _boidSettings.moveSpeed);
	}
	
	private Vector3 GetCohesionMove(Vector3 boidPosition, Collider[] neighbouringBoids) {
		int amountOfBoidsCalculated = 1;
		var sumBoidPositions = boidPosition;
		foreach (var neighbouringBoid in neighbouringBoids) {
			var neighbourPosition = neighbouringBoid.transform.position;
			sumBoidPositions += neighbourPosition;
			amountOfBoidsCalculated++;
		}
		var averageBoidPosition = sumBoidPositions / amountOfBoidsCalculated;

		var cohesionMoveVector = (averageBoidPosition - boidPosition).normalized;
		return cohesionMoveVector;
	}

	private void ApplyCohesionMove(BoidAgent boid, Vector3 moveVector, float force) {
		boid.Rigidbody.AddForce(moveVector * (force * Time.deltaTime), ForceMode.Impulse);
	}

	private void ApplyAlignmentBehavior(BoidAgent boid) {
		var neighbouringBoids = GetBoidsInRange(boid, _boidSettings.neighborsRange);
		var averageNeighboursAlignment = GetAverageAlignment(boid, neighbouringBoids);
	}

	private Quaternion GetAverageAlignment(BoidAgent boid, Collider[] neighbouringBoids) {
		Quaternion averageAlignmnent = new Quaternion();
		//TODO: continue scripting
		return averageAlignmnent;
	}

	private void ApplyForwardsMotion(BoidAgent boid) {
		boid.Rigidbody.AddForce(boid.transform.forward * (_boidSettings.moveSpeed * Time.deltaTime), ForceMode.Impulse);
	}

	private Collider[] GetBoidsInRange(BoidAgent boid, float range) {
		var boidsInRange = Physics.OverlapSphere(boid.transform.position, range, boidsLayerMask);
		boidsInRange = boidsInRange.Where(val => val.gameObject != boid.gameObject).ToArray();
		Debug.Log(boidsInRange.Length);
		return boidsInRange;
	}

	private bool AreGameobjectsEqual(GameObject objectA, GameObject objectB) {
		return objectA == objectB;
	}
}