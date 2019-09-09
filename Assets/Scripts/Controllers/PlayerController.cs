using System.Collections;
using ReadOnlyData;
using UnityEngine;
using static ReadOnlyData.PhysicsLayers.Layers;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
	[SerializeField] private float moveSpeed = 10;
	[SerializeField] private float jumpForce = 5;
	[SerializeField] private GravityAttractor planet;
	[SerializeField] private float groundCheckDistance;
	[SerializeField] private FPPCameraController playerCamera;
	[SerializeField] private Transform cameraAttachPoint;
	[SerializeField] private Transform groundChecker;
	[SerializeField] private float jumpTimeBuffer = 1f;
	
	[SerializeField] private bool drawGroundCheckSphere;
	[SerializeField] private bool isPlanetPresent = true;
	[SerializeField] private bool canJump = true;
	
	[SerializeField] private float groundedDrag = 12f;
	[SerializeField] private float inAirDrag = 0f;

	private Rigidbody _rigidbody;
	private Transform _playerCameraTransform;
	private Transform _playerCameraParent;
	private bool _isMoving;
	private bool _isGrounded;
	
	private Vector3 _moveVector;

	private void Start() {
		_rigidbody = gameObject.GetComponent<Rigidbody>();
		_playerCameraTransform = playerCamera.transform;
		_playerCameraParent = _playerCameraTransform.parent;
	}

	private void Update() {
		//read user axis input
		var inputX = Input.GetAxisRaw(InputStrings.Axis.Horizontal) * moveSpeed * Time.deltaTime;
		var inputZ = Input.GetAxisRaw(InputStrings.Axis.Vertical) * moveSpeed * Time.deltaTime;

		//set move metadata booleans
		if (inputX == 0 && inputZ == 0) {
			_isMoving = false;
		} else {_isMoving = true;}

		_isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckDistance, LayerMask.GetMask(PhysicsLayers.GetLayerName(Ground)));
		
		//apply movement logic
		if (_isMoving && _isGrounded) {
			_moveVector = transform.TransformDirection(new Vector3(inputX,0, inputZ));
			//"TranformDirection" to convert from localspace to worldspace (used by AddForce)

			_rigidbody.drag = groundedDrag;
		}
		else if (_isGrounded) {
			_rigidbody.velocity = new Vector3(0,_rigidbody.velocity.y, 0);
			_moveVector = Vector3.zero;
		}
		else {
			_moveVector = Vector3.zero;
			_rigidbody.drag = inAirDrag;
		}
		
		//update rotations on player & camera
		if (isPlanetPresent) {
			var vectorTowardsPlanet = (planet.transform.position - transform.position).normalized;
			RealignPlayerRotation(-vectorTowardsPlanet);
			_playerCameraParent.rotation = Quaternion.LookRotation(-vectorTowardsPlanet, -transform.forward)
			                   * Quaternion.AngleAxis(90f, Vector3.right);
		}
		else {
			RealignPlayerRotation(Vector3.up);
		}
		
		//update camera position
		MoveCameraWithPlayer();
	}

	private void RealignPlayerRotation(Vector3 upwardsDirection) {
		var playerCameraForward = _playerCameraTransform.forward;
		var newPlayerRot = Quaternion.LookRotation(upwardsDirection, -playerCameraForward)
		                   * Quaternion.AngleAxis(90f, Vector3.right);
		transform.localRotation = newPlayerRot;
	}

	private void MoveCameraWithPlayer() {
		_playerCameraTransform.position = cameraAttachPoint.position;
	}

	private void FixedUpdate() {
		//apply player movement in physics
		_rigidbody.AddForce(_moveVector,ForceMode.VelocityChange);
		//TODO: Fix player sliding
		
		if (Input.GetKey(KeyCode.Space) && _isGrounded && canJump) {
			_rigidbody.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
			canJump = false;
			StartCoroutine(PlayerInAir());
		}
	}

	private IEnumerator PlayerInAir() {
		yield return new WaitForSeconds(jumpTimeBuffer);
		for (;;) {
			if (_isGrounded) {
				canJump = true;
				yield break;
			}
			yield return null;
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		if (drawGroundCheckSphere) {
			Gizmos.DrawWireSphere(groundChecker.position,groundCheckDistance);
		}
	}
}
