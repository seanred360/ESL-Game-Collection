using SlicingGame;
using UnityEngine;

public class Fruit : MonoBehaviour {

	public float startForce = 15f;

	Rigidbody2D rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.AddForce(transform.up * startForce, ForceMode2D.Impulse);
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Blade")
		{
			Vector3 direction = (col.transform.position - transform.position).normalized;
			if(col.tag == "Bomb") { GetComponent<DestroyBomb>().ActivateDestructionPerObjecType(); }
			else
			Destroy(gameObject,3f);
		}
	}

}
