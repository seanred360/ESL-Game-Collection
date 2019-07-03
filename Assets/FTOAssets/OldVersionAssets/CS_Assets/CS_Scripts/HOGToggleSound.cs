using UnityEngine;
using System.Collections;

namespace HiddenObjectGame
{
	/// <summary>
	/// This script toggles a sound source when clicked on. It also records the sound state (on/off) in a PlayerPrefs. In order to detect clicks you need to attach a collider to this object.
	/// </summary>
	public class HOGToggleSound:MonoBehaviour 
	{
		[Tooltip("The tag of the sound object")]
		public string soundObjectTag = "GameController";

		[Tooltip("The source of the sound")]
		public Transform soundObject;
	
		[Tooltip("The PlayerPrefs name of the sound")]
		public string playerPref = "SoundVolume";
	
		[Tooltip("The default volume of the sound. 0 to 1")]
		public float defaultVolume = 1;

		// The index of the current value of the sound
		internal float currentState;
	
		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// Awake is used to initialize any variables or game state before the game starts. Awake is called only once during the 
		/// lifetime of the script instance. Awake is called after all objects are initialized so you can safely speak to other 
		/// objects or query them using eg. GameObject.FindWithTag. Each GameObject's Awake is called in a random order between objects. 
		/// Because of this, you should use Awake to set up references between scripts, and use Start to pass any information back and forth. 
		/// Awake is always called before any Start functions. This allows you to order initialization of scripts. Awake can not act as a coroutine.
		/// </summary>
		void Awake()
		{
			if ( !soundObject && soundObjectTag != string.Empty )    soundObject = GameObject.FindGameObjectWithTag(soundObjectTag).transform;

			// Get the current state of the sound from PlayerPrefs
			if( soundObject )
				currentState = PlayerPrefs.GetFloat(playerPref, soundObject.GetComponent<AudioSource>().volume);
			else   
				currentState = PlayerPrefs.GetFloat(playerPref, defaultVolume);
		
			// Set the sound in the sound source
			SetSound();
		}

		void OnMouseDown()
		{
			ToggleSound();
		}
	
		/// <summary>
		/// Sets the sound volume
		/// </summary>
		void SetSound()
		{
			if ( !soundObject && soundObjectTag != string.Empty )    soundObject = GameObject.FindGameObjectWithTag(soundObjectTag).transform;

			// Set the sound in the PlayerPrefs
			PlayerPrefs.SetFloat(playerPref, currentState);

			Color newColor = GetComponent<SpriteRenderer>().color;

			// Update the graphics of the button image to fit the sound state
			if( currentState == defaultVolume )
				newColor.a = 1;
			else
				newColor.a = 0.5f;

			GetComponent<SpriteRenderer>().color = newColor;

			// Set the value of the sound state to the source object
			if( soundObject ) 
				soundObject.GetComponent<AudioSource>().volume = currentState;
		}
	
		/// <summary>
		/// Toggle the sound. Cycle through all sound modes and set the volume and icon accordingly
		/// </summary>
		void ToggleSound()
		{
			if ( currentState == defaultVolume )    currentState = 0;
			else    currentState = defaultVolume;
		
			SetSound();
		}
	
		/// <summary>
		/// Starts the sound source.
		/// </summary>
		void StartSound()
		{
			if( soundObject )
				soundObject.GetComponent<AudioSource>().Play();
		}
	}
}











