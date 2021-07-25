using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class PlanetTraversalTarget : MonoBehaviour {
	public Object targetScene;
	//TODO: rework to test if input asset is really a scene
	public string targetSceneName;
	
	private void OnValidate() {
		targetSceneName = targetScene.name;
	}
}
