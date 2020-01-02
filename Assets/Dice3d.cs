using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice3d : MonoBehaviour
{
    public SpriteRenderer floatingNumber;
    public Sprite[] numbers;
    int numberRolled;
    public AudioSource audio;

    public void Change1()
    {
        floatingNumber.sprite = numbers[0];
        numberRolled = 1;
        //audio.Play();
    }
    public void Change2()
    {
        floatingNumber.sprite = numbers[1];
        numberRolled = 2;
        //audio.Play();
    }
    public void Change3()
    {
        floatingNumber.sprite = numbers[2];
        numberRolled = 3;
        audio.Play();
    }
}
