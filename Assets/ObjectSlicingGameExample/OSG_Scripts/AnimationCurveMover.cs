using UnityEngine;
using System.Collections;

/// <summary>
/// This class is a simple class that allows you to pick/adjust a unity curve to make an object (typically a ui element) slide on to
/// the screen.  There are 3 of these that I use to Scale,Rotate, and Move things.  They allow me to tween ui elements without a large
/// tween library being imported/used.  Though the 3 classes usually are not in every project.  ColorSwitchClone uses the AnimationCurveScaler.
/// </summary>
public class AnimationCurveMover : MonoBehaviour
{
    public bool showDebugInfo;                                          // boolean - should the debug info be shown? def not in production/performance testing.
    public bool quickSlideOnEnable = true;                              // boolean - do we want the object to start its slide right when the game starts?
    public bool destroyAfterMove;                                       // boolean - should we destroy this move component after the move?
    [Range(0, 1)]               //clamp range (slider in inspector)
    public float moveSpeed = 1;                                         // how fast the object should move (note.. the further away the object is the faster it will move)
    public AnimationCurve aCurve;                                       // reference to our animation curve...*** Make sure you pick/create a curve in the inspector or this component will not work.
    private Transform _transform;                                       // cache a ref to our transform
    private Vector3 objStartPosition;                                   // the position the object should start at.
    [Range(0, 5)]               //clamp range (slider in inspector)
    public float moveDelayTime = 0;                                   // the delay before any movement.. yes? enter a float for the wait in seconds.
    private float step;                                                 // step is our incremented variable.  step gets assigned our moveSpeed * Time.deltaTime.
    public Vector3 slideFromDistance = new Vector3(-200, 0, 0);         // the distance or location that the object should slide from.
    private bool isReadyAfterDelay = false;                             // this boolean gets set active when the delay has finished and the slide should start(if a delay was used).

    //A interesting 0-1 animationCurve recommended;
    //or whatever looks nice for what you are going for.

    //Can use this to make ui elements or other gameobjects
    // "slide" in from off screen.  Whatever the objects editor
    // position is at game start will be where it moves to.  The
    //public vector3 is where it will move from (further would make
    // is faster b/c it has a greater distance to travel in the same
    // amount of time)

    // Use this for initialization
    void Start()
    {
        //cache our transform component on start
        _transform = GetComponent<Transform>();
        //set our objStartPosition to our current position... The position we are moving to (because we start the game with it where it should move to).
        //Then just manually enter where it should come from... 
        //I.e. in unity world space if OBJ starts at 0,0,0 and we set slideFromDistance to 500 on the y, 
        //and 500 on the x, then at the start it would slide diagonally from off-screen from the top right corner of the view-port))
        objStartPosition = _transform.position;
        //if we set it to quickSlideOnEnable then...
        if (quickSlideOnEnable)
        {
            //move it via "transform.translate" to the position "slideFromDistance"
            _transform.Translate(slideFromDistance);
        }
        //if NOT quickSlideOnEnable then we Invoke the slide method after moveDelayTime.
        Invoke("Delay", moveDelayTime);
        //if we wanted to show the DebugInfo then if the boolean is checked in the inspector then the following will happen...
        if (showDebugInfo)
        {
            //commented out all debugs for release.
            //Debug.Lot all the position data
            Debug.Log("Start Position " + objStartPosition.ToString());
            Debug.Log("Slide From Position " + slideFromDistance.ToString());
            //invoke repeating a method that will give us some debug info in .1f and every quarter second after.  ***Note never use that/tick the option
            //for a release/performance testing build... it'll kill the performance.
            InvokeRepeating("GetDebugInfo", 0.1f, 0.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if isReadAfterDelay is true, then we start moving the object.
        if (isReadyAfterDelay == true)
        {
            //linearly interpolate the position from current "_transform.position" to our "objStartPosition" by our curve(using Curve.Evaluate(time)
            _transform.position = Vector3.Lerp(_transform.position, objStartPosition, aCurve.Evaluate(step));
            //increase our step by moveSpeed var times Time.deltaTime.
            step += moveSpeed * Time.deltaTime;
            //if we want this AnimationCurveMover component destroyed when the move is over set to true.
            if (destroyAfterMove)
            {
                //if it was set to true & next if the "step" is greater to / equal to 1, then move is done.
                if (step >= 1)
                {
                    //if we wanted to be notified via the console when it was done moving then "showDebugInfo" would have to be ticked in the inspector.
                    if (showDebugInfo)
                    {
                        //debugs commented out for release.
                        Debug.Log("Destroying/Removing Script Component");
                    }
                    //destroy after move was selected, and we are done, so destroy this component.
                    Destroy(this);
                }
            }
            //else if we are NOT destroying after move (we can reset it and it will loop through the above move again... never really use it in this way, but it is an
            //option..
            else
            {
                //if move is finished(i.e. step is greater/equal to 1)
                if (step >= 1)
                {
                    //set step back to zero
                    step = 0;
                    //transform.translate the object back to the "slideFromDistance"(position)
                    _transform.Translate(slideFromDistance);
                    //repeat
                }
            }
        }
    }

    /// <summary>
    /// This Method is used if a "delay" was selected before the Move happens.  Having a number of these setup on different
    /// objects can allow a chain of events.. I.e. a blank canvas with a border/background slides up, then the first option slides 
    /// in from the left, then the second from the left, etc... they could all just have a little longer of a delay.
    /// </summary>
    private void Delay()
    {
        //once the Invoke of this method is complete we just set the isReadyAfterDelay Boolean to true.
        isReadyAfterDelay = true;
    }
    /// <summary>
    /// This Method just calls Debug.Log and formats some of the position/time data.  If the boolean showDebugInfo is true
    /// then this Method is called every quarter second.. giving us necessary data. You can step through/break with these and
    /// accurately place chains of moving,rotating,scaling object.
    /// </summary>
    private void GetDebugInfo()
    {
        //if this was set to true (necessary to be called... just redundant.)
        if (showDebugInfo)
        {
            Debug.Log("Current Step Progress " + step.ToString() + " " + "ObjPosition " + _transform.position.ToString());
        }
    }

    // This function is called when the behaviour becomes disabled or inactive
    public void OnDisable()
    {
        //on disable cancel all invokes... obviously :)
        CancelInvoke();
    }
}

