using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eater : MonoBehaviour
{
    FoodPicker foodPicker;
    int foodNumber;
    public Transform selectedItem;
    public GameObject gumSplatter;
    public GameObject gumBubble;
    GumGameManager gameManager;


    void Start()
    {
        foodPicker = GameObject.FindObjectOfType<FoodPicker>();
        gameManager = GameObject.FindObjectOfType<GumGameManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        foodNumber = collision.gameObject.GetComponent<Food>().foodNumber;
        foodPicker.CheckAnswer(foodNumber);
    }

    public void PlayInflateSound()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayMusic(2);
        gumBubble.GetComponent<SpriteRenderer>().color = selectedItem.GetComponent<Image>().color;
    }

    public void PlayPopSound()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySFX(2);
        gumSplatter.GetComponent<SpriteRenderer>().color = selectedItem.GetComponent<Image>().color;
        if(gameManager.AreAllFoodsEaten())
        {
            StartCoroutine(StartGameOver());
        }
    }

    IEnumerator StartGameOver()
    {
        yield return new WaitForSeconds(3f);
        gameManager.GameOver();
    }
}
