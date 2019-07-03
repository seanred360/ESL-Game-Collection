using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BDGameControl : MonoBehaviour
{
    public GameObject[] bombs;
    public GameObject[] vocabPics;
    public AudioManager audioManager;
    public Camera cam;
    public GameObject gameOverCanvas;
    public GameObject buttonCanvas;
    public AudioSource BGM;

    public GameObject happyParticle;
    Vector2 screenPos;
    Vector2 worldPos;
    GameObject clone;

    public int numberChosen;
    int bombToDestroy;

    public string _imagePath;
    Sprite[] loadedSprites;
    public Transform parent;

    private void Awake()
    {
        _imagePath = LevelNumber.bookName + LevelNumber.numberOfLevel;
        if (_imagePath == "0")
            _imagePath = "KBA/u11";
        Debug.Log(_imagePath);

        object[] textures = Resources.LoadAll(_imagePath, typeof(Sprite));
        loadedSprites = new Sprite[textures.Length];
        for (int i = 0; i < textures.Length; i++)
        {
            loadedSprites[i] = (Sprite)textures[i];
        }

        for (int i = 0; i < textures.Length; i++)
        {
           vocabPics[i].GetComponent<Image>().sprite = loadedSprites[i];
        }
        ShufflePictures();
    }
    private void Start()
    {
        PickNumber();
        for (int i = 0; i < bombs.Length; i++)
        {
            vocabPics[i].transform.SetParent(parent);
        }
    }

    public void PickNumber()
    {
        numberChosen = Random.Range(0, bombs.Length);
    }

    public void CheckAnswer(int useIndex)
    {
        bombToDestroy = useIndex;
        if (numberChosen == useIndex)
        {
            SelectTrue();
        }
        if (numberChosen != useIndex)
        {
            SelectFalse();
        }
    }

    public void SelectTrue()
    {
        BGM.enabled = false;
        audioManager.PlaySFX(0);
        buttonCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
    }

    public void SelectFalse()
    {
        audioManager.PlayMusic(Random.Range(0,5));
        screenPos = Input.mousePosition;
        worldPos = cam.ScreenToWorldPoint(screenPos);
        clone = Instantiate(happyParticle, worldPos, Quaternion.identity);
    }

    void ShufflePictures() //shuffle the pictures location
    {
        for (int i = 0; i < bombs.Length; i++)
        {
            Vector3 temp = bombs[i].transform.position;
            int randomIndex = Random.Range(0, bombs.Length);
            bombs[i].transform.position = bombs[randomIndex].transform.position;
            bombs[randomIndex].transform.position = temp;
        }
    }
}
