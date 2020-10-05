using UnityEngine;
using System.Collections;

/// <summary>
/// Simple Class attached to the MainCamera that Calls a Method on the ScreenFaderSingleton, which "Instantiates" the ScreenFaderSingleton.  This 
/// Class has no other purpose... It did have some other logic/Jobs initially, but it has since been simplified. We could really do this on any script.
/// It just so happened that I used to use this script for other functionality... so in the end as functionality was moved to other classes, It was 
/// left behind only calling the method and instantiating the ScreenFaderSingleton.  This class is attached the all scene cameras... so that the game
/// can be started in all scenes (not just the first scene (Scene0 - Splash Screen)).. *** Note:  The game is intended to be started at Scene 0...
/// BUT in the case of testing/using the editor... this being on every camera will prevent Reference Exceptions, that wouldn't exist otherwise.
/// </summary>
public class FaderCaller : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
        //Call the DebugSpawn()... that will create the ScreenFaderSingleton gameObject...
        ScreenFaderSingleton.Instance.DebugSpawn();
    }

}
