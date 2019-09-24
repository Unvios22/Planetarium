using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AlignToPlanetUtil : MonoBehaviour {
	public Transform Planet;
	public void RealignToPlanet() {
		var vectorTowardsPlanet = (Planet.position - transform.position).normalized;
		transform.transform.up = -vectorTowardsPlanet;
	}
}
