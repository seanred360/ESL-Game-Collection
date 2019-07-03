using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ScrambledWordGame.Types;

namespace ScrambledWordGame
{
    /// <summary>
    /// This script controls the game, starting it, following game progress, and finishing it with game over.
    /// </summary>
    public class SWGGameController : MonoBehaviour
    {
        // Holds the current event system
        internal EventSystem eventSystem;

        //[Tooltip("(This is the old words list. In the next update it will be removed completely, so remember to move your words to one of the attached lists SWGWords or SWGWordsWithHints ) A list of all the words in the game. This list only contains words, with no hints. You can either have a list of word only, or words with hints, but not both")]
        // The words list is used to hold words ( without hints ) from the component SWGWords that is attached to the component SWGGameController
        internal string[] words;

        [Tooltip("A list of all the words in the game. In this list each word has a text/image hint to accompany it. You can either have a list of word only, or words with hints, but not both")]
        internal WordWithHints[] wordsWithHints = new WordWithHints[0];
        internal Text textHintObject;
        internal Image imageHintObject;
        internal Button buttonHint;
        
        // Holds the object that displays letters in a grid ( "LetterGrid" in the heirarchy )
        internal RectTransform lettersGrid;

        // Holds the default letter object that displays a single letter ( "Letter" in the heirarchy )
        internal RectTransform letterDefault;

        // The currently selected letter
        internal Transform letterCurrent;

        // The currently selected word or sentence
        internal string currentWord = "";

        // The index number of the word we found from the list
        internal int wordFoundIndex = 0;
        internal bool wordFound = false;

        // The length of the longest word in the words list, this is used to make sure we don't finish the game before reaching the last part of the game
        internal int longestWordLength = 0;

        [Tooltip("Randomize the list of words so that we don't get the same words each time we start the game")]
        public bool randomizeList = true;

        [Tooltip("Scramble whole words instead of scrambling individual letters")]
        public bool mixWholeSentences = false;

        // The number of letters/words in the word/sentence group. This means we will look for a word/sentence with X letters/words and display it")]
        internal int lettersCount = 10000;

        // The number of words in the sentence, and he number ofwords in the previous sentence
        internal int wordsInSentence;

        [Tooltip("The number of words to display from a certain level before moving on to the next level ( Each level displays words with certain number of letters such as 3 letters, 4 letters, etc )")]
        public int wordsPerLevel = 5;
        internal int wordsCount = 1;
        
        [Tooltip("How many points we get for each word in the level. This value is multiplied by the number of the level we are on. Ex: Level 1 gives 100 points, Level 2 gives 200 points.")]
        public float bonusPerLevel = 100;

        [Tooltip("How many extra seconds we add to the timer in the level. This value is multiplied by the number of the level we are on. Ex: Level 1 gives 5 seconds, Level 2 gives 10 seconds.")]
        public float timeBonusPerLevel = 5;

        [Tooltip("How many seconds are left before game is over")]
        public float time = 10;
        internal float timeLeft;
        internal bool timeRunning = false;

        //The canvas of the timer in the game, the UI object and its various parts
        internal GameObject timerIcon;
        internal Image timerBar;
        internal Text timerText;
        internal Text timeBonusText;

        // Should we show the hint when the word is displayed? If we don't show the hint at the start, a hint button will appear, which we can press to show the hint
        internal bool showHintAtStart = false;

        // Which percentage of the bonus points we lose when we show the hint, for example 0.5 means we lose 50% of the bonus, while 0 means we lose all of the bonus
        internal float hintBonusLoss = 0.5f;
        internal float hintBonusLossCount = 1;

        // The current level we are on, 1 being the first level in the game
        internal int currentLevel = 1;

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
        // sean quit button
        public string quitButton = "Quit";

        // Are we swapping two letters now?
        internal bool isSwapping = false;

        // A general use index
        internal int index = 0;

        void Awake()
        {
            // If we have a start canvas, pause the game and display it. Otherwise, just start the game.
            if (startCanvas)
            {
                // Show the start screen
                startCanvas.gameObject.SetActive(true);

                Pause(false);
            }

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

            // Assign the letters grid which holds the letters of a word
            if (GameObject.Find("LettersGrid")) lettersGrid = GameObject.Find("LettersGrid").GetComponent<RectTransform>();

            // Assign the letters grid which holds the letters of a word
            if (GameObject.Find("LettersGrid/Letter")) letterDefault = GameObject.Find("LettersGrid/Letter").GetComponent<RectTransform>();

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

            // Assign the text hint object and hide it
            if ( GameObject.Find("TextHint") )
            {
                textHintObject = GameObject.Find("TextHint").GetComponent<Text>();

                textHintObject.gameObject.SetActive(false);
            }

            // Assign the image hint object and hide it
            if ( GameObject.Find("ImageHint") )
            {
                imageHintObject = GameObject.Find("ImageHint").GetComponent<Image>();

                imageHintObject.gameObject.SetActive(false);
            }

            // Assign the hint button and hide it
            if ( GameObject.Find("ButtonHint") )
            {
                // Assign the hint button
                buttonHint = GameObject.Find("ButtonHint").GetComponent<Button>();

                // If there is a text hint object, make clicking on the hint button show it
                if ( textHintObject) buttonHint.onClick.AddListener(delegate () { ShowHint(); });

                // If there is an image hint object, make clicking on the hint button show it
                if ( imageHintObject ) buttonHint.onClick.AddListener(delegate () { ShowHint(); });

                // Hide the hint button
                buttonHint.gameObject.SetActive(false);
            }

            //Assign the sound source for easier access
            if (GameObject.FindGameObjectWithTag(soundSourceTag)) soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);

            // Deactivate all the letter objects
            foreach (Transform letterObject in lettersGrid) letterObject.gameObject.SetActive(false);

            //If we have words in the list, continue
            if ( wordsWithHints.Length > 0 ) 
            {
                // Go through all the words in the list and find the longest word and shortest word
                for (index = 0; index < wordsWithHints.Length; index++)
                {
                    // If the current word is longer, record its length so we know when the list reaches the end ( longest word )
                    if (wordsWithHints[index].word.Length > longestWordLength) longestWordLength = wordsWithHints[index].word.Length;

                    // Check if this "word" is actually a sentence with more than one word
                    if (wordsWithHints[index].word.Contains(" ") && mixWholeSentences == true )
                    {
                        wordsInSentence = 1;

                        // Go through the sentence and count the number of words in it
                        for (int wordIndex = 0; wordIndex < wordsWithHints[index].word.Length; wordIndex++) if (wordsWithHints[index].word[wordIndex].ToString() == " ") wordsInSentence++;

                        // If the current sentence is longer, record its length so we know when the list reaches the end ( sentence with the most number of words in it )
                        if (wordsInSentence > longestWordLength) longestWordLength = wordsInSentence;

                        // If the current sentence is shorter, record its length so we know which sentence length we should look for at the start of the game
                        if ( wordsInSentence < lettersCount && wordsWithHints[index].word.Length > 0 ) lettersCount = wordsInSentence;
                    }
                    else
                    {
                        // If the current word is longer, record its length so we know when the list reaches the end ( longest word )
                        if (wordsWithHints[index].word.Length > longestWordLength) longestWordLength = wordsWithHints[index].word.Length;

                        // If the current word is shorter, record its length so we know which word length we should look for at the start of the game
                        if (wordsWithHints[index].word.Length < lettersCount && wordsWithHints[index].word.Length > 0) lettersCount = wordsWithHints[index].word.Length;
                    }
                }

                // Randomize the list of words
                if (randomizeList == true) wordsWithHints = ShuffleWords(wordsWithHints);

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
                //Toggle pause/unpause in the game
                if (Input.GetButtonDown(quitButton))
                {
                    Quit();
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
        /// Shuffles the specified words list, and returns it
        /// </summary>
        /// <param name="words">A list of words</param>
        WordWithHints[] ShuffleWords(WordWithHints[] words)
        {
            // Go through all the words and shuffle them
            for (index = 0; index < wordsWithHints.Length; index++)
            {
                // Hold the word in a temporary variable
                WordWithHints tempword = wordsWithHints[index];

                // Choose a random index from the question list
                int randomIndex = UnityEngine.Random.Range(index, wordsWithHints.Length);

                // Assign a random question from the list
                wordsWithHints[index] = words[randomIndex];

                // Assign the temporary question to the random question we chose
                words[randomIndex] = tempword;
            }

            return words;
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
            if (timeBonusText)
            {
                // Activate the time bonus object
                timeBonusText.gameObject.SetActive(true);

                // Set the time bonus value in the text
                timeBonusText.GetComponent<Text>().text = "+" + (timeBonusPerLevel * currentLevel).ToString();

                // Play the animation
                if (timeBonusText.GetComponent<Animation>() ) timeBonusText.GetComponent<Animation>().Play();
            }

            // Add bonus time to the timer
            timeLeft += timeBonusPerLevel * currentLevel;

            // Wait for a while
            yield return new WaitForSeconds(delay);

            // Go to the next level
            currentLevel++;
            
            // If the level text has an animation, play it
            if (levelText.GetComponent<Animation>()) levelText.GetComponent<Animation>().Play();

            // Increase the letter count for the new level
            lettersCount++;

            // Update the level attributes
            StartCoroutine("UpdateLevel");

            //If there is a source and a sound, play it from the source
            if (soundSource && soundLevelUp) soundSource.GetComponent<AudioSource>().PlayOneShot(soundLevelUp);
        }
        
        /// <summary>
        /// Updates the level, showing the next word
        /// </summary>
        IEnumerator UpdateLevel()
        {
            // Hide the text and image hints, and the hint button, if they exist
            if ( imageHintObject) imageHintObject.gameObject.SetActive(false);
            if ( textHintObject ) textHintObject.gameObject.SetActive(false);
            if ( buttonHint ) buttonHint.gameObject.SetActive(false);

            // Display the current level we are on
            if (levelText) levelText.text = levelNamePrefix + " " + (currentLevel).ToString();

            // Used to indicate if we found the word we need, and the index of the word we found
            wordFound = false;

            // Clear the current word
            currentWord = "";

            // Go through the word list and find the next word for the current letter count
            for ( index = 0 ; index < wordsWithHints.Length ; index++ )
            {
                // Check if this "word" is actually a sentence with more than one word
                if ( wordsWithHints[index].word.Contains(" ") && mixWholeSentences == true )
                {
                    wordsInSentence = 1;
                    
                    // Go through the sentence and count the number of words in it
                    for ( int wordIndex = 0; wordIndex < wordsWithHints[index].word.Length; wordIndex++ )
                    {
                        if (wordsWithHints[index].word[wordIndex].ToString() == " ") wordsInSentence++;
                    }

                    // If we find a sentence with the correct number of words count, then we found our "word"
                    if ( wordsInSentence == lettersCount ) wordFound = true;
                }
                else if ( wordsWithHints[index].word.Length == lettersCount ) // If we find a word with the correct letters count, then we found our word
                {
                    wordFound = true;
                }

                // This is the index of the letter objects, which lets us check if we need to use an available letter object or create a new letter object
                //int letterObjectIndex = 0;
                
                // Display the word/sentence we found
                if ( wordFound == true )
                {
                    // Go through each letter in the word and assign it to a letter object
                    for ( int letterIndex = 0; letterIndex < wordsWithHints[index].word.Length; letterIndex++ )
                    {
                        // Create a new letter
                        RectTransform newLetter = Instantiate(letterDefault) as RectTransform;

                        // Put it inside the letters grid
                        newLetter.transform.SetParent(lettersGrid);
                        
                        // Set the scale to the default letter's scale
                        newLetter.position = letterDefault.position;

                        // Set the scale to the default letter's scale
                        newLetter.localScale = letterDefault.localScale;

                        // Activate the letter object
                        newLetter.gameObject.SetActive(true);

                        // If this is a sentence, assign the whole word to the letter object
                        if (wordsInSentence > 1)
                        {
                            // Clear the default letter so that we can fill the box with a word
                            newLetter.Find("Text").GetComponent<Text>().text = "";
                                
                            // Add all the letters of the word to the letter object
                            while ( letterIndex < wordsWithHints[index].word.Length && wordsWithHints[index].word[letterIndex].ToString() != " " )
                            {
                                newLetter.Find("Text").GetComponent<Text>().text += wordsWithHints[index].word[letterIndex].ToString();

                                letterIndex++;
                            }

                            // Add the word to the "currentWord" string which holds the entire sentence without spaces, for comparison
                            currentWord += newLetter.Find("Text").GetComponent<Text>().text;
                        }
                        else
                        {
                            // Set the correct letter from the word into this letter object
                            newLetter.Find("Text").GetComponent<Text>().text = wordsWithHints[index].word[letterIndex].ToString();
                        }
                    }

                    // Assign the current word, so that we can check if it matches the order of letters
                    if ( wordsInSentence <= 1 ) currentWord = wordsWithHints[index].word;

                    // If we have a sentence, calculate the average size of a tile which will fit a word inside it
                    if (wordsInSentence > 1) lettersGrid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(currentWord.Length * 50 / wordsInSentence, 100);

                    // If we have a text hint, display it in the text hint box, if it exists
                    if  ( textHintObject )
                    {
                        // If there is a text hint, show it
                        if ( wordsWithHints[index].textHint != null && wordsWithHints[index].textHint != string.Empty )
                        {
                            // Either show the text when the word appears, or instead show a button that needs to be clicked before showing the hint
                            if ( showHintAtStart == true ) textHintObject.gameObject.SetActive(true);
                            else if ( buttonHint ) buttonHint.gameObject.SetActive(true);

                            // Set the text for the text hint
                            textHintObject.text = wordsWithHints[index].textHint;

                            // Play the intro animation of the text hint, if it exists
                            if (textHintObject.GetComponent<Animation>()) textHintObject.GetComponent<Animation>().Play("HintIntro");
                        }
                        else // Otherwise, hide the hint object
                        {
                            textHintObject.gameObject.SetActive(false);
                        }
                    }

                    // If we have an image hint, display it in the image hint box, if it exists
                    if ( imageHintObject )
                    {
                        // If there is an image hint, show it
                        if ( wordsWithHints[index].imageHint )
                        {
                            // Either show the text when the word appears, or instead show a button that needs to be clicked before showing the hint
                            if (showHintAtStart == true) imageHintObject.gameObject.SetActive(true);
                            else if ( buttonHint ) buttonHint.gameObject.SetActive(true);

                            // Set the image in the image hint
                            imageHintObject.sprite = wordsWithHints[index].imageHint;

                            // Play the intro animation of the image hint, if it exists
                            if (imageHintObject.GetComponent<Animation>()) imageHintObject.GetComponent<Animation>().Play("HintIntro");
                        }
                        else // Otherwise, hide the hint object
                        {
                            imageHintObject.gameObject.SetActive(false);
                        }
                    }

                    // Clear the word so that we don't choose it again
                    wordsWithHints[index].word = "";

                    // Shuffle the letter objects 
                    foreach ( Transform letterObject in lettersGrid )
                    {
                        // Either place the letter object at the end of the letter list, or at the start, randomly.
                        if (Random.value > 0.5f) letterObject.SetAsFirstSibling();
                        else letterObject.SetAsLastSibling();
                    }

                //Run another check to see if we accidentally created a word that matches the originial one
                    // Create a new empty word which we will fit out the letters into
                    string tempWord = "";

                    yield return new WaitForEndOfFrame();

                    // Go through each letter and add it to the temporary word
                    for (index = 0; index < lettersGrid.childCount; index++)
                    {
                        // If the object is active, add a letter to it
                        if (lettersGrid.GetChild(index).gameObject.activeSelf == true)
                        {
                            // If we have an empty space in the word, add it. Otherwise add the letter we have
                            if (lettersGrid.GetChild(index).Find("Text").GetComponent<Text>().text == "") tempWord += " ";
                            else tempWord += lettersGrid.GetChild(index).Find("Text").GetComponent<Text>().text;
                        }
                    }

                    // If the words still match, shuffle them some more
                    if (tempWord == currentWord)
                    {
                        lettersGrid.GetChild(0).SetAsLastSibling();
                        lettersGrid.GetChild(0).SetAsFirstSibling();
                    }

                    // Start the timer
                    timeRunning = true;

                    // Record the index of the word we found from the list
                    wordFoundIndex = index;
                    
                    // End the check loop, because we found the word we want
                    break;
                }
            }

            // Wait for a moment, or for the duration of the animation if it exists
            if (letterDefault.GetComponent<Animation>()) yield return new WaitForSeconds(letterDefault.GetComponent<Animation>().GetClip("LetterSpawn").length);
            else yield return new WaitForSeconds(0.5f);

            // Make the letter buttons interactable again
            foreach (Transform letterObject in lettersGrid) letterObject.GetComponent<Button>().enabled = true;

            // Select the first letter in the word
            if ( eventSystem && lettersGrid.Find("Letter(Clone)") ) eventSystem.SetSelectedGameObject(lettersGrid.Find("Letter(Clone)").gameObject);

            if ( wordFound == false && currentWord.Length < longestWordLength && wordsCount != 1 )
            {
                // Timeout for searching
                int timeout = wordsWithHints.Length;

                // Keep looking for the next level letters count
                while (wordFound == false && timeout > 0)
                {
                    // Increase the letter count for the new level
                    lettersCount++;

                    // Update the level attributes
                    StartCoroutine("UpdateLevel");

                    timeout--;

                }
            }

            // If we didn't find any words with the required letters count...
            if ( wordFound == false )
            {
                // If we didn't find any words with the required letters count, try to move up to a higher letters count and look again
                if (currentWord.Length < longestWordLength && wordsCount != 1 )
                {
                    // Reset the word count, which is used to see how many words we have left until the next level
                    wordsCount = 1;
                    
                    // Level up
                    StartCoroutine("LevelUp", 0.2f);
                }
                else
                {
                    // If we have no more words in the list, run the victory event

                    // Stop the timer
                    timeRunning = false;

                    // Run the victory event
                    StartCoroutine(Victory(0.5f));
                }
            }
        }

        /// <summary>
        /// Selects a letter, and swaps it with another selected letter
        /// </summary>
        /// <param name="source"></param>
        public void SelectLetter( Transform source )
        {
            // You are allowed to select a letter only if you are not swapping one already, the timer is running, and the game has not ended
            if ( isSwapping == false && isGameOver == false && timeRunning == true )//&& source.GetComponent<Animation>().isPlaying == false )
            {
                // If we don't have a letter selected, select this one
                if (letterCurrent == null)
                {
                    // Assign the currently selected letter
                    letterCurrent = source;

                    // Play the animation
                    if (letterCurrent.GetComponent<Animation>()) letterCurrent.GetComponent<Animation>().Play("LetterSelect");
                }
                else if ( letterCurrent == source ) // If we select the same letter we have previously selected, deselect it
                {
                    // Play the animation
                    if (letterCurrent.GetComponent<Animation>()) letterCurrent.GetComponent<Animation>().Play("LetterIdle");

                    // Clear the current letter selection
                    letterCurrent = null;
                }
                else // Otherwise, if we selected a new letter, swap it with the previously selected one
                {
                    // We are swapping letters
                    isSwapping = true;
                    
                    // Start swapping the two letters
                    StartCoroutine(SwapLetters(letterCurrent, source.position, source, letterCurrent.position));

                    // Play the animation
                    if (letterCurrent.GetComponent<Animation>()) letterCurrent.GetComponent<Animation>().Play("LetterIdle");

                    //If there is a source and a sound, play it from the source
                    if (soundSource && soundSwap) soundSource.GetComponent<AudioSource>().PlayOneShot(soundSwap);
                }

                //If there is a source and a sound, play it from the source
                if (soundSource && soundSelect) soundSource.GetComponent<AudioSource>().PlayOneShot(soundSelect);
            }
        }

        /// <summary>
        /// Swaps two letters smoothly
        /// </summary>
        /// <param name="firstLetter"> The first letter we selected</param>
        /// <param name="firstTarget"> The position that the first letter is moving to</param>
        /// <param name="secondLetter"> The second letter we selected</param>
        /// <param name="secondTarget"> The position that the second letter is moving to</param>
        /// <returns></returns>
        IEnumerator SwapLetters( Transform firstLetter, Vector3 firstTarget, Transform secondLetter, Vector3 secondTarget)
        {
            // How many steps the swap process should take
            int iterations = 20;

            // Smoothly swap the two letters
            while ( iterations > 0 )
            {
                yield return new WaitForFixedUpdate();

                // Move the first letter towards its target position
                //firstLetter.position = Vector3.Slerp(firstLetter.position, firstTarget, Time.deltaTime * 10);
                firstLetter.position = new Vector3( Mathf.Lerp(firstLetter.position.x, firstTarget.x, Time.deltaTime * 10), Mathf.Lerp(firstLetter.position.y, firstTarget.y, Time.deltaTime * 10), firstTarget.z);

                // Move the second letter towards its target position
                //secondLetter.position = Vector3.Slerp(secondLetter.position, secondTarget, Time.deltaTime * 10);
                secondLetter.position = new Vector3( Mathf.Lerp(secondLetter.position.x, secondTarget.x, Time.deltaTime * 10), Mathf.Lerp(secondLetter.position.y, secondTarget.y, Time.deltaTime * 10), secondTarget.z);

                iterations--;
            }

            // Set the letters in their final positions
            firstLetter.position = secondLetter.position;
            secondLetter.position = firstLetter.position;

            // Swap the positions of the letter objects in the hierarchy, so that they appear in the correct order in the letters grid
            
            // Remember the hierarchy position of the second letter
            int tempIndex = secondLetter.GetSiblingIndex();

            // Place the second letter in the hierarchy position of the first letter
            secondLetter.SetSiblingIndex(letterCurrent.GetSiblingIndex());

            // Place the first letter in the hierarchy position of the second letter ( which we remembered earlier )
            letterCurrent.SetSiblingIndex(tempIndex);

            // Clear the currently selected letter
            letterCurrent = null;

            // We are not swapping letters anymore
            isSwapping = false;

            // Check if the current word matches the order of letters we have
            StartCoroutine(CheckWord());
        }

        /// <summary>
        /// Checks if the current word matches the order of letters we have
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckWord()
        {
            // Create a new empty word which we will fit out the letters into
            string tempWord = "";

            // Go through each letter and add it to the temporary word
            for ( index = 0; index < lettersGrid.childCount; index++ )
            {
                // If the object is active, add a letter to it
                if (lettersGrid.GetChild(index).gameObject.activeSelf == true)
                {
                    // If we have an empty space in the word, add it. Otherwise add the letter we have
                    if (lettersGrid.GetChild(index).Find("Text").GetComponent<Text>().text == "") tempWord += " ";
                    else tempWord += lettersGrid.GetChild(index).Find("Text").GetComponent<Text>().text;
                }
            }

            // If the words match, we win!
            if (tempWord == currentWord)
            {
                // Go through all the letter objects, and play the correct animation
                foreach (Transform letterObject in lettersGrid)
                {
                    // If the letter object is visible, animate it
                    if (letterObject.gameObject.activeSelf == true)
                    {
                        // Animate the letter
                        if (letterObject.GetComponent<Animation>()) letterObject.GetComponent<Animation>().Play("LetterCorrect");

                        // Make the letter button non-interactable
                        if (letterObject.GetComponent<Button>()) letterObject.GetComponent<Button>().enabled = false;
                    }
                }

                // Add the bonus to the score, based on the level number we are in
                ChangeScore(bonusPerLevel * currentLevel * hintBonusLossCount);

                // Reset the bonus loss value
                hintBonusLossCount = 1;

                //If there is a source and a sound, play it from the source
                if (soundSource && soundCorrect) soundSource.GetComponent<AudioSource>().PlayOneShot(soundCorrect);

                // Play the outro animation of the text hint, if it exists
                if (textHintObject && imageHintObject.GetComponent<Animation>()) textHintObject.GetComponent<Animation>().Play("HintOutro");

                // Play the outro animation of the image hint, if it exists
                if ( imageHintObject && imageHintObject.GetComponent<Animation>()) imageHintObject.GetComponent<Animation>().Play("HintOutro");

                // Wait for a moment, or for the duration of the animation if it exists
                if (letterDefault.GetComponent<Animation>()) yield return new WaitForSeconds(letterDefault.GetComponent<Animation>().GetClip("LetterCorrect").length);
                else yield return new WaitForSeconds(0.5f);
                
                // Destroy all letter objects, except the default letter object
                foreach (Transform letterObject in lettersGrid)
                {
                    if (letterObject == letterDefault) letterObject.gameObject.SetActive(false);
                    else Destroy(letterObject.gameObject);
                }
                
                // Increase the word count for this level. When we reach the required wordsPerLevel, we level up or win the game
                wordsCount++;
                
                // If we reached the required words per level, level up
                if ( wordsCount > wordsPerLevel && wordsCount != 1)
                {
                    // Reset the word count
                    wordsCount = 1;

                    // Level up
                    StartCoroutine("LevelUp", 0.2f);
                }
                else
                {
                    StartCoroutine("UpdateLevel");
                }
            }
        }

        /// <summary>
        /// Show a text or image hint, and reduce the bonus value
        /// </summary>
        public void ShowHint()
        {
            // Set the bonus loss value
            hintBonusLossCount = hintBonusLoss;

            // Make the hint button inactive
            if (buttonHint) buttonHint.gameObject.SetActive(false);

            // Show the text hint
            if ( textHintObject && wordsWithHints[index].textHint != string.Empty  ) textHintObject.gameObject.SetActive(true);

            // Show the image hint
            if ( imageHintObject && wordsWithHints[index].imageHint ) imageHintObject.gameObject.SetActive(true);
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
                if (soundSource && soundGameOver) soundSource.GetComponent<AudioSource>().PlayOneShot(soundGameOver);
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

        public void Quit()
        {
            Application.Quit();
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