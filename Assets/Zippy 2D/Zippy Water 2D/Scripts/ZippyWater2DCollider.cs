using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZippyWater2DCollider : MonoBehaviour {
	[HideInInspector]	public int index;
	[HideInInspector]	public float waveForce;
	[HideInInspector]	public float waveHeight;	
	[HideInInspector]	public Transform cacheTransform;
	ZippyWater2D water;
	public float height;

	void Awake() {
		cacheTransform = transform;
		water = transform.parent.GetComponent<ZippyWater2D>();
		if (water.forced == null) water.forced = new List<Collider2D>();
	}

	void OnTriggerStay2D(Collider2D hit) {
		if (!water.enabledOutsideScreen && !water.cacheMeshRenderer.isVisible) return;
		Rigidbody2D r = hit.transform.GetComponent<Rigidbody2D>();
		if (r == null) return;
		if (water.wavePower <= 0) return;
		if (ContainsCollider(hit)) return;
			if (waveForce > 0)
				r.AddForce(Vector2.up * waveForce * water.wavePower);
			else
				r.AddForce(Vector2.up * waveForce * water.wavePower * .1f);
		if(water.waveTorque != 0) r.AddTorque(waveForce * water.wavePower * Random.Range(0f, water.waveTorque));
		water.forced.Add(hit);
	}

	bool ContainsCollider(Collider2D hit) {
		for(int i = 0; i < water.forced.Count; i++) {
			if (water.forced[i] == hit) return true;
		}
		return false;
	}

	void FixedUpdate() {
		if (!water.enabledOutsideScreen && !water.cacheMeshRenderer.isVisible) return;
		if (water.forced.Count > 0) water.forced.Clear();
	}

	void OnTriggerExit2D(Collider2D hit) {
		if (CheckIfObjectIsInWater(hit.transform)) water.waterObjects.Remove(hit.transform);
		if (!water.enabledOutsideScreen && !water.cacheMeshRenderer.isVisible) return;
		Rigidbody2D r = hit.transform.GetComponent<Rigidbody2D>();
		if (r == null) return;
		if (r.position.y > cacheTransform.position.y) {
			water.Splash(index, Mathf.Clamp(r.velocity.y, -water.maxForce, water.maxForce));
			r.AddForce(Vector2.right * waveForce * water.wavePower * Random.Range(-.1f, .1f) * r.velocity.y);
			r.AddTorque(waveForce * water.wavePower * Random.Range(-.1f, .1f) * r.velocity.y);
		}
	}

	void OnTriggerEnter2D(Collider2D hit) {
		if(!CheckIfObjectIsInWater(hit.transform)) water.waterObjects.Add(hit.transform);
		if (!water.enabledOutsideScreen && !water.cacheMeshRenderer.isVisible) return;
		Rigidbody2D r = hit.transform.GetComponent<Rigidbody2D>();
		if (r == null) return;
		if (r.position.y > cacheTransform.position.y) {
			water.Splash(index, Mathf.Clamp(r.velocity.y * r.mass, -water.maxForce, water.maxForce));		
		}
	}

	bool CheckIfObjectIsInWater(Transform t) {
		for(int i=0; i < water.waterObjects.Count; i++) {
			if(water.waterObjects[i] == t) {
				return true;
			}
		}
		return false;
	}

}