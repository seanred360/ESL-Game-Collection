using UnityEngine;
using System.Collections;

namespace HiddenObjectGame
{
	/// <summary>
	/// This script shakes an object when it runs, with values for strength and time. You can set which object to shake, and if you keep the object value empty it 
	/// will shake the object it's attached to.	
	/// </summary>
	public class HOGShakeTarget:MonoBehaviour 
	{
		//Caching the transform for quicker access
		private Transform thisTransform;
		
		//The original position of the camera
		private Vector3 originPos;
		
		//How violently to shake the camera
		public Vector3 strength;
		private Vector3 strengthDefault;
		
		//How quickly to settle down from shaking
		public float decay = 0.8f;
		
		//How many seconds to shake
		public float shakeTime = 1;
		private float shakeTimeDefault;
		
		// The object that is shaken
		public Transform shakeTarget;
		
		//Is this effect playing now?
		public bool isShaking = false;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start() 
		{
			thisTransform = transform; //Caching transform for quicker access
			
			strengthDefault = strength;
			
			shakeTimeDefault = shakeTime;
			
			//Record the original position of the camera
			if ( !shakeTarget )    shakeTarget = thisTransform;
			
			originPos = shakeTarget.transform.position;
		}
		
		// Update is called once per frame
		void Update() 
		{
			if ( isShaking == true )
			{
				if ( shakeTime > 0 )
				{		
					shakeTime -= Time.deltaTime;
					
					//Move the camera in all directions based on strength
					shakeTarget.position = new Vector3( originPos.x + Random.Range(-strength.x, strength.x), originPos.y + Random.Range(-strength.y, strength.y), originPos.z + Random.Range(-strength.z, strength.z));
					
					//Gradually reduce the strength value
					strength *= decay;
				}
				else if ( shakeTarget.position != originPos )
				{
					shakeTime = 0;
					
					//Reset the camera position
					shakeTarget.position = originPos;
					
					isShaking = false;
				}
			}
		}

		void StartShake()
		{
			isShaking = true;
			
			strength = strengthDefault;
			
			shakeTime = shakeTimeDefault;
		}
	}
}







