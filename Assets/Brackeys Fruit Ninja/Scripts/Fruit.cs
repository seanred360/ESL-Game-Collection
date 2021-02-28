using SlicingGame;
using UnityEngine;

public class Fruit : MonoBehaviour {

	public float startForce = 15f;
	public LinecastCutterBehaviour linecastCutterBehaviour;

	Rigidbody2D rb;

	Vector2 entryPoint;
	Vector2 exitPoint;

	public int points;

	void Start ()
	{
		linecastCutterBehaviour = GameObject.FindObjectOfType<LinecastCutterBehaviour>();
		rb = GetComponent<Rigidbody2D>();
		rb.AddForce(transform.up * startForce, ForceMode2D.Impulse);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Blade")
		{
			entryPoint = collision.transform.position;
			Vector3 direction = (collision.transform.position - transform.position).normalized;
			Destroy(gameObject,3f);
		}
	}

    private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Blade")
		{
			exitPoint = collision.transform.position;
			linecastCutterBehaviour.LinecastCut(entryPoint, exitPoint, linecastCutterBehaviour.layerMask.value);
			points++;
			Debug.Log(points);
			GetComponent<Collider2D>().enabled = false;
			Invoke("Destroy", .5f);
		}
	}

    private void Destroy()
    {
		Destroy(this); 
    }
}
