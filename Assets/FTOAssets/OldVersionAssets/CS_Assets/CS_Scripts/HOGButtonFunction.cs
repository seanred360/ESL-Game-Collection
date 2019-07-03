using UnityEngine;
using System.Collections;

namespace HiddenObjectGame
{
	/// <summary>
	/// This script runs a function in a target object when clicked on. In order to detect clicks you need to attach a collider to this object.
	/// </summary>
	public class HOGButtonFunction:MonoBehaviour 
	{
		private Transform thisTransform;
		
		//The target object in which the function needs to be executed
		public Transform functionTarget;
		
		//The name of the function that will be executed
		public string functionName;
		
		//A delay before running the function
		public float functionDelay = 0;
		
		//The sound of the click and the source of the sound
		public AudioClip soundClick;
		public Transform soundSource;
		
		//Should there be a click effect
		public bool clickEffect = true;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start() 
		{
			thisTransform = transform;
		}

		/// <summary>
		/// Raises the mouse down event.
		/// </summary>
		IEnumerator OnMouseDown()
		{
			//Create an effect
			if ( clickEffect == true )    ClickEffect();
			
			//Play a sound from the source
			if ( soundSource )    if ( soundSource.GetComponent<AudioSource>() )    soundSource.GetComponent<AudioSource>().PlayOneShot(soundClick);
			
			//Wait a while
			yield return new WaitForSeconds(functionDelay);

			//Run the function at the target object
			if ( functionName != string.Empty )
			{  
				if ( functionTarget )    
				{
					functionTarget.SendMessage(functionName);
				}
			}
		}
		
		/// <summary>
		/// Create an effect, making the object large and then gradually smaller
		/// </summary>
		IEnumerator ClickEffect()
		{
			//Register the original size of the object
			var initScale = thisTransform.localScale;
			
			//Resize it to be larger
			thisTransform.localScale = initScale * 1.1f;
			
			//Gradually reduce its size back to the original size
			while ( thisTransform.localScale.x > initScale.x * 1.01f )
			{
				yield return new WaitForFixedUpdate();

				thisTransform.localScale = new Vector3( thisTransform.localScale.x - 1 * Time.deltaTime, thisTransform.localScale.x - 1 * Time.deltaTime, thisTransform.localScale.z);
			}
			
			//Reset the size to the original
			thisTransform.localScale = thisTransform.localScale = initScale;
		}	
	}
}








