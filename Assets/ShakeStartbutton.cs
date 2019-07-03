using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShakeStartbutton : MonoBehaviour
{
    public GameObject gameControl;

    // Start is called before the first frame update
    void Start()
    {
        gameControl = GameObject.Find("GameManager");
        this.transform.DOShakeScale(1f, .05f, 10, 90, true).SetLoops(999999999);
        StartCoroutine(FadeTextToFullAlpha(1f,this.GetComponent<Text>()));
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public void StartPlayer1()
    {
        GameControl.p1Finished = false;
        GameControl.gameOver = false;
        StartCoroutine(Wait());
    }

    public void StartPlayer2()
    {
        GameControl.p2Finished = false;
        GameControl.gameOver = false;
        StartCoroutine(Wait2());
    }

    private IEnumerator Wait()
    {
        this.GetComponent<Text>().color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(1.5f); //play the round start sound before beginning
        gameControl.GetComponent<PictureArray>().StartPlayer1();
        this.gameObject.SetActive(false);
    }

    private IEnumerator Wait2()
    {
        this.GetComponent<Text>().color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(1.5f);
        gameControl.GetComponent<PictureArray>().StartPlayer2();
        this.gameObject.SetActive(false);
    }
}
