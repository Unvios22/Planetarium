using System.Collections.Generic;
namespace UnityEngine {
	public class GravityAttractor : MonoBehaviour {

		[SerializeField] private float attractionForce = 4;
		private List<Rigidbody> _attractedBodies = new List<Rigidbody>();

		private void FixedUpdate() {
			foreach (var attractedRigidbody in _attractedBodies) {
				var attractorPosition = transform.position;
				var rigidbodyPosition = attractedRigidbody.position;
				
				var forceVector = (attractorPosition - rigidbodyPosition).normalized;
				
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