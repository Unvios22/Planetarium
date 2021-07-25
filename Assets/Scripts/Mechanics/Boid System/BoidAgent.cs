using System;
using UnityEngine;

[RequireComponent(typeof(MeshCollider),typeof(Rigidbody))]
public class BoidAgent : MonoBehaviour {
	[NonSerialized] public Rigidbody Rigidbody;

	private void Start() {
		Rigidbody = GetComponent<Rigidbody>();
	}
}
