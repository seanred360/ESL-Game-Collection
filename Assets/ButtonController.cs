using UnityEngine;
using UnityEngine.UI;

public static class ButtonController
{
    public static GameObject[] objs;
    public static GameObject stopButton;

    public static void EnableButton()
    {
        objs = GameObject.FindGameObjectsWithTag("ButtonTag1");
        foreach (GameObject ButtonTag1 in objs)
        {
            ButtonTag1.GetComponent<Button>().interactable = true;
        }
    }

    public static void DisableButton()
    {
        objs = GameObject.FindGameObjectsWithTag("ButtonTag1");
        foreach (GameObject ButtonTag1 in objs)
        {
            ButtonTag1.GetComponent<Button>().interactable = false;
        }
    }

    public static void ShowStopButton()
    {
        if (stopButton == null)
            stopButton = GameObject.FindGameObjectWithTag("StopButton");
        stopButton.SetActive(true);
    }
    public static void HideStopButton()
    {
        if (stopButton == null)
            stopButton = GameObject.FindGameObjectWithTag("StopButton");
        stopButton.SetActive(false);
    }
}