using System;
using ReadOnlyData;
namespace UnityEngine {
	
	[RequireComponent(typeof(Rigidbody))]
	public class GravityBody : MonoBehaviour {
		public float gravityMultiplier = 1;
		[NonSerialized] public Rigidbody Rigidbody;
		
		private GravityAttractor _gravityAttractor;
		private void Start() {
			Rigidbody = gameObject.GetComponent<Rigidbody>();
			_gravityAttractor = GameObject.FindGameObjectWithTag(Tags.GRAVITY_ATTRACTOR).GetComponent<GravityAttractor>();
			_gravityAttractor.AddAttractedBody(this);
		}

		private void OnDestroy() {
			_gravityAttractor.RemoveAttractedBody(this);
		}
	}
}