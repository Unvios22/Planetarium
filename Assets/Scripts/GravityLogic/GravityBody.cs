using ReadOnlyData;
namespace UnityEngine {
	
	[RequireComponent(typeof(Rigidbody))]
	public class GravityBody : MonoBehaviour {
		private Rigidbody _rigidbody;
		private GravityAttractor _gravityAttractor;
		private void Start() {
			_rigidbody = gameObject.GetComponent<Rigidbody>();
			_gravityAttractor = GameObject.FindGameObjectWithTag(Tags.GRAVITY_ATTRACTOR).GetComponent<GravityAttractor>();
			_gravityAttractor.AddAttractedRigidbody(_rigidbody);
		}
	}
}