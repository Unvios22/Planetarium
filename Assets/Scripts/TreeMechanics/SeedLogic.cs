using System;
using System.Collections;
using System.Collections.Generic;
using ReadOnlyData;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeedLogic : MonoBehaviour {
	[SerializeField] private GameObject treePrefab;
	[SerializeField] private float minTreeGrowTime;
	[SerializeField] private float maxTreeGrowTime;
	
	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.layer == (int)PhysicsLayers.Layers.Ground) {
			StartCoroutine(SpawnTree(other.transform));
		}
	}

	private IEnumerator SpawnTree(Transform groundTransform) {
		var timeToWait = Random.Range(minTreeGrowTime, maxTreeGrowTime);
		yield return new WaitForSeconds(timeToWait);

		var grownTree = Instantiate(treePrefab, transform.position, Quaternion.identity);
		var grownTreeTransform = grownTree.transform;
		var gravityVector = (groundTransform.position - grownTreeTransform.position).normalized;
		grownTreeTransform.up = -gravityVector;

		var degreesToRotate = Random.Range(0f, 360f);
		grownTreeTransform.RotateAround(grownTreeTransform.position, grownTreeTransform.up, degreesToRotate);
		grownTree.GetComponent<SeedSpawner>().SeedDestroyed();
		Destroy(gameObject);
	}
}
