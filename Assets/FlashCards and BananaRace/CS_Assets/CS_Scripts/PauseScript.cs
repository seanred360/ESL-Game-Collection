using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseScript : MonoBehaviour {

    internal bool isPaused = false;
    public GameObject pauseCanvas;
    //internal GameObject buttonBeforePause;
    //internal EventSystem eventSystem;

    void Awake()
    {
        // Activate the pause canvas early on, so it can detect info about sound volume state
        
        if (pauseCanvas) pauseCanvas.SetActive(false);
    }

    void Start()
    {
        //Input.multiTouchEnabled = false;
    }

    public void Pause(bool showMenu)
    {
        isPaused = true;

        //Set timescale to 0, preventing anything from moving
        Time.timeScale = 0;

        // Remember the button that was selected before pausing
        //if (eventSystem) buttonBeforePause = eventSystem.currentSelectedGameObject;

        //Show the pause screen and hide the game screen
        if (showMenu == true)
        {
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(true);
        }

        if (GameObject.FindGameObjectWithTag("ButtonTag1"))
            ButtonController.DisableButton();
    }

    public void Unpause()
    {
            isPaused = false;

            //Set timescale back to the current game speed
            Time.timeScale = 1;

            //Hide the pause screen and show the game screen
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(false);

            // Select the button that we pressed before pausing
            //if (eventSystem) eventSystem.SetSelectedGameObject(buttonBeforePause);

            if (GameObject.FindGameObjectWithTag("ButtonTag1"))
                ButtonController.EnableButton();
    }

    public void UnFreezeTime()
    {
        Time.timeScale = 1;
    }

}
