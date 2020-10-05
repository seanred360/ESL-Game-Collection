using UnityEngine;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using UnityEngine.UI;

/// <summary>
/// The SettingsAndPauseMenu Class handles the "Slide-Up Menu, Displaying Player Level & Experience, and Pausing the Game.  It
/// also has several buttons that handle some Scene Loading.  There are "Home", "Reload", and "Quit" Buttons.  The Mute/UnMute 
/// Volume Button is on this Slide-Up Menu too.
/// </summary>
public class SettingsAndPauseMenu : MonoBehaviour
{
    [Header("Drag the child object 'FullSizeBG' here", order = 0)]                   //InspectorLabels
    public GameObject tintedColorBG;                                                // this is the "backGround" for the slide menu(black graphic with a black color)
    [Header("Drag the child object 'MenuWindow' here", order = 1)]                   //InspectorLabels
    public GameObject pauseMenu;                                                    // the "pauseMenu" is the RectTransform/Image that is the parent to the buttons/images of the Slide Menu
    [Header("Should the Menu Move if timeScale is at zero?", order = 1)]            //InspectorLabels
    public bool ignoreGameTime;                                                     // whether or not the menu should stop if Time.timeScale is set to 0.0f
    [Space(5f, order = 2)]                                                          //InspectorLabels
    [Header("If you check useScreenSizeCalculations there is no", order = 3)]        //InspectorLabels
    [Space(-10f, order = 4)]                                                        //InspectorLabels
    [Header("need to fill out the public Vec3s below...", order = 5)]                //InspectorLabels
    [Space(5f, order = 6)]                                                          //InspectorLabels
    public bool useScreenSizeCalculations;                                          // use screenSizeCalculations will use screen width/height (at start of game) to determine slide to/from locations
    [Header("Make Sure you have a Curve Selected or it wont move", order = 7)]       //InspectorLabels
    public AnimationCurve slideCurve;                                               // the animation curve that represents how the Slide Menu will slide(just like the AnimationCurveMover
    [Range(0, 10)]                                                                  //InspectorLabels
    public float moveSpeed = 1;                                                     // the speed at which the Menu will slide.
    [Header("**Remember: Canvas RectTransforms are in ScreenSpace, ", order = 8)]   //InspectorLabels
    [Space(-10f, order = 9)]                                                        //InspectorLabels
    [Header("not World.  The LowerLeft side of the screen is 0,0", order = 10)]     //InspectorLabels

    [Space(5f, order = 11)]                                             //InspectorLabels

    public static SettingsAndPauseMenu instance;
    public bool theGameIsResetting;                                     //bool to use to Make sure the level isn't loading... we dont want to pause the
                                                                        //game while the fadercanvas is blocking our view
    public Vector3 slideFromPosition;                                   //the vector3 that the menu should slide from (no need to enter anything in inspector if using "useScreenSizeCalculations"
    public Vector3 slideToPosition;                                     //the vector3 that the menu should slide to... (no need to enter anything in inspector if using "useScreenSizeCalculations"

    private float step;                                                 // the incremented step along the slide process.  this is th value we increment with deltaTime.
    private float deltaTime;                                            // our customer deltaTime var if we use "ignoreGameTime"
    private float lastRealTime;                                         // the reference we store the real "lastRealTime" in every frame (used to get our deltaTime)
    private Transform _transform;                                       // reference to this transform
    public Text menuScoreField;                                        // the field that shows the player score
    public Text menuLevelField;                                        // the field that shows the player score
    private bool readyToMove;                                           // boolean that determines if we are ready to slide the menu
    private bool isPaused;                                              // is the game paused? boolean and method change Time.timeScale between 0 and 1 
    private float localSoundVolume;                                     //localSoundVolume var (is a fraction of the Global GameVariables.soundVol)
    private AudioSource musicASource;                                   //reference to the audio source that background music plays from    
    private bool muteAudio;                                             //Boolean muteAudio controls Music and Sfx State(Stored in PlayerPrefs as int 1-Yes / 0-NO)
    private Slider musicOnOffSlider;                                     // the UI Slider we use to Show the toggled state of the Musics On/OFF (in settings menu pop-up)       
    private bool onlyMenuIsUp;                                          // a boolean used to make sure we don't throw off the paused state. When it pops up at end of round..Time.timeScale is still 1 :)

    // Use this for pre-initialization
    void Awake()
    {
        //this is the text field that displays our "player Score"
        //menuScoreField = GameObject.FindGameObjectWithTag(Tags.playerMenuScoreText).GetComponent<Text>();
        //this is the text field that displays our "player Level"
        //menuLevelField = GameObject.FindGameObjectWithTag(Tags.playerMenuLevelText).GetComponent<Text>();
        //tell our static reference THIS is what we are accessing.
        instance = this;
    }


    //Debug.Log Method with param
    public void Debugging(string msg)
    {
        Debug.Log(" '"+ msg + "' ");
    }

    //Debug.Log Method with default 'Debugging is being Called'
    public void Debugging(/*string msg*/)
    {
        Debug.Log(" 'Debugging is being Called' ");
    }


    // Use this for initialization
    void Start()
    {
        //we assign our "_transform" var the transform of our "pauseMenu"
        _transform = pauseMenu.GetComponent<Transform>();

        //we assign our lastRealTime var the initial Time.realtimeSinceStartup
        lastRealTime = Time.realtimeSinceStartup;

        //if useScreenSizeCalculations is set to true, then we need to assign the values to slideTO and slideFrom Vector3's because the inspector values
        //will be disregarded/overwritten
        if (useScreenSizeCalculations)
        {
            //var slideToPosition gets assigned the return value of GetSlideToVecFromScreenSize();
            slideToPosition = GetSlideToVecPerScreenSize();

            //var slideFromPosition gets assigned the return value of GetSlideFromVecFromScreenSize().  We have to pass this Method -Screen.height
            //because we are actually passing it the float that the menu should slide from.  so we take the screen height of our screen and use
            //the negative value to make sure its off screen.
            slideFromPosition = GetSlideFromVecPerScreenSize(-Screen.height);

        }
        // invokeRepeat the OSG_SlowUpdate method so that we can run "less-critical" methods/actions inside of that loop.
        InvokeRepeating("OSG_SlowUpdate", 0.25f, 0.25f);
        //this setups up our audio references and pulls from PlayerPrefs whether or not the music should be muted.
        SetupAudio();

        //theGameIsResetting = false;

    }


    /// <summary>
    /// A Simple PauseMethod that Only Changes the Time.timeScale to 0 or 1 depending on pause state.
    /// </summary>
    public void PauseGame()
    {
        //our boolean isPaused gets its value flipped...
        isPaused = !isPaused;

        //then a conditional that checks the very same boolean...
        if (isPaused)
        {
            //if isPaused is true then Time.timeScale = 0f
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;

        }
    }


    /// <summary>
    /// This Method is in several Classes.  It is the intention of this Method to provide a "SlowUpdate" for non critical tasks.  
    /// I will use this InvokeRepeating loop as often as I can, and at as high of an interval as I can.  To try and lighten the 
    /// load of Update.  This will not always provide benefit.  Most of the time if I put something in SlowUpdate then it does
    /// make some sort of difference.  If you try to do something like this in your own projects, until you know exactly what
    /// will benefit from this, make sure you test it.  Always test and see which is more efficient... sometimes its an endless
    /// battle between responsiveness/efficiency, and there are times/cases where using something like this will decrease performance.
    /// AKWAYS TEST.
    /// </summary>
    void OSG_SlowUpdate()
    {
        UpdateUIText();

    }

    public void TheGameIsResetting()
    {
        theGameIsResetting = true;
        Invoke("TheGameIsDoneResetting", 3.6f);
    }
    public void TheGameIsDoneResetting()
    {
        theGameIsResetting = false;
    }



    // Update is called once per frame
    void Update()
    {
        //our menu moving method ready to go in update...
        MoveMenuWhenReady();


        //we monitor the state of these inputs (android Keycode.Menu, or Standalone/web M key on keyboard.
        //if no other keys come up we will make Esc... Make sure the level isn't loading... we dont want to pause the
        //game while the fadercanvas is blocking our view
        if (Input.GetKeyDown(KeyCode.Escape) && !theGameIsResetting)
        {
            //pause game and bring up settings menu
            CallPauseAndMenu();
        }

    }


    /// <summary>
    /// This Method calls the settings Menu and Pauses the game.  This is the method called from the android "back" button, or from Esc on other
    /// platforms.  It is also called a half second after round end.
    /// </summary>
    public void CallPauseAndMenu()
    {
        //if our menu isn't already pulled up by round end.
        if (!onlyMenuIsUp)
        {
            //flip the value of this boolean.  This will make the MoveMenuWhenReady() Method change the Menu state.
            //it waits to move it.
            readyToMove = !readyToMove;

            //if we get a key down on either then PauseGame() and ChangeSettingsCanvasDirection() get called...
            PauseGame();
            //ChangeSettingsCanvasDirection();
        }

    }

    public void CallMenuOnly()
    {
        //set onlyMenuIsUp to true (so we can't call "Pause and Menu")... double menus would send the menu away... but would pause.. which would be bad...
        onlyMenuIsUp = true;

        //flip the value of this boolean.  This will make the MoveMenuWhenReady() Method change the Menu state.  it waits to move it.
        readyToMove = !readyToMove;
    }


    private void SetupAudio()
    {
        //we use GetComponentInChildren to get a reference to our slider (non intractable.. just shows music setting)
        musicOnOffSlider = GetComponentInChildren<Slider>();

        //set localSoundVol to that of half of the Static soundVolume
        localSoundVolume = GameVariables.soundVolume * 0.1f;
        //get audio source from this object
        //aSource = transform.parent.gameObject.GetComponentsInChildren<AudioSource>()[0];
        musicASource = transform.parent.gameObject.GetComponentsInChildren<AudioSource>()[1];

        //set audio source volume to the new localSoundVolume
        //aSource.volume = localSoundVolume;
        musicASource.volume = localSoundVolume;

        //if we retrieve the PlayerPrefs int and its 1(yes), we set our muted boolean to true, and our mute
        //property on the audio source to true.
        if (GameVariables.mutedVolume == 1)
        {
            muteAudio = true;
            musicASource.mute = true;
            musicOnOffSlider.value = PlayerPrefs.GetInt("mutedAudio");

        }
        //else if its 0, we set both to false... this is how we use PlayerPrefs with and int value for a boolean... there is also PlayerPrefs2
        //as an option.. I used that for a little while back in the day.  truth be told though, neither are a good solution for player stats, but for
        // some quick things like settings... it is sufficient for testing purposes.   The only real reason why you wouldn't want to use it in production
        //is because it isn't secure, and it isn't hard to find...
        else if (GameVariables.mutedVolume == 0)
        {
            muteAudio = false;
            musicASource.mute = false;
            musicOnOffSlider.value = PlayerPrefs.GetInt("mutedAudio");

        }

    }


    /// <summary>
    /// This Method Mutes the Audio and Remembers the State, because it stores the state in playerPrefs as an Integer.
    /// </summary>
    public void MuteAudio()
    {
        //flip boolean state
        muteAudio = !muteAudio;
        //if its true, we set audioSources mute property to true, and PlayerPrefs.SetInt Key-mutedAudio Value-1("true")
        if (muteAudio)
        {
            musicASource.mute = true;

            PlayerPrefs.SetInt("mutedAudio", 1);
            musicOnOffSlider.value = PlayerPrefs.GetInt("mutedAudio");

        }
        // else if its false we do the opposite
        else
        {
            musicASource.mute = false;

            PlayerPrefs.SetInt("mutedAudio", 0);
            musicOnOffSlider.value = PlayerPrefs.GetInt("mutedAudio");


        }
    }


    /// <summary>
    /// This Method is called often to keep the score fields of the Slide Menu updated with the Player Experience and Level.
    /// </summary>
    private void UpdateUIText()
    {
        //menuScoreField.text gets assigned the store GameVariables.Experience.ToString()
        menuScoreField.text = GameVariables.Experience.ToString();

        //the same thing happens to the menuLevelField(not the "Level" in this case is not the game "Level" but
        //the players "level".. which levels up every 500 ball. Obviously for a production game you would setup 
        //some awards, equipment changes, celebratory particle systems!!! You did it level.. 1,2,3!! etc...
        menuLevelField.text = GameVariables.Level.ToString();
    }


    /// <summary>
    /// This Method returns a Vector3 that is the center of the screen.  **Note: this is method is called on game/scene
    /// start.  So that means if you change the game template and it the game is made to have an adjustable screen orientation
    /// the Slide Menu will not be oriented the right way.  I have the game locked at Landscape Left, and that is not an issue, but
    /// if that changes this would have to be called after the orientation change as well.  Possibly altered further.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetSlideToVecPerScreenSize()
    {
        //we create two new integers and assign the x-axis int screen.width divided by 2, so that we get half way
        //across the screen.  
        int screenX = Screen.width / 2;

        //Then the y-axis integer gets assigned the screen.height divided by 2... again half 
        //across screen.
        int screenY = Screen.height / 2;

        //we create a new Vec3 and name it "screenSizeToVec" and assign it screenX for x, screenY for y, and z is left blank.
        //the  (float)  before our integer variables casts them to floats for us.  Vector3s take three float values but
        //when dealing with screen space you can usually just leave the z value at zero and ignore it. You can also just enter
        //the x,y value and not put anything for z, but it will still be there... it will just be a value of 0.
        Vector3 screenSizeToVec = new Vector3((float)screenX, (float)screenY, 0f);

        //then we return the screenSizeToVec to the caller;
        return screenSizeToVec;
    }


    /// <summary>
    /// This method is similar to the GetSlideToVecPerScreenSize().  This Method takes one parameter.  This
    /// Method takes a float (i use -screen.height) and it uses this for the position the Slide Menu is "Off Screen".
    /// This could have just as well been in the method(requiring no parameter), but I left it this was because 
    /// initially I was feeding different public variables via the inspector.  Later I decided that using the
    /// -Screen.height was pretty decent solution given that all screens are different, and that it may lead to a 
    /// device independent solution.  For that reason you could probably make some additional changes/additions to this
    /// method, and remove the need for a parameter, then seal it up.(same with the one Above)  (Read Comments from 
    /// the other version of this method too, since they both deal with the same issues/situations)
    /// </summary>
    /// <param name="yPos"></param>
    /// <returns></returns>
    private Vector3 GetSlideFromVecPerScreenSize(float yPos)
    {
        //create a new integer and assign it Screen.width / 2f (middle of screen)
        int screenX = Screen.width / 2;

        //create a new vector3 and assign it a new Vector3 and give it screenX, yPos, 0f
        Vector3 screenSizeFromVec = new Vector3((float)screenX, yPos, 0f);

        //return that new vector3.
        return screenSizeFromVec;
    }


    /// <summary>
    /// This Method Slides our Settings/Pause Menu into view on the screen.  This Menu is above everything
    /// except the ScreenFader Image.  When this method is called it enables the tintedColorBG and then slides
    /// up from the bottom of the screen.  This class can allow this movement to be controlled by the game time,
    /// or it can ignore game time.
    /// </summary>
    private void SlideMenuIntoView()
    {
        //activate the tintedBG because our Settings/Pause Menu is going to be coming into view!
        tintedColorBG.SetActive(true);

        //our deltaTime var get assigned Time.deltaTime.
        deltaTime = Time.deltaTime;

        //if ignoreGameTime then....
        if (ignoreGameTime)
        {
            //if ignoreGameTime was checked in the inspector then we want to control the value/variable "deltaTime".
            //We subtract our lastRealTime variable(that we stored in start) from Time.realtimeSinceStartup...
            deltaTime = (Time.realtimeSinceStartup - lastRealTime);

            //then right afterwards we update lastRealTime with the current Time.realtimeSinceStartup..
            //this effectively makes it so that there is a small difference between the Time.realtimeSinceStartup and
            //our "lastRealTime".  We subtract last frames Time.realtimeSinceStartup from the Actual Time.realtimeSinceStartup;
            //this gives us the deltaTime, but without us having to use Time.deltaTime.  So now we can multiply movement by our
            //"deltaTime".
            lastRealTime = Time.realtimeSinceStartup;
        }

        //our cached _transfroms's "position" gets assigned Vector3.Lerp( our slide FROM Pos , our slide TO Pos, and the
        // and then use our curve.)  The step acts as our Time/progression and so we evaluate the curve value at the step time.
        _transform.position = Vector3.Lerp(slideFromPosition, slideToPosition, slideCurve.Evaluate(step));
        //then step gets incremented by moveSpeed * deltaTime

        step += moveSpeed * deltaTime;
        //if our "step" variable is greater than or equal to "1f" then we are done!

        if (step >= 1f)
        {
            //step gets assigned 1f (in-case it was over...) BECAUSE when SlideMenuOutOfView() is called we will run "step" back
            //to 0f
            step = 1f;
        }

    }

    /// <summary>
    /// This Method Slides the Settings/Pause Menu off the screen.  So this Method does the opposite of SlideMenuIntoView();
    /// </summary>
    private void SlideMenuOutOfView()
    {
        //our deltaTime var get assigned Time.deltaTime.
        deltaTime = Time.deltaTime;

        //if ignoreGameTime then....
        if (ignoreGameTime)
        {
            //if ignoreGameTime was checked in the inspector then we want to control the value/variable "deltaTime".
            //We subtract our lastRealTime variable(that we stored in start) from Time.realtimeSinceStartup...
            deltaTime = (Time.realtimeSinceStartup - lastRealTime);

            //then right afterwards we update lastRealTime with the current Time.realtimeSinceStartup..
            //this effectively makes it so that there is a small difference between the Time.realtimeSinceStartup and
            //our "lastRealTime".  We subtract last frames Time.realtimeSinceStartup from the Actual Time.realtimeSinceStartup;
            //this gives us the deltaTime, but without us having to use Time.deltaTime.  So now we can multiply movement by our
            //"deltaTime".
            lastRealTime = Time.realtimeSinceStartup;
        }

        //our cached _transfroms's "position" gets assigned Vector3.Lerp( our slide FROM Pos , our slide TO Pos, and the
        // and then use our curve.)  The step acts as our Time/progression and so we evaluate the curve value at the step time.

        _transform.position = Vector3.Lerp(slideFromPosition, slideToPosition, slideCurve.Evaluate(step));
        //then step gets decremented by moveSpeed * deltaTime

        step -= moveSpeed * deltaTime;
        //if our "step" variable is less than or equal to "0f" then we are done! (and back off the screen)!
        if (step <= 0)
        {
            //step gets assigned 0f(in-case we overshot).  We want the ending results to be 0 or 1, because otherwise
            //if the menu is moved a lot (i like spamming keys from time to time...) it doesn't slowly wobble off the
            //screen because we are using a float and moving something a very small number per frame.)
            step = 0;

            //Deactivate our tintedBG because the Settings/Pause Menu is now off screen.
            tintedColorBG.SetActive(false);
        }
    }


    /// <summary>
    /// This Method is called via Update, and it is constantly waiting for the readyToMove boolean to change.  When the 
    /// boolean changes the appropriate slideMenu___OfView() will be called, and It will keep getting called as it moves
    /// the Menu(Vector3.Lerp is not inside a loop/coroutine so we leave MoveMenuWhenReady in Update().
    /// </summary>
    private void MoveMenuWhenReady()
    {
        //if boolean is true...
        if (readyToMove)
        {
            //slide the menu onto the screen
            SlideMenuIntoView();
        }
        // else if its false...
        else
        {
            //slide the menu off-screen or leave it there!
            SlideMenuOutOfView();
        }
    }


    /// <summary>
    /// This Method is linked to a UI button, and it fades the screen to black, and quits the application when pressed.  
    /// It does a few other things to get us to our "Quit State", but those are described below.  These Fader____ Methods 
    /// in this class all end with calling one of our Scene Manager Methods on our Screen Fader Singleton.
    /// </summary>
    public void FaderExitApplication()
    {
        //we call PauseGame() (PauseGame and changing readyToMove have to be done to get to this point I.e. to hit the "Quit" button
        //on the slide menu then we had to pause/flip readyToMove to even see it.  SO, that means that when we call them again, we are
        //actually "un-pausing" the game, and we are "moving the menu back off screen".  As if we hit the button we wanted, so no need to
        //leave the pause/menu canvas around.  So.....
        TheGameIsResetting();

        //if paused...
        if (isPaused)
        {
            //PauseGame was already called so we call it again to un-pause the game.
            PauseGame();
        }
        //flip the readyToMove boolean again so the Menu slides off screen.
        readyToMove = !readyToMove;

        //change onlyMenuIsUp back to false... because this method closes the menu after we choose a loadLevel button.
        onlyMenuIsUp = false;

        //we change Static Var BallsMissed to zero...
        GameVariables.BallsMissed = 0;

        //we call FadeAndQuitApplication() on our static reference Instance, of our screenFaderSingleton.
        ScreenFaderSingleton.Instance.FadeAndQuitApplication();
    }


    /// <summary>
    /// This Method is linked to a UI Button, and it fades and reloads the current level when pressed.
    /// </summary>
    public void FaderReloadLevel()
    {
        TheGameIsResetting();

        //if paused...
        if (isPaused)
        {
            //PauseGame was already called so we call it again to un-pause the game.
            PauseGame();
        }
        //flip the readyToMove boolean again so the Menu slides off screen.
        readyToMove = !readyToMove;

        //change onlyMenuIsUp back to false... because this method closes the menu after we choose a loadLevel button.
        onlyMenuIsUp = false;

        //we change Static Var BallsMissed to zero...
        GameVariables.BallsMissed = 0;

        //we call FadeAndReloadLevel() on our static reference Instance, of our screenFaderSingleton.
        ScreenFaderSingleton.Instance.FadeAndReloadLevel();

    }


    /// <summary>
    /// This Method is linked to a UI Button, and it fades and loads the main menu scene when pressed.
    /// </summary>
    public void FaderLoadMainMenu()
    {
        TheGameIsResetting();

        //if paused...
        if (isPaused)
        {
            //PauseGame was already called so we call it again to un-pause the game.
            PauseGame();
        }
        //flip the readyToMove boolean again so the Menu slides off screen.
        readyToMove = !readyToMove;

        //change onlyMenuIsUp back to false... because this method closes the menu after we choose a loadLevel button.
        onlyMenuIsUp = false;

        //we change Static Var BallsMissed to zero...
        GameVariables.BallsMissed = 0;

        //we call FadeAndReloadLevel() on our static reference Instance, of our screenFaderSingleton.
        ScreenFaderSingleton.Instance.FadeAndLoadMainMenu();
    }

}
