using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MemoryRepeatGame
{
    /// <summary>
    /// This script controls the game, starting it, following game progress, and finishing it with game over.
    /// </summary>
    public class MRGGameController : MonoBehaviour
    {
        // Holds the current event system
        internal EventSystem eventSystem;

        [Tooltip("The object that holds all the buttons on the pad")]
        public Transform buttonsHolder;
        //internal Button[] buttons;
        public Button[] buttons;

        [Tooltip("The sequence of buttons that need to be pressed in order to win. If you enter these manually, the game will always start with them. If you leave them empty, the game will be entirely random")]
        internal int[] sequence = new int[0];
        internal int[] sequenceTemp;
        internal int sequenceIndex = 0;

        [Tooltip("The delay between each two tones in seconds. Lower number means faster dialing and harder game")]
        public static float sequenceSpeed = 1f;

        [Tooltip("The number of sequence rounds we need to pass in order to win")]
        public int rounds = 5;
        internal int roundsCount = 0;

        [Tooltip("The text object on the dial pad that displays current sequence, win/lose status")]
        public Text roundsText;

        [Tooltip("The name prefix of a level or round or stage")]
        public string roundMessage = "ROUND ";

        [Tooltip("How many points we get for each word in the level. This value is multiplied by the number of the level we are on. Ex: Level 1 gives 100 points, Level 2 gives 200 points.")]
        public float bonusPerLevel = 100;
        internal float bonusMultiplier = 1;

        [Tooltip("How many extra seconds we add to the timer in the level.")]
        public float timeBonusPerLevel = 1;

        [Tooltip("How many seconds are left before game is over")]
        public float time = 10;
        internal float timeLeft;
        
        //The canvas of the timer in the game, the UI object and its various parts
        internal GameObject timerIcon;
        internal Image timerBar;
        internal Text timerText;
        internal Text timeBonusText;

        //The current score of the player
        internal float score = 0;
        internal float scoreCount = 0;

        // The score text object which displays the current score of the player. This text object should be placed inside the GameController object and named "LevelScore"
        internal Text scoreText;
        internal string scoreTextPadding;

        // The highest score we got in this game
        internal float highScore = 0;

        // Holds the cursor object which shows us where we are aiming when using a keyboard or gamepad
        internal RectTransform cursor;

        [Tooltip("The menu that appears at the start of the game. This is used show a description of the game or add other buttons you may want")]
        public Transform startCanvas;

        [Tooltip("The menu that appears when we pause the game")]
        public Transform pauseCanvas;

        [Tooltip("The menu that appears if we lose the game, when time runs out")]
        public Transform gameOverCanvas;
        public Transform gameoverBG;

        [Tooltip("The menu that appears if we win the game, when finishing all the words")]
        public Transform victoryCanvas;

        // Is the game over?
        internal bool isGameOver = false;

        [Tooltip("The level of the main menu that can be loaded after the game ends")]
        public string mainMenuLevelName = "HSGStartMenu";

        [Tooltip("Various sounds and their source")]
        public AudioClip soundCorrect;
        public AudioClip soundWrong;
        public AudioClip soundTimeUp;
        public AudioClip soundGameOver;
        public AudioClip soundVictory;

        public string soundSourceTag = "Sound";
        internal GameObject soundSource;

        // The button that will restart the game after game over
        public string confirmButton = "Submit";

        // The button that pauses the game. Clicking on the pause button in the UI also pauses the game
        public string pauseButton = "Cancel";
        internal bool isPaused = false;
        internal GameObject buttonBeforePause;

        // A general use index
        internal int index = 0;
        public Transform touchParticle;
        public Camera cam;
        public AudioManager audioManager;
        public GameObject buttonPrefab;
        public string _imagePath = "KBA/u1";
        public Sprite[] sprites;


        void Awake()
        {
            _imagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + LevelData.Singleton.wordGroupToUse;

            if (_imagePath == "0")
            {
                _imagePath = "KBA/u6";
                Debug.Log("Can't find the image path");
            }

            PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name + "HighScore");
            // Activate the pause canvas early on, so it can detect info about sound volume state
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(true);

            // If we have a start canvas, pause the game and display it. Otherwise, just start the game.
            if (startCanvas)
            {
                // Show the start screen
                startCanvas.gameObject.SetActive(true);
     
                Pause(false);
            }
        }

        /// <summary>
        /// Start is only called once in the lifetime of the behaviour.
        /// The difference between Awake and Start is that Start is only called if the script instance is enabled.
        /// This allows you to delay any initialization code, until it is really needed.
        /// Awake is always called before any Start functions.
        /// This allows you to order initialization of scripts
        /// </summary>
        void Start()
        {
            audioManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioManager>();

            // Disable multitouch so that we don't tap two answers at the same time
            Input.multiTouchEnabled = false;

            // Cache the current event system so we can position the cursor correctly
            eventSystem = UnityEngine.EventSystems.EventSystem.current;

            //Hide the game over ,victory ,and larger image screens
            if (gameOverCanvas) gameOverCanvas.gameObject.SetActive(false);
            if (victoryCanvas) victoryCanvas.gameObject.SetActive(false);
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(false);

            // Assign the buttons holder which holds all the buttons
            if ( buttonsHolder == null && GameObject.Find("ButtonsHolder")) buttonsHolder = GameObject.Find("ButtonsHolder").transform;

            Object[] loadedSprites = Resources.LoadAll(_imagePath, typeof(Sprite));
            sprites = new Sprite[loadedSprites.Length];
            for (int i = 0; i < loadedSprites.Length; i++)
            {
                sprites[i] = (Sprite)loadedSprites[i];
                GameObject newButton = Instantiate(buttonPrefab);
                newButton.transform.SetParent(buttonsHolder);
                newButton.GetComponent<Image>().sprite = sprites[i];
                newButton.GetComponent<AudioSource>().clip = (Resources.Load<AudioClip>("Sounds/" + newButton.GetComponent<Image>().sprite.name));
                newButton.GetComponent<Button>().onClick.AddListener(delegate () { PressButton(newButton.GetComponent<Button>()); });
            }

            // Create an array of buttons that will hold all the buttons
            buttons = new Button[buttonsHolder.childCount];

            // Get all the buttons from the buttons holder and assign them to the buttons array
            for ( index = 0; index < buttonsHolder.childCount; index++ )
            {
                buttons[index] = buttonsHolder.GetChild(index).GetComponent<Button>();
            }
            // Assign the score text object
            if (GameObject.Find("ScoreText"))
            {
                scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

                scoreTextPadding = scoreText.text;
            }

            //Update the score
            UpdateScore();

            //Get the highscore for the player
            highScore = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "HighScore", 0);

            //Assign the timer icon and text for quicker access
            if (GameObject.Find("TimerIcon"))
            {
                timerIcon = GameObject.Find("TimerIcon");
                if (GameObject.Find("TimerIcon/Bar")) timerBar = GameObject.Find("TimerIcon/Bar").GetComponent<Image>();
                if (GameObject.Find("TimerIcon/Text")) timerText = GameObject.Find("TimerIcon/Text").GetComponent<Text>();

                if (GameObject.Find("TimeBonusText"))
                {
                    timeBonusText = GameObject.Find("TimeBonusText").GetComponent<Text>();

                    timeBonusText.gameObject.SetActive(false);
                }
            }

            // Set the maximum value of the timer
            timeLeft = time;

            // Assign the cursor object and hide it
            if (GameObject.Find("Cursor"))
            {
                cursor = GameObject.Find("Cursor").GetComponent<RectTransform>();

                cursor.gameObject.SetActive(false);
            }

            //Assign the sound source for easier access
            if (GameObject.FindGameObjectWithTag(soundSourceTag)) soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);

            // Reset the round count. This is for example when you start playing and then abort the game, when you play again the round starts from 1
            roundsCount = 1;

            // If the sequence is not preset, add a note to it
            AddNote();

            // Show the first round on the screen text
            if (roundsText)
            {
                // If the game has just one round, don't display a number next to the round message
                roundsText.text = roundMessage + sequence.Length;
            }

            // Activate all the dial buttons so the player can interact with them
            for (index = 0; index < buttons.Length; index++) buttons[index].interactable = false;

        }

        /// <summary>
	    /// Update is called every frame, if the MonoBehaviour is enabled.
	    /// </summary>
	    void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
                PlayerPrefs.DeleteAll();
            // Make the score count up to its current value
            if (scoreCount < score)
            {
                // Count up to the courrent value
                scoreCount = Mathf.Lerp(scoreCount, score, Time.deltaTime * 10);

                // Update the score text
                UpdateScore();
            }

            //If the game is over, listen for the Restart and MainMenu buttons
            if (isGameOver == true)
            {
                //The jump button restarts the game
                if (Input.GetButtonDown(confirmButton))
                {
                    Restart();
                }

                //The pause button goes to the main menu
                if (Input.GetButtonDown(pauseButton))
                {
                    MainMenu();
                }
            }
            else
            {
                if (cursor)
                {
                    // If we use the keyboard or gamepad, keyboardControls take effect
                    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) cursor.gameObject.SetActive(true);

                    // If we move the mouse in any direction or click it, or touch the screen on a mobile device, then keyboard/gamepad controls are lost
                    if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0 || Input.GetMouseButtonDown(0) || Input.touchCount > 0) cursor.gameObject.SetActive(false);

                    // Place the cursor at the position of the selected object
                    if (eventSystem.currentSelectedGameObject) cursor.position = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().position;
                }

                // Count down the time until game over
                if (timeLeft > 0 )
                {
                    // Count down the time
                    timeLeft -= Time.deltaTime;
                }

                // Update the timer
                UpdateTime();

                //Toggle pause/unpause in the game
                if (Input.GetButtonDown(pauseButton))
                {
                    if (isPaused == true) Unpause();
                    else Pause(true);
                }
            }
        }

        /// <summary>
        /// Adds a random note to the sequence
        /// </summary>
        public void AddNote()
        {
            // Keep the current sequence in a temporary array
            sequenceTemp = sequence;

            // Create a new sequence with one extra note slot
            sequence = new int[sequence.Length + 1];

            // Add the current sequence to the new array
            for (index = 0; index < sequenceTemp.Length; index++) sequence[index] = sequenceTemp[index];

            // Choose a random dial button ( a random note )
            int randomNote = Mathf.FloorToInt(Random.Range(0, buttons.Length));

            // Set the random note as the last note in the sequence
            sequence[sequence.Length - 1] = randomNote;

            // Play the sequence of notes
            StartCoroutine("PlaySequence");
        }

        /// <summary>
        /// Play the sequence of notes, highlight each played button, and play each note with the relevant pitch
        /// </summary>
        /// <returns></returns>
        IEnumerator PlaySequence()
        {
            // Clear the message text
            //if (roundsText) roundsText.text = "";

            if (EventSystem.current) EventSystem.current.SetSelectedGameObject(null);

            // Delay for 1 second between rounds
            yield return new WaitForSeconds(1);

            // Show the current round on the screen text
            if (roundsText) roundsText.text = roundMessage + sequence.Length;

            // Deactivate all the dial buttons so the player can't interact with them while the sequence is playing
            for (index = 0; index < buttons.Length; index++) buttons[index].interactable = false;

            // Go through the sequence, play each note at the correct pitch, and highlight the button
            foreach (int note in sequence)
            {
                // Play the button sound, if it exists
                if (buttons[note].GetComponent<AudioSource>()) buttons[note].GetComponent<AudioSource>().Play();

                // Play the button highlight animation
                //buttons[note].GetComponent<Animator>().Play("ButtonPress"); //sean

                // Delay for a while before playing the next nore in the sequence
                yield return new WaitForSeconds(sequenceSpeed);
            }

            // Activate all the dial buttons so the player can interact with them again
            for (index = 0; index < buttons.Length; index++) buttons[index].interactable = true;

            //if (EventSystem.current) EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        }


        /// <summary>
        /// Presses a button on the dial pad, playing the relevant note and checking if we are following the sequence or not
        /// </summary>
        /// <param name="sourceButton"></param>
        public void PressButton(Button sourceButton)
        {
            // Find the index of the button we pressed so we can compare it in the sequence and play the correct pitch
            int buttonIndex = sourceButton.transform.GetSiblingIndex();

            // Play the button highlight animation
            if(touchParticle.gameObject.activeSelf == true)
            {
                touchParticle.gameObject.SetActive(false);
            }
            touchParticle.gameObject.SetActive(true);
            touchParticle.transform.position = cursor.position;
            buttons[buttonIndex].GetComponent<Animator>().Play("ButtonPress");
            iTween.ShakePosition(cam.gameObject, new Vector3(.035f, .035f, 0), 0.3f);

            // If we pressed the correct note in the sequence, go to the next note
            if (buttonIndex == sequence[sequenceIndex])
            {
                // Go to the next note in the sequence
                sequenceIndex++;

                // If we reached the end of the sequence go to the next round
                if (sequenceIndex >= sequence.Length)
                {
                    // Add to the player's score
                    //ChangeScore(bonusPerLevel * roundsCount); this is the old way to do score
                    ChangeScore(bonusPerLevel);

                    // If we have a time bonus text, show the time bonus we got
                    if (timeBonusText)
                    {
                        // Activate the time bonus object
                        timeBonusText.gameObject.SetActive(true);
                        timeBonusText.transform.GetChild(0).gameObject.SetActive(true);
                        timeBonusText.transform.GetChild(0).transform.position = timeBonusText.transform.position;

                        // Set the time bonus value in the text
                        timeBonusText.GetComponent<Text>().text = "+" + (timeBonusPerLevel * roundsCount).ToString();

                        // Play the animation
                        if (timeBonusText.GetComponent<Animation>()) timeBonusText.GetComponent<Animation>().Play();
                    }

                    // Add bonus time to the timer
                    timeLeft += timeBonusPerLevel * roundsCount;

                    // Update the ( maximum ) time, so the fillamount is correct
                    time = timeLeft;

                    // Go to the next round and add a note
                    if (roundsCount < rounds)
                    {
                        // Increase round count
                        roundsCount++;
                        iTween.ShakePosition(cam.gameObject, new Vector3(.5f, .5f, 0), 0.3f);

                        // Reset the sequence note index
                        sequenceIndex = 0;

                        // Add a note to the sequence
                        AddNote();
                    }
                    else // Otherwise, if we reached the last round, win the game
                    {
                        // Update the level attributes
                        StartCoroutine(Victory(0.5f));
                    }

                    if ( soundSource && soundCorrect)
                    {
                        soundSource.GetComponent<AudioSource>().pitch = 1;

                        soundSource.GetComponent<AudioSource>().PlayOneShot(soundCorrect);
                    }
                }
            }
            else // Otherwise, if we pressed the wrong note we lose the game
            {
                // Deactivate all the dial buttons so the player can't interact with them after we lose
                for (index = 0; index < buttons.Length; index++) buttons[index].interactable = false;

                // Reset the sequence index
                sequenceIndex = 0;

                // Update the level attributes
                StartCoroutine(GameOver(0.5f));

                if (soundSource && soundWrong )
                {
                    soundSource.GetComponent<AudioSource>().pitch = 1;

                    soundSource.GetComponent<AudioSource>().PlayOneShot(soundWrong);
                }

            }
        }


        /// <summary>
        /// Pause the game, and shows the pause menu
        /// </summary>
        /// <param name="showMenu">If set to <c>true</c> show menu.</param>
        public void Pause(bool showMenu)
        {
            isPaused = true;

            //Set timescale to 0, preventing anything from moving
            Time.timeScale = 0;

            // Remember the button that was selected before pausing
            if (eventSystem) buttonBeforePause = eventSystem.currentSelectedGameObject;

            //Show the pause screen and hide the game screen
            if (showMenu == true)
            {
                if (pauseCanvas) pauseCanvas.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Resume the game
        /// </summary>
        public void Unpause()
        {
            isPaused = false;

            //Set timescale back to the current game speed
            Time.timeScale = 1;

            //Hide the pause screen and show the game screen
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(false);

            // Select the button that we pressed before pausing
            if (eventSystem) eventSystem.SetSelectedGameObject(buttonBeforePause);
        }

        /// <summary>
        /// Updates the timer text, and checks if time is up
        /// </summary>
        void UpdateTime()
        {
            // Update the time only if we have a timer icon canvas assigned
            if (timerIcon)
            {
                // Update the timer circle, if we have one
                if (timerBar)
                {
                    // If the timer is running, display the fill amount left. Otherwise refill the amount back to 100%
                    timerBar.fillAmount = timeLeft / time;
                }

                // Update the timer text, if we have one
                if (timerText)
                {
                    // If the timer is running, display the timer left. Otherwise hide the text
                    timerText.text = Mathf.RoundToInt(timeLeft).ToString();
                }

                // Time's up!
                if (timeLeft <= 0)
                {
                    // Run the game over event
                    StartCoroutine(GameOver(1.5f));
                    
                    // Play the timer icon Animator
                    if (timerIcon.GetComponent<Animation>()) timerIcon.GetComponent<Animation>().Play();

                    //If there is a source and a sound, play it from the source
                    if (soundSource && soundTimeUp) soundSource.GetComponent<AudioSource>().PlayOneShot(soundTimeUp);
                }
            }
        }

        /// <summary>
	    /// Change the score
	    /// </summary>
	    /// <param name="changeValue">Change value</param>
	    void ChangeScore(float changeValue)
        {
            score += changeValue;

            //Update the score
            UpdateScore();
        }

        /// <summary>
        /// Updates the score value and checks if we got to the next level
        /// </summary>
        void UpdateScore()
        {
            //Update the score text
            if (scoreText) scoreText.text = Mathf.CeilToInt(scoreCount).ToString(scoreTextPadding);
        }

        /// <summary>
        /// Runs the game over event and shows the game over screen
        /// </summary>
        IEnumerator GameOver(float delay)
        {
            if (isGameOver == false)
            {
                isGameOver = true;

                if (soundSource) soundSource.GetComponent<AudioSource>().pitch = 1;

                if (timerIcon.GetComponent<Animation>()) timerIcon.GetComponent<Animation>().Stop();

                yield return new WaitForSeconds(delay);

                //Show the game over screen
                if (gameOverCanvas)
                {
                    //Show the game over screen
                    gameOverCanvas.gameObject.SetActive(true);
                    gameoverBG.gameObject.SetActive(true);
                    iTween.ShakePosition(cam.gameObject, new Vector3(1f, 1f, 0), .3f);

                    //Write the score text, if it exists
                    if (gameOverCanvas.Find("ScoreTexts/TextScore")) gameOverCanvas.Find("ScoreTexts/TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();

                    //Write the high sscore text
                    gameOverCanvas.Find("ScoreTexts/TextHighScore").GetComponent<Text>().text = "TOP " + highScore.ToString();

                    //If there is a source and a sound, play it from the source
                    if (soundSource && soundGameOver) soundSource.GetComponent<AudioSource>().PlayOneShot(soundGameOver);

                    //Check if we got a high score
                    if (score > highScore)
                    {
                        highScore = score;
                        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "HighScore", score);
                        yield return new WaitForSeconds(soundGameOver.length);
                        audioManager.PlaySFX(Random.Range(1, 2));//high score voice
                        yield return new WaitForSeconds(1.5f);
                        // score number voice
                        if (score == 100)
                        {
                            audioManager.PlaySFX(3);
                        }
                        if (score == 200)
                        {
                            audioManager.PlaySFX(4);
                        }
                        if (score == 300)
                        {
                            audioManager.PlaySFX(5);
                        }
                        if (score == 400)
                        {
                            audioManager.PlaySFX(6);
                        }
                        if (score == 500)
                        {
                            audioManager.PlaySFX(7);
                        }
                        if (score == 600)
                        {
                            audioManager.PlaySFX(8);
                        }
                        if (score == 700)
                        {
                            audioManager.PlaySFX(9);
                        }
                        if (score == 800)
                        {
                            audioManager.PlaySFX(10);
                        }
                        if (score == 900)
                        {
                            audioManager.PlaySFX(11);
                        }
                        if (score == 1000)
                        {
                            audioManager.PlaySFX(12);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Runs the victory event and shows the victory screen
        /// </summary>
        IEnumerator Victory(float delay)
        {
            if (isGameOver == false)
            {
                isGameOver = true;

                if (soundSource )    soundSource.GetComponent<AudioSource>().pitch = 1;

                if (timerIcon.GetComponent<Animation>()) timerIcon.GetComponent<Animation>().Stop();

                yield return new WaitForSeconds(delay);

                //Show the game over screen
                if (victoryCanvas)
                {
                    //Show the victory screen
                    victoryCanvas.gameObject.SetActive(true);

                    // If we have a TextScore and TextHighScore objects, then we are using the single player victory canvas
                    if (victoryCanvas.Find("Base/ScoreTexts/TextScore") && victoryCanvas.Find("Base/ScoreTexts/TextHighScore"))
                    {
                        //Write the score text, if it exists
                        victoryCanvas.Find("Base/ScoreTexts/TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();

                        //Check if we got a high score
                        if (score > highScore)
                        {
                            highScore = score;

                            //Register the new high score
                            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "HighScore", score);
                        }

                        //Write the high sscore text
                        victoryCanvas.Find("Base/ScoreTexts/TextHighScore").GetComponent<Text>().text = "HIGH SCORE " + highScore.ToString();
                    }

                    //If there is a source and a sound, play it from the source
                    if (soundSource && soundVictory) soundSource.GetComponent<AudioSource>().PlayOneShot(soundVictory);
                }
            }
        }

        /// <summary>
        /// Restart the current level
        /// </summary>
        void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Restart the current level
        /// </summary>
        void MainMenu()
        {
            SceneManager.LoadScene(mainMenuLevelName);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
