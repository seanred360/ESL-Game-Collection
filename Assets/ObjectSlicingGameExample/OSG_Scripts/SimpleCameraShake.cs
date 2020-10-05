using UnityEngine;
using System.Collections;

/// <summary>
/// This Class "shakes" the camera's transform.position in a coroutine, to give a nice jiggly effect. :)
/// </summary>
public class SimpleCameraShake : MonoBehaviour {

    public float shakeDuration = 0.5f;                      //float (0.5f is pretty decent) how long to shake.
    public float shakeMagnitude = 0.1f;                     //float (0.1f is pretty decent) how hard to shake.
    private Transform shakeCam;                             //Transform reference to the camera that will shake. Will setup itself - private
    private Vector3 startPos = new Vector3(0,0,0);          //Vector3 our camera's start position... will setup itself - private.
    //public string shakeCameraTag;                         //LeftOvers from old version... no longer necessary but may be again someday.

    // Use this for initialization
    void Start () 
	{
        //we store the cameras transform component in the "shakeCam" var.
        shakeCam = GetComponent<Transform>();
        //we store the camera's starting position in the "startPos" var.
        startPos = shakeCam.position;
    }

    /// <summary>
    /// This is the Method we call to start the shake effect.  For good measure it makes sure that the coroutine is not
    /// running(probably unnecessary), and the Starts the Shake Coroutine... I just find it easier to use StartShake();
    /// </summary>
    public void StartShake()
    {
        //stop all coroutines... for good measure.  No reason.
        StopAllCoroutines();
        //Start our Shake Coroutine!
        StartCoroutine("Shake");
    }

    /// <summary>
    /// This IEnumerator controls the shake.  Whatever values we set for the shakeDuration, and shakeMagnitude determine
    /// the severity of the shake.
    /// </summary>
    /// <returns></returns>
    IEnumerator Shake()
    {
        //create a float (elapsedTime) and set it to zero;
        float elapsedTime = 0.0f;

        //while our elapsedTime variable is less than "shakeDuration" then we Shake...
        while (elapsedTime < shakeDuration)
        {
            // elapsedTime plus gets Time.deltaTime (we will do this each pass until the above conditional
            // is Not true (elapsedTime < shakeDuration).
            elapsedTime += Time.deltaTime;

            //create a float called percent complete and give it the quotient of elapsedTime / shakeDuration... this will
            //give us the current progress/percent complete of the shake effect.
            float percentComplete = elapsedTime / shakeDuration;


            // long explanation of line Number 93 "float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);" if needed :)
            #region
            //We create a float to be used later to do some dampening to the shakeMagnitude.  Using this method the damper
            //value will always be between 0 and 1(clamped to the 0 - 1 range).  If you look at the formula just below... 
            //Mathf.Clamp(value,min,max) I'll try to explain what is happening.  Basically it is dampened in the since that
            //from 0% - 75% percentComplete noting changes, and then from 75% - 100% we move from 1f to 0f (at 0.04f a frame).

            //So... damper = 1f - Mathf.Clamp(4f * percentComplete - 3f,  *** lets forget the ( 0.0f Min ,
            // and 1.0f Max ) for now.  We are left with this...

            // "damper"  =  1f - (4f * percentComplete - 3f)...

            // So... when elapsedTime is created at 0f, the first few increases will only be the small amounts (the delta between frames).
            //Time.deltaTime is small (i.e. 0.01659) definitely small... lets say after 3 frames elapsedTime is about 0.05 (0.04977).
            // Now that elapsedTime is basically 0.05 lets try this out... 

            //damper starts at 1f...  then we subtract (4f * percentComplete - 3f).

            //"percentComplete" right now is 0.05f / 0.5f = 0.1f;  We are 10% complete in our shake.  This equates to (4f * .1f - 3f) = -2.6f. 
            //Since we are clamped to values between 0 and 1 the result is 0.

            //So again, back to damper... the new float "damper" gets 1f minus 0f(-2.6f clamped between 0f and 1f equals 0f;
            //So as of right now... nothing changes.
            //"damper" still equals 1 at this point.  "damper" will equal one right up until the final stretch... when we approach 75% completion
            //the value that is subtracted from "damper's 1f" will be 0f(actually 0f).  Once we get to .76 percentComplete the calculation will be
            //   damper = 1f - 0.04 = 0.96f; ... now it will start decreasing damper down to 0f over the final frames.

            // From this "76%" "percentComplete" forward it will quickly get higher and higher until at 100% when...
            //damper = 1f - "1f"; and so then finally at the end... dampers result will be 0f.  Anything times 0, is 0;  
            //That is how the shakeMagnitude ends below.. when the effect is over.  This is how "damper" is dampened.

            //***summarized***
            //"the "damper" makes nothing happen for the first 75%, then the last 25% everything starts
            //to change and the "damper" variable equals 0.  See lines 112 and 115 for damper's use(line 118 (z axis is commented out)).
            #endregion
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            //region contains explanation of "float x = Random.value * 2.0f - 1.0f;" if needed :)
            #region
            //  ^   ^   ^   ^   ^   ^   ^   ^
            //the above method allows us to generate a random value between -1 and 1 using a little extra arithmetic...
            //because we are using Random.value and then multiply by 2 and subtract 1.  2 examples....

            //1.) Random.value returns 0.44 * 2 - 1 which equals -0.12
            //2.) Random.value returns 0.54 * 2 - 1 which equals  0.08

            // in this way when Random.value returns values from 0.50 to 1 we get positive values from 0 to 1 in .02 increments(good enough)
            // and when we get values from 0.00 to 0.49 we get values from -0.02 to -1 in .02 increments(GOOD enough).

            // The same method is used for the z or y value below (depends on the orientation of the game/camera.
            #endregion

            //use either z or y... for this game its "Y" (ObjectSlicingGame)
            // map value to [-1, 1]
            float y = Random.value * 2.0f - 1.0f;

            // x * x * shakeMagnitude * damper
            x *= shakeMagnitude * damper;

            // y * y * shakeMagnitude * damper
            y *= shakeMagnitude * damper;

            // z * z * shakeMagnitude * damper
            //z *= shakeMagnitude * damper;

            //now we actually move the camera, shakeCam.position = new Vector3(0,0,0) and we use the x,y,z values to do position.x + x or position.y + y, respectfully
            shakeCam.position = new Vector3(shakeCam.position.x + x, shakeCam.position.y + y, shakeCam.position.z);

            //we are done, we yield return null;
            yield return null;
        }

        //when we are done looping through "while elapsedTime < shakeDuration... we need to set the cameras position back to the start position....
        //otherwise each time shake is called it could actually start wiggling itself off the screen.  Slowly, but surely.  Slim chances the ending
        //shake pos would leave you in the same spot.
        shakeCam.position = startPos;
    }
}
