using UnityEngine;
using System.Collections;

public class ZippyWater2DBeachBall : MonoBehaviour {
	Rigidbody2D cacheRigidbody2D;
	public float timer = 3f;
	public float minForce = 100f;
	public float force = 500f;

	// Use this for initialization
	void Start () {
		cacheRigidbody2D = GetComponent<Rigidbody2D>();
		Invoke("Bounce", Random.Range(0, timer * 2));
	}
	
	// Update is called once per frame
	void Bounce () {
		float f = force * cacheRigidbody2D.mass;
		float ff = minForce * cacheRigidbody2D.mass;
		cacheRigidbody2D.AddForce(Vector2.up * Random.Range(ff, f));
		cacheRigidbody2D.AddTorque(Random.Range(-f*.25f, f * .25f));
		Invoke("Bounce", Random.Range(timer, timer*2));
	}
}
