using System;
using ReadOnlyData;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
	[SerializeField] private float moveSpeed = 10;
	[SerializeField] private float jumpStrength;
	[SerializeField] private float groundCheckDistance;
	[SerializeField] private FPPCameraController playerCamera;
	[SerializeField] private Transform cameraAttachPoint;
	[SerializeField] private Transform groundChecker;
	[SerializeField] private bool drawGroundCheckSpehere;

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
		//read user input
		var inputX = Input.GetAxisRaw(InputStrings.Axis.Horizontal) * moveSpeed * Time.deltaTime;
		var inputZ = Input.GetAxisRaw(InputStrings.Axis.Vertical) * moveSpeed * Time.deltaTime;

		//set move metadata booleans
		if (inputX == 0 && inputZ == 0) {
			_isMoving = false;
		} else {_isMoving = true;}
		
		Debug.Log(inputX + "|||" + inputZ);
		Debug.Log("grounded:" + _isGrounded);
		Debug.Log("moving:" + _isMoving);

		_isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckDistance, LayerMask.GetMask(PhysicsLayers.GetLayerName(PhysicsLayers.Layers.Ground)));
		
		//apply movement logic
		if (_isMoving && _isGrounded) {
			_moveVector = transform.TransformDirection(new Vector3(inputX,0, inputZ));
			//Tranform Direction to convert from localspace to worldspace (used by AddForce)
		}
		else if (_isGrounded) {
			_rigidbody.velocity = new Vector3(0,_rigidbody.velocity.y, 0);
			_moveVector = Vector3.zero;
		}
		else {
			_moveVector = Vector3.zero;
		}
		Debug.Log("Move vector:" + _moveVector);
		RotatePlayerToCameraRot();
		MoveCameraWithPlayer();
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
		_rigidbody.AddForce(_moveVector,ForceMode.Impulse);
		//TODO: Fix player sliding
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		if (drawGroundCheckSpehere) {
			Gizmos.DrawWireSphere(groundChecker.position,groundCheckDistance);
		}
	}
}
