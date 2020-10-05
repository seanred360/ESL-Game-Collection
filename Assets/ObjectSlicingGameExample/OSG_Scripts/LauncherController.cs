using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SlicingGame
{
    /// <summary>
    /// LauncherController Class handles the Launching of the gameObjects.  Balls and Bombs.
    /// </summary>
    public class LauncherController : MonoBehaviour
    {
        public static LauncherController LaunchControllerInstance;                  // our static reference to this LaunchController
        [Header("Bottom Ball Launchers, and how many to Launch")]
        public GameObject[] bottomBallLaunchers;                                    // an array of the ball launchers at the bottom of the dojo
        public int bottomLauncherSalvoAmount;                                       // the salvo amt (an int that determines how many balls are fired)
        [Header("Side Ball Launchers(Frenzy), and how many to Launch")]
        public GameObject[] sideBallLaunchers;                                      // an array of the ball launchers at the side of the dojo (for Frenzy PowerUp)
        public int sideLauncherSalvoAmount;                                         // the salvo amt (an int that determines how many ball are fired during a frenzy
        [Header("How Long To Wait For Ball to Spawn?(MIN)")]
        public float minWaitTime;                                                   // Min time to wait before ball are spawned (when requested)
        [Header("How Long To Wait For Ball to Spawn?(MAX)")]
        public float maxWaitTime;                                                   // Max time to wait before ball are spawned (when requested)
        [Header("How Long Between Regular Bottom Ball Launches?")]
        public float timeBetweenLaunches;                                           // the time between salvos (there is a little break between them)
        public float timeBetweenRandomLaunches;                                     // time between random launches (there are some salvos that come at random intervals too(breaks up the interval launches)
        public float timeBetweenBombLaunches;                                       // the time between bomb object's launches
        [Header("What should the Max Number of Ball Launches be?")]
        public int maximumSimultaneousBallLaunches;                                // the max number of simultaneous ball launches (max salvo amt)
        [Tooltip("This is the number that after the 'maxSimultaneousBallLaunches' is reached, that the launcher will start over at")]
        public int resetBallLauncherAmount;                                        // if we reach the max number of launches, what number should we drop it back down to?
        [Header("How Many More Ball In Chill Mode?")]
        public int chillModeExtraBalls;                                             // How many extra ball would we like launched in relax mode?  there are no ball and bombs, so why not add ball!?
        public int maxBombSalvoAmt;                                                 // the max number of bombs that can be launched at one time

        [Header("Starting Number of Bombs launched")]
        [Header(" **goes up each loop** ")]
        [Space()]
        public int BombSalvoAmount;                                                 // the amount of bombs that will be launched during bomb salvos

        private float initialTimeBetweenLaunches;                                   // the initial time between the ball launches
        private List<BallLauncher> bottomLaunchersScriptReference;                  // a List that contains all of the Bottom BallLaunchers (the gameObject the "BallLaunchers" script is attached to)
        private List<BallLauncher> sideLaunchersScriptReference;                    // a List that contains all of the Side BallLaunchers (the gameObject the "BallLaunchers" script is attached to)
        private CountdownTimer timer;                                               // a reference to our "CountdownTimer". GameController,DojoBoundary,LauncherContoller(this), and CountdownTimer are all on GameController gameObject
        private int cycleThruSideLaunchersForFrenzy;                                // the int that is used for looping through the side launchers

        // Use this for pre-initialization
        void Awake()
        {
            //make sure our static LaunchController Instance is set to THIS gameObject
            LaunchControllerInstance = this;

            //our timer reference by calling GetComponent on THIS gameObject
            timer = GetComponent<CountdownTimer>();

            //Use GameObjects FindObjectWithTag method to setup our references to the bottom and side BallLaunchers.
            bottomBallLaunchers = GameObject.FindGameObjectsWithTag(Tags.bottomBallLaunchers);
            sideBallLaunchers = GameObject.FindGameObjectsWithTag(Tags.sideBallLaunchers);

            //Initialize the lists  "bottomLaunchersScriptReference" && "sideLaunchersScriptReference"
            bottomLaunchersScriptReference = new List<BallLauncher>();
            sideLaunchersScriptReference = new List<BallLauncher>();

            //loop through all of the bottom ball launchers
            for (int i = 0; i < bottomBallLaunchers.Length; i++)
            {
                //now add each bottom ball launcher to our List ("bottomLaunchersScriptReference")
                bottomLaunchersScriptReference.Add(bottomBallLaunchers[i].GetComponent<BallLauncher>());
            }
            //loop through all of the side ball launchers
            for (int j = 0; j < sideBallLaunchers.Length; j++)
            {
                //now add each side ball launcher to our List ("sideLaunchersScriptReference")
                sideLaunchersScriptReference.Add(sideBallLaunchers[j].GetComponent<BallLauncher>());
            }
        }


        // Use this for initialization
        void Start()
        {
            //we store our "timeBetweenLaunches" in the InitialTimeBetweenLaunches" variable. (so we have a back up of the original value)
            initialTimeBetweenLaunches = timeBetweenLaunches;
        }


        /// <summary>
        /// This Method calls for the First Ball Launch.
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        private IEnumerator FirstBallLaunch(int amt)
        {
            //we wait for 1f seconds
            yield return new WaitForSeconds(1f);
            //Then we "RequestBallSalvoFromBottom(and pass in the "amt" that was passed with the method call)"
            RequestBallSalvoFromBottom(amt);
        }


        /// <summary>
        /// This method adjusts some of the launcher settings/times when relax is the gameMode.
        /// </summary>
        private void ChangeToRelaxSettings()
        {
            //we assign timeBetweenLaunches 4.
            timeBetweenLaunches = 4;
            //we assign initialTimeBetweenLaunches 4 as well.
            initialTimeBetweenLaunches = 4;
            //then we set the "maximumSimultaneousBallLaunches" to 11;
            maximumSimultaneousBallLaunches = 11;

        }


        /// <summary>
        /// This Method is where all of the "Launching" happens. We monitor the "GameMode" we are in based on a Switch that compares our gameModes 
        /// var to the GameModes Enum.  So we can have different behaviors for different GameModes... though in this example they are virtually the
        /// same, beside the fact that in the regular mode we also launch Bombs mixed in with the balls.
        /// </summary>
        public void ReduceLaunchTimersAndLaunchObjects()
        {

            //before we get into gameMode specific code... timeBetweenLaunches gets decremented by Time.deltaTime;
            timeBetweenLaunches -= Time.deltaTime;
            //if "timeBetweenLaunches" is less than or equal to 0f, then we... LAUNCH BALLS
            if (timeBetweenLaunches <= 0f)
            {
                //create a new int named "r" and give it a random value between 0 and 3 (inclusive, exclusive... so values can be 0,1,2)
                int r = Random.Range(0, 3);//NOTE We create the random int to give a slight variation to the feel of the launches/rounds.

                //now for the gameMode that we are in.
                switch (GameController.GameControllerInstance.gameModes)
                {


                    /////////////////////////////////////
                    ////////______REGULAR-MODE_____////////
                    /////////////////////////////////////


                    //if we are in GameMode RegularGameMode
                    case GameModes.RegularGameMode:


                        //we check to see what random value was generated...

                        //if r equals 0, we....
                        if (r == 0)
                        {

                            //if our BallsMissed var is less than 3(then the game must still be running)
                            if (GameVariables.BallsMissed/*GameController.GameControllerInstance.BallsMissed*/ < 3)
                            {
                                //we request a Ball Salve From Bottom Launchers and we pass the " bottomLauncherSalvoAmount "
                                RequestBallSalvoFromBottom(bottomLauncherSalvoAmount);

                                //increment launch amount
                                bottomLauncherSalvoAmount++;

                                //reset timer
                                timeBetweenLaunches = initialTimeBetweenLaunches;
                            }
                        }


                        //if r equals 1, we...
                        if (r == 1)
                        {
                            //if our BallsMissed var is less than 3(then the game must still be running)
                            if (GameVariables.BallsMissed/*GameController.GameControllerInstance.BallsMissed*/ < 3)
                            {
                                //we request a Ball Salvo From Bottom Launchers BUT we pass the "BombSalvoAmount"(bc this pass we are doing bombs and balls).
                                RequestBallSalvoFromBottom(BombSalvoAmount + 2);// few more balls than whatever the bombSalvoAmount is...

                                //we also request a bomb salvo from the bottom launchers and we pass the "BombSalvoAmount - 1"(want to keep the bomb num manageable at the beginning)
                                RequestBombSalvoFromBottomLauncher(BombSalvoAmount - 1);

                                //bottomLauncherSalvoAmount++;
                                BombSalvoAmount++;

                                //timeBetweenLaunchers gets assigned the value of "initialTimeBetweenLaunches"
                                timeBetweenLaunches = initialTimeBetweenLaunches;
                                //if the bombSalvoAmount is greater than maxBombSalvoAmt, then we...
                                if (BombSalvoAmount > maxBombSalvoAmt)
                                {
                                    //we reset the bomb salvo amount to some other value between 1 and the maxBombSalvoAmt
                                    BombSalvoAmount = Random.Range(1, maxBombSalvoAmt);
                                }
                            }



                        }


                        //if r equals 2, we...
                        if (r == 2)
                        {

                            //if our BallsMissed var is less than 3(then the game must still be running)
                            if (GameVariables.BallsMissed/*GameController.GameControllerInstance.BallsMissed*/ < 3)
                            {
                                ///////   NOTE - If 2 is the random value AND we are in RegularGameMode Mode... this version will only Instantiate one Bomb(for the cases, where
                                /////       Bombs don't spawn for a while... we will spawn ball and One Bomb.  This is kind of a lazy copy and paste from Int (1) above, but
                                ////        it will cover some of the gaps.  We use this 0,1,2 system to try and create a little variation... and random nature to the rounds.
                                ////

                                //we request a Ball Salvo From Bottom Launchers BUT we pass the "BombSalvoAmount"(bc this pass we are doing bombs and balls again).
                                RequestBallSalvoFromBottom(BombSalvoAmount + 1);

                                //we also request a bomb salvo from the bottom launchers and we pass the "BombSalvoAmount"
                                RequestBombSalvoFromBottomLauncher(BombSalvoAmount);

                                //bottomLauncherSalvoAmount++;
                                BombSalvoAmount++;

                                //timeBetweenLaunchers gets assigned the value of "initialTimeBetweenLaunches"
                                timeBetweenLaunches = initialTimeBetweenLaunches;
                                //if the bombSalvoAmount is greater than maxBombSalvoAmt, then we...
                                if (BombSalvoAmount > maxBombSalvoAmt)
                                {
                                    //we reset the bomb salvo amount to some other value between 1 and the maxBombSalvoAmt
                                    BombSalvoAmount = Random.Range(1, maxBombSalvoAmt);
                                }
                            }


                        }


                        // if the ( bottomLauncherSalvoAmount is equal= to maximumSimultaneousBallLaunches ) then...
                        if (bottomLauncherSalvoAmount == maximumSimultaneousBallLaunches)
                        {
                            //we need to assign the "resetBallLauncherAmount" to our "bottomLauncherSalvoAmount" variable.
                            bottomLauncherSalvoAmount = resetBallLauncherAmount;
                        }



                        break;


                    /////////////////////////////////////
                    ////////______CHILL-MODE_____////////
                    /////////////////////////////////////


                    //if we are in GameMode ChillGameMode
                    case GameModes.ChillGameMode:

                        //RelaxMode is for ball only... no matter what r is assigned
                        if (r == 0 || r == 1 || r == 2)
                        {
                            //if the time is above 1.5 second(changed from zero to prevent a launch right after GameOver Screen
                            if (timer.timeLeft > 1.5f)
                            {
                                //launch ball
                                RequestBallSalvoFromBottom(bottomLauncherSalvoAmount + chillModeExtraBalls);
                                //increment the salvo amount.
                                bottomLauncherSalvoAmount++;
                                //we reset the time between launches...
                                timeBetweenLaunches = initialTimeBetweenLaunches;
                            }

                        }

                        //if the "bottomLauncherSalvoAmount" is at the "maximumSimultaneousBallLaunches"
                        if (bottomLauncherSalvoAmount == maximumSimultaneousBallLaunches)
                        {
                            //then we assign "resetBallLauncherAmount" to "bottomLauncherSalvoAmount"
                            bottomLauncherSalvoAmount = resetBallLauncherAmount + chillModeExtraBalls;
                        }


                        break;
                    default:

                        break;
                }

            }

        }


        /// <summary>
        /// This Method Stops all of the Object Launchers. 
        /// </summary>
        private void CancelAllObjectLaunchers()
        {
            //set the timer to 0
            timer.timeLeft = 0;
            //call StopAllCoroutines.
            StopAllCoroutines();
        }


        /// <summary>
        /// This Coroutine is called by "WaitToLaunchBottomBalls".  It fulfills the launching of the ball.  The Coroutine does the "actual firing".
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitToLaunchBottomBalls()
        {
            //we wait a random interval between minWaitTime and maxWaitTime
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            //create a random variable from 0 to the length of the bottomeBallLaunchers array.
            int r = Random.Range(0, bottomBallLaunchers.Length);
            //the we call LoadAndFireRandomBall on the bottomLauncherScriptReference element at position r.
            bottomLaunchersScriptReference[r].LoadAndFireRandomBall();
        }



        /// <summary>
        /// This Coroutine is called by "RequestBallSalvoFromSide".  It fulfills the launching of the ball from the side(frenzy).  The Coroutine does the "actual firing".
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitToLaunchSideBalls()
        {
            //we wait a random interval between minWaitTime and maxWaitTime
            yield return new WaitForSeconds(Random.Range(0f, 5f));
            //if the variable "cycleThruSideLaunchersForFrenzy" (the number of the side launchers we are on), is greater than the number of sideBallLaunchers then we set
            //the cycleThruSideLaunchersForFrenzy back to zero so we can start at the first side launcher again.
            if (cycleThruSideLaunchersForFrenzy > sideBallLaunchers.Length - 1)
            {
                //reset the cycleThruSideLaunchersForFrenzy to zero
                cycleThruSideLaunchersForFrenzy = 0;
            }
            //the we call LoadAndFireRandomBall on the sideLaunchersScriptReference element at position ("cycleThruSideLaunchersForFrenzy")
            sideLaunchersScriptReference[cycleThruSideLaunchersForFrenzy].LoadAndFireRandomBall();
            //we increment cycleThruSideLaunchersForFrenzy
            cycleThruSideLaunchersForFrenzy++;
        }



        /// <summary>
        /// This Coroutine is called by "RequestOtherObjectSalvoFromBottomLauncher".  It fulfills the launching of the ball.  The Coroutine does the "actual firing".
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        private IEnumerator WaitToLaunchBottomBomb()
        {
            //we wait a random interval between minWaitTime and maxWaitTime
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            //create a random variable from 0 to the length of the bottomBallLaunchers array.
            int r = Random.Range(0, bottomBallLaunchers.Length);
            //the we call LoadAndFireOtherObject on the bottomLauncherScriptReference element at position r, and pass the objectType.
            bottomLaunchersScriptReference[r].LoadAndFireBomb()/*LoadAndFireOtherObject(objectType)*/;
        }



        /// <summary>
        /// This Method is called When ball should be launched from the bottom.  We pass the number of balls that we want launched.  This Method start the actual coroutines that call the 
        /// fire method on the "Ball Launcher"
        /// </summary>
        /// <param name="numOfBall"></param>
        public void RequestBallSalvoFromBottom(int numOfBall)
        {
            //loop through  "numOfBall"
            for (int i = 0; i < numOfBall; i++)
            {
                //and call startCoroutine for the number iterations in the loop
                StartCoroutine(WaitToLaunchBottomBalls());
            }
        }


        /// <summary>
        /// This Method is called When ball should be launched from the side.  We pass the number of ball that we want launched.  This Method starts the actual coroutines that call the 
        /// fire method on the "Ball Launcher"
        /// </summary>
        /// <param name="numOfBalls"></param>
        public void RequestBallSalvoFromSide(int numOfBalls)
        {
            //loop through  "numOfBalls"
            for (int i = 0; i < numOfBalls; i++)
            {
                //and call startCoroutine for the number iterations in the loop
                StartCoroutine(WaitToLaunchSideBalls());
            }
        }


        /// <summary>
        /// This Method is called When a bomb should be launched from the bottom.  We pass "numOfBombs" for the AMT of other bombs we want.  This
        /// method starts the actual coroutines that call the fire method on the "Ball Launcher".    /// </summary>
        /// <param name="numOfBombs"></param>
        public void RequestBombSalvoFromBottomLauncher(int numOfBombs)
        {
            //loop through  "numOfBombs"
            for (int i = 0; i < numOfBombs; i++)
            {
                //and call startCoroutine on each iteration
                StartCoroutine(WaitToLaunchBottomBomb());
            }
        }

    }
}
