using UnityEngine;
using System.Collections;

public class ZippyWater2DCenterOfMass : MonoBehaviour {
	[Tooltip("Set the point of balance. Useful for keeping things upright in the water.")]
	public Vector2 CenterOfMass;

	void Awake () {
		GetComponent<Rigidbody2D>().centerOfMass = CenterOfMass;
	}

#if UNITY_EDITOR
	void OnDrawGizmos () {
		if (Application.isPlaying) return;
		Gizmos.DrawWireSphere(transform.position + (Vector3)CenterOfMass, .3f);
	}
#endif
}
