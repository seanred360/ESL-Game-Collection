using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlashyLetter : MonoBehaviour
{
    AudioManager audioManager;
    public GameObject popParticle;
    BubbleGameController gameManager;
    Image image;
    float interval = .2f;
    int health = 15;

    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(FlashText());
        transform.DOMove(Vector3.zero, 3f);
        gameManager = GameObject.Find("GameManager").GetComponent<BubbleGameController>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    IEnumerator FlashText()
    {
        image.DOColor(Color.white,interval);
        yield return new WaitForSeconds(interval);
        image.DOColor(Color.yellow, interval);
        yield return new WaitForSeconds(interval);
        StartCoroutine(FlashText());
    }

    public void LoseHealth()
    {
        if(health <= 0) { PopBubble(); }
        health -= 1;
        audioManager.PlaySFX(0);
    }

    public void PopBubble()
    {
        audioManager.PlayMusic(Random.Range(0, 2));
        audioManager.PlaySFX(4);
        Instantiate(popParticle, transform.position, Quaternion.identity);
        gameManager.GameOver();
        Destroy(gameObject);
    }
}
