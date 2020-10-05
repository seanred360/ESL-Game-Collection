using UnityEngine;
using System.Collections;

/// <summary>
/// This class was created to determine whether or not a shot 3 second tutorial canvas should be shown or not.  Once it has been displayed,
/// it will not show again.  As of this moment I feel like this is probably sufficient.  Most people understand how to play simple swipe 
/// games like this.  Maybe we could just create a button that re-enables it, but that is up to you!
/// </summary>
public class DeactivateTutorial : MonoBehaviour
{
    public GameObject tutorialCavas;        //reference to the tutorialCanvas.
    public float destroyTime;               //how long we should wait before destroying it.

	// Use this for initialization
	void Start ()
    {
        //if this key(showedTut) exists, and is equal to "1", then...
        if (PlayerPrefs.GetInt("showedTut") == 1)
        {
            //we deactivate the canvas.  It has been shown already....
            DeactivateTutorialCanvas(0);
        }
        //else we...
        else
        {
            //deactivate the tutorialCanvas in "destroyTime" number of seconds. (this is the amount of time it'd the canvas would be shown)
            DeactivateTutorialCanvas(destroyTime);
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        //if any key presses are detected it also destroys the tutorialCanvas.
        if (Input.anyKey)
        {
            //call our method that destroys the canvas (and pass 0, so that it happens pretty quickly)...
            DeactivateTutorialCanvas(0.5f);
        }
	
        
	}

    /// <summary>
    /// This is the method that Destroys the tutorialCanvas... It takes a float as an argument to be used as the destroy delay.
    /// </summary>
    /// <param name="time"></param>
    void DeactivateTutorialCanvas(float time)
    {
        //set this key in playerprefs, so that we know that the tutorial has already been showed to the player.
        PlayerPrefs.SetInt("showedTut", 1);
        //and destroy the canvas, in time.
        Destroy(tutorialCavas, time);
    }
}
