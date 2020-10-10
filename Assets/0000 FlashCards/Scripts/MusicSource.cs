using UnityEngine;

public class MusicSource : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
    }
}
