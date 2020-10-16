using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PictureArray : MonoBehaviour {

    public List<GameObject> pictures;
    public int numberChosen;
    public GameObject soundSource;
    AudioManager audioManager;
    public GameObject player1Watch;
    public GameObject player2Watch;
    public GameObject whoWinsText;
    public GameObject gameManager;
    public GameObject particleController;

    private bool coroutineAllowed = true;
    private int whosTurn = 1;

    public string _imagePath;
    Sprite[] sprites;
    public List<Sprite> unselectedSprites;
    public GameObject buttonPrefab;
    public Transform buttonsHolder;


    void Awake()
    {
        sprites = LevelDataChanger.instance.LoadSprites();
        unselectedSprites.AddRange(sprites);
        //_imagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + LevelData.Singleton.wordGroupToUse;

        //if (_imagePath == "0")
        //{
        //    _imagePath = "KBA/u1";
        //    Debug.Log("Can't find the image path");
        //}

        //if (buttonsHolder == null && GameObject.Find("ButtonsHolder")) buttonsHolder = GameObject.Find("ButtonsHolder").transform;

        //Object[] loadedSprites = Resources.LoadAll(_imagePath, typeof(Sprite));
        //sprites = new Sprite[loadedSprites.Length];
        //for (int i = 0; i < loadedSprites.Length; i++)
        //{
        //    sprites[i] = (Sprite)loadedSprites[i];
        //    unselectedSprites.Add(sprites[i]);
        //}

        for (int i = 0; i < sprites.Length; i++)
        {
            if(i < 8) // 8 is the max number of buttons I want
            {
                int rand = Random.Range(0, unselectedSprites.Count);
                GameObject newButton = Instantiate(buttonPrefab);
                newButton.transform.SetParent(buttonsHolder, false);
                newButton.GetComponent<Image>().sprite = unselectedSprites[rand];
                newButton.GetComponent<AudioSource>().clip = (Resources.Load<AudioClip>("Sounds/" + newButton.GetComponent<Image>().sprite.name));
                newButton.gameObject.name = newButton.GetComponent<Image>().sprite.name;
                int index = i;
                newButton.GetComponent<Button>().onClick.AddListener(delegate () { CheckAnswer(index); });
                pictures.Add(newButton);
                unselectedSprites.RemoveAt(rand);
            }
        }
    }
        // Use this for initialization
    void Start()
    {
        particleController = GameObject.FindGameObjectWithTag("ButtonGroup");
        soundSource = GameObject.FindGameObjectWithTag("Sound");
        audioManager = soundSource.GetComponent<AudioManager>();
        //pictures = GameObject.FindGameObjectsWithTag("ButtonTag1");
    }

    public void PickNumber()
    {
        if (GameControl.gameOver == false)
        {
            numberChosen = Random.Range(0, pictures.Count);
            pictures[numberChosen].GetComponent<AudioSource>().Play();
            //audioManager.PlaySFX(numberChosen);
            ButtonController.EnableButton();
            StartCoroutine(ChangePosition());// shuffle the pictures location
        }  
    }

    IEnumerator ChangePosition() //shuffle the pictures location
    {
        yield return new WaitForSeconds(1f);
        audioManager.PlayMusic(0);
        particleController.GetComponent<ParticleController>().EnableMixParticles();
        for (int i = 0; i < pictures.Count; i++)
        {
            Vector3 temp = pictures[i].transform.position;
            int randomIndex = Random.Range(0, pictures.Count);
            pictures[i].transform.position = pictures[randomIndex].transform.position;
            pictures[randomIndex].transform.position = temp;
        }
    }
    public void CheckAnswer(int useIndex)
    {
        Debug.Log(useIndex);
        ButtonController.DisableButton();
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
        if (coroutineAllowed)
           StartCoroutine(RollTheDice());
        audioManager.PlayMusic(1);
        particleController.GetComponent<ParticleController>().EnableCorrectParticles();
    }

    public void SelectFalse()
    {
        if (coroutineAllowed)
           StartCoroutine(BackRollTheDice());
        audioManager.PlayMusic(2);
    }

    public IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        if (whosTurn == 1)
        {
            GameControl.MovePlayer(1);
        }
        else if (whosTurn == -1)
        {
            GameControl.MovePlayer(2);
        }
        //whosTurn *= -1;
        coroutineAllowed = true;
        yield return new WaitForSeconds(2f);
        PickNumber();
    }

    public IEnumerator BackRollTheDice()
    {
        coroutineAllowed = false;
        if (whosTurn == 1)
        {
            GameControl.BackMovePlayer(1);
        }
        else if (whosTurn == -1)
        {
            GameControl.BackMovePlayer(2);
        }
        //whosTurn *= -1;
        coroutineAllowed = true;
        yield return new WaitForSeconds(2f);
        PickNumber();
    }

    public void StartPlayer1()
    {
        PickNumber();
        player1Watch.GetComponent<StopWatch>().updateTimer = true;
    }

    public void StartPlayer2()
    {
        GameControl.gameOver = false;
        player2Watch.GetComponent<StopWatch>().updateTimer = true;
        whosTurn *= -1;
        ButtonController.EnableButton();
        PickNumber();
    }
}
