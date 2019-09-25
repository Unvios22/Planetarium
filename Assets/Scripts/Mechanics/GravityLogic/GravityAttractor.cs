using System.Collections.Generic;
namespace UnityEngine {
	public class GravityAttractor : MonoBehaviour {

		[SerializeField] private float attractionForce = 4;
		private List<GravityBody> _attractedBodies = new List<GravityBody>();

		private void FixedUpdate() {
			foreach (var attractedBody in _attractedBodies) {
				var attractedRigidbody = attractedBody.Rigidbody;
				var attractorPosition = transform.position;
				var rigidbodyPosition = attractedRigidbody.position;
				
				var forceVector = (attractorPosition - rigidbodyPosition).normalized;
				
				attractedRigidbody.AddForce(attractedBody.gravityMultiplier * attractionForce * forceVector);
			}
		}

		public void AddAttractedBody(GravityBody attractedBody) {
			_attractedBodies.Add(attractedBody);
		}

		public void RemoveAttractedBody(GravityBody attractedBody) {
			_attractedBodies.Remove(attractedBody);
		}
	}
}