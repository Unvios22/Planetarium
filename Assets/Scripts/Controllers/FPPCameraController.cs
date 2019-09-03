using System;
using System.Collections;
using System.Collections.Generic;
using ReadOnlyData;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Camera))]
public class FPPCameraController : MonoBehaviour {

	[SerializeField] private float mouseSensitivity;
	[SerializeField] private float maxY = 80f;
	[SerializeField] private float minY = -80f;

	[SerializeField] private float maxX = 360f;
	[SerializeField] private float minX = 0f;

	[Range(60f, 120f)]
	[SerializeField] private float fieldOfView = 75f;
	[SerializeField] private bool invertYRot;
	
	[SerializeField] private float xRot;
	[SerializeField] private float yRot;

	private Camera _camera;
	private Transform _cameraTransform;

	private void Start() {
		_cameraTransform = transform;
	}

	private void LateUpdate() {
		var xInputRot = Input.GetAxisRaw(InputStrings.Axis.MouseX) * mouseSensitivity * Time.deltaTime;
		var yInputRot = Input.GetAxisRaw(InputStrings.Axis.MouseY) * mouseSensitivity * Time.deltaTime;

		yInputRot = invertYRot ? yInputRot : -yInputRot;
		//accounting for inverted/not inverted Y axis control
		
		xRot += xInputRot;
		yRot += yInputRot;

		ApplyRotConstraints();
		ApplyRotation();
	}

	private void ApplyRotConstraints() {
		if (yRot > maxY) {
			yRot = maxY;
		} 
		else if (yRot < minY) {
			yRot = minY;
		}
		
		//TODO: possible user input loss
		//In theory: rot 355 + 10 degree input rot in frame = 365 rot -> rounded to 0 with 5 input degree loss
		if (xRot > maxX) {
			xRot = minX;
		} 
		else if (xRot < minX) {
			xRot = maxX;
		}
	}

	private void ApplyRotation() {
		var newLocalCameraRotation = transform.localRotation;
		newLocalCameraRotation = Quaternion.Euler(yRot,xRot,newLocalCameraRotation.z);
		transform.localRotation = newLocalCameraRotation;
		//Y mouse rot = X camera rot
		//X mouse rot = Y camera rot
	}
	
	public void RotateTo(float xRot, float yRot) {
		this.xRot = xRot;
		this.yRot = yRot;
	}

	private void OnValidate() {
		_camera = gameObject.GetComponent<Camera>();
		_camera.fieldOfView = fieldOfView;
	}
}
