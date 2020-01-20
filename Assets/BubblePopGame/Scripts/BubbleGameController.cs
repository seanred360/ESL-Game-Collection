using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BubbleGameController : MonoBehaviour
{
    public Transform canvas;
    public GameObject endScreen;
    public AudioManager endSounds;
    public float spawnInterval;
    public GameObject bubblePrefab;
    public GameObject finalBubble;
    public Transform[] spawnPoints;
    Vector2 chosenSpawn;
    Vector2 previousSpawn;
    public int points,maxPoints = 45;
    bool gameover;

    public string _imagePath;
    public Sprite[] sprites;

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
    }

    private void Update()
    {
        if(points >= maxPoints && gameover == false)
        {
            gameover = true;
            
            finalBubble.SetActive(true);
        }
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
            GameObject clone = Instantiate(bubblePrefab, chosenSpawn, Quaternion.identity);
            clone.transform.SetParent(canvas);
            clone.transform.localScale = new Vector3(2, 2, 2);
            Destroy(clone, 8f);
            spawnInterval = Random.Range(.5f, 2f);
            Invoke("SpawnBubble", spawnInterval);
        }
    }

    public void StartGameOver()
    {
        Invoke("GameOver",2f);
    }
    void GameOver()
    {
        endScreen.SetActive(true);
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
