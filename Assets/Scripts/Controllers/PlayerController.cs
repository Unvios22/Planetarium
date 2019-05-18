using System;
using ReadOnlyData;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
	[SerializeField] private float moveSpeed = 10;
	[SerializeField] private float jumpForce = 5;
	[SerializeField] private GravityAttractor planet;
	[SerializeField] private float groundCheckDistance;
	[SerializeField] private FPPCameraController playerCamera;
	[SerializeField] private Transform cameraAttachPoint;
	[SerializeField] private Transform groundChecker;
	[SerializeField] private bool drawGroundCheckSpehere;

	[SerializeField] private float groundedDrag = 12f;
	[SerializeField] private float inAirDrag = 0f;

	private Rigidbody _rigidbody;
	private Transform _playerCameraTransform;
	private bool _isMoving;
	private bool _isGrounded;
	
	private Vector3 _moveVector;

	private void Start() {
		_rigidbody = gameObject.GetComponent<Rigidbody>();
		_playerCameraTransform = playerCamera.transform;
	}

	private void Update() {
		
		//read user axis input
		var inputX = Input.GetAxisRaw(InputStrings.Axis.Horizontal) * moveSpeed * Time.deltaTime;
		var inputZ = Input.GetAxisRaw(InputStrings.Axis.Vertical) * moveSpeed * Time.deltaTime;

		//set move metadata booleans
		if (inputX == 0 && inputZ == 0) {
			_isMoving = false;
		} else {_isMoving = true;}

		_isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckDistance, LayerMask.GetMask(PhysicsLayers.GetLayerName(PhysicsLayers.Layers.Ground)));
		
		//apply movement logic
		if (_isMoving && _isGrounded) {
			_moveVector = transform.TransformDirection(new Vector3(inputX,0, inputZ));
			//Tranform Direction to convert from localspace to worldspace (used by AddForce)

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
		RotatePlayerToCameraRot();
		RealignToPlanet();
		MoveCameraWithPlayer();
		
		Debug.Log(_isGrounded);
	}

	private void RealignToPlanet() {
		var vectorTowardsPlanet = (planet.transform.position - transform.position).normalized;
		transform.up = -vectorTowardsPlanet;
		_playerCameraTransform.up = -vectorTowardsPlanet;
	}

	private void RotatePlayerToCameraRot() {
		var cameraYRot = _playerCameraTransform.localEulerAngles.y;
		var playerRotation = transform.rotation;
		var newPlayerRotation = Quaternion.Euler(playerRotation.x, cameraYRot, playerRotation.z);
		transform.rotation = newPlayerRotation;
	}

	private void MoveCameraWithPlayer() {
		_playerCameraTransform.position = cameraAttachPoint.position;
	}

	private void FixedUpdate() {
		_rigidbody.AddForce(_moveVector,ForceMode.VelocityChange);
		//TODO: Fix player sliding
		
		if (Input.GetKey(KeyCode.Space) && _isGrounded) {
			_rigidbody.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		if (drawGroundCheckSpehere) {
			Gizmos.DrawWireSphere(groundChecker.position,groundCheckDistance);
		}
	}
}
