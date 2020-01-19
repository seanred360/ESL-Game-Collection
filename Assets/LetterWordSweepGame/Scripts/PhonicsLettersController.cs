using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhonicsLettersController : MonoBehaviour
{
    public List<GameObject> wordObjects;
    public AudioManager audioManager;
    bool canPlaySFX = true;
    public GameObject touchParticle;
    public GameObject letters;
    public GameObject endScreen;
    public AudioManager endSounds;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        wordObjects.AddRange(GameObject.FindGameObjectsWithTag("Umbrella"));
    }

    private void Update()
    {
        if(wordObjects.Count <=  0 && canPlaySFX)
        {
            audioManager.PlaySFX(1);
            GameObject.Find("Big Letter").GetComponent<Image>().color = new Color(0f,.72f,1f,1f);
            GameObject.Find("Small Letter").GetComponent<Image>().color = new Color(0f, .72f, 1f, 1f);
            letters.GetComponent<Animator>().Play("LetterDance");
            StartCoroutine(OpenEndScreen());
            canPlaySFX = false;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            Instantiate(touchParticle, new Vector3(p.x, p.y, 0.0f), Quaternion.identity);

        }
    }

    IEnumerator OpenEndScreen()
    {
        yield return new WaitForSeconds(5.3f);
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
