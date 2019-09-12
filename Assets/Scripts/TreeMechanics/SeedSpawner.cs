
using System.Collections;
using UnityEngine;

public class SeedSpawner : MonoBehaviour {
	private static int _seedAmount;
	[SerializeField] private float minSpreadTime = 2f;
	[SerializeField] private float maxSpreadTime = 5f;
	[SerializeField] private int amountOfSeedsSpawned;
	[SerializeField] private int treeLifeSeconds;
	[SerializeField] private GameObject seedPrefab;
	[SerializeField] private float seedEjectionForce;
	[SerializeField] private Transform seedSpawnPoint;
	[SerializeField] private int maxSeedAmount;

	public void SeedDestroyed() {
		_seedAmount--;
	}
	
	private void OnEnable() {
		StartCoroutine(SpreadSeeds());
		StartCoroutine(Die());
	}

	private IEnumerator SpreadSeeds() {
		var timeToWait = Random.Range(minSpreadTime, maxSpreadTime);
		yield return new WaitForSeconds(timeToWait);
		for (int i = 0; i < amountOfSeedsSpawned; i++) {
			if (_seedAmount >= maxSeedAmount) {
				yield break;
			}
			SpawnSeed();
		}
	}	

	private void SpawnSeed() {
		_seedAmount++;
		Debug.Log("Seeds: " + _seedAmount);
		var seed = Instantiate(seedPrefab,seedSpawnPoint.position, Quaternion.identity);
		var forceVector = Random.onUnitSphere;
		seed.GetComponent<Rigidbody>().AddForce(forceVector * seedEjectionForce,ForceMode.Impulse);
	}

	private IEnumerator Die() {
		yield return new WaitForSeconds(treeLifeSeconds);
		Destroy(gameObject);
	}
}
