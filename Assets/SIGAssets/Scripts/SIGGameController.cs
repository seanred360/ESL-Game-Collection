using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScrambledImageGame
{
    /// <summary>
    /// This script controls the game, starting it, following game progress, and finishing it with game over.
    /// </summary>
    //[RequireComponent(typeof(Rigidbody))]
    public class SIGGameController : MonoBehaviour
    {
        // Holds the current event system
        internal EventSystem eventSystem;

        [Tooltip("The size of the grid the image is sliced to")]
        public Vector2 gridSize = new Vector2(2, 2);

        [Tooltip("Increase the grid size by this value when finishing a level. The increase is applied alternately to either the width or height of the grid")]
        public int increaseGridSize = 1;

        // A list of all the images the game. Each one of these is sliced into scrambled tiles.
        internal Texture[] images;
        internal Transform imageCurrent;

        // Holds the default image object that displays a part of the image in the grid ( "ImageObject" in the heirarchy )
        internal RawImage imageObject;

        // Holds the object that displays images in a grid ( "GridObject" in the heirarchy )
        internal RectTransform gridObject;
        
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

        [Tooltip("How lng to wait after unscrambling the image, and before moving to the next level")]
        public float delayAfterLevel = 1;

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
        public AudioClip soundSwap;
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
        internal int indexB = 0;

        public bool babyMode;
        public GameObject imageNameButton;

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
            
            // Assign the image object and hide it
            if (GameObject.Find("ImageObject"))
            {
                imageObject = transform.Find("ImageObject").GetComponent<RawImage>();
                imageObject.gameObject.SetActive(false);
            }

            // Assign the image grid which holds the parts of an image
            if (GameObject.Find("GridObject")) gridObject = transform.Find("GridObject").GetComponent<RectTransform>();

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

            if (GetComponent<SIGImages>()) images = GetComponent<SIGImages>().images;
            else Debug.LogWarning("No images list found, please attach a SIGImages component to this gamecontroller");

            if ( images.Length > 0 )
            {
                // Randomize the list of images
                if (randomizeList == true) images = ShuffleImages(images);

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

                // Tilt the image grid based on touch position, and return to center when we pass a level
                if (timeRunning == true) gridObject.eulerAngles = new Vector3((Input.mousePosition.y - Screen.height * 0.5f) * 0.01f, (Input.mousePosition.x - Screen.width * 0.5f) * -0.01f, 0);
                else gridObject.eulerAngles = Vector3.zero;
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
        Texture[] ShuffleImages(Texture[] shuffledImages)
        {
            // Go through all the images and shuffle them
            for (index = 0; index < shuffledImages.Length; index++)
            {
                // Hold the image in a temporary variable
                Texture tempImage = shuffledImages[index];

                // Choose a random index from the question list
                int randomIndex = UnityEngine.Random.Range(index, shuffledImages.Length);

                // Assign a random question from the list
                images[index] = shuffledImages[randomIndex];

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
            if (timeBonusText)
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

            // Go to the next level
            currentLevel++;
            
            // If the level text has an animation, play it
            if (levelText.GetComponent<Animation>()) levelText.GetComponent<Animation>().Play();

            if (gridSize.x == gridSize.y) gridSize += new Vector2(increaseGridSize, 0);
            else gridSize += new Vector2(0, increaseGridSize);

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

            // Set the current image
            imageObject.texture = images[currentLevel];

            // Slice the image into a grid based on the image grid size
            Vector2 tileSize = new Vector2(imageObject.rectTransform.sizeDelta.x / gridSize.x, imageObject.rectTransform.sizeDelta.y / gridSize.y);

            // Set the size of the grid object to the same size as the image object it holds
            gridObject.sizeDelta = imageObject.rectTransform.sizeDelta;

            // Set the position of the grid object to the same position as the image object it holds
            gridObject.position = imageObject.transform.position;

            int tileIndex = 0;

            // Go through the rows and columns and create sliced tiles out of the image
            for (index = 0; index < gridSize.x; index++)
            {
                for (indexB = 0; indexB < gridSize.y; indexB++)
                {
                    // Create a new image
                    RawImage imagePart = Instantiate(imageObject) as RawImage;

                    // Set a tile slice
                    imagePart.uvRect = new Rect(index * (1 / gridSize.x), indexB * (1 / gridSize.y), 1 / gridSize.x, 1 / gridSize.y);

                    // Set the size of the tile
                    imagePart.rectTransform.sizeDelta = gridObject.GetComponent<GridLayoutGroup>().cellSize = tileSize;

                    // Put the tile inside the grid object
                    imagePart.rectTransform.SetParent(gridObject);

                    // Set the name of the tile based on its row and column number
                    imagePart.name = tileIndex.ToString();

                    tileIndex++;

                    // Activate the object
                    imagePart.gameObject.SetActive(true);
                }
            }
            
            // Shuffle the image objects 
            foreach ( Transform gridTile in gridObject )
            {
                // Either place the image object at the end of the image list, or at the start, randomly.
                if (Random.value > 0.5f) gridTile.SetAsFirstSibling();
                else gridTile.SetAsLastSibling();
            }

            //Run another check to see if we accidentally created a image that matches the originial one
            
            yield return new WaitForEndOfFrame();

            // Start the timer
            timeRunning = true;
            
            // Wait for a moment, or for the duration of the animation if it exists
            if (imageObject.GetComponent<Animation>()) yield return new WaitForSeconds(imageObject.GetComponent<Animation>().GetClip("TileSpawn").length);
            else yield return new WaitForSeconds(0.5f);

            // Make the image buttons interactable again
            foreach (Transform gridTile in gridObject) gridTile.GetComponent<Button>().enabled = true;

            // Select the first image in the image
            if ( eventSystem && gridObject.Find("00") ) eventSystem.SetSelectedGameObject(gridObject.Find("00").gameObject);
        }

        /// <summary>
        /// Selects a image, and swaps it with another selected image
        /// </summary>
        /// <param name="source"></param>
        public void SelectImage( Transform source )
        {
            // You are allowed to select a image only if you are not swapping one already, the timer is running, and the game has not ended
            if ( isSwapping == false && isGameOver == false && timeRunning == true )//&& source.GetComponent<Animation>().isPlaying == false )
            {
                // If we don't have a image selected, select this one
                if (imageCurrent == null)
                {
                    // Assign the currently selected image
                    imageCurrent = source;

                    // Play the animation
                    if (imageCurrent.GetComponent<Animation>()) imageCurrent.GetComponent<Animation>().Play("TileSelect");
                }
                else if ( imageCurrent == source ) // If we select the same image we have previously selected, deselect it
                {
                    // Play the animation
                    if (imageCurrent.GetComponent<Animation>()) imageCurrent.GetComponent<Animation>().Play("TileIdle");

                    // Clear the current image selection
                    imageCurrent = null;
                }
                else // Otherwise, if we selected a new image, swap it with the previously selected one
                {
                    // We are swapping images
                    isSwapping = true;
                    
                    // Start swapping the two images
                    StartCoroutine(SwapImages(imageCurrent, source.position, source, imageCurrent.position));

                    // Play the animation
                    if (imageCurrent.GetComponent<Animation>()) imageCurrent.GetComponent<Animation>().Play("TileIdle");

                    //If there is a source and a sound, play it from the source
                    if (soundSource && soundSwap) soundSource.GetComponent<AudioSource>().PlayOneShot(soundSwap);
                }

                //If there is a source and a sound, play it from the source
                if (soundSource && soundSelect) soundSource.GetComponent<AudioSource>().PlayOneShot(soundSelect);

                // Go through all the image objects, and play the correct animation
                foreach (Transform imageObject in gridObject)
                {
                    // Make the image button non-interactable
                    if (imageObject.GetComponent<Button>()) imageObject.GetComponent<Button>().enabled = true;
                }
            }
        }

        /// <summary>
        /// Swaps two images smoothly
        /// </summary>
        /// <param name="firstImage"> The first image we selected</param>
        /// <param name="firstTarget"> The position that the first image is moving to</param>
        /// <param name="secondImage"> The second image we selected</param>
        /// <param name="secondTarget"> The position that the second image is moving to</param>
        /// <returns></returns>
        IEnumerator SwapImages( Transform firstImage, Vector3 firstTarget, Transform secondImage, Vector3 secondTarget)
        {
            // How many steps the swap process should take
            int iterations = 15;

            // Smoothly swap the two images
            while ( iterations > 0 )
            {
                yield return new WaitForFixedUpdate();

                // Move the first image towards its target position
                //firstImage.position = Vector3.Slerp(firstImage.position, firstTarget, Time.deltaTime * 10);
                firstImage.position = new Vector3( Mathf.Lerp(firstImage.position.x, firstTarget.x, Time.deltaTime * 10), Mathf.Lerp(firstImage.position.y, firstTarget.y, Time.deltaTime * 10), firstTarget.z);

                // Move the second image towards its target position
                //secondImage.position = Vector3.Slerp(secondImage.position, secondTarget, Time.deltaTime * 10);
                secondImage.position = new Vector3( Mathf.Lerp(secondImage.position.x, secondTarget.x, Time.deltaTime * 10), Mathf.Lerp(secondImage.position.y, secondTarget.y, Time.deltaTime * 10), secondTarget.z);

                iterations--;
            }

            iterations = 5;

            while (iterations > 0)
            {
                // Set the images in their final positions
                firstImage.localPosition = new Vector3(secondImage.localPosition.x, secondImage.localPosition.y, Mathf.Lerp(secondImage.localPosition.z,0, Time.deltaTime * 10));
                secondImage.localPosition = new Vector3(firstImage.localPosition.x, firstImage.localPosition.y, Mathf.Lerp(firstImage.localPosition.z, 0, Time.deltaTime * 10));

                iterations--;
            }

            // Set the images in their final positions
            firstImage.localPosition = new Vector3(secondImage.localPosition.x, secondImage.localPosition.y, 0);
            secondImage.localPosition = new Vector3(firstImage.localPosition.x, firstImage.localPosition.y, 0);

            // Swap the positions of the image objects in the hierarchy, so that they appear in the correct order in the images grid

            // Remember the hierarchy position of the second image
            int tempIndex = secondImage.GetSiblingIndex();

            // Place the second image in the hierarchy position of the first image
            secondImage.SetSiblingIndex(imageCurrent.GetSiblingIndex());

            // Place the first image in the hierarchy position of the second image ( which we remembered earlier )
            imageCurrent.SetSiblingIndex(tempIndex);

            // Clear the currently selected image
            imageCurrent = null;

            // We are not swapping images anymore
            isSwapping = false;

            // Check if the current image matches the order of images we have
            StartCoroutine(CheckImage());
        }

        /// <summary>
        /// Checks if the current image matches the order of images we have
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckImage()
        {
            // Create a new empty image which we will fit out the images into
            bool correctMatch = true;

            // Go through each image and add it to the temporary image
            for (index = 0; index < gridObject.childCount; index++)
            {
                // If the object is active, add a image to it
                if ( gridObject.GetChild(index).name != index.ToString() )
                {
                    correctMatch = false;

                    break;
                }
            }

            // If the images match, we win!
            if (correctMatch == true)
            {
                // Stop the timer
                timeRunning = false;

                //If there is a source and a sound, play it from the source
                if (soundSource && soundCorrect)
                {
                    soundSource.GetComponent<AudioSource>().PlayOneShot(soundCorrect);
                    soundSource.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/" + imageObject.GetComponent<RawImage>().texture.name));
                }
                    
                // Go through all the image objects, and play the correct animation
                foreach (Transform imageObject in gridObject)
                {
                    // Make the image button non-interactable
                    if (imageObject.GetComponent<Button>()) imageObject.GetComponent<Button>().enabled = false;
                }

                ShowImageName(true);
                yield return new WaitForSeconds(delayAfterLevel);

                // Go through all the image objects, and play the correct animation
                foreach (Transform imageObject in gridObject)
                {
                    // Animate the image
                    if (imageObject.GetComponent<Animation>()) imageObject.GetComponent<Animation>().Play("ImageCorrect");
                }

                // Add the bonus to the score, based on the level number we are in
                ChangeScore(bonusPerLevel * (currentLevel+1));
                
                // Wait for a moment, or for the duration of the animation if it exists
                if (imageObject.GetComponent<Animation>()) yield return new WaitForSeconds(imageObject.GetComponent<Animation>().GetClip("ImageCorrect").length);
                else yield return new WaitForSeconds(0.5f);

                // Destroy all image objects, except the default image object
                foreach (Transform imageObject in gridObject)
                {
                    Destroy(imageObject.gameObject);
                }

                // If we reached the required images per level, level up
                if ( currentLevel < images.Length - 1 )
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

        public void ShowImageName(bool showText)
        {
            isPaused = true;

            //Set timescale to 0, preventing anything from moving
            Time.timeScale = 0;

            // Remember the button that was selected before pausing
            if (eventSystem) buttonBeforePause = eventSystem.currentSelectedGameObject;

            //Show the pause screen and hide the game screen
            if (showText == true)
            {
                if (imageNameButton)
                {
                    imageNameButton.GetComponent<Text>().text = imageObject.GetComponent<RawImage>().texture.name;
                    imageNameButton.gameObject.SetActive(true);
                }

                // Hide the game screen
                //gameObject.SetActive(false);
            }
        }
        public void HideImageName()
        {
            isPaused = false;

            //Set timescale back to the current game speed
            Time.timeScale = 1;

            //Hide the pause screen and show the game screen
            if (imageNameButton) imageNameButton.gameObject.SetActive(false);

            // Show the game screen
            //gameObject.SetActive(true);

            // Select the button that we pressed before pausing
            if (eventSystem) eventSystem.SetSelectedGameObject(buttonBeforePause);
        }
    }
}