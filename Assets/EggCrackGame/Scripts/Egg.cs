using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public GameObject[] cracks;
    public GameObject crackedEgg;
    int crackNum;
    AudioManager audioManager;
    public Animator word;
    EggGameController gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<EggGameController>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void CrackEgg()
    {
        if (crackNum >= cracks.Length)
        {
            crackedEgg.SetActive(true);
            audioManager.PlaySFX(3);
            word.Play("EggWord");
            gameManager.MoveEggWord();
            gameObject.SetActive(false);
        }
        else
        {
            foreach (GameObject crack in cracks)
            {
                crack.SetActive(false);
            }
            cracks[crackNum].SetActive(true);
            crackNum += 1;
            audioManager.PlaySFX(Random.Range(0, 2));
            audioManager.PlaySFX(4);
        }
    }
}
