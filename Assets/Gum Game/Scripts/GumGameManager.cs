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
    public AudioManager endSounds;
    bool canPlayEndSounds = true;

    bool m_isGameOver = false;
    public bool IsGameOver { get { return m_isGameOver; } set { m_isGameOver = value; } }

    public string _imagePath;
    Sprite[] sprites;
    public List<Sprite> unselectedSprites;

    private void Awake()
    {
        m_food = (Object.FindObjectsOfType<Food>() as Food[]).ToList();

        _imagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + LevelData.Singleton.wordGroupToUse;
        Debug.Log(_imagePath);

        if (_imagePath == "0")
        {
            _imagePath = "KBA/u1";
            Debug.Log("Can't find the image path");
        }

        Object[] loadedSprites = Resources.LoadAll(_imagePath, typeof(Sprite));
        sprites = new Sprite[loadedSprites.Length];
        for (int i = 0; i < loadedSprites.Length; i++)
        {
            sprites[i] = (Sprite)loadedSprites[i];
            unselectedSprites.Add(sprites[i]);
        }

        // shuffle the words list to get different words every game
        for (int i = 0; i < m_food.Count; i++)
        {
            int rand = Random.Range(0, unselectedSprites.Count);
            m_food[i].transform.Find("Text").GetComponent<Text>().text = unselectedSprites[rand].name;
            m_food[i].gameObject.GetComponent<AudioSource>().clip = (Resources.Load<AudioClip>("Sounds/" + unselectedSprites[rand].name));
            unselectedSprites.RemoveAt(rand);
        }
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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
