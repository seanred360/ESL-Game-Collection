using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FoodPicker : MonoBehaviour {

    public int numberChosen;
    GameObject soundSource;
    AudioManager audioManager;
    GumGameManager gameManager;
    Eater eater;
    public List<GameObject> uneatenFood;

    // Use this for initialization
    void Awake () {
        eater = GameObject.FindObjectOfType<Eater>().GetComponent<Eater>();
        gameManager = GameObject.Find("GameManager").GetComponent<GumGameManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        uneatenFood.AddRange(GameObject.FindGameObjectsWithTag("Food"));
    }

    void Start()
    {
        AssignFoodNumber();
        PickNumber();
    }

    public void PickNumber()
    {
        if ((!gameManager.IsGameOver) && (uneatenFood.Count > 0))
        {
            foreach (GameObject fd in uneatenFood) { fd.SetActive(true); }
            Debug.Log("picking number");
            numberChosen = Random.Range(0, uneatenFood.Count);
            uneatenFood[numberChosen].GetComponent<AudioSource>().Play();
        }
    }

    public void CheckAnswer(int useIndex)
    {
        if (numberChosen == useIndex)
        {
            SelectTrue(useIndex);
            foreach (GameObject fd in uneatenFood) { fd.SetActive(false); }
            //eater.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        }
        else
        if (numberChosen != useIndex)
        {
            SelectFalse(useIndex);
        }
    }

    public void SelectTrue(int useIndex)
    {
        eater.GetComponent<Animator>().Play("ChewGum");
        uneatenFood[useIndex].GetComponent<Food>().isEaten = true;
        uneatenFood.RemoveAt(useIndex);
        AssignFoodNumber();
        Invoke("PickNumber",7);
        //PickNumber();
        Debug.Log("Correct");
        audioManager.PlaySFX(0);
    }

    public void SelectFalse(int useIndex)
    {
        eater.GetComponent<Animator>().Play("Refuse");
        Debug.Log("Wrong");
        gameManager.GetComponent<Mover>().Move(uneatenFood[useIndex]);
        audioManager.PlaySFX(1);
    }

    void AssignFoodNumber()
    {
        for (int i = 0; i < uneatenFood.Count; i++)
        {
            if (uneatenFood[i].name == uneatenFood[i].name)
            {
                uneatenFood[i].GetComponent<Food>().foodNumber = i;
            }
        }
    }
}
