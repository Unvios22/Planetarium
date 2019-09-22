
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitSpawner : MonoBehaviour {
	[SerializeField] private float minFruitSpawnTime = 2f;
	[SerializeField] private float maxFruitSpawnTime = 5f;
	[SerializeField] private GameObject fruitPrefab;
	[SerializeField] private int maxSpawnedFruitAmount;
	[SerializeField] private int spawnedFruitAmount;

	[SerializeField] private MeshFilter treeLeaves;

	public void FruitDestroyed() {
		spawnedFruitAmount--;
	}
	
	private void OnEnable() {
		StartCoroutine(SpawnFruit());
	}

	private IEnumerator SpawnFruit() {
		while (spawnedFruitAmount < maxSpawnedFruitAmount) {
			var timeToWait = Random.Range(minFruitSpawnTime, maxFruitSpawnTime);
			yield return new WaitForSeconds(timeToWait);
			InstantiateFruit();
		}
	}

	private void InstantiateFruit() {
		//get random point on treeLeaves mesh
		var vertices = treeLeaves.mesh.vertices;
		var randomVertex = vertices[Random.Range(0, vertices.Length)];
		var spawnPosition = treeLeaves.transform.TransformPoint(randomVertex);

		var spawnedFruit = Instantiate(fruitPrefab, spawnPosition, Quaternion.identity);
		spawnedFruit.transform.parent = transform;

		spawnedFruitAmount++;
	}
}
