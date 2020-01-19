using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GumGameManager : MonoBehaviour
{
    public GameObject endScreen;
    FoodPicker foodPicker;
    public List<Food> m_food;
    public AudioManager endSounds,audioManager;
    bool canPlayEndSounds = true;

    bool m_isGameOver = false;
    public bool IsGameOver { get { return m_isGameOver; } set { m_isGameOver = value; } }

    public string _imagePath;
    Sprite[] sprites;
    public List<Sprite> unselectedSprites;

    internal GameObject timerIcon;
    internal Image timerBar;
    internal Text timerText;
    internal Text timeBonusText;
    public float time = 10;
    internal float timeLeft;

    private void Awake()
    {
        m_food = (Object.FindObjectsOfType<Food>() as Food[]).ToList();

        _imagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + LevelData.Singleton.wordGroupToUse;
        Debug.Log(_imagePath);

        if (_imagePath == "0")
        {
            _imagePath = "KBA/u1/phonics";
            Debug.Log("Can't find the image path");
        }

        Object[] loadedSprites = Resources.LoadAll(_imagePath, typeof(Sprite));
        sprites = new Sprite[loadedSprites.Length];
        for (int i = 0; i < loadedSprites.Length; i++)
        {
            sprites[i] = (Sprite)loadedSprites[i];
            unselectedSprites.Add(sprites[i]);
        }

        if(sprites.Length < 5) { m_food[0].gameObject.SetActive(false); m_food.RemoveAt(0);  }

        // shuffle the words list to get different words every game
        for (int i = 0; i < m_food.Count; i++)
        {
            int rand = Random.Range(0, unselectedSprites.Count);
            m_food[i].transform.Find("Text").GetComponent<Text>().text = unselectedSprites[rand].name;
            m_food[i].gameObject.GetComponent<AudioSource>().clip = (Resources.Load<AudioClip>("Sounds/" + unselectedSprites[rand].name));
            unselectedSprites.RemoveAt(rand);
        }
        timeLeft = time;

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
        if (timeLeft > 0)
        {
            // Count down the time
            timeLeft -= Time.deltaTime;
        }
        UpdateTime();
    }

    public bool AreAllFoodsEaten()
    {
        foreach (Food food in m_food)
        {
            if (!food.isEaten)
            {
                return false;
            }
        }
        Debug.Log("all food eaten");
        return true;
    }

    public void GameOver()
    {
        canPlayEndSounds = false;
        endScreen.SetActive(true);
        endSounds.PlayMusic(0);
        endSounds.PlaySFX(Random.Range(0, 2));
    }

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
                Invoke("GameOver" ,1.5f);

                // Play the timer icon Animator
                if (timerIcon.GetComponent<Animation>()) timerIcon.GetComponent<Animation>().Play();

                //If there is a source and a sound, play it from the source
                audioManager.PlayPointsSound(0);
            }
        }
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
