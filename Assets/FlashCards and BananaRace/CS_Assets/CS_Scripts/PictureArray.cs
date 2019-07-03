using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureArray : MonoBehaviour {

    public GameObject[] pictures;
    public int numberChosen;
    public GameObject soundSource;
    AudioManager audioManager;
    public GameObject player1Watch;
    public GameObject player2Watch;
    public GameObject whoWinsText;
    public GameObject gameManager;
    public GameObject particleController;

    private bool coroutineAllowed = true;
    private int whosTurn = 1;

    // Use this for initialization
    void Start () {
        particleController = GameObject.FindGameObjectWithTag("ButtonGroup");
        soundSource = GameObject.FindGameObjectWithTag("Sound");
        audioManager = soundSource.GetComponent<AudioManager>();
        pictures = GameObject.FindGameObjectsWithTag("ButtonTag1");
    }

    public void PickNumber()
    {
        if (GameControl.gameOver == false)
        {
            numberChosen = Random.Range(0, pictures.Length);
            audioManager.PlaySFX(numberChosen);
            ButtonController.EnableButton();
            StartCoroutine(ChangePosition());// shuffle the pictures location
        }  
    }

    IEnumerator ChangePosition() //shuffle the pictures location
    {
        yield return new WaitForSeconds(1f);
        audioManager.PlayMusic(0);
        particleController.GetComponent<ParticleController>().EnableMixParticles();
        for (int i = 0; i < pictures.Length; i++)
        {
            Vector3 temp = pictures[i].transform.position;
            int randomIndex = Random.Range(0, pictures.Length);
            pictures[i].transform.position = pictures[randomIndex].transform.position;
            pictures[randomIndex].transform.position = temp;
        }
    }
    public void CheckAnswer(int useIndex)
    {
        ButtonController.DisableButton();
        if (numberChosen == useIndex)
        {
            SelectTrue();
        }
        if (numberChosen != useIndex)
        {
            SelectFalse();
        }
    }

    public void SelectTrue()
    {
        if (coroutineAllowed)
           StartCoroutine(RollTheDice());
        audioManager.PlayMusic(1);
        particleController.GetComponent<ParticleController>().EnableCorrectParticles();
    }

    public void SelectFalse()
    {
        if (coroutineAllowed)
           StartCoroutine(BackRollTheDice());
        audioManager.PlayMusic(2);
    }

    public IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        if (whosTurn == 1)
        {
            GameControl.MovePlayer(1);
        }
        else if (whosTurn == -1)
        {
            GameControl.MovePlayer(2);
        }
        //whosTurn *= -1;
        coroutineAllowed = true;
        yield return new WaitForSeconds(2f);
        PickNumber();
    }

    public IEnumerator BackRollTheDice()
    {
        coroutineAllowed = false;
        if (whosTurn == 1)
        {
            GameControl.BackMovePlayer(1);
        }
        else if (whosTurn == -1)
        {
            GameControl.BackMovePlayer(2);
        }
        //whosTurn *= -1;
        coroutineAllowed = true;
        yield return new WaitForSeconds(2f);
        PickNumber();
    }

    public void StartPlayer1()
    {
        PickNumber();
        player1Watch.GetComponent<StopWatch>().updateTimer = true;
    }

    public void StartPlayer2()
    {
        GameControl.gameOver = false;
        player2Watch.GetComponent<StopWatch>().updateTimer = true;
        whosTurn *= -1;
        ButtonController.EnableButton();
        PickNumber();
    }
}
