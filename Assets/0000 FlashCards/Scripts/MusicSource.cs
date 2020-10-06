using UnityEngine;

public class MusicSource : MonoBehaviour
{
    AudioSource audioSource;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
    }
}
