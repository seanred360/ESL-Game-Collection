using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MemoryRepeatGame
{
	/// <summary>
	/// Plays a sound from an audio source.
	/// </summary>
	public class MRGPlaySound : MonoBehaviour
	{
		[Tooltip("The sound to play")]
		public AudioClip sound;
        
		[Tooltip("Should we play the sound when the game starts")]
		public bool playOnStart = false;

        [Tooltip("Should we play the sound when clicking this button?")]
        public bool playOnClick = true;

        [Tooltip("The tag of the sound source")]
		public string soundSourceTag = "Sound";
        static AudioSource soundSource;

        void Start()
        {
            if (soundSource == null) soundSource = GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>();

            // Listen for a click to play a sound
            if ( GetComponent<Button>() ) GetComponent<Button>().onClick.AddListener(delegate { PlaySound(); });
        }

        /// <summary>
        /// This runs whenever the gameobject is activated
        /// </summary>
        void OnEnable()
        {
			if( playOnStart == true )    PlaySound();
		}

		/// <summary>
		/// Plays the sound
		/// </summary>
		public void PlaySound()
		{
			// If there is a sound source tag and audio to play, play the sound from the audio source based on its tag
			if ( soundSourceTag != string.Empty && sound )    soundSource.PlayOneShot(sound);
            MRGGameController.sequenceSpeed = sound.length;
		}
	}
}