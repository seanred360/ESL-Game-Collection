using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TwoOfAKindGame
{
    /// <summary>
    /// This script controls the game, starting it, following game progress, and finishing it with game over.
    /// </summary>
    public class TOKGameController : MonoBehaviour
    {
        [Tooltip("Match the image to the name of the image")]
        public bool matchImageToName = false;

        // Holds the current event system
        internal EventSystem eventSystem;

        // Defines the type of pairs we have. These are either "ImageText" or "TextText" 
        internal string pairsType = "";

        [Tooltip("A list of all the image cards in the game. The text is derived from the text inside the card")]
        [HideInInspector]
        public Sprite[] pairsImage = null;

        [Tooltip("A list of all the text cards in the game. The comparison is made based on the name of the image")]
        [HideInInspector]
        public string[] pairsText = null;

        [Tooltip("The default card button which all other card buttons are duplicated from and displayed in the cards grid")]
        public RectTransform cardObject;

        [Tooltip("A list of the colors that the cards change to between levels")]
        public Color[] cardColors;
        internal int cardColorIndex = 0;

        [Tooltip("The object that holds all the cards")]
        public RectTransform cardsGridObject;

        [Tooltip("Randomize the list of pairs so that we don't get the same pairs each time we start the game")]
        public bool randomizePairs = true;

        [Tooltip("How many points we get for each matched pair. This value is multiplied by the number of the level we are on. Ex: Level 1 give 100 points, Level 2 gives 200 points.")]
        public float bonusPerLevel;

        // The current level we are on, 1 being the first level in the game
        internal int currentLevel = 1;

        // The text object that shows which level we are in. This text object should be placed inside the GameController object and named "LevelText"
        internal Text levelText;

        [Tooltip("The name of a numbered level (ex: Round 1, Level 1, etc)")]
        public string levelNamePrefix = "ROUND";

        [Tooltip("If set to true, the game will repeat from the start, shuffling the questions again")]
        public bool endlessMode = true;

        // An array that holds all the pairs of the current level. This is used so that we can later remove them easily when the level is completed.
        internal Transform[] pairsArray;

        // The index of the current pair we are on. This increases as we advance in the levels
        internal int currentPair = 0;

        // The number of pairs left in the current level
        internal int pairsLeft = 0;

        [Tooltip("The number of pairs in the first level")]
        public int pairsCount = 4;

        [Tooltip("The number of pairs added to the game in each level")]
        public int pairsIncrease = 2;

        [Tooltip("The maximum number of pairs allowed in the game")]
        public int pairsMaximum = 8;

        // Did we select the first object in the pair, or the second?
        internal bool firstOfPair = true;

        // Holds the first object of the pair that the player selects.
        internal Transform firstObject;

        //The current score of the player
        internal float score = 0;
        internal float scoreCount = 0;

        // The score text object which displays the current score of the player. This text object should be placed inside the GameController object and named "LevelScore"
        internal Text scoreText;
        internal string scoreTextPadding;

        // The highest score we got in this game
        internal float highScore = 0;

        [Tooltip("How many seconds we get in each level")]
        public float time = 10;
        internal float timeLeft;
        
        //The canvas of the timer in the game, the UI object and its various parts
        internal GameObject timerIcon;
        internal Image timerBar;
        internal Text timerText;

        // Holds the cursor object which shows us where we are aiming when using a keyboard or gamepad
        internal RectTransform cursor;

        [Tooltip("The menu that appears when we pause the game")]
        public Transform pauseCanvas;

        [Tooltip("The menu that appears if we lose the game, when time runs out")]
        public Transform gameOverCanvas;

        [Tooltip("The menu that appears if we win the game, when finishing all the pairs")]
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

        // A general use index
        internal int index = 0;
        public bool babyMode = false;

        /// <summary>
        /// Start is only called once in the lifetime of the behaviour.
        /// The difference between Awake and Start is that Start is only called if the script instance is enabled.
        /// This allows you to delay any initialization code, until it is really needed.
        /// Awake is always called before any Start functions.
        /// This allows you to order initialization of scripts
        /// </summary>
        void Start()
        {
            // Disable multitouch so that we don't tap two answers at the same time ( prevents multi-answer cheating, thanks to Miguel Paolino for catching this bug )
            Input.multiTouchEnabled = false;

            // Cache the current event system so we can position the cursor correctly
            eventSystem = UnityEngine.EventSystems.EventSystem.current;

            //Hide the game over ,victory ,and larger image screens
            if (gameOverCanvas) gameOverCanvas.gameObject.SetActive(false);
            if (victoryCanvas) victoryCanvas.gameObject.SetActive(false);
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(false);

            // Assign the score text object
            if (GameObject.Find("ScoreText"))
            {
                scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

                scoreTextPadding = scoreText.text;
            }

            //Update the score
            UpdateScore();

            // Assign the level text object
            if (GameObject.Find("LevelText")) levelText = GameObject.Find("LevelText").GetComponent<Text>();

            //Get the highscore for the player
            highScore = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "HighScore", 0);

            //Assign the timer icon and text for quicker access
            if (GameObject.Find("TimerIcon"))
            {
                timerIcon = GameObject.Find("TimerIcon");
                if (GameObject.Find("TimerIcon/Bar")) timerBar = GameObject.Find("TimerIcon/Bar").GetComponent<Image>();
                if (GameObject.Find("TimerIcon/Text")) timerText = GameObject.Find("TimerIcon/Text").GetComponent<Text>();
            }

            // Assign the cursor object and hide it, but only for non-mobile devices
            if (transform.Find("Cursor"))
            {
                cursor = transform.Find("Cursor").GetComponent<RectTransform>();

                cursor.gameObject.SetActive(false);
            }

            // Set the maximum value of the timer
            timeLeft = time;

            //Assign the sound source for easier access
            if (GameObject.FindGameObjectWithTag(soundSourceTag)) soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);

            // Shuffle all the pairs in the game. If we have a TextText array, shuffle it instead.
            if (pairsImage.Length > 0)
            {
                // Set the pairs type
                pairsType = "Image";

                // Randomize the list of pairs
                if (randomizePairs == true) pairsImage = ShuffleImagePairs(pairsImage);

                // Create the first level, Image - Text game mode
                UpdateLevel();
            }
            else if (pairsText.Length > 0)
            {
                // Set the pairs type
                pairsType = "Text";

                // Randomize the list of pairs
                if (randomizePairs == true) pairsText = ShuffleTextPairs(pairsText);

                // Create the first level, Text - Text game mode
                UpdateLevel();
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
                if ( cursor )
                {
                    // If we use the keyboard or gamepad, keyboardControls take effect
                    if ( Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 ) cursor.gameObject.SetActive(true);

                    // If we move the mouse in any direction or click it, or touch the screen on a mobile device, then keyboard/gamepad controls are lost
                    if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0 || Input.GetMouseButtonDown(0) || Input.touchCount > 0) cursor.gameObject.SetActive(false);

                    // Place the cursor at the position of the selected object
                    if (eventSystem.currentSelectedGameObject) cursor.position = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().position;
                }

                // Count down the time until game over
                if (timeLeft > 0)
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

                // Hide the game screen
                //gameObject.SetActive(false);
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
            //gameObject.SetActive(true);

            // Select the button that we pressed before pausing
            if (eventSystem) eventSystem.SetSelectedGameObject(buttonBeforePause);
        }

        /// <summary>
        /// Selects an object for the pair. If two objects are selected, they are compared to see if they match
        /// </summary>
        /// <param name="selectedObject"></param>
        public void SelectObject(Transform selectedObject)
        {
            if (firstOfPair == true) // Selecting the first object of a pair
            {
                // Make the selected object unclickable 
                selectedObject.GetComponent<Button>().interactable = false;

                // Select the next available button for keyboard/gamepad controls
                if (eventSystem) eventSystem.SetSelectedGameObject(selectedObject.gameObject);

                // Animate the object when selected
                if (selectedObject.GetComponent<Animator>()) selectedObject.GetComponent<Animator>().Play("Select");

                firstObject = selectedObject;

                firstOfPair = false;

                //If there is a source and a sound, play it from the source
                if (soundSource)
                {
                    // Play the select sound
                    if (soundSelect) soundSource.GetComponent<AudioSource>().PlayOneShot(soundSelect);

                    // If this button has a sound assingned to it, play it
                    if (selectedObject.GetComponent<TOKPlaySound>()) selectedObject.GetComponent<TOKPlaySound>().PlaySoundCurrent();
                }

                // Play the sound associated with this card name
                if (soundSource)
                {
                    soundSource.GetComponent<AudioSource>().Stop();

                    soundSource.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/" + selectedObject.Find("CardFront/Text").GetComponent<Text>().text));
                }
            }
            else
            {
                // Go through all the buttons and make them unclickable
                foreach (RectTransform imageButton in cardsGridObject) imageButton.GetComponent<Button>().interactable = false;

                // Animate the object when selected
                if (selectedObject.GetComponent<Animator>()) selectedObject.GetComponent<Animator>().Play("Select");
                
                //If there is a source and a sound, play it from the source
                if (soundSource)
                {
                    // Play the select sound
                    if (soundSelect) soundSource.GetComponent<AudioSource>().PlayOneShot(soundSelect);

                    // If this button has a sound assingned to it, play it
                    if (selectedObject.GetComponent<TOKPlaySound>()) selectedObject.GetComponent<TOKPlaySound>().PlaySoundCurrent();
                }
                
                if (firstObject == selectedObject) //If we click on the same object that we already selected, unselect it and set it back to idle state ( in animation )
                {
                    if (selectedObject.GetComponent<Animator>())
                    {
                        firstObject.GetComponent<Animator>().Play("Unselect");
                        selectedObject.GetComponent<Animator>().Play("Unselect");
                    }
                }
                else // Check if the two objects match
                {
                    StartCoroutine("Compare", selectedObject);
                }

                // Play the sound associated with this card name
                if (soundSource)
                {
                    soundSource.GetComponent<AudioSource>().Stop();

                    soundSource.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/" + selectedObject.Find("CardFront/Text").GetComponent<Text>().text));
                }
            }
        }

        /// <summary>
        /// Compares two selected card objects. If they are the same we get bonus and the cards are removed from the level. If wrong, the cards are returned to face down
        /// </summary>
        /// <param name="selectedObject"></param>
        /// <returns></returns>
        IEnumerator Compare(Transform selectedObject)
        {
            yield return new WaitForSeconds(0.4f);

            // If we selected two objects, check if they match. In Image - Text mode we compare the image of the object to the name of the other object. In Text - Text mode we compare the pairs and see if they have the same firstText and secondText values
            if ( firstObject.name == selectedObject.name )
            {
                // Play the correct animation
                if (selectedObject.GetComponent<Animator>())
                {
                    firstObject.GetComponent<Animator>().Play("Correct");
                    selectedObject.GetComponent<Animator>().Play("Correct");
                }

                // Add the bonus to the score
                ChangeScore(bonusPerLevel * currentLevel);

                // Make the buttons uninteractable, so we don't have to move through them in the grid
                firstObject.GetComponent<Button>().interactable = false;
                selectedObject.GetComponent<Button>().interactable = false;

                // Stop listening for a click on these objects
                firstObject.GetComponent<Button>().onClick.RemoveAllListeners();// RemoveListener(delegate { SelectObject(firstObject); });
                selectedObject.GetComponent<Button>().onClick.RemoveAllListeners();// RemoveListener(delegate { SelectObject(selectedObject); });

                //If there is a source and a sound, play it from the source
                if (soundSource && soundCorrect) soundSource.GetComponent<AudioSource>().PlayOneShot(soundCorrect);

                // One less pait to match
                pairsLeft--;

                // If there are no more pairs to match, move to the next level
                if (pairsLeft <= 0) StartCoroutine("LevelUp", 0.5f);
            }
            else // If there is no match between the two objects
            {
                // Play the wrong animation
                if (selectedObject.GetComponent<Animator>())
                {
                    firstObject.GetComponent<Animator>().Play("Wrong");
                    selectedObject.GetComponent<Animator>().Play("Wrong");
                }

                //If there is a source and a sound, play it from the source
                if (soundSource && soundWrong) soundSource.GetComponent<AudioSource>().PlayOneShot(soundWrong);
            }

            yield return new WaitForSeconds(0.3f);

            // Reset the pair check
            firstOfPair = true;

            // Go through all the buttons and make them clickable again
            foreach (RectTransform imageButton in cardsGridObject) imageButton.GetComponent<Button>().interactable = true;

            // Select one of the clickable buttons in the cards grid
            if (eventSystem)
            {
                foreach (RectTransform imageButton in cardsGridObject)
                {
                    if (imageButton.GetComponent<Button>().interactable == true) eventSystem.SetSelectedGameObject(imageButton.gameObject);
                }
            }

            // Select the next available button for keyboard/gamepad controls
            if (eventSystem) eventSystem.SetSelectedGameObject(selectedObject.gameObject);
        }

        /// <summary>
        /// Shuffles the specified sprite pairs list, and returns it
        /// </summary>
        /// <param name="s">A list of sprite pairs</param>
        Sprite[] ShuffleImagePairs(Sprite[] pairs)
        {
            // Go through all the sprite pairs and shuffle them
            for (index = 0; index < pairsImage.Length; index++)
            {
                // Hold the pair in a temporary variable
                Sprite tempPair = pairsImage[index];

                // Choose a random index from the sprite pairs list
                int randomIndex = UnityEngine.Random.Range(index, pairsImage.Length);

                // Assign a random sprite pair from the list
                pairsImage[index] = pairsImage[randomIndex];

                // Assign the temporary sprite pair to the random question we chose
                pairsImage[randomIndex] = tempPair;
            }

            return pairs;
        }

        /// <summary>
        /// Shuffles the specified pairs list, and returns it
        /// </summary>
        /// <param name="s">A list of pairs</param>
        string[] ShuffleTextPairs(string[] pairs)
        {
            // Go through all the pairs and shuffle them
            for (index = 0; index < pairsText.Length; index++)
            {
                // Hold the pair in a temporary variable
                string tempPair = pairsText[index];

                // Choose a random index from the question list
                int randomIndex = UnityEngine.Random.Range(index, pairsImage.Length);

                // Assign a random question from the list
                pairsImage[index] = pairsImage[randomIndex];

                // Assign the temporary question to the random question we chose
                pairsText[randomIndex] = tempPair;
            }

            return pairs;
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
            if (scoreText) scoreText.text = Mathf.CeilToInt(scoreCount).ToString(scoreTextPadding);
        }

        /// <summary>
		/// Levels up, and increases the difficulty of the game
		/// </summary>
		IEnumerator LevelUp(float delay)
        {
            // Set the maximum value of the timer
            timeLeft = time;

            // Animate all previous buttons out of the scene
            for (index = 0; index < cardsGridObject.childCount; index++)
            {
                if (cardsGridObject.GetChild(index) != cardObject) cardsGridObject.GetChild(index).GetComponent<Animator>().Play("Remove");
            }

            yield return new WaitForSeconds(delay);

            // Show the next set of pairs from the list
            if ( currentPair < pairsImage.Length || currentPair < pairsText.Length )
            {
                currentLevel++;

                // Increase the number of pairs in the level
                pairsCount += pairsIncrease;

                // Limit the number of pairs to the maximum value
                pairsCount = Mathf.Clamp(pairsCount, 0, pairsMaximum);

                //If there is a source and a sound, play it from the source
                if (soundSource && soundLevelUp)
                {
                    soundSource.GetComponent<AudioSource>().Stop();

                    soundSource.GetComponent<AudioSource>().PlayOneShot(soundLevelUp);
                }

                // Update the level attributes based on the type of pairs we have
                if (pairsType == "Image") UpdateLevel();
                else if (pairsType == "Text") UpdateLevel();
            }
            else // Otherwise if we finished all pairs in the game, go to the Victory screen
            {
                if ( endlessMode == true )
                {
                    currentLevel = 1;
                    currentPair = 0;
                    // Increase the number of pairs in the level
                    pairsCount += pairsIncrease;

                    // Limit the number of pairs to the maximum value
                    pairsCount = Mathf.Clamp(pairsCount, 0, pairsMaximum);

                    //If there is a source and a sound, play it from the source
                    if (soundSource && soundLevelUp)
                    {
                        soundSource.GetComponent<AudioSource>().Stop();

                        soundSource.GetComponent<AudioSource>().PlayOneShot(soundLevelUp);
                    }

                    // Update the level attributes based on the type of pairs we have
                    if (pairsType == "Image") UpdateLevel();
                    else if (pairsType == "Text") UpdateLevel();
                }
                else
                {
                    // Run the victory event
                    StartCoroutine(Victory(0.5f));
                }
            }
        }

        /// <summary>
        /// Creates a new card based on the default card object
        /// </summary>
        public void CreateCard( bool showName )
        {
            // Create a new button
            RectTransform newButton = Instantiate(cardObject) as RectTransform;

            // Set the name of the card so we can compare it with its twin
            newButton.name = pairsLeft.ToString();

            // Listen for a click to change to the next stage
            newButton.GetComponent<Button>().onClick.AddListener(delegate { SelectObject(newButton); });

            // Put it inside the button grid
            newButton.transform.SetParent(cardsGridObject);

            // Set the scale to the default button's scale
            newButton.localScale = cardObject.localScale;

            // Set the position 0 local inside the parent image holder
            newButton.localPosition = Vector3.zero;
            
            // Show the name text of the card
            if (showName == true)
            {
                newButton.Find("CardFront/Image").GetComponent<Image>().enabled = false;
                //newButton.Find("CardFront/Image").GetComponent<Image>().sprite = pairsImage[index];

                newButton.Find("CardFront/Text").GetComponent<Text>().enabled = true;
                newButton.Find("CardFront/Text").GetComponent<Text>().text = pairsImage[index].name;

            }
            else // Show the image of the card
            {
                newButton.Find("CardFront/Image").GetComponent<Image>().enabled = true;
                newButton.Find("CardFront/Image").GetComponent<Image>().sprite = pairsImage[index];

                newButton.Find("CardFront/Text").GetComponent<Text>().enabled = false;
                newButton.Find("CardFront/Text").GetComponent<Text>().text = pairsImage[index].name;

            }

            // Select the object for keyboard/gamepad controls
            if (eventSystem) eventSystem.SetSelectedGameObject(newButton.gameObject);
        }

        /// <summary>
        /// Updates the level, showing the next set of pairs ( Used for Image - Text matching )
        /// </summary>
        public void UpdateLevel()
        {
            // Remove all previous cards except the default one
            for ( index = 0; index < cardsGridObject.childCount; index++)
            {
                if (cardsGridObject.GetChild(index) != cardObject) Destroy(cardsGridObject.GetChild(index).gameObject);
            }
            
            // Display the current level we are on
            if (levelText) levelText.text = levelNamePrefix + " " + (currentLevel).ToString();

            // If we have a default button assigned and we have a list of pairs, duplicate the button and display all the pairs in the grid
            if ( cardObject )
            {
                // Activate the default card object while we duplicate new cards from it
                cardObject.gameObject.SetActive(true);

                // Set the color of the card front and back
                if (cardObject.Find("CardFront").GetComponent<Image>()) cardObject.Find("CardFront").GetComponent<Image>().color = cardColors[cardColorIndex];
                if (cardObject.Find("CardFront/CardBack").GetComponent<Image>()) cardObject.Find("CardFront/CardBack").GetComponent<Image>().color = cardColors[cardColorIndex];

                //Switch to the next card color
                if (cardColorIndex < cardColors.Length - 1) cardColorIndex++;
                else cardColorIndex = 0;

                // The number of pairs we need to create for this level 
                int levelPairs = currentPair + pairsCount;

                // Create the image buttons by duplicating the default one
                for (index = currentPair; index < pairsImage.Length; index++)
                {
                    // If we passed through all the pairs in the game, stop creating them
                    if (index >= levelPairs) break;

                    // Create two buttons for this image
                    CreateCard(false);

                    if (matchImageToName == true) CreateCard(true);
                    else CreateCard(false);

                    pairsLeft++;

                    currentPair++;
                }

                // Create the text buttons by duplicating the default one
                for (index = currentPair; index < pairsText.Length; index++)
                {
                    // If we passed through all the pairs in the game, stop creating them
                    if (index >= levelPairs) break;

                    // Create two buttons for this image
                    CreateCard(false);

                    if ( matchImageToName == true ) CreateCard(true);
                    else CreateCard(false);

                    pairsLeft++;

                    currentPair++;
                }

                // Deactivate the default card object as we don't need it anymore
                cardObject.gameObject.SetActive(false);

                // Reset the pair check. The next time we click on an object it will be the first of a pair
                firstOfPair = true;
            }

            // Scramble all available card buttons
            for (index = 1; index < cardsGridObject.childCount; index++)
            {
                if (Random.value > 0.5f) cardsGridObject.GetChild(index).SetAsFirstSibling();
                else cardsGridObject.GetChild(index).SetAsLastSibling();
            }
        }

        /// <summary>
        /// Runs the game over event and shows the game over screen
        /// </summary>
        IEnumerator GameOver(float delay)
        {
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
                if (soundSource && soundGameOver)
                {
                    soundSource.GetComponent<AudioSource>().Stop();

                    soundSource.GetComponent<AudioSource>().PlayOneShot(soundGameOver);
                }
            }
        }

        /// <summary>
        /// Runs the victory event and shows the victory screen
        /// </summary>
        IEnumerator Victory(float delay)
        {
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
                if (soundSource && soundVictory)
                {
                    soundSource.GetComponent<AudioSource>().Stop();

                    soundSource.GetComponent<AudioSource>().PlayOneShot(soundVictory);
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
}
