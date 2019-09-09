using System;
using System.Collections;
using System.Collections.Generic;
using ReadOnlyData;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Camera))]
public class FPPCameraController : MonoBehaviour {

	[SerializeField] private float mouseSensitivity;
	
	[SerializeField] private float maxX = 80f;
	[SerializeField] private float minX = -80f;

	[Range(60f, 120f)]
	[SerializeField] private float fieldOfView = 75f;
	[SerializeField] private bool invertYRot;
	
	private Camera _camera;

	private void LateUpdate() {
		var xMouseInputRot = Input.GetAxisRaw(InputStrings.Axis.MouseX) * mouseSensitivity * Time.deltaTime;
		var yMouseInputRot = Input.GetAxisRaw(InputStrings.Axis.MouseY) * mouseSensitivity * Time.deltaTime;

		yMouseInputRot = invertYRot ? yMouseInputRot : -yMouseInputRot;
		//accounting for inverted/not inverted Y axis control
		
		var xCameraInputRot = yMouseInputRot;
		//Y mouse rot = X camera rot
		
		var yCameraInputRot = xMouseInputRot;
		//X mouse rot = Y camera rot
		
		ApplyRotation(xCameraInputRot, yCameraInputRot);
		ApplyRotConstraints(xCameraInputRot);
	}
	
	private void ApplyRotation(float xRot, float yRot) {
		var localCameraRotation = transform.localEulerAngles;
		var totalXRot = localCameraRotation.x + xRot;
		var totalYRot = localCameraRotation.y + yRot;
		transform.localRotation = Quaternion.Euler(totalXRot,totalYRot,localCameraRotation.z);
	}

	private void ApplyRotConstraints(float userInputXRot) {
		var startingRotation = transform.localEulerAngles;
		var xRot = startingRotation.x;

		if (xRot > maxX) {
			
		}
		
		Debug.Log(xRot);
//		Debug.Log("internal max: " + _internalMaxX);
//		Debug.Log("internal min: " + _internalMinX);
//		if (xRot < _internalMaxX && xRot > _internalMinX && userInputXRot < 0) {
//			xRot = _internalMaxX;
//			Debug.Log("Too high!");
//		} 
//		else if (xRot > _internalMinX && xRot < _internalMaxX && userInputXRot > 0) {
//			xRot = _internalMinX;
//			Debug.Log("Too low!");
//		}

		var correctedRotation = Quaternion.Euler(xRot, startingRotation.y, startingRotation.z);
		transform.localRotation = correctedRotation;
	}

	private void OnValidate() {
		_camera = gameObject.GetComponent<Camera>();
		_camera.fieldOfView = fieldOfView;
	}
}
