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
    float speed = 3;
    float acceleration = 3;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<BubbleGameController>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        GetComponent<Image>().sprite = bubbles[Random.Range(0, bubbles.Length)];
        wordIndex = Random.Range(0, words.Length);
        //wordObject.sprite = words[wordIndex];
        wordObject.sprite = gameManager.sprites[wordIndex];
        gameObject.GetComponent<AudioSource>().clip = (Resources.Load<AudioClip>("Sounds/" + wordObject.sprite.name));
    }

    private void Update()
    {
        transform.position = transform.position + new Vector3(0,speed * Time.deltaTime, 0);

        //if (gameManager.points % 100 == 0)
        //{
        //    speed += .05f;
        //}


        speed += acceleration * Time.deltaTime;
        //transform.Translate(Vector3.left * change);
    }

    public void PopBubble()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.GetComponent<Button>().interactable = false;
        //transform.Find("Word").GetComponent<Image>().enabled = false;
        audioManager.PlayMusic(Random.Range(0, 2));
        //audioManager.PlaySFX(wordIndex);
        gameObject.GetComponent<AudioSource>().Play();
        Instantiate(popParticle,transform.position,Quaternion.identity);
        gameManager.points += 1;
        Destroy(gameObject,3f);
    }
}
