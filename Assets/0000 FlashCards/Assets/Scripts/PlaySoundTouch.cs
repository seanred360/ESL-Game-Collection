using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySoundTouch : MonoBehaviour
{
    [Tooltip("The sound to play")]
    public AudioClip sound;

    [Tooltip("Should we play the sound when the game starts")]
    public bool playOnStart = false;

    [Tooltip("Should we play the sound when clicking this button?")]
    public bool playOnClick = true;

    [Tooltip("The tag of the sound source")]
    public string soundSourceTag = "Sound";

    void Start()
    {
        // Listen for a click to play a sound
        if (GetComponent<Button>()) GetComponent<Button>().onClick.AddListener(delegate { PlaySound(); });
    }

    /// <summary>
    /// This runs whenever the gameobject is activated
    /// </summary>
    void OnEnable()
    {
        if (playOnStart == true) PlaySound();
    }

    /// <summary>
    /// Plays the sound
    /// </summary>
    public void PlaySound()
    {
        // If there is a sound source tag and audio to play, play the sound from the audio source based on its tag
        if (soundSourceTag != string.Empty && sound)
        {
            // Play the sound
            GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(sound);
        }
        else
        {
            GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().Stop();

            if (this.GetComponent<Image>())
            {
                GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/" + 
                    this.GetComponent<Image>().sprite.name));
            }
            

            if (this.GetComponent<Text>())
            {
                GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/" + 
                    this.GetComponent<Text>().text));
            }
        }
           
    }
}
