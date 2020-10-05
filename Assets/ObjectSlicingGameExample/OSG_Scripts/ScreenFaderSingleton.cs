using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

/// <summary>
/// This Class is Derived from our Singleton Base Class, and it is accessed via static
/// Reference.  Anytime you want to temporarily Fade the Screen, or Load/Restart a Scene, 
/// there is a Public Method to do so.  It also monitors Input for Esc/M(WebGL/StandAlone),
/// Back/Menu keys(Android), so that when back is pressed anywhere it will load the previous
/// Scene(using the fader), and then Exit if on Main Menu(still using the fader).
/// </summary>
public class ScreenFaderSingleton : Singleton<ScreenFaderSingleton>
{

    protected ScreenFaderSingleton() { }
    public RawImage screenFadeTexture;          //our raw image that is black and in front of everything (blocks that cam view)
    public float fadeInDuration = 0.85f;        //how long the fader takes to fade in
    public float fadeOutDuration = 0.95f;       //how long the fader takes to fade out
    private float fadeInAlpha = 0;              //what should the alpha be when we have faded in
    private float fadeOutAlpha = 1;             //what should the alpha be when we have faded in
    public GameObject faderPrefab;              //the reference to the fader prefab in the resources folder.


    // Use this for pre-initialization
    void Awake()
    {
        //Check to see if our Fader Reference is null. is FaderReferenceSetup.Instance(ouFaderReference) null or is there one somewhere
        //in the scene? If there is, We Fade its alpha in and out to create the "fade" effect.  That FaderCanvas and RawImage Child will
        //be set to DontDestroy and Will follow us through all the scenes.
        if (FaderReferenceSetup.ourFaderReference != null)
        {
            screenFadeTexture = FaderReferenceSetup.ourFaderReference.faderRawImage;
        }


        //else if one is NOT found anywhere in the scene, then we need to Create a Canvas Object.
        //scenes 1 and 2 do not have these FaderCanvas GameObjects by default, because it is assumed 
        //that one was brought from scene0, and games usually start at the beginning.
        else
        {
            //if inside the editor and you start the game in scene1 or scene2, we reference a GO Var from our static class
            faderPrefab = GameVariables.screenFaderPrefab;

            //Then we use that prefab reference to instantiate one in the scene
            Instantiate(faderPrefab, transform.position, transform.rotation);

            //NOTE: REMEMBER...
            //If the game is started at Scene0 then there is not an issue.  The FaderCanvas GameObject is in that scene
            //and is set to DoNotDestroyOnLoad.  So it travels with us as be changes scenes.  Because You may want to jump
            //into any scene in the editor, and so we don't have to have the FaderCanvas GO in the Scene1/2, We will just
            //instantiate the ScreenFader Canvas GameObject, by referencing the static "GameVariables.screenFaderPrefab" 
            //because it points to the one prefab that is actually in the Resources folder(and it is there for this reason.)

            //Debugs commented out(like everywhere else in release version)
            //Debug.Log("Game was not started from Scene0. So the FaderCanvas Gameobject was created \n from a reference to its prefab inside the Resources folder...(MSG:1of3)" );
            //Debug.Log("this should not be an issue in the build because usually a game starts at scene0 \n to avoid this issue, always start the game...(MSG:2of3)");
            //Debug.Log(" from scene0 so that the fader is created and persists with the other persistent \n objects.(MSG:3of3)");

            //Now we continue...  We Instantiated the FaderCanvas and now we are pointed to the RawImage Child GO.
            screenFadeTexture = FaderReferenceSetup.ourFaderReference.faderRawImage;

        }

    }

    // Use this for initialization
    void Start()
    {
        //if this ScreenFader was instanced on the "splash scene(scene 0)" then
        //fade and load level.  See FadeAndLoadLevel() Summary for more info.
#if UNITY_5_3_OR_NEWER
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            FadeAndLoadLevel();
        }
#else
        if(Application.loadedLevel == 0)
        {
            FadeAndLoadLevel();
        }
#endif

    }



    /// <summary>
    /// Delayed FadeOut by 1.1Seconds(I would recommend changing the fade time by sending a parameter to
    /// this method), but for my purposes while hand tuning I just used hard-coded numbers. 
    /// 
    /// In the future it will probably be a Fade() Method ==> Fade(boolean fadeType,float time); 
    ///     //to fade both ways by time
    /// </summary>
    public void DelayedFadeOut()
    {
        Invoke("FadeOut", 1.1f);
    }

    /// <summary>
    /// Method that Invokes FadeOut, Then LoadLevels, Then FadeIn, all 1 second after the other.
    /// </summary>
    public void FadeAndLoadLevel()
    {
        Invoke("FadeOut", 2f);
        Invoke("LoadLevel", 3f);
        Invoke("FadeIn", 4f);
    }


    /// <summary>
    /// This Method Invokes the screenFader immediately, and the calls the
    /// LoadLevel Method(which checks the current scene and loads the next).
    /// Then it Invokes the screenFader again after 3seconds to fade screen
    /// back to starting transparency.
    /// </summary>
    public void FadeAndLoadLevelFaster()
    {
        Invoke("FadeOut", 0f);
        Invoke("LoadLevel", 1f);
        Invoke("FadeIn", 3f);
    }


    /// <summary>
    /// Method that is a modified version of the original FadeAndLoadLevel(above), that activates the fader
    /// in the same way, but in the middle starts a simple coroutine that way A parameter can be sent.  This avoids
    /// using unity invoke(no parameters), nor .net MethodBase.Invoke.  We pass the LoadLevel IEnumerator a wait time
    /// (formerly invoke wait time, and a integer (level to load), and then continue as usual.  This was the minimal 
    /// amount of work to amend the SingletonScreenFader for use in Object Slicing Game, because there are several additional
    /// scenes.
    /// </summary>
    public void FadeAndLoadSpecificLevel(string level)
    {
        Invoke("FadeOut", 1f);
        StartCoroutine(LoadSpecificLevel(2.2f, level));
        Invoke("FadeIn", 2.75f);
    }


    /// <summary>
    /// Method that is a modified version of the new "FadeAndLoadSpecificLevel(int level)" that just happens faster...
    /// it immediately fades, and then after fade is complete it starts the load level coroutine and then fades back in.
    /// this is the second of the 2 additional methods that were added to this class from the previous template release.
    /// There were some other modifications made, but those changes are noted in the location of the changes.
    /// </summary>
    public void FadeAndLoadSpecificLevelFaster(string level)
    {
        Invoke("FadeOut", 0f);
        StartCoroutine(LoadSpecificLevel(1f, level));
        Invoke("FadeIn", 2f);
    }



    /// <summary>
    /// This Method Invokes the screenFader immediately, and then calls ZeroOutPlayerScore, and then
    /// ReloadLevel Method, then FadeIn.
    /// </summary>
    public void FadeAndReloadLevel()
    {
        Invoke("FadeOut", 0f);
        Invoke("ZeroOutPlayerScore", 1.15f);
        Invoke("ReloadCurrentLevel", 1.25f);
        Invoke("FadeIn", 2.25f);
    }

    /// <summary>
    /// Method the Invokes FadeOut, ZeroOutPlayerScore, ReturnToPreviousLevel, and finally FadeIn.
    /// Note See Individual Methods for Summary.
    /// </summary>
    public void FadeAndLoadPreviousLevel()
    {
        Invoke("FadeOut", 0f);
        Invoke("ZeroOutPlayerScore", 1.15f);
        Invoke("ReturnToPreviousScene", 1.25f);
        Invoke("FadeIn", 2.25f);
    }

    /// <summary>
    /// Simple Method that fades out, and after 1.25 seconds quits the application
    /// </summary>
    public void FadeAndQuitApplication()
    {
        Invoke("FadeOut", 0f);
        Invoke("ExitApplication", 1.25f);
    }

    /// <summary>
    /// Simple Method that Fades Out, calls ReturnToPreviousScene, and then Fades In
    /// </summary>
    public void FadeAndGoBack()
    {
        Invoke("FadeOut", 0f);
        Invoke("ReturnToPreviousScene", 1.25f);
        Invoke("FadeIn", 2f);
    }

    public void FadeAndLoadMainMenu()
    {
        Invoke("FadeOut", 0f);
        Invoke("LoadMainMenu", 1f);
        Invoke("FadeIn", 3f);
    }

    /// <summary>
    /// Simple Method that calls the scenemanager.load scene Method, and sends GetActiveScene().buildIndex
    /// Integer as the parameter.  It will reload the current loaded scene no matter what scene it is called
    /// in.
    /// </summary>
    private void ReloadCurrentLevel()
    {
#if UNITY_5_3_OR_NEWER
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
#else
        Application.LoadLevel(Application.loadedLevel);
#endif
    }

    /// <summary>
    /// Method that calls Application.Quit if the Game is Running.
    /// </summary>
    private void ExitApplication()
    {

        if (Application.isPlaying)
        {
            Application.Quit();
        }


    }

    /// <summary>
    /// This method added to ScreenFaderSingleton for osg.  This will load the Main Menu Scene (1) if you are
    /// in any of the 3 gameMode scenes(i.e. 2,3,4).
    /// </summary>
    private void LoadMainMenu()
    {

#if UNITY_5_3_OR_NEWER
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4)
        {
            SceneManager.LoadScene(1);
        }

#else
        if(Application.loadedLevel == 2 || Application.loadedLevel == 3 || Application.loadedLevel == 4)
        {
            Application.LoadLevel(1);
        }
#endif

    }

    /// <summary>
    /// This is The Method that is Invoked along with the FadeIn/Out Methods in the Majority of the
    /// "Fade...And...." Methods.  It is hard-coded on a per project basis.  I use this Singleton Fader/Scene
    ///  Manager in most of my Projects.  So for ColorSwitchClone there are 3 Scenes. 1=Splash, 2=Menu, 
    ///  3=Game.  The LoadLevel Method just loads Scene1 if currently on Scene0 or loads Scene2 if currently 
    ///  on Scene1.
    /// </summary>
    private void LoadLevel()
    {
#if UNITY_5_3_OR_NEWER
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadScene(2);
        }
#else
        if(Application.loadedLevel == 0)
        {
            Application.LoadLevel(1);
        }
        if(Application.loadedLevel == 1)
        {
            Application.LoadLevel(2);
        }

#endif

    }


    IEnumerator LoadSpecificLevel(float waitTime, string levelToLoad)
    {
        yield return new WaitForSeconds(waitTime);

#if UNITY_5_3_OR_NEWER

        SceneManager.LoadScene(levelToLoad);


#else
        Application.LoadLevel(levelToLoad);

#endif

        yield return null;
    }

    /// <summary>
    /// This is the method that is Invoked in the "Fade...And..." Methods above.  It checks the current Scene, and loads the
    /// scene before it.
    /// </summary>
    private void ReturnToPreviousScene()
    {
#if UNITY_5_3_OR_NEWER
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(1);
        }
#else
        if(Application.loadedLevel == 2)
        {
            Application.LoadLevel(1);
        }
#endif

    }

    /// <summary>
    /// This Method clears the player score.  Note does note access playerPrefs.  We only use this
    /// to take care of the score in the GameVariables.-> "roundTypeScore" variable.  The only time a new
    /// "score" is saved to PlayerPrefs is when the player reaches a new "highScore", so 
    /// ANY other time we just zero it out when scene change/restart happens.
    /// </summary>
    public void ZeroOutPlayerScore()
    {
        //static integer in GameVariables gets Zero;
        //commenting out.. not used in osg GameTemplate
        //GameVariables.playerScore = 0;

        //above was used with ColorSwitchClone Only that which is noted has been changed for use with
        //Object Slicing Game example.  We don't necessarily use just a "playerScore", we use "ballCutIn_X_GameMode"
        //so because of this we will do... 
        GameVariables.RegularModeScore = 0;
        GameVariables.ChillModeScore = 0;

    }



    /// <summary>
    /// Empty Method I used during Initial setup, I usually have the "spawn" debug running, because in
    /// my logs it is the first piece of feedback i get in a scene.  The FadeCaller on the camera calls
    /// this method, because the first time the singleton instance is accessed it is instantiated, and then 
    /// set to DoNotDestroyOnLoad, so this ScreenFaderSingleton is with us from the splash scene load on-ward.
    /// I just comment it out for release testing.
    /// </summary>
    public void DebugSpawn()
    {
        //Debug.Log("spawn");
    }

    /// <summary>
    /// This is the Actual FadeIn() Method.  All other public methods Invoke this at a specific time, 
    /// and then take care of Scene loads/changes while screen is obscured.
    /// it uses Unity's CrossFadeAlpha Method on the OverSized RawImage with a black material that covers 
    /// the Screen View.
    /// </summary>
    private void FadeIn()
    {
        //if we have reference to our RawImage
        if (screenFadeTexture != null)
        {
            //cross-fade alpha for gradual fade in.
            screenFadeTexture.CrossFadeAlpha(fadeInAlpha, fadeInDuration, true);
        }
        else
        {
            //Setup reference again... probably unnecessary.  Left over from before the
            //fader canvas was made to persist between scenes as well as the ScreenFaderSingleton,
            //and the BackGroundMenuAndMusic GameObject.
            screenFadeTexture = FaderReferenceSetup.ourFaderReference.faderRawImage;
            //Because Unity Destroys things without any order we throw in an extra check
            //to make sure we are not trying to cross-fade the alpha if the RawImage is destroyed
            //before this script... Which i did catch happening on play-mode exit a couple times,
            //but this solved it.
            if (!FaderReferenceSetup.applicationIsQuiting)
            {
                //cross-fade alpha for gradual fade in.
                screenFadeTexture.CrossFadeAlpha(fadeInAlpha, fadeInDuration, true);
            }
        }
    }

    /// <summary>
    /// This is the Actual FadeOut() Method.  All other public methods Invoke this at a specific time, 
    /// when they have finished all the Scene Loads/Changes.  It reveals the SceneView.
    /// </summary>
    private void FadeOut()
    {
        //if we have reference to our RawImage
        if (screenFadeTexture != null)
        {
            //cross-fade alpha for gradual fade out.
            screenFadeTexture.CrossFadeAlpha(fadeOutAlpha, fadeOutDuration, true);
        }
        else
        {
            //Setup reference again... probably unnecessary.  Left over from before the
            //fader canvas was made to persist between scenes as well as the ScreenFaderSingleton,
            //and the BackGroundMenuAndMusic GameObject.
            screenFadeTexture = FaderReferenceSetup.ourFaderReference.faderRawImage;
            //Because Unity Destroys things without any order we throw in an extra check
            //to make sure we are not trying to cross-fade alpha if the RawImage is destroyed
            //before this script... which i did catch happening on play-mode exit a couple times,
            //but this solved it.
            if (!FaderReferenceSetup.applicationIsQuiting)
            {
                //cross-fade alpha for gradual fade out.
                screenFadeTexture.CrossFadeAlpha(fadeOutAlpha, fadeOutDuration, true);
            }
        }
    }

    //when this is enabled call FadeIn() (Black Raw Image becomes Visible);
    //now scene is not visible.  now we can FadeOut to show the screen(started black)
    void OnEnable()
    {
        //Call FadeIn() Method;
        FadeIn();
    }

    //when this is disabled call FadeOut() (Black Raw Image becomes Invisible);
    //now everything in the camera view can be seen.
    void OnDisable()
    {
        //Call FadeOut() Method;
        FadeOut();
    }
}
