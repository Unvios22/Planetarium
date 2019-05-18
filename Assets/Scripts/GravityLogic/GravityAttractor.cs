using System;
using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine {
	public class GravityAttractor : MonoBehaviour {

		[SerializeField] private float attractorMass;
		private List<Rigidbody> _attractedBodies = new List<Rigidbody>();

		private void FixedUpdate() {
			foreach (var attractedRigidbody in _attractedBodies) {
				var attractorPositon = transform.position;
				var rigidbodyPosition = attractedRigidbody.position;
				
				var forceVector = (attractorPositon - rigidbodyPosition).normalized;
				var distance = Vector3.Distance(attractorPositon, rigidbodyPosition);
				var attractionForce = (attractorMass * attractedRigidbody.mass) / (distance * distance);
				
				attractedRigidbody.AddForce(forceVector * attractionForce);
			}
		}

		public void AddAttractedRigidbody(Rigidbody attractedRigidbody) {
			_attractedBodies.Add(attractedRigidbody);
		}

		public void RemoveAttractedRigidbody(Rigidbody attractedRigidbody) {
			_attractedBodies.Remove(attractedRigidbody);
		}
	}
}