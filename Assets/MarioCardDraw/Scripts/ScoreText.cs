using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    internal float instanceTime = 0;

    private void Awake()
    {
        //Find all the music objects in the scene
        GameObject[] scoreTexts = GameObject.FindGameObjectsWithTag(tag);

        //Keep only the music object which has been in the game for more than 0 seconds
        if (scoreTexts.Length > 1)
        {
            foreach (var scoreText in scoreTexts)
            {
                if (scoreText.GetComponent<ScoreText>().instanceTime <= 0) Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }
}
