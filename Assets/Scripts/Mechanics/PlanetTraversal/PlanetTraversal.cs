using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetTraversal : MonoBehaviour {
	[SerializeField] private Transform playerCameraTransform;
	[SerializeField] private List<Transform> traversalTargets;
	[SerializeField, Range(0, 180)] private float traversalLookDegreeOffset;
	[SerializeField] private Transform currentTarget;
	[SerializeField] private float traversalProgressMax;
	[SerializeField] private float traversalProgressSpeed;
	[SerializeField] private float traversalDecreaseSpeed;

	private float _traversalLookOffsetInternal;
	//dictionary of traversal targets with each target's traversal progress meter
	private Dictionary<PlanetTraversalTarget, float> _traversalTargetsInternal;

	private void Start() {
		OnValidate();
	}

	private void Update() {
		CheckPlayerLookAtTargets();
		CheckTargetsProgress();
	}

	private void CheckPlayerLookAtTargets() {
		for (int i = 0; i < _traversalTargetsInternal.Count; i++) {
			var traversalTarget = _traversalTargetsInternal.Keys.ElementAt(i);
			//TODO: dictionary keyCollection can't be accesed through indexing. Check how LINQ does it shenanigans ^^^^
			var targetTransform = traversalTarget.transform;
			//test if player is looking at targetTransform within provided offset
			var toTargetVector = (playerCameraTransform.position - targetTransform.position).normalized;
			var playerLookOffset = Vector3.Dot(toTargetVector, playerCameraTransform.forward);
			
			Debug.DrawLine(targetTransform.position, playerCameraTransform.position, Color.magenta);
			Debug.Log("Internal look offset: " + playerLookOffset);

			var traversalProgressMeter = _traversalTargetsInternal[traversalTarget];
			if (playerLookOffset < _traversalLookOffsetInternal) {
				//increase target's traversal progress meter
				traversalProgressMeter += traversalProgressSpeed * Time.deltaTime;
				currentTarget = traversalTarget.transform;
			}
			else if (traversalProgressMeter > 0){
				//decrease target's traversal progress meter
				traversalProgressMeter -= traversalDecreaseSpeed * Time.deltaTime;
			}
			_traversalTargetsInternal[traversalTarget] = traversalProgressMeter;
		}
	}

	private void CheckTargetsProgress() {
		for (int i = 0; i < _traversalTargetsInternal.Count; i++) {
			var targetProgressPair = _traversalTargetsInternal.ElementAt(0);
			//TODO: dictionary can't be accesed through indexing. Check how LINQ does it shenanigans ^^^^
			Debug.Log("Traversing to " + targetProgressPair.Key.name + ": " + targetProgressPair.Value/traversalProgressMax * 100 + "%");
			if (targetProgressPair.Value >= traversalProgressMax) {
				//reset progress meter of target that is being traversed to
				_traversalTargetsInternal[targetProgressPair.Key] = 0f;
				//TODO: add a dedicated scene loader script to project
				Debug.Log("LOADING SCENE!");
				SceneManager.LoadScene(targetProgressPair.Key.targetSceneName);
				break;
			}
		}
	}

	private void OnValidate() {
		RemapTargetOffsetInternal();
		CacheTraversalTargetsInternal();
	}

	private void RemapTargetOffsetInternal() {
		//remap from values ranging 0 -> 180 to values ranging 1 -> (-1)
		_traversalLookOffsetInternal = (traversalLookDegreeOffset - 0) / (180 - 0) * (1 - (-1)) + (-1);
		
		//TODO: check if works properly
		//TODO: rework remapping into float extension method
	}

	private void CacheTraversalTargetsInternal() {
		Debug.Log("ADAIKDJASIDI!");
		var targetsDictionary = new Dictionary<PlanetTraversalTarget, float>();
		foreach (var targetTransform in traversalTargets) {
			var targetScript = targetTransform.GetComponent<PlanetTraversalTarget>();
			targetsDictionary.Add(targetScript, 0f);
		}
		_traversalTargetsInternal = targetsDictionary;
	}
}
