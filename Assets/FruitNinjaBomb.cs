using UnityEngine;

public class FruitNinjaBomb : MonoBehaviour
{

    public float startForce = 15f;
    Rigidbody2D rb;

    public GameObject explosion;
    public ChromaticAberration chroma;
    public SimpleCameraShake shake;

    void Awake()
    {
        //Setup Shake and Chroma Script References... both scripts are on the main camera
        shake = Camera.main.GetComponent<SimpleCameraShake>();
        chroma = Camera.main.GetComponent<ChromaticAberration>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * startForce, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Blade")
        {
            Vector3 direction = (col.transform.position - transform.position).normalized;
            BombDestroy();
        }
    }

    void BombDestroy()
    {
        //call our Start Camera Shake Method;
        shake.StartShake();
        //call our Start Chromatic Aberration Method;
        chroma.StartAberration();

        explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        //Set this object inactive so we can use it again.
        gameObject.SetActive(false);
    }
}
