using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FindTheObject
{
    /// <summary>
    /// This script controls the game, starting it, following game progress, and finishing it with game over.
    /// </summary>
    //[RequireComponent(typeof(Rigidbody))]
    public class FTOGameController : MonoBehaviour
    {
        // Holds the current event system
        internal EventSystem eventSystem;

        //A list of all the possible objects in the game. The hidden object for each level will be randomly selected from this list
        internal Sprite[] imagesList;
        internal string[] textsList;
        private int hiddenObjectIndex;
        internal GameObject objectIcon;
        
        [Tooltip("How many objects we need to find to win this level")]
        public Vector2 hiddenObjectsRange = new Vector2(1, 3);
        internal int numberOfHiddenObjects = 1;

        [Tooltip("The size of each object in the grid when starting the game")]
        public float imageSize = 100;

        [Tooltip("How much the image size changes after each level")]
        public float imageSizeChange = -5;

        [Tooltip("The smallest size allowed for an image in the grid")]
        public float minimumImageSize = 20;

        [Range(-0.5f, 0)]
        [Tooltip("How close to each other the objects are")]
        public float gapRatio = 0;

        //How many objects we found so far this level
        internal int foundObjects = 0;

        // Holds the object that displays images in a grid ( "GridObject" in the heirarchy )
        internal RectTransform gridObject;

        // Holds the default image button that holds each image in the grid and can be pressed
        internal Button imageButton;
        
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

        public bool playWordSound = true;

        public bool babyMode = false;

        void Awake()
        {
            babyMode = LevelNumber.babyMode;
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

            if (imagesList != null && imagesList.Length > 0)
            {
                transform.Find("ObjectIcon/Icon/Text").gameObject.SetActive(false);
            }
            else if (textsList != null && textsList.Length > 0)
            {
                transform.Find("ObjectIcon/Icon").GetComponent<Image>().enabled = false;
            }


            if ((imagesList != null && imagesList.Length > 0) || (textsList != null && textsList.Length > 0))
            {
                // Create the first level
                StartCoroutine("CreateLevel");
            }
            else
            {
                Debug.LogWarning("No images/texts list found, please add a FTOImages or FTOTexts component to the gamecontroller");
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
            if (babyMode)
            {
                timeLeft = 9999999;
                timerText.gameObject.SetActive(false);
                timerIcon.gameObject.SetActive(false);
                timerBar.gameObject.SetActive(false);
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

            if ( (imagesList != null && imagesList.Length > 0) || (textsList != null && textsList.Length > 0) )
            {
                // Create the first level
                StartCoroutine("CreateLevel");
            }

            //If there is a source and a sound, play it from the source
            if (soundSource && soundLevelUp) soundSource.GetComponent<AudioSource>().PlayOneShot(soundLevelUp);
        }

        /// <summary>
        /// This function creates a level, placing objects evenly within the object area, and then adding the hidden objects over them.
        /// </summary>
        IEnumerator CreateLevel()
        {
            // Display the current level we are on
            if (levelText) levelText.text = levelNamePrefix + " " + (currentLevel + 1).ToString();
            
            // Set the image size in the grid
            gridObject.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * imageSize;

            // Set spacing in the grid to 0, to avoid miscalucaltion of number of images needed to fill grid
            gridObject.GetComponent<GridLayoutGroup>().spacing = Vector2.one * imageSize * gapRatio - Vector2.one * 5;

            // Calculate the number of images we need to create in order to fill the grid space
            float gridlWidth = gridObject.rect.width * gridObject.localScale.x;
            float gridHeight = gridObject.rect.height * gridObject.localScale.y;

            // Calculate the number columns and rows we need
            int columns = Mathf.FloorToInt(gridlWidth / (imageSize * (1+gapRatio)));// * 2;
            int rows = Mathf.FloorToInt(gridHeight / (imageSize * (1+gapRatio)));// * 2;

            // Randomize the list of pairs
            if (imagesList != null && imagesList.Length > 0)    imagesList = ShuffleSprites(imagesList);
            else if (textsList != null && textsList.Length > 0)    textsList = ShuffleTexts(textsList);

            //Choose a random number of objects to be hidden
            numberOfHiddenObjects = Mathf.RoundToInt(Random.Range(hiddenObjectsRange.x, hiddenObjectsRange.y));

            // A counter for the images in the list. If we reach the end 
            index = 0;

            // Create a set of images based on the total grid size
            while ( index < columns * rows - (numberOfHiddenObjects - 1) )
            {
                // Create a new image button from the default one
                Button newImageButton = Instantiate(imageButton) as Button;

                // Set the parent of the button as the grid
                newImageButton.transform.SetParent(gridObject);

                // Reset the scale and position of the button
                //newImageButton.transform.Find("Icon").localPosition += Vector3.right * Random.Range(-0.5f * imageSize, 0.5f * imageSize);

                //newImageButton.transform.Find("Icon").localScale *= Random.Range(0.9f,1.1f);

                if (imagesList != null && imagesList.Length > 0)
                {
                    //newImageButton.transform.eulerAngles = new Vector3(newImageButton.transform.eulerAngles.x, newImageButton.transform.eulerAngles.y, Random.Range(0, 360));
                    newImageButton.transform.Find("Icon").localEulerAngles = Vector3.forward * Random.Range(0, 360);

                    if (index == 0) newImageButton.transform.Find("Icon").GetComponent<Image>().sprite = imagesList[0];
                    else newImageButton.transform.Find("Icon").GetComponent<Image>().sprite = imagesList[Mathf.FloorToInt(Random.Range(1, imagesList.Length))];

                    newImageButton.transform.Find("Text").gameObject.SetActive(false);
                }
                else if (textsList != null && textsList.Length > 0)
                {
                    newImageButton.transform.Find("Text").localEulerAngles = Vector3.forward * Random.Range(0, 360);

                    // Set the random image to the button
                    if (index == 0) newImageButton.transform.Find("Text").GetComponent<Text>().text = textsList[0];
                    else newImageButton.transform.Find("Text").GetComponent<Text>().text = textsList[Mathf.FloorToInt(Random.Range(1, textsList.Length))];

                    newImageButton.transform.Find("Icon").gameObject.SetActive(false);

                }

                // Set the button to be a wrong button when clicking on it
                newImageButton.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine("WrongAnswer", newImageButton.GetComponent<Button>()); });

                // Activate the button object
                newImageButton.gameObject.SetActive(true);


                //newImageButton.transform.localEulerAngles = Vector3.forward * Random.Range(0, 360);

                index++;
            }

            // Make the first image in the randomized list as the one that is hidden
            Button firstImageButton = gridObject.GetChild(0).GetComponent<Button>();

            // Remove the wrong button action from this button
            firstImageButton.onClick.RemoveAllListeners();

            // Add a correct button action to this button
            firstImageButton.onClick.AddListener(delegate { StartCoroutine("CorrectAnswer", firstImageButton); });

            // Set the icon so we know what we should be looking for
            if (imagesList != null && imagesList.Length > 0) transform.Find("ObjectIcon/Icon").GetComponent<Image>().sprite = imagesList[0];
            else if (textsList != null && textsList.Length > 0) transform.Find("ObjectIcon/Icon/Text").GetComponent<Text>().text = textsList[0];

            // Show the number of objects we have to find
            //transform.Find("ObjectIcon/Text").GetComponent<Text>().text = numberOfHiddenObjects.ToString();
            transform.Find("ObjectIcon/Text").GetComponent<Text>().text = transform.Find("ObjectIcon/Icon").GetComponent<Image>().sprite.name + " X" + numberOfHiddenObjects.ToString();

            if (GetComponent<FTOTexts>())
            {
                soundSource.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/" + transform.Find("ObjectIcon/Icon/Text").GetComponent<Text>().text));
                transform.Find("ObjectIcon/Text").GetComponent<Text>().text = numberOfHiddenObjects.ToString();
            }
            if (GetComponent<FTOImages>())
            {
                soundSource.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/" + transform.Find("ObjectIcon/Icon").GetComponent<Image>().sprite.name));
            }

            index = 1;

            while ( index < numberOfHiddenObjects )
            {
                // Create a new image button from the default one
                Button newImageButton = Instantiate(gridObject.GetChild(0).GetComponent<Button>()) as Button;

                // Set the parent of the button as the grid
                newImageButton.transform.SetParent(gridObject);

                // Reset the scale and position of the button
                //newImageButton.transform.localScale = Vector3.one;
                //newImageButton.transform.localPosition = Vector3.zero;

                newImageButton.transform.Find("Icon").localEulerAngles = Vector3.forward * Random.Range(0, 360);


                // Remove the wrong button action from this button
                newImageButton.onClick.RemoveAllListeners();

                // Add a correct button action to this button
                newImageButton.onClick.AddListener(delegate { StartCoroutine("CorrectAnswer", newImageButton); });

                // Activate the button object
                newImageButton.gameObject.SetActive(true);

                index++;
            }

            // We will do several randomization passes on the pairs array, shuffling them
            int randomCount = gridObject.childCount - 1;

            // Repeat the shuffle several times
            while (randomCount > 0)
            {
                int randomIndex = Mathf.FloorToInt(Random.Range(0, randomCount));

                // Choose one of the elements in the array randomly and put it at the start
                if (gridObject.GetChild(randomIndex)) gridObject.GetChild(randomIndex).SetAsFirstSibling();

                randomIndex = Mathf.FloorToInt(Random.Range(0, randomCount));

                // Choose another of the elements in the array randomly and put it at the end
                if (gridObject.GetChild(randomIndex)) gridObject.GetChild(randomIndex).SetAsLastSibling();

                randomCount--;
            }


            yield return new WaitForEndOfFrame();

            // Start the timer
            timeRunning = true;

            // Wait for a moment
            yield return new WaitForSeconds(0.5f);

            // Make the image buttons interactable again
            foreach (Transform gridTile in gridObject) gridTile.GetComponent<Button>().enabled = true;

            // Select the first image in the image
            if (eventSystem && gridObject.Find("00")) eventSystem.SetSelectedGameObject(gridObject.Find("00").gameObject);
        }

        /// <summary>
        /// Shuffles the specified sprite objects list, and returns it
        /// </summary>
        /// <param name="objects">A list of sprite objects</param>
        Sprite[] ShuffleSprites(Sprite[] objects)
        {
            // Go through all the sprite objects and shuffle them
            for (index = 0; index < objects.Length; index++)
            {
                // Hold the pair in a temporary variable
                Sprite tempPair = objects[index];

                // Choose a random index from the sprite objects list
                int randomIndex = UnityEngine.Random.Range(index, objects.Length);

                // Assign a random sprite pair from the list
                objects[index] = objects[randomIndex];

                // Assign the temporary sprite pair to the random question we chose
                objects[randomIndex] = tempPair;
            }

            return objects;
        }

        /// <summary>
        /// Shuffles the specified sprite objects list, and returns it
        /// </summary>
        /// <param name="objects">A list of sprite objects</param>
        string[] ShuffleTexts(string[] objects)
        {
            // Go through all the string  and shuffle them
            for (index = 0; index < objects.Length; index++)
            {
                // Hold the pair in a temporary variable
                string tempPair = objects[index];

                // Choose a random index from the string  list
                int randomIndex = UnityEngine.Random.Range(index, objects.Length);

                // Assign a random string from the list
                objects[index] = objects[randomIndex];

                // Assign the temporary string  to the random question we chose
                objects[randomIndex] = tempPair;
            }

            return objects;
        }


        /// <summary>
        /// Selects the correct answer, highlight it, then remove all images and go to the next level
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        IEnumerator CorrectAnswer(Button button)
        {
            if ((GetComponent<FTOTexts>()) && (playWordSound))
            {
                soundSource.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/" + transform.Find("ObjectIcon/Icon/Text").GetComponent<Text>().text));
                transform.Find("ObjectIcon/Text").GetComponent<Text>().text = numberOfHiddenObjects.ToString();
            }
            if ((GetComponent<FTOImages>()) && (playWordSound))
            {
                soundSource.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/" + transform.Find("ObjectIcon/Icon").GetComponent<Image>().sprite.name));
            }

            //If there is a source and a sound, play it from the source
            if (soundSource && soundCorrect) soundSource.GetComponent<AudioSource>().PlayOneShot(soundCorrect);
           
            // Animate the image
            if (button.GetComponent<Animation>()) button.GetComponent<Animation>().Play("ImageCorrect");

            // Set the icon so we know what we should be looking for
            if (imagesList != null && imagesList.Length > 0) button.transform.Find("Icon").GetComponent<Image>().color = new Color(1,1,1,.5f);
            else if (textsList != null && textsList.Length > 0) button.transform.Find("Text").GetComponent<Text>().color = new Color(1, 1, 1, .5f);

            numberOfHiddenObjects--;

            // Update the number of objects we have to find
            //transform.Find("ObjectIcon/Text").GetComponent<Text>().text = numberOfHiddenObjects.ToString();
            transform.Find("ObjectIcon/Text").GetComponent<Text>().text = transform.Find("ObjectIcon/Icon").GetComponent<Image>().sprite.name + " X" + numberOfHiddenObjects.ToString();

            if (GetComponent<FTOTexts>())
                transform.Find("ObjectIcon/Text").GetComponent<Text>().text = numberOfHiddenObjects.ToString();

            button.interactable = false;
            
            if (numberOfHiddenObjects <= 0)
            {
                // Stop the timer
                timeRunning = false;

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

                    if (gridObject.childCount < 100) yield return new WaitForEndOfFrame();
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

                if (imagesList != null && currentLevel < imagesList.Length - 1)
                {
                    // Level up
                    StartCoroutine("LevelUp", 0.2f);
                }
                else if (textsList != null && currentLevel < textsList.Length - 1)
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