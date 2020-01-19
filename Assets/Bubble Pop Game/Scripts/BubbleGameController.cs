using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int points;
    bool gameover;


    // Start is called before the first frame update
    void Start()
    {
        SpawnBubble();
    }

    private void Update()
    {
        if(points >= 45 && gameover == false)
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

    public void GameOver()
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
