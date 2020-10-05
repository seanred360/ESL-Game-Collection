using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    public Sprite[] bubbles;
    public Sprite[] words;
    public GameObject popParticle;
    public Image wordObject;
    AudioManager audioManager;
    BubbleGameController gameManager;
    int wordIndex;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<BubbleGameController>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        GetComponent<Image>().sprite = bubbles[Random.Range(0, bubbles.Length)];
        wordIndex = Random.Range(0, words.Length);
        wordObject.sprite = gameManager.sprites[wordIndex];
        gameObject.GetComponent<AudioSource>().clip = (Resources.Load<AudioClip>("Sounds/" + wordObject.sprite.name));
        transform.localScale = new Vector3(1, 1, 1) * Random.Range(.3f,3f);
    }

    private void Update()
    {
        transform.position = transform.position + new Vector3(0,gameManager.speed * Time.deltaTime, 0);

        gameManager.speed += (gameManager.acceleration * Time.deltaTime / 100);
        //Debug.Log( "current speed " + (gameManager.speed += (gameManager.acceleration * Time.deltaTime / 100)));
    }

    public void PopBubble()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.GetComponent<Button>().interactable = false;
        audioManager.PlayMusic(Random.Range(0, 2));
        gameObject.GetComponent<AudioSource>().Play();
        Instantiate(popParticle,transform.position,Quaternion.identity);
        Destroy(gameObject,3f);
        gameManager.UpdateScore(1);
    }
}
