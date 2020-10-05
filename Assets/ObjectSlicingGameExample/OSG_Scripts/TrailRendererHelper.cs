using UnityEngine;
using System.Collections;

/// <summary>
/// The TrailRendererHelper is for when you pool trail renderers, or gameObjects that have trail renderers.  If you have ever
/// seen a projectile with a trail renderer being re-used if often shows a trail coming back to the gun-barrel, as if it had shot
/// properly(trail and all), but then when you went to re-use it and instantiate it back at the gun barrels position it would 
/// create a trail from it's old used impact point TO the guns barrel, and then not always work right for that fire.  There are
/// several fixes on the forums for this issue, but I favor this one.  If i am using trail renderers at all, I automatically add
/// a trail renderer helper... whether the issue would exist or not.
/// </summary>
public class TrailRendererHelper : MonoBehaviour
{
    protected TrailRenderer mTrail;     // reference to this trail renderer.
    protected float mTime = 0;          // a variable to be used with the trail renderers time property.

    // Use this for pre-initialization
    void Awake()
    {
        //cache the trail renderer reference with get component...
        mTrail = gameObject.GetComponent<TrailRenderer>();
        //IF our trail renderer is null....
        if (mTrail == null)
        {
            //debug.log because we aren't on a trail renderer or something else weird is going on.
            Debug.Log("[TrailRendererHelper.Awake] invalid TrailRenderer.");
            //return... there is a problem.
            return;
        }
        //our mTime var is assigned our trail renderers time field (in awake so the initial setting).
        mTime = mTrail.time;
    }

    // This function is called when the object becomes enabled and active
    void OnEnable()
    {
        //again.... if the trail renderer is null then...
        if (mTrail == null)
        {
            //return because there is an issue/problem or this isn't really a trail renderer.
            return;
        }
        //StartCoroutine... ResetTrails()
        StartCoroutine(ResetTrails());
    }

    /// <summary>
    /// this Method simply changes the trail renderers time to zero.  then it waits until the end of frame, and it changes the 
    /// trail renderers time back to the time it was during Awake()...
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetTrails()
    {
        //set trail renderer time to zero
        mTrail.time = 0;
        //wait until the end of the frame..
        yield return new WaitForEndOfFrame();
        //set trail renderer time to the Awake Time, so now each time the trail renderer is enabled(if it were on a pooled bullet or rocket, 
        //it would change the move time to Zero so nothing shows up (like back to the gun trails), and then at end of frame it gives it back its 
        //original time for use
        mTrail.time = mTime;
    }
}
