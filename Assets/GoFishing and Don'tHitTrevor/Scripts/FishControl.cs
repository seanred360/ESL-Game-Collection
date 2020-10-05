using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishControl : MonoBehaviour {

	private Rigidbody2D rb2d;
    private CapsuleCollider2D cc2d;
    float moveX;
    float moveY;
    Vector2 oldVelocity;
    Vector2 originalScale;
    public Text fishNumber;
    public Canvas gameOverCanvas;
    

	// Use this for initialization
	void Start () {

        fishNumber = GetComponentInChildren<Text>();
        fishNumber.enabled = false;
        originalScale = transform.localScale;
        moveX = Random.Range(-24, 24);
        moveY = Random.Range(-24, 24);

        rb2d = GetComponent<Rigidbody2D> ();
        cc2d = GetComponent<CapsuleCollider2D>();
		Invoke ("GoBall", 2);
	}

    void GoBall()
    {
        float rand = Random.Range(0, 2);
        if (rand < 1)
        {
            rb2d.AddForce(new Vector2(50, -49));
            rb2d.AddForce(new Vector2(moveX, moveY));
        }
        else
        {
            rb2d.AddForce(new Vector2(-50, -49));
            rb2d.AddForce(new Vector2(moveX, moveY));
        }
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
            rb2d.freezeRotation = false;
            cc2d.enabled = true;
            rb2d.isKinematic = false;
            this.transform.localScale = originalScale;
            this.transform.localScale += new Vector3(0,0,1);// stops the image from going behind the canvas
        }
        else //freeze
        {
            oldVelocity = rb2d.velocity;
            rb2d.freezeRotation = true;
            rb2d.velocity = new Vector2(0, 0);
            cc2d.enabled = false;
            rb2d.isKinematic = true;
            this.transform.localScale = this.transform.localScale*2;
            fishNumber.enabled = true;
            Destroy(gameObject,10f);
        }
    }

	void RestartGame() {
		ResetBall ();
		Invoke ("GoBall", 1);
	}
}
