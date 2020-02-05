using System;
using System.Collections.Generic;
using ReadOnlyData;
using UnityEngine;

public class BoidBehavior : MonoBehaviour {
	[SerializeField] private BoidSettings _boidSettings;
	[SerializeField] private bool separationBehavior, alignmentBehavior, cohesionBehavior, moveForwards;

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
		if (separationBehavior) {ApplySeparationBehavior(boid);}
		if (alignmentBehavior) {ApplyAlignmentBehavior(boid);}
		if (cohesionBehavior) {ApplyCohesionBehavior(boid);}
		if (moveForwards) {ApplyForwardsMotion(boid);}
	}

	private void ApplySeparationBehavior(BoidAgent boid) {
		var tooCloseBoids = GetBoidsInRange(boid.transform.position, _boidSettings.avoidanceRange);
		var avoidanceMove = GetAvoidanceMove(boid.transform.position, tooCloseBoids);
		ApplyAvoidanceMove(boid, avoidanceMove, _boidSettings.moveSpeed);
	}
	
	private Collider[] GetBoidsInRange(Vector3 position, float range) {
		var boidsInRange = Physics.OverlapSphere(position, range, boidsLayerMask);
		return boidsInRange;
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
		boid.Rigidbody.AddForce(direction * force, ForceMode.Impulse);
	}
	
	private void ApplyAlignmentBehavior(BoidAgent boid) { }
	
	private void ApplyCohesionBehavior(BoidAgent boid) { }

	private void ApplyForwardsMotion(BoidAgent boid) { }

}