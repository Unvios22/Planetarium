using ReadOnlyData;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FPPCameraController : MonoBehaviour {

	[SerializeField] private float mouseSensitivity;
	
	[SerializeField] private float maxX = 80f;
	[SerializeField] private float minX = -80f;

	[Range(60f, 120f)]
	[SerializeField] private float fieldOfView = 75f;
	[SerializeField] private bool invertYRot;
	
	private Camera _camera;
	private float _totalXRotCache;

	public void ClearYRotation() {
		var localRotationAngles = transform.localEulerAngles;
		transform.localRotation = Quaternion.Euler(localRotationAngles.x, 0f, localRotationAngles.z);
	}

	private void LateUpdate() {
		var xMouseInputRot = Input.GetAxisRaw(InputStrings.Axis.MouseX) * mouseSensitivity * Time.deltaTime;
		var yMouseInputRot = Input.GetAxisRaw(InputStrings.Axis.MouseY) * mouseSensitivity * Time.deltaTime;

		yMouseInputRot = invertYRot ? yMouseInputRot : -yMouseInputRot;
		//accounting for inverted/not inverted Y axis control
		
		var xCameraInputRot = yMouseInputRot;
		//Y mouse rot = X camera rot
		
		var yCameraInputRot = xMouseInputRot;
		//X mouse rot = Y camera rot
		
		_totalXRotCache += xCameraInputRot;
		//caching total X rotation to easily control maxX & minX rot
		
		xCameraInputRot = ApplyXRotConstraints(xCameraInputRot);
		ApplyRotation(xCameraInputRot, yCameraInputRot);
	}
	
	private void ApplyRotation(float xRot, float yRot) {
		var localCameraRotation = transform.localEulerAngles;
		var totalXRot = localCameraRotation.x + xRot;
		var totalYRot = localCameraRotation.y + yRot;
		transform.localRotation = Quaternion.Euler(totalXRot,totalYRot,localCameraRotation.z);
	}

	private float ApplyXRotConstraints(float xInputRot) {
		//camera too low
		if (_totalXRotCache > maxX) {
			xInputRot -= (_totalXRotCache - maxX);
			_totalXRotCache = maxX;
			return xInputRot ;
		}
		//camera too high
		if (_totalXRotCache < minX) {
			xInputRot += (minX - _totalXRotCache);
			_totalXRotCache = minX;
			return xInputRot ;
		}
		return xInputRot;
	}

	private void OnValidate() {
		_camera = gameObject.GetComponent<Camera>();
		_camera.fieldOfView = fieldOfView;
		maxX = Mathf.Clamp(maxX, 0f, 90f);
		minX = Mathf.Clamp(minX, -90f, 0);
	}
}
