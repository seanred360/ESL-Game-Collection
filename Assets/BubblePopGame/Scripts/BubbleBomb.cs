using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleBomb : MonoBehaviour
{
    public GameObject explodeParticle;
    AudioManager audioManager;
    BubbleGameController gameManager;
    public GameObject poopSplatterPrefab;


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<BubbleGameController>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        transform.localScale = new Vector3(1, 1, 1) * Random.Range(1f, 3f);
    }

    private void Update()
    {
        transform.position = transform.position + new Vector3(0, gameManager.speed * Time.deltaTime, 0);

        gameManager.speed += (gameManager.acceleration * Time.deltaTime / 100);
        //Debug.Log( "current speed " + (gameManager.speed += (gameManager.acceleration * Time.deltaTime / 100)));
    }

    public void Explode()
    {
        audioManager.PlayVoice(1);
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.GetComponent<Button>().interactable = false;
        audioManager.PlayVoice(1);
        gameObject.GetComponent<AudioSource>().Play();
        Instantiate(explodeParticle, transform.position, Quaternion.identity);
        GameObject splatter = Instantiate(poopSplatterPrefab, transform.position, Quaternion.identity);
        splatter.transform.SetParent(gameManager.canvas);
        splatter.transform.localScale = transform.localScale;
        Destroy(gameObject, 3f);
        gameManager.UpdateScore(-2);
    }
}
