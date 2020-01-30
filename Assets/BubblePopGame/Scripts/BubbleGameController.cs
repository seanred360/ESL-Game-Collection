using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BubbleGameController : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Transform canvas;
    public GameObject endScreen, bubblePrefab, bombPrefab, finalBubble,changePlayersIcon,cam,BGM;
    public bool isMultiplayer;
    public AudioManager endSounds,audioManager;
    public float spawnInterval;
    Vector2 chosenSpawn, previousSpawn;
    public int points,maxPoints = 45;
    bool gameover;
    public Text scoreText;

    public string _imagePath;
    public Sprite[] sprites;

    public float speed = 3;
    public float acceleration = 3;

    internal GameObject timerIcon;
    internal Image timerBar;
    internal Text timerText;
    internal Text timeBonusText;
    public float time = 10;
    internal float timeLeft;


    private void Awake()
    {
        _imagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + LevelData.Singleton.wordGroupToUse;
        Debug.Log(_imagePath);

        if (_imagePath == "0")
        {
            _imagePath = "KBA/u8/phonics";
            Debug.Log("Can't find the image path");
        }

        Object[] loadedSprites = Resources.LoadAll(_imagePath, typeof(Sprite));
        sprites = new Sprite[loadedSprites.Length];
        for (int i = 0; i < loadedSprites.Length; i++)
        {
            sprites[i] = (Sprite)loadedSprites[i];
            if(sprites[i].name.Length < 2)
            {
                finalBubble.transform.Find("Letter").GetComponent<Image>().sprite = sprites[i];
                finalBubble.GetComponent<FlashyLetter>().letterNameSound.clip = (Resources.Load<AudioClip>("Sounds/" + sprites[i].name));
                finalBubble.GetComponent<FlashyLetter>().phonicsSound.clip = (Resources.Load<AudioClip>("Sounds/PhonicsSounds/soundch_" + sprites[i].name));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnBubble();

        timeLeft = time;

        if (isMultiplayer)
        {
            StartCoroutine(ChangePlayers());
        }

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
    }

    private void Update()
    {
        if(points >= maxPoints && gameover == false)
        {
            gameover = true;
            
            finalBubble.SetActive(true);
        }

        if (timeLeft > 0)
        {
            // Count down the time
            timeLeft -= Time.deltaTime;
        }
        UpdateTime();
    }

    void UpdateTime()
    {
        if(gameover == false)
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
                    gameover = true;
                    // Run the game over event
                    Invoke("GameOver", 1.5f);

                    // Play the timer icon Animator
                    if (timerIcon.GetComponent<Animation>()) timerIcon.GetComponent<Animation>().Play();

                    //If there is a source and a sound, play it from the source
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayVoice(0);
                }
            }
        } 
    }

    public void DisableTimer()
    {
        timeLeft = 999;
        timerBar.gameObject.SetActive(false);
        timerIcon.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    public void UpdateScore(int pts)
    {
        points += pts;
        scoreText.text = points.ToString();
    }

    void SpawnBubble()
    {
        if (gameover == false)
        {
            while (chosenSpawn == previousSpawn)
            {
                chosenSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            }

            previousSpawn = chosenSpawn;
            int bombChance = Random.Range(0, 4);
            GameObject clone;

            if (bombChance == 0) { clone = Instantiate(bombPrefab, chosenSpawn, Quaternion.identity); }
            else { clone = Instantiate(bubblePrefab, chosenSpawn, Quaternion.identity); }

            clone.transform.SetParent(canvas);
            clone.transform.localScale = new Vector3(2, 2, 2);
            Destroy(clone, 8f);
            spawnInterval = Random.Range(.5f, 2f);
            Invoke("SpawnBubble", spawnInterval);
        }
    }

    IEnumerator ChangePlayers()
    {
        //yield return new WaitForSeconds(Random.Range(8,15));
        yield return new WaitForSeconds(10);
        if (isMultiplayer)
        {
            iTween.ShakePosition(cam, new Vector3(1f, 1f, 0), 3f);
            audioManager.PlayVoice(2);
            changePlayersIcon.SetActive(true);
            StartCoroutine(ChangePlayers());
            yield return new WaitForSeconds(3.6f);
            changePlayersIcon.SetActive(false);
        }
    }

    void DisableChangePlayerIcon()
    {

    }
    public void StartGameOver()
    {
        Invoke("GameOver",2f);
    }
    void GameOver()
    {
        StopAllCoroutines();
        if (changePlayersIcon) { changePlayersIcon.SetActive(false); }
        BGM.SetActive(false);
        endScreen.SetActive(true);
        endScreen.SetActive(true);
        endSounds.PlayMusic(0);
        //endSounds.PlaySFX(Random.Range(0, 2));
        endSounds.PlaySFX(3);
        gameover = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
