using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This Class sets up a way to reference the FaderCanvas and the RawImage which is a child of the FaderCanvas.  This Class is attached to
/// the RawImage Child.  This reference is used to access the Raw Image in case the reference is lost... that way we do not have any issues.
/// </summary>
public class FaderReferenceSetup : MonoBehaviour 
{
    public RawImage faderRawImage;                              // the reference to our RawImage that is a raw image component without a texture (just solid black)
    public GameObject parentCanvas;                             // reference to the parent canvas (the parent of the RawImage).
    public static FaderReferenceSetup ourFaderReference;        // the static reference to this class.  So we can access this from anywhere.
    public static bool applicationIsQuiting;                    // another static variable which becomes "true" if the application is closing.

    // Use this for pre-initialization
    void Awake()
    {
        //Assign the Parent GameObject to the parentCanvas var.
        parentCanvas = this.transform.root.gameObject/*this.transform.parent.gameObject*/;
        //make sure that it persists through scene load using DontDestroyOnLoad
        DontDestroyOnLoad(parentCanvas);

        //Make this instance the only one, or destroy it if one exists.
        if (ourFaderReference == null)
        {
            ourFaderReference = this;
        }
        else if (ourFaderReference != this)
        {
            Destroy(gameObject);
        }
        //the RawImage this script is attached to is assigned to the faderRawImage var
        faderRawImage = this.GetComponent<RawImage>();

    }

    //this was setup when there was a issue of Unity destroying the RawImage/Canvas before Destroying the ScreenFaderSingleton OnApplicationQuit
    //(During editor testing). This is in place so that OnDestroy(of the Image or Canvas), I can set applicationIsQuitting to true.  
    //So that when the fader is using CrossFadeAlpha to fade the screen, it doesn't get stuck in a situation where it is trying to CrossFade the alpha of
    //this RawImage after/while its being destroyed, and before the ScreenFaderSingleton is Destroyed.  That would result in a NullReferenceException.
    void OnDestroy()
    {
        applicationIsQuiting = true;
    }

}
