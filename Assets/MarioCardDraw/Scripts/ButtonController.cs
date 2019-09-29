using UnityEngine;
using UnityEngine.UI;

namespace MarioCardDraw
{
    public static class ButtonController
    {
        public static GameObject[] objs;
        public static GameObject stopButton;

        public static void EnableButton()
        {
            objs = GameObject.FindGameObjectsWithTag("DisableThis");
            foreach (GameObject buttonToDisable in objs)
            {
                buttonToDisable.GetComponent<Button>().interactable = true;
            }
        }

        public static void DisableButton()
        {
            objs = GameObject.FindGameObjectsWithTag("DisableThis");
            foreach (GameObject buttonToDisable in objs)
            {
                buttonToDisable.GetComponent<Button>().interactable = false;
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
}