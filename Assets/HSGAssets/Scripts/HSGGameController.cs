using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This script controls the game, starting it, following game progress, and finishing it with game over.
/// </summary>
public class HSGGameController : MonoBehaviour
{
    // Holds the current event system
    internal EventSystem eventSystem;
    
    [Tooltip("A list of all the possible objects that you can guess")]
    public Sprite[] shapeList;
    internal int shapeIndex = 0;
    internal RectTransform shapeHolder;

    [Tooltip("This object holds all the answer buttons in the scene")]
    public Transform answersHolder;

    // A list of all the possible answers in the game, based on the names of all the objects in the game
    internal string[] allAnswers;

    public ParticleSystem revealEffect;
    
    [Tooltip("How many points we get for each word in the level. This value is multiplied by the number of the level we are on. Ex: Level 1 gives 100 points, Level 2 gives 200 points.")]
    public float bonusPerLevel = 100;
    internal float bonusMultiplier = 1;

    [Tooltip("How many extra seconds we add to the timer in the level.")]
    public float timeBonus = 3;

    [Tooltip("How many seconds are left before game is over")]
    public float time = 10;
    internal float timeLeft;
    internal bool timeRunning = false;

    [Tooltip("How long to wait between questions. During this the timer is stopped")]
    public float timeBetweenQuestions = 1;
    
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

    // The highest score we got in this game
    internal float highScore = 0;

    [Tooltip("If set to true, the game will repeat from the start, shuffling the questions again")]
    public bool endlessMode = false;

    // Holds the cursor object which shows us where we are aiming when using a keyboard or gamepad
    internal RectTransform cursor;

    [Tooltip("The menu that appears at the start of the game. This is used show a description of the game or add other buttons you may want")]
    public Transform startCanvas;

    [Tooltip("The menu that appears when we pause the game")]
    public Transform pauseCanvas;

    [Tooltip("The menu that appears if we lose the game, when time runs out")]
    public Transform gameOverCanvas;

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
    public AudioClip soundLevelUp;
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
    internal int objectIndex = 0;

    public GameObject levelNumberObject;
    public string _imagePath = "KBA/u1";
    public bool babyMode = false;

    void Awake()
    {
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
        _imagePath = LevelNumber.bookName + LevelNumber.numberOfLevel;

        Debug.Log(_imagePath);

        Object[] textures = Resources.LoadAll(_imagePath, typeof(Sprite));
        shapeList = new Sprite[textures.Length];
        for (int i = 0; i < textures.Length; i++)
        {
            shapeList[i] = (Sprite)textures[i];
        }

        // Disable multitouch so that we don't tap two answers at the same time
        Input.multiTouchEnabled = false;

        // Cache the current event system so we can position the cursor correctly
        eventSystem = UnityEngine.EventSystems.EventSystem.current;

        //Hide the game over ,victory ,and larger image screens
        if (gameOverCanvas) gameOverCanvas.gameObject.SetActive(false);
        if (victoryCanvas) victoryCanvas.gameObject.SetActive(false);
        if (pauseCanvas) pauseCanvas.gameObject.SetActive(false);

        // Assign the shape holder which displays the current shape
        if (GameObject.Find("ShapeHolder")) shapeHolder = GameObject.Find("ShapeHolder").GetComponent<RectTransform>();

        // Assign the score text object
        if (GameObject.Find("ScoreText")) scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

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

        // Get all the names of all the objects and place them in an array. We will use this later to assign correct and wrong answers for each object
        // Create a list of all possible answers in the game, based on all the objects in the game
        allAnswers = new string[shapeList.Length];

        //Assign the answers into the list of answers for each object
        for (index = 0; index < allAnswers.Length; index++)
        {
            // Get the name of the object and put it into the list
            allAnswers[index] = shapeList[index].name;
        }
        
        // Randomize the objects
        ShuffleArray<Object>(shapeList);

        if (shapeList.Length > 0)
        {
            // Create the first level
            StartCoroutine("UpdateLevel");

            // Show the answer objects
            foreach (Transform answerObject in answersHolder) answerObject.gameObject.SetActive(true);

            // Show the object holder
            shapeHolder.gameObject.SetActive(true);
        }
    }

    /// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
    {
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
            if (timeLeft > 0 && timeRunning == true )
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
        babyMode = LevelNumber.babyMode;
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
        if (babyMode)
        {
            timeLeft = 99999999;
            timerBar.gameObject.SetActive(false);
            timerIcon.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
        }
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

                // Play the hide animation of the object
                shapeHolder.GetComponent<Animator>().Play("ObjectHide");

                // Hide all the answer buttons
                for (index = 0; index < answersHolder.childCount; index++)
                {
                    // Make the answer button unclickable
                    answersHolder.GetChild(index).GetComponent<Button>().interactable = false;

                    // Play the hide animation of the button
                    answersHolder.GetChild(index).GetComponent<Animator>().Play("ButtonHide");
                }

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
        if (scoreText) scoreText.text = Mathf.CeilToInt(scoreCount).ToString();
    }

    /// <summary>
	/// Levels up, and increases the difficulty of the game
	/// </summary>
	IEnumerator LevelUp()
    {
        // Wait a little and stop the timer
        yield return new WaitForSeconds(1.0f);
        
        // Play the hide animation of the object
        shapeHolder.GetComponent<Animator>().Play("ObjectHide");

        // Hide all the answer buttons
        for (index = 0; index < answersHolder.childCount; index++)
        {
            // Play the hide animation of the button
            answersHolder.GetChild(index).GetComponent<Animator>().Play("ButtonHide");
        }

        // Wait for a while
        yield return new WaitForSeconds(0.5f);

        // Destroy the current object
        //Destroy(shapeHolder.gameObject);

        // Go to the next object
        objectIndex++;

        // If we reached the end of the object list, we win the game
        if (objectIndex >= shapeList.Length)
        {
            if ( endlessMode == true && isGameOver == false )
            {
                objectIndex = 0;

                // Randomize the objects
                ShuffleArray<Object>(shapeList);

                // Update the level attributes
                StartCoroutine("UpdateLevel");
            }
            else
            {
                // Update the level attributes
                StartCoroutine(Victory(0.5f));
            }
            
        }
        else if (isGameOver == false)
        {
            // Update the level attributes
            StartCoroutine("UpdateLevel");

            //If there is a source and a sound, play it from the source
            //if (soundSource && soundLevelUp) soundSource.GetComponent<AudioSource>().PlayOneShot(soundLevelUp);
        }
    }

    /// <summary>
    /// Updates the level, showing the next word
    /// </summary>
    IEnumerator UpdateLevel()
    {
        // Make all the answer buttons clickable again, and show them
        for (index = 0; index < answersHolder.childCount; index++)
        {
            // Make the answer button unclickable
            answersHolder.GetChild(index).GetComponent<Button>().interactable = true;

            // Play the hide animation of the button
            answersHolder.GetChild(index).GetComponent<Animator>().Play("ButtonShow");
        }

        shapeHolder.GetComponent<Image>().sprite = shapeList[objectIndex];

        // Rename the object based on the name of the model ( by default when instantiated it has a "(Clone)" suffix
        shapeHolder.name = shapeList[objectIndex].name;

        // Play the 'show' animation of the object
        shapeHolder.GetComponent<Animator>().Play("ObjectShow");

        // Turn the object black
        //shapeHolder.GetComponent<Image>().name = picName;
        //if ((picName != "red") || (picName != "blue") || (picName != "yellow") || (picName != "green") || (picName != "black") || (picName != "orange"))
        if (_imagePath != "KBA/u3" || _imagePath != "KBB/u8")
        {
            shapeHolder.GetComponent<Image>().color = Color.black;
        }
        // Randomize the list of answers
        ShuffleArray<string>(allAnswers);

        // Fill all the answer objects with wrong answers
        for (index = 0; index < answersHolder.childCount; index++)
        {
            answersHolder.GetChild(index).Find("Text").GetComponent<Text>().text = allAnswers[index];
        }

        // A check to see if the correct answer exists among the current answers
        bool answerExists = false;

        // Check if we have a correct answer among all the answers
        foreach (Transform answerObject in answersHolder)
        {
            if (answerObject.Find("Text").GetComponent<Text>().text == shapeHolder.name)
            {
                answerExists = true;
            }
        }

        // If we don't have a correct answer, add it to the answers
        if (answerExists == false) answersHolder.GetChild(0).Find("Text").GetComponent<Text>().text = shapeHolder.name;

        // Shuffle the answer buttons 
        for (index = 0; index < answersHolder.childCount; index++)
        {
            //Either place the letter object at the end of the letter list, or at the start, randomly.
            if (Random.value > 0.5f) answersHolder.GetChild(index).SetAsFirstSibling();
            else answersHolder.GetChild(index).SetAsLastSibling();
        }

        yield return new WaitForEndOfFrame();

        // Start the timer
        timeRunning = true;
        
        // Make the answer buttons interactable again
        foreach (Transform answerObject in answersHolder)
        {
            answerObject.GetComponent<Button>().enabled = true;
        }

        // Select the first answer in the list of answer buttons
        if (eventSystem && answersHolder.GetChild(0)) eventSystem.SetSelectedGameObject(answersHolder.GetChild(0).gameObject);
    }

    public static void ShuffleArray<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }

    public void ChooseAnswer(Transform answerButton)
    {
        // Stop the timer
        //timeRunning = false;

        // If the text of the answer is the same as the name of the object, then we are correct
        if (answerButton.Find("Text").GetComponent<Text>().text == shapeHolder.name)
        //if ( answerButton.Find("Text").GetComponent<Text>().text != string.Empty)
        {
            // Stop the timer
            timeRunning = false;

            // Add to the player's score
            ChangeScore(bonusPerLevel * (objectIndex+1));

            // If we have a time bonus text, show the time bonus we got
            if (timeBonusText)
            {
                // Activate the time bonus object
                timeBonusText.gameObject.SetActive(true);
                
                // Set the time bonus value in the text
                timeBonusText.GetComponent<Text>().text = "+" + timeBonus.ToString();

                // Play the animation
                if (timeBonusText.GetComponent<Animation>()) timeBonusText.GetComponent<Animation>().Play();
            }

            // Add bonus time to the timer
            timeLeft += timeBonus;

            // Play the 'correct' animation
            answerButton.GetComponent<Animator>().Play("ButtonCorrect");

            // Play the reveal particle effect
            if (revealEffect) revealEffect.Play();

            // If there is a source and a sound, play it from the source
            if (soundSource && soundCorrect) soundSource.GetComponent<AudioSource>().PlayOneShot(soundCorrect);

            // Gradually reveal the blacked out object
            StartCoroutine("RevealObject", 0.1f);
        }
        else
        {
            // Play the 'wrong' animation
            answerButton.GetComponent<Animator>().Play("ButtonWrong");

            // If there is a source and a sound, play it from the source
            if (soundSource && soundWrong) soundSource.GetComponent<AudioSource>().PlayOneShot(soundWrong);
        }

        // Make all the answer buttons unclickable
        for (index = 0; index < answersHolder.childCount; index++)
        {
            // Make the answer button unclickable
            answersHolder.GetChild(index).GetComponent<Button>().interactable = false;
        }

        // Go to the next level
        StartCoroutine("LevelUp");
    }

    IEnumerator RevealObject()
    {
        if (soundSource)
        {
            //soundSource.GetComponent<AudioSource>().Stop();

            soundSource.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/" + shapeHolder.GetComponent<Image>().name));
        }

        // Play the reveal animation of the object
        shapeHolder.GetComponent<Animator>().Play("ObjectReveal");

        // Set a timeout for revealing the object, so that we don't get stuck in an infinte loop
        float revealTimeout = 0.6f;
        
        // Animate the reveal from black to white ( showing real colors of object )
        while (revealTimeout > 0)
        {
            // Gradually change the color from black to the actual color of the object
            Color tempColor = Color.Lerp(shapeHolder.GetComponent<Image>().color, Color.white, Time.deltaTime * 5);

            // Apply the change to the revealed object
            shapeHolder.GetComponent<Image>().color = tempColor;

            // Wait a little, so that the effect is animated
            yield return new WaitForSeconds(Time.deltaTime);

            // Reduce from the reveal timeout
            revealTimeout -= Time.deltaTime;
        }
        
        // Set the final color to full white ( showing all the colors of the object )
        shapeHolder.GetComponent<Image>().color = Color.white;
    }

    /// <summary>
    /// Runs the game over event and shows the game over screen
    /// </summary>
    IEnumerator GameOver(float delay)
    {
        if (isGameOver == false)
        {
            isGameOver = true;

            if (timerIcon.GetComponent<Animation>()) timerIcon.GetComponent<Animation>().Stop();

            yield return new WaitForSeconds(delay);

            //Show the game over screen
            if (gameOverCanvas)
            {
                //Show the game over screen
                gameOverCanvas.gameObject.SetActive(true);
                
                //Write the score text, if it exists
                if (gameOverCanvas.Find("ScoreTexts/TextScore")) gameOverCanvas.Find("ScoreTexts/TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();

                //Check if we got a high score
                if (score > highScore)
                {
                    highScore = score;

                    //Register the new high score
                    PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "HighScore", score);
                }

                //Write the high sscore text
                gameOverCanvas.Find("ScoreTexts/TextHighScore").GetComponent<Text>().text = "TOP " + highScore.ToString();

                //If there is a source and a sound, play it from the source
                if (soundSource && soundGameOver) soundSource.GetComponent<AudioSource>().PlayOneShot(soundGameOver);
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
}
