using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class StopWatch : MonoBehaviour {

    public Text counterText;
    public float seconds, minutes;
    public bool updateTimer = false;
    float playerTime;
    public GameObject player1Text, player2Text, player1MoveText, player2MoveText, player1Watch,player2Watch, winnerText;
    public GameObject audioManager;

    // Use this for initialization
    void Start () {
        player1Text.SetActive(false);
        player2Text.SetActive(false);
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);
        winnerText.SetActive(false);

        audioManager = GameObject.FindGameObjectWithTag("Sound");

        counterText = GetComponent<Text>() as Text;
        updateTimer = false;
        playerTime = 0;
    }

    void Update()
    {
        if (updateTimer)
        {
            //minutes = (float)(Time.time / 60f);
            seconds += Time.deltaTime;
            counterText.text = seconds.ToString("0");
            //counterText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            player1Text.SetActive(true);
            audioManager.GetComponent<AudioManager>().PlayMusic(3);
        }
    }

    public void Player1Finished()
    {
        player1Text.SetActive(true);
        player1MoveText.SetActive(false);
        player2MoveText.SetActive(true);
        audioManager.GetComponent<AudioManager>().PlayMusic(3);

        updateTimer = false;
        playerTime = Mathf.Round(seconds);
        counterText.text = playerTime.ToString("0");
        player1Text.GetComponent<Text>().text = playerTime.ToString("0" + " seconds!");
    }

    public void Player2Finished()
    {
        player2Text.SetActive(true);
        player2MoveText.SetActive(false);
        audioManager.GetComponent<AudioManager>().PlayMusic(3);

        updateTimer = false;
        playerTime = Mathf.Round(seconds);
        counterText.text = playerTime.ToString("0");
        player2Text.GetComponent<Text>().text = playerTime.ToString("0" + " seconds!");
    }

    public void DisplayWinner()
    {
        winnerText.SetActive(true);

        if(player1Watch.GetComponent<StopWatch>().playerTime < player2Watch.GetComponent<StopWatch>().playerTime)
        {
            //player 1 wins
            winnerText.GetComponent<Text>().text = "Player 1 wins!";
            audioManager.GetComponent<AudioManager>().PlayMusic(4);
        }

        if (player1Watch.GetComponent<StopWatch>().playerTime > player2Watch.GetComponent<StopWatch>().playerTime)
        {
            //player 2 wins
            winnerText.GetComponent<Text>().text = "Player 2 wins!";
            audioManager.GetComponent<AudioManager>().PlayMusic(4);
        }

        if (player1Watch.GetComponent<StopWatch>().playerTime == player2Watch.GetComponent<StopWatch>().playerTime)
        {
            //It's a tie
            winnerText.GetComponent<Text>().text = "It's a tie!";
            audioManager.GetComponent<AudioManager>().PlayMusic(4);
        }
    }
}
