using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBabyMode : MonoBehaviour
{
    bool currentState = false;
    GameObject gameManager;

    private void Awake()
    {
        LevelNumber.babyMode = false;
        SetBabyMode();
    }

    public void SetBabyMode()
    {
        Color newColor = GetComponent<Image>().material.color;

        // Update the graphics of the button image to fit the sound state
        if (currentState == true)
            newColor.a = 1;
        else
            newColor.a = 0.5f;

        GetComponent<Image>().color = newColor;

        // Set the value of the sound state to the source object
        LevelNumber.babyMode = currentState;
    }

    /// <summary>
    /// Toggle the sound. Cycle through all sound modes and set the volume and icon accordingly
    /// </summary>
    public void BabyModeToggle()
    {
        // turn the volume on or off
        if (currentState == true)
        {
            currentState = false;
            this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("turnoff"));
        }
        else
        {
            currentState = true;
            this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("turnon"));
        }

        SetBabyMode();
    }
}
