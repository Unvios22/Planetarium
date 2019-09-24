using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(AlignToPlanetUtil))]
public class AlignToPlanetEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		var alignScript = (AlignToPlanetUtil) target;
		if (GUILayout.Button("Realign this to planet!")) {
				alignScript.RealignToPlanet();
		}
	}
}