using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSource : MonoBehaviour
{
    int currentVolume = 1;
    AudioSource audioSource;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();

         audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
    }
}
