using UnityEngine;

[CreateAssetMenu(fileName = "BoidSettings")]
public class BoidSettings : ScriptableObject {
	public float moveSpeed;
	public float neighborsRange;
	public float avoidanceRange;
}
