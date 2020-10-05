using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SlicingGame
{
    /// <summary>
    /// This is an Enum that contains the different possible game types.
    /// </summary>
    public enum GameModes
    {
        RegularGameMode,
        ChillGameMode
    }


    /// <summary>
    /// GameController Class handles all of the starting and stopping of rounds.  It also constantly monitors the game
    /// state. That way as soon as theres a loss, or time is up... it'll end the game.  This class also keeps the UI
    /// text fields updated with the score.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public static GameController GameControllerInstance;        //static reference to this game controller
        private CountdownTimer roundTimer;                          //a reference to the countdown timer(connected to same parent body)
        [Header("Select GameMode for Testing Purposes.")]
        public GameModes gameModes;                                 //our game controllers GameModes Enum variable... gameModes
        [Header("Drop the 'GameOverCanvas' Here")]
        public GameObject gameOverPanel;                            //the gameOverPanel that hold the Game Over UI Image
        public GameObject[] blueXRegMode;                           //regular mode blue x's.  The 3 X's on the UI that you start with in RegularGameMode Mode
        public GameObject[] redXRegMode;                            //RegularGameMode mode red x's.  The XXX on the UI (under the Blue X's), that are show when you miss ball.
        public Text regularModeText;                                //the txt that is the regular mode current store
        public Text chillModeText;                                  //the txt that is the relax mode current store
        public Text regularModeHighestText;                         //the txt that is the regular mode highest store
        public Text chillModeHighestText;                           //the txt that is the relax mode highest store
        public GameObject slicerGO;                                 //a reference to our osgTouchSlicer
        private bool gameHasStarted;                                //boolean game has started???
        public bool gameIsRunning;                                  //boolean game is running??
        public float waitForMenuAtEnd;                              //how long to wait before the settings/pause menu pops up?

        // Use this for pre-initialization
        void Awake()
        {
            //our instance reference to this GameController is assigned THIS.
            GameControllerInstance = this;
            //roundTimer reference is assigned the CountdownTimer that is connected to this gameobject.
            roundTimer = GetComponent<CountdownTimer>();
            //we make sure the gameOverPanel is inactive... (the game just started, son!)
            gameOverPanel.SetActive(false);
            //boolean gameIsRunning is True.
            gameIsRunning = true;
        }


        //OnEnable is called when the object becomes enabled and active
        void OnEnable()
        {
            //we zero out all of our static variables dealing with score.
            GameVariables.BallsMissed = 0;
            GameVariables.RegularModeScore = 0;
            GameVariables.ChillModeScore = 0;
            //then we RESET the splatterQuadSpawnDistance to 55f.  This static var is reset every round.  we increment it when we spawn a splatter, so that
            //they always spawn on top of the previous one... they stop around 45f worse case scenario, and in orthographic mode that is still acceptable.
            GameVariables.splatterQuadSpawnDistance = 55f;
        }

        // Use this for initialization
        void Start()
        {
            //start coroutine... ChooseGameModeAndCallRoundStart()... Long Name
            StartCoroutine(ChooseGameModeAndCallRoundStart());

            //and we start osg_SlowUpdate.  We are trying to offload some secondary methods/workloads that don't need as frequent of updates.
            InvokeRepeating("OSG_SlowUpdate", 0.33f, 0.33f);

        }

        /// <summary>
        /// OSG_SlowUpdate Method again... Run Unimportant stuff here.  In this class at the least the UI updates will be in here.
        /// </summary>
        private void OSG_SlowUpdate()
        {
            //if game is running we will...
            if (gameIsRunning)
            {
                //call the updateUIText method.
                UpdateUIText();
            }

        }

        // Update is called once per frame
        void Update()
        {
            //if game is running we will...
            if (gameIsRunning)
            {
                MonitorGameState();
                //UpdateUIText();//moved to SlowUpdat()
            }

        }


        /// <summary>
        /// UpdateUIText does exactly what you would imagine.  It updates UI Text.
        /// </summary>
        private void UpdateUIText()
        {
            //we update all the current scores.
            regularModeText.text = GameVariables.RegularModeScore.ToString();
            chillModeText.text = GameVariables.ChillModeScore.ToString();
            //we update the highest scores.
            regularModeHighestText.text = GameVariables.RegularModeHighestScore.ToString();
            chillModeHighestText.text = GameVariables.ChillModeHighestScore.ToString();
        }


        /// <summary>
        /// The UpdatePlayerExperienceAndLevel method adds the destroyed ball the players "Experience". and
        /// then Stores the new experience in PlayerPrefs/
        /// </summary>
        /// <param name="ballsDestroyed"></param>
        private void UpdatePlayerExperienceAndLevel(int ballsDestroyed)
        {
            //update Experience with ballsDestroyed value
            GameVariables.Experience += ballsDestroyed;
            //save Experience in PlayerPrefs
            PlayerPrefs.SetInt(Tags.experience, GameVariables.Experience);

        }


        /// <summary>
        /// This UpdateHighestScore takes in the ending round score(if it is higher than the current highest score) and then depending
        /// on the selected mode/variable It will write the update to PlayerPrefs.
        /// </summary>
        /// <param name="amt"></param>
        private void UpdateHighestScore(int amt)
        {
            switch (gameModes)
            {
                //if in RegularGameMode game mode...
                case GameModes.RegularGameMode:
                    //RegularModeHighestScore gets assigned "amt"
                    GameVariables.RegularModeHighestScore = amt;

                    //store the new amount in PlayerPrefs
                    PlayerPrefs.SetInt(Tags.highestRegularScore, GameVariables.RegularModeScore);
                    break;


                //if in ChillGameMode game mode...
                case GameModes.ChillGameMode:

                    //RegularModeHighestScore gets assigned "amt"
                    GameVariables.ChillModeHighestScore = amt;

                    //store the new amount in PlayerPrefs
                    PlayerPrefs.SetInt(Tags.highestChillScore, GameVariables.ChillModeScore);
                    break;

                default:
                    //Debug.Log("Danger, WIll Robinson, Danger");
                    break;

            }

        }

        /// <summary>
        /// This method stops the game time.
        /// </summary>
        private void StopTime()
        {
            //set Time.timeScale to 0f;
            Time.timeScale = 0.0f;
        }


        /// <summary>
        /// A redundant local private method to access our settings/menu instance var(and the method CallPauseAndMenu()).  So that we can invoke this
        /// Method about a half a second after round end. 
        /// </summary>
        private void LocalPauseAndSettingsMenuCall()
        {
            //SettingsAndPauseMenu.instance.CallPauseAndMenu();

            //call the settings menu onto the screen(without the pause).
            SettingsAndPauseMenu.instance.CallMenuOnly();
        }


        /// <summary>
        /// MonitorGameState is the main Method in the GameController Class.  The Method 
        /// </summary>
        private void MonitorGameState()
        {

            ////////////////////////////////
            ////_____REGULAR-MODE______////
            ///////////////////////////////


            //if the current game mode is RegularGameMode...
            if (gameModes == GameModes.RegularGameMode)
            {
                //the switch monitors how many missed ball there are... It monitors the Static BallsMissed in GameVariables
                switch (GameVariables.BallsMissed)
                {
                    //if zero ball have been missed...
                    case (0):
                        //Debug.Log("lost none yet");

                        //No ball have been lost yet... Continue as you were...
                        //Access the LaunchControllers static Instance and call ReduceLaunchTimersAndLaunchObjects...
                        LauncherController.LaunchControllerInstance.ReduceLaunchTimersAndLaunchObjects();

                        break;
                    //if one ball have been missed...
                    case (1):
                        //Debug.Log("lost one so far");

                        //remove blueX 0 and set the redX 0 active... When the red X is activated its wiggle animation will play.
                        blueXRegMode[0].SetActive(false);
                        redXRegMode[0].SetActive(true);

                        //Access the LaunchControllers static Instance and call ReduceLaunchTimersAndLaunchObjects...
                        LauncherController.LaunchControllerInstance.ReduceLaunchTimersAndLaunchObjects();

                        break;
                    //if two ball have been missed...
                    case (2):
                        //Debug.Log("lost two so far");

                        //move on to the second X's - Disable Blue, and Activate Red
                        blueXRegMode[1].SetActive(false);
                        redXRegMode[1].SetActive(true);
                        //Access the LaunchControllers static Instance and call ReduceLaunchTimersAndLaunchObjects...
                        LauncherController.LaunchControllerInstance.ReduceLaunchTimersAndLaunchObjects();


                        break;
                    //if three or more have been lost the game is over....
                    default:
                        //Debug.Log("3 or some other amount...");

                        //disable the final blue x, and enable the final red x.
                        blueXRegMode[2].SetActive(false);
                        redXRegMode[2].SetActive(true);

                        //Debug.Log("Game Over");

                        //activate the gameOverPanel
                        gameOverPanel.SetActive(true);

                        //disable the OSGTouchSlicer so the player does not pick-up any more ball(monitoring/scoring are about to be turned off)
                        slicerGO.SetActive(false);

                        //invoke our settings/menu canvas to provoke a response...
                        Invoke("LocalPauseAndSettingsMenuCall", waitForMenuAtEnd);

                        //if we access the GameVariables Static variables, and check... Is the HighestRegularModeScore less than the current RegularModeScore??
                        if (GameVariables.RegularModeHighestScore < GameVariables.RegularModeScore)
                        {
                            //if it is, then we need to update the HighestScore... so we call UpdateHighestScore(GameVariables.RegularModeScore)
                            UpdateHighestScore(GameVariables.RegularModeScore);
                        }
                        //and we update Player Experience by calling UpdatePlayerExperienceAndLevel and pass in GameVariables.RegularModeScore.
                        UpdatePlayerExperienceAndLevel(GameVariables.RegularModeScore);

                        //set gameIsRunning to false.
                        gameIsRunning = false;

                        //and Break! we are done with this case.
                        break;
                }
            }



            ////////////////////////////////
            ////______CHILL-MODE_______////
            ///////////////////////////////


            //if the current game mode is Arcade...
            if (gameModes == GameModes.ChillGameMode)
            {
                //if roundTimer.timeLeft is less than or equal to 0.01 && gameHasStarted
                if (roundTimer.timeLeft <= 0.01 && gameHasStarted)
                {
                    //Debug.Log("Game Over");

                    //activate the gameOverPanel
                    gameOverPanel.SetActive(true);

                    //OSGTouchSlicer gets disabled
                    slicerGO.SetActive(false);

                    //invoke our settings/menu canvas to provoke a response...
                    Invoke("LocalPauseAndSettingsMenuCall", waitForMenuAtEnd);

                    //if GameVariables.ArcadeModeHighestScore is less than GameVariables.ArcadeModeScore then clearly we need to update the highest score
                    if (GameVariables.ChillModeHighestScore < GameVariables.ChillModeScore)
                    {
                        //we update the highest score by calling UpdateHighestScore(GameVariables.ArcadeModeScore(ArcadeModeScore))
                        UpdateHighestScore(GameVariables.ChillModeScore);
                    }

                    //We also need to update player experience... we pass in our new score to do that.
                    UpdatePlayerExperienceAndLevel(GameVariables.ChillModeScore);

                    //and we set gameIsRunning to false
                    gameIsRunning = false;

                }
                //else... game clock is still running, and we should still be launching...
                else
                {
                    //Access the LaunchControllers static Instance and call ReduceLaunchTimersAndLaunchObjects...
                    LauncherController.LaunchControllerInstance.ReduceLaunchTimersAndLaunchObjects();

                }
            }
        }


        /// <summary>
        /// Method called by Coroutine to start a RegularGameMode Mode round.  If hasTimer is true then we call StartTimer on our CountdownTimer Class, and we
        /// pass the amount of time that should be on the clock.  Then we change the boolean "gameHasStarted" which will remain true until round end.
        /// </summary>
        /// <param name="hasTimer"></param>
        /// <param name="gameTime"></param>
        private void StartRegularModeGame(bool hasTimer, float gameTime)
        {
            //if hasTimer is true... (in Regular mode it should be false, so the following code block will not be run... (it'll pick up at "roundTimer.DisableTimerText();") **(Regular mode does not)
            if (hasTimer)
            {
                //this should not happen in Regular mode....
                roundTimer.StartTimer(gameTime);
            }
            //now we call roundTimer.DisableTimerText (Note:This should only remove timer in Regular mode (because timer is not set(it should remove the timer display text)
            roundTimer.DisableTimerText();

            //now we set the gameHasStarted boolean to true! Game is Starting!!!
            gameHasStarted = true;

        }


        /// <summary>
        /// Method called by Coroutine to start a ChillGameMode Mode round.  If hasTimer is true then we call StartTimer on our CountdownTimer Class, and we
        /// pass the amount of time that should be on the clock.  Then we change the boolean "gameHasStarted" which will remain true until round end.
        /// </summary>
        /// <param name="hasTimer"></param>
        /// <param name="gameTime"></param>
        private void StartChillModeGame(bool hasTimer, float gameTime)
        {
            //if has timer is true then we...  **(relax mode does)
            if (hasTimer)
            {
                //will call StartTimer and pass in the gameTime(60f) for Arcade Mode...
                roundTimer.StartTimer(gameTime);

            }

            //then we set gameHasStarted to true, and start slicing!
            gameHasStarted = true;

        }

        /// <summary>
        /// This Method Zeros out the "timeLeft" and disables the OSGTouchSlicer.
        /// </summary>
        private void ZeroGameTimeAndEndGame()
        {
            //assign a value of 0f to roundTimer.timeLeft.
            roundTimer.timeLeft = 0f;

            //disable osg.touchSlicer GO
            slicerGO.SetActive(false);

        }


        /// <summary>
        /// This method Ends the Game. It activates all the Red X's, Disables the Blue X's, and sets BallsMissed to 5.
        /// </summary>
        public void TakeBallAndEndGame()
        {
            //set our static var BallsMissed to 5
            GameVariables.BallsMissed = 5;

            //if the gameModes is equal to GameModes.RegularGameMode
            if (gameModes == GameModes.RegularGameMode)
            {
                //deactivate the slicer Game Object
                slicerGO.SetActive(false);

                //use a for loop to go through all of the Blue x's...
                for (int i = 0; i < blueXRegMode.Length; i++)
                {
                    ///if we find a blue X that is active then we...
                    if (blueXRegMode[i].activeInHierarchy)
                    {
                        //disable it..
                        blueXRegMode[i].SetActive(false);
                    }
                }
                //use for loop to go through all of the Red x's...
                for (int i = 0; i < redXRegMode.Length; i++)
                {
                    //if we find a Red X that is active then we...
                    if (!redXRegMode[i].activeInHierarchy)
                    {
                        //disable it...
                        redXRegMode[i].SetActive(true);
                    }
                }
            }
        }



        /// <summary>
        /// This Method is called at the start of the scene... It starts the game with the appropriate gameObjects/Settings
        /// </summary>
        /// <returns></returns>
        IEnumerator ChooseGameModeAndCallRoundStart()
        {
            switch (gameModes)
            {


                //if we are in GameModes.RegularGameMode then we need to...
                case GameModes.RegularGameMode:

                    //set the current regular score to zero...
                    GameVariables.RegularModeScore = 0;

                    //call the method StartRegularModeGame with these variables... false(hasTimer), and 0f(time to set Timer for).
                    StartRegularModeGame(false, 0f);

                    //loop through the number of x's in the array...
                    for (int i = 0; i < blueXRegMode.Length; i++)
                    {
                        //for each blue x we set it active...
                        blueXRegMode[i].SetActive(true);
                    }

                    //then we are done...
                    break;


                //if we are in GameModes.ChillGameMode then we need to...
                case GameModes.ChillGameMode:

                    //wait for a few seconds while "60 seconds" and "Go!!" text slide on screen and then fade.
                    yield return new WaitForSeconds(3.5f);

                    //our Static variable GameVariables.ChillModeScore needs to be set to zero (to make sure it is cleared)
                    GameVariables.ChillModeScore = 0;

                    //then we call StartRelaxModeGame() and pass in true(hasTimer), and 90f(TimerTime)
                    StartChillModeGame(true, 90f);

                    //break... we are done.
                    break;



                default:
                    //do nothing, because I cant think of anything to do but scream, ERROR!!!!
                    break;


            }

            //yield return null... this coroutine is over!
            yield return null;
        }

    }
}
