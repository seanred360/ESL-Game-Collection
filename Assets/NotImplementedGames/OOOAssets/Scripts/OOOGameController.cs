using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OddOneOut
{
    /// <summary>
    /// This script controls the game, starting it, following game progress, and finishing it with game over.
    /// </summary>
    //[RequireComponent(typeof(Rigidbody))]
    public class OOOGameController : MonoBehaviour
    {
        // Holds the current event system
        internal EventSystem eventSystem;

        [Tooltip("The size of each image in the grid when starting the game")]
        public float imageSize = 100;

        [Tooltip("How much the image size changes after each level")]
        public float imageSizeChange = -5;

        [Tooltip("The smallest size allowed for an image in the grid")]
        public float minimumImageSize = 20;

        [Tooltip("An list of angles that will be chosen randomly for each object in the grid")]
        public int[] randomAngles = new int[] {0, 90, 180, 270};

        // A list of all the images the game. Each one of these is sliced into scrambled tiles.
        internal OOOImageGroup.ImageGroup[] imageGroups;
        internal Transform imageCurrent;

        // Holds the object that displays images in a grid ( "GridObject" in the heirarchy )
        internal RectTransform gridObject;

        // Holds the default image button that holds each image in the grid and can be pressed
        internal Button imageButton;

        [Tooltip("Randomize the list of images so that we don't get the same images each time we start the game")]
        public bool randomizeList = true;
        
        [Tooltip("How many points we get for each image in the level. This value is multiplied by the number of the level we are on. Ex: Level 1 gives 100 points, Level 2 gives 200 points.")]
        public float bonusPerLevel = 100;

        [Tooltip("How many extra seconds we add to the timer in the level. This value is multiplied by the number of the level we are on. Ex: Level 1 gives 5 seconds, Level 2 gives 10 seconds.")]
        public float timeBonusPerLevel = 5;

        [Tooltip("How many seconds are left before game is over")]
        public float time = 10;
        internal float timeLeft;
        internal bool timeRunning = false;

        [Tooltip("Should we reset the timer after the level? For example if we started at 30 seconds, we reset back to 30. This is not related to the additional time bonus per level increase")]
        public bool resetTimerAfterLevel = false;
        
        //The canvas of the timer in the game, the UI object and its various parts
        internal GameObject timerIcon;
        internal Image timerBar;
        internal Text timerText;
        internal Text timeBonusText;
        
        // The current level we are on, 1 being the first level in the game
        internal int currentLevel = 0;

        // The text object that shows which level we are in. This text object should be placed inside the GameController object and named "LevelText"
        internal Text levelText;

        [Tooltip("The name of a numbered level ( ex: Round 1, Level 1, etc")]
        public string levelNamePrefix = "Level";

        //The current score of the player
        internal float score = 0;
        internal float scoreCount = 0;

        // The score text object which displays the current score of the player. This text object should be placed inside the GameController object and named "LevelScore"
        internal Text scoreText;

        // The highest score we got in this game
        internal float highScore = 0;

        // Holds the cursor object which shows us where we are aiming when using a keyboard or gamepad
        internal RectTransform cursor;
        
        [Tooltip("The menu that appears when we pause the game")]
        public Transform pauseCanvas;

        [Tooltip("The menu that appears if we lose the game, when time runs out")]
        public Transform gameOverCanvas;

        [Tooltip("The menu that appears if we win the game, when finishing all the images")]
        public Transform victoryCanvas;

        // Is the game over?
        internal bool isGameOver = false;

        [Tooltip("The level of the main menu that can be loaded after the game ends")]
        public string mainMenuLevelName = "CS_StartMenu";

        [Tooltip("Various sounds and their source")]
        public AudioClip soundSelect;
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

        // Are we swapping two images now?
        internal bool isSwapping = false;

        // A general use index
        internal int index = 0;

        void Awake()
        {
            // Activate the pause canvas early on, so it can detect info about sound volume state
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(true);
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
            // Disable multitouch so that we don't tap two answers at the same time
            Input.multiTouchEnabled = false;

            // Cache the current event system so we can position the cursor correctly
            eventSystem = UnityEngine.EventSystems.EventSystem.current;

            //Hide the game over ,victory ,and larger image screens
            if (gameOverCanvas) gameOverCanvas.gameObject.SetActive(false);
            if (victoryCanvas) victoryCanvas.gameObject.SetActive(false);
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(false);

            // Assign the score text object
            if (GameObject.Find("ScoreText")) scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

            //Update the score
            UpdateScore();

            // Assign the level text object
            if (GameObject.Find("LevelText")) levelText = GameObject.Find("LevelText").GetComponent<Text>();
            
            // Assign the image grid which holds the parts of an image
            if (transform.Find("GridObject")) gridObject = transform.Find("GridObject").GetComponent<RectTransform>();

            // Assign the image button which holds each image in the grid
            if (transform.Find("ImageObject"))
            {
                imageButton = transform.Find("ImageObject").GetComponent<Button>();

                imageButton.gameObject.SetActive(false);
            }

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
            if ( GameObject.Find("Cursor") )
            {
                cursor = GameObject.Find("Cursor").GetComponent<RectTransform>();

                cursor.gameObject.SetActive(false);
            }
            
            //Assign the sound source for easier access
            if (GameObject.FindGameObjectWithTag(soundSourceTag)) soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);

            if (GetComponent<OOOImageGroup>()) imageGroups = GetComponent<OOOImageGroup>().imageGroups;
            else Debug.LogWarning("No images list found, please attach a SIGImages component to this gamecontroller");

            if (imageGroups.Length > 0 )
            {
                // Randomize the list of images
                if (randomizeList == true) imageGroups = ShuffleImages(imageGroups);

                // Create the first level
                StartCoroutine("UpdateLevel");
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
                if ( timeLeft > 0 && timeRunning == true )
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

                // Hide the game screen
                gameObject.SetActive(false);
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

            // Show the game screen
            gameObject.SetActive(true);

            // Select the button that we pressed before pausing
            if (eventSystem) eventSystem.SetSelectedGameObject(buttonBeforePause);
        }

        /// <summary>
        /// Shuffles the specified images list, and returns it
        /// </summary>
        /// <param name="images">A list of images</param>
        OOOImageGroup.ImageGroup[] ShuffleImages(OOOImageGroup.ImageGroup[] shuffledImages)
        {
            // Go through all the images and shuffle them
            for (index = 0; index < shuffledImages.Length; index++)
            {
                // Hold the image in a temporary variable
                OOOImageGroup.ImageGroup tempImage = shuffledImages[index];

                // Choose a random index from the question list
                int randomIndex = UnityEngine.Random.Range(index, shuffledImages.Length);

                // Assign a random question from the list
                imageGroups[index] = shuffledImages[randomIndex];

                // Assign the temporary question to the random question we chose
                shuffledImages[randomIndex] = tempImage;
            }

            return shuffledImages;
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
                    StartCoroutine(GameOver(0.5f));

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
		IEnumerator LevelUp(float delay)
        {
            // Stop the timer
            timeRunning = false;

            // If we have a time bonus text, show the time bonus we got
            if (timeBonusText && timeBonusPerLevel > 0 )
            {
                // Activate the time bonus object
                timeBonusText.gameObject.SetActive(true);

                // Set the time bonus value in the text
                timeBonusText.GetComponent<Text>().text = "+" + (timeBonusPerLevel * (currentLevel + 1)).ToString();

                // Play the animation
                if (timeBonusText.GetComponent<Animation>() ) timeBonusText.GetComponent<Animation>().Play();
            }

            // Add bonus time to the timer
            if ( resetTimerAfterLevel == true ) timeLeft = time + timeBonusPerLevel * (currentLevel + 1);
            else timeLeft += timeBonusPerLevel * (currentLevel + 1);

            // Wait for a while
            yield return new WaitForSeconds(delay);

            // Reduce the size of each image
            imageSize = Mathf.Clamp(imageSize + imageSizeChange, minimumImageSize, imageSize);

            // Go to the next level
            currentLevel++;
            
            // If the level text has an animation, play it
            if (levelText.GetComponent<Animation>()) levelText.GetComponent<Animation>().Play();

            // Update the level attributes
            StartCoroutine("UpdateLevel");

            //If there is a source and a sound, play it from the source
            if (soundSource && soundLevelUp) soundSource.GetComponent<AudioSource>().PlayOneShot(soundLevelUp);
        }
        
        /// <summary>
        /// Updates the level, showing the next image
        /// </summary>
        IEnumerator UpdateLevel()
        {
            // Display the current level we are on
            if (levelText) levelText.text = levelNamePrefix + " " + (currentLevel + 1).ToString();

            // Choose a random image from the current image group
            int randomImage = Random.Range(0, Mathf.FloorToInt(imageGroups[currentLevel].images.Length));

            // Set the image size in the grid
            gridObject.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * imageSize;

            // Set spacing in the grid to 0, to avoid miscalucaltion of number of images needed to fill grid
            gridObject.GetComponent<GridLayoutGroup>().spacing = Vector2.zero;

            // Calculate the number of images we need to create in order to fill the grid space
            float gridlWidth = gridObject.rect.width * gridObject.localScale.x;
            float gridHeight = gridObject.rect.height * gridObject.localScale.y;

            // Calculate the number columns and rows we need
            int columns = Mathf.FloorToInt(gridlWidth / imageSize);
            int rows = Mathf.FloorToInt(gridHeight / imageSize);

            // Create a set of images based on the total grid size
            for (index = 0; index < columns*rows; index++)
            {
                // Create a new image button from the default one
                Button newImageButton = Instantiate(imageButton) as Button;

                // Set the parent of the button as the grid
                newImageButton.transform.SetParent(gridObject);

                // Reset the scale and position of the button
                newImageButton.transform.localScale = Vector3.one;
                newImageButton.transform.localPosition = Vector3.zero;

                // Give a random rotation ( in 4 directions ) for the button
                if ( randomAngles.Length > 0 )    newImageButton.transform.eulerAngles = Vector3.forward * randomAngles[Mathf.FloorToInt(Random.Range(0, randomAngles.Length))];

                // Set the random image to the button
                newImageButton.GetComponent<Image>().sprite = imageGroups[currentLevel].images[randomImage];

                // Set the button to be a wrong button when clicking on it
                newImageButton.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine("WrongAnswer", newImageButton.GetComponent<Button>()); });

                // Activate the button object
                newImageButton.gameObject.SetActive(true);
            }

            // Choose a different image from the current group to be assigned to one of the buttons
            if (randomImage > 0) randomImage -= 1;// Random.Range(0, Mathf.FloorToInt(imageGroups[currentLevel].images.Length));
            else if (randomImage < imageGroups[currentLevel].images.Length - 1) randomImage += 1;// Random.Range(0, Mathf.FloorToInt(imageGroups[currentLevel].images.Length));
            //else randomImage = Random.Range(1, Mathf.FloorToInt(imageGroups[currentLevel].images.Length - 1));

            // Choose randomly one of the buttons and give it a different image from the group
            int randomButton = Random.Range(0, Mathf.FloorToInt(gridObject.childCount));

            // Assign the image to the random button
            gridObject.GetChild(randomButton).GetComponent<Image>().sprite = imageGroups[currentLevel].images[randomImage];

            // Remove the wrong button action from this button
            gridObject.GetChild(randomButton).GetComponent<Button>().onClick.RemoveAllListeners();

            // Add a correct button action to this button
            gridObject.GetChild(randomButton).GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine("CorrectAnswer", gridObject.GetChild(randomButton).GetComponent<Button>()); });
            
            yield return new WaitForEndOfFrame();

            // Start the timer
            timeRunning = true;
            
            // Wait for a moment
            yield return new WaitForSeconds(0.5f);

            // Make the image buttons interactable again
            foreach (Transform gridTile in gridObject) gridTile.GetComponent<Button>().enabled = true;

            // Select the first image in the image
            if ( eventSystem && gridObject.Find("00") ) eventSystem.SetSelectedGameObject(gridObject.Find("00").gameObject);
        }

        /// <summary>
        /// Selects the correct answer, highlight it, then remove all images and go to the next level
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        IEnumerator CorrectAnswer(Button button)
        {
            // Stop the timer
            timeRunning = false;

            //If there is a source and a sound, play it from the source
            if (soundSource && soundCorrect) soundSource.GetComponent<AudioSource>().PlayOneShot(soundCorrect);

            // Animate the image
            if (button.GetComponent<Animation>()) button.GetComponent<Animation>().Play("ImageCorrect");

            // Go through all the image objects, and play the correct animation
            foreach (Transform imageObject in gridObject)
            {
                // Make the image button non-interactable
                if (imageObject.GetComponent<Button>()) imageObject.GetComponent<Button>().enabled = false;
            }

            yield return new WaitForSeconds(0.3f);

            // Go through all the image objects, and play the correct animation
            foreach (Transform imageObject in gridObject)
            {
                // Animate the image
                if (imageObject.GetComponent<Animation>()) imageObject.GetComponent<Animation>().Play("ImageOutro");

                if (gridObject.childCount < 100 ) yield return new WaitForEndOfFrame();
                //yield return new WaitForSeconds(1.0f/ gridObject.childCount);


            }

            // Add the bonus to the score, based on the level number we are in
            ChangeScore(bonusPerLevel * (currentLevel + 1));

            // Wait for a moment
            yield return new WaitForSeconds(0.5f);

            // Destroy all image objects, except the default image object
            foreach (Transform imageObject in gridObject)
            {
                Destroy(imageObject.gameObject);
            }

            // If we reached the required images per level, level up
            if (currentLevel < imageGroups.Length - 1)
            {
                // Level up
                StartCoroutine("LevelUp", 0.2f);
            }
            else
            {
                // Run the victory event
                StartCoroutine(Victory(0.5f));
            }
        }

        /// <summary>
        /// Selects the wrong answer
        /// </summary>
        /// <param name="button"></param>
        public void WrongAnswer( Button button )
        {
            //If there is a source and a sound, play it from the source
            if (soundSource && soundWrong) soundSource.GetComponent<AudioSource>().PlayOneShot(soundWrong);

            // Animate the image
            if (button.GetComponent<Animation>()) button.GetComponent<Animation>().Play("ImageWrong");

            // Add the bonus to the score, based on the level number we are in
            //ChangeScore(bonusPerLevel * (currentLevel + 1));
        }
        
        /// <summary>
        /// Runs the game over event and shows the game over screen
        /// </summary>
        IEnumerator GameOver(float delay)
        {
            timeRunning = false;

            isGameOver = true;

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
                gameOverCanvas.Find("ScoreTexts/TextHighScore").GetComponent<Text>().text = "HIGH SCORE " + highScore.ToString();

                //If there is a source and a sound, play it from the source
                if (soundSource && soundGameOver) soundSource.GetComponent<AudioSource>().PlayOneShot(soundGameOver);
            }
        }

        /// <summary>
        /// Runs the victory event and shows the victory screen
        /// </summary>
        IEnumerator Victory(float delay)
        {
            timeRunning = false;

            isGameOver = true;

            yield return new WaitForSeconds(delay);

            //Show the game over screen
            if (victoryCanvas)
            {
                //Show the victory screen
                victoryCanvas.gameObject.SetActive(true);

                // If we have a TextScore and TextHighScore objects, then we are using the single player victory canvas
                if (victoryCanvas.Find("ScoreTexts/TextScore") && victoryCanvas.Find("ScoreTexts/TextHighScore"))
                {
                    //Write the score text, if it exists
                    victoryCanvas.Find("ScoreTexts/TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();

                    //Check if we got a high score
                    if (score > highScore)
                    {
                        highScore = score;

                        //Register the new high score
                        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "HighScore", score);
                    }

                    //Write the high sscore text
                    victoryCanvas.Find("ScoreTexts/TextHighScore").GetComponent<Text>().text = "HIGH SCORE " + highScore.ToString();
                }

                //If there is a source and a sound, play it from the source
                if (soundSource && soundVictory) soundSource.GetComponent<AudioSource>().PlayOneShot(soundVictory);
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
}