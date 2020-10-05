using UnityEngine;

/// <summary>
/// A Simple Object Follow Class.  In this game use this class to make the TrailRenderer Follow the "Ball Slicer".  This script is 
/// independent of Time.timeScale.  It can be used to allow this follow script to work even if Time.timeScale is 0f.  Of course 
/// the object that it follows would have to have the same ability, otherwise if Time.timeScale was set to zero, then it would not
/// move, and not need to be followed :).  This really does not have a purpose in the OSG_Template... I had added it to combat 
/// another issue, but when that did not go as planned I just left the ability to use as such.
/// </summary>
public class ObjectFollow : MonoBehaviour
{
    public Transform target;            // The position that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.
    public bool ignoreGameTime;         // should we be able to follow the target if Time.timeScale = 0f? if so... ignore Game Time.
    Vector3 offset;                     // The initial offset from the target.
    float lastRealTime;                 // The variable we store the difference between realTimeSinceStartup in.  We compare it to it's
                                        //    Continued.... it's own value from the frame before to create our own deltaTime.

    // Use this for initialization
    void Start()
    {
        //get realTime since startup
        lastRealTime = Time.realtimeSinceStartup;

        // Calculate the initial position offset.
        offset = transform.position - target.position;
    }

    // This function is called every fixed frame-rate frame, if the MonoBehaviour is enabled
    void FixedUpdate()
    {
        //every fixed update we give our float variable "deltaTime" the actual value of Time.deltaTime
        //so that we can just use this variable no matter what (ie. whether we go time independent or not)
        //since we give it the legitimate value of Time.deltaTime every FIXED Update it is the same.
        float deltaTime = Time.deltaTime;
        //if this boolean was set to true in the inspector...
        if (ignoreGameTime)
        {
            //if ignoreGameTime was checked in the inspector then we want to control the value/variable "deltaTime".
            //We subtract our lastRealTime variable(that we stored in start) from Time.realtimeSinceStartup...
            deltaTime = (Time.realtimeSinceStartup - lastRealTime);

            //then right afterwards we update lastRealTime with the current Time.realtimeSinceStartup..
            //this effectively makes it so that there is a small difference between the Time.realtimeSinceStartup and
            //our "lastRealTime".  We subtract the last frames Time.realtimeSinceStartup from the Actual Time.realtimeSinceStartup;
            //this gives us the deltaTime, but without us having to use Time.deltaTime.  So now we can multiply movement by our
            //"deltaTime".
            lastRealTime = Time.realtimeSinceStartup;
        }
        // Create a position the camera is aiming for based on the offset from the target.
        Vector3 targetCamPos = target.position + offset;

        // Smoothly interpolate between the camera's current position and it's target position...
        //using the amount of "smoothing" we want, and multiplied by deltaTime. Depending whether or 
        //not the ignoreGameTime boolean was checked "deltaTime" will either be the real Time.deltaTime
        //or it'll be our calculation of the time between frames that we deduced from Time.realtimeSinceStartup
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * deltaTime);
    }


}
