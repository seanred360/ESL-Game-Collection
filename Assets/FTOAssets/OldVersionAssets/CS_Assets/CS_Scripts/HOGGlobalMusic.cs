using UnityEngine;
using System.Collections;

namespace HiddenObjectGame
{
	/// <summary>
	/// handles a global music source which carries over from scene to scene without resetting the music track.
	/// You can have this script attached to a music object and include that object in each scene, and the script will keep
	/// only the oldest music source in the scene.
	/// </summary>
	public class HOGGlobalMusic:MonoBehaviour 
	{
		//The tag of the music source
		public string musicTag = "Music";
		
		//The time this instance of the music source has been in the game
		internal float instanceTime = 0;

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// Awake is used to initialize any variables or game state before the game starts. Awake is called only once during the 
		/// lifetime of the script instance. Awake is called after all objects are initialized so you can safely speak to other 
		/// objects or query them using eg. GameObject.FindWithTag. Each GameObject's Awake is called in a random order between objects. 
		/// Because of this, you should use Awake to set up references between scripts, and use Start to pass any information back and forth. 
		/// Awake is always called before any Start functions. This allows you to order initialization of scripts. Awake can not act as a coroutine.
		/// </summary>
		void  Awake()
		{
			//Find all the music objects in the scene
			GameObject[] musicObjects = GameObject.FindGameObjectsWithTag(musicTag);
            
			//Keep only the music object which has been in the game for more than 0 seconds
			if ( musicObjects.Length > 1 )
			{
				foreach( var musicObject in musicObjects )
				{
					if ( musicObject.GetComponent<HOGGlobalMusic>().instanceTime <= 0 )    Destroy(gameObject);
				}
			}
		}

		void Start()
		{
            //Don't destroy this object when loading a new scene
            DontDestroyOnLoad(transform.gameObject);
        }
	}
}
