using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour {

	private Rigidbody2D rb2d;
    private CircleCollider2D cc2d;
    float moveX;
    float moveY;
    Vector2 oldVelocity;
    Vector2 originalScale;

	void GoBall() {
		float rand = Random.Range (0, 2);
		if (rand < 1) {
			rb2d.AddForce (new Vector2 (50, -49));
            rb2d.AddForce(new Vector2(moveX,moveY));
		} else {
			rb2d.AddForce (new Vector2 (-50, -49));
            rb2d.AddForce(new Vector2(moveX, moveY));
        }
	}

	// Use this for initialization
	void Start () {
        originalScale = transform.localScale;
        moveX = Random.Range(-24, 24);
        moveY = Random.Range(-24, 24);

        rb2d = GetComponent<Rigidbody2D> ();
        cc2d = GetComponent<CircleCollider2D>();
		Invoke ("GoBall", 2);
	}

    void ResetBall() {
		rb2d.velocity = new Vector2 (0, 0);
		transform.position = Vector2.zero;
	}

    public void FreezeBall()
    {
        if (rb2d.velocity == Vector2.zero) //unfreeze
        {
            rb2d.velocity = oldVelocity;
            cc2d.enabled = true;
            this.transform.localScale = originalScale;
            this.transform.localScale += new Vector3(0,0,1);// stops the image from going behind the canvas
        }
        else //freeze
        {
            oldVelocity = rb2d.velocity;
            rb2d.velocity = new Vector2(0, 0);
            cc2d.enabled = false;
            this.transform.localScale = this.transform.localScale*2;
        }
    }

	void RestartGame() {
		ResetBall ();
		Invoke ("GoBall", 1);
	}
}
