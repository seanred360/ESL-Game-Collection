using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    internal int index = 0;
    internal int currentScore = 0;
    public RectTransform cardObjectPrefab;
    public RectTransform clone;
    public Canvas cardCanvas;
    public Canvas gameOverCanvas;
    QuestionsList questionsList;
    internal Text timerText;
    internal Text P1ScoreText;
    internal Text P2ScoreText;
    internal Text P1ScoreAddText;
    internal Text P2ScoreAddText;
    internal Text currentScoreText;

    internal GameObject timerIcon;
    internal Image timerBar;
    AudioManager audioManager;

    internal float time = 3;
    internal float timeLeft;
    internal bool timeRunning = false;

    public GameObject mario,luigi;
    public GameObject greenPipe;
    public Transform midPoint;
    public Rigidbody2D coinPrefab;
    bool canDropCoins = true;
    int numberDropped;
    int numberToDrop;

    // Start is called before the first frame update
    void Awake()
    {
        questionsList = GetComponent<QuestionsList>();
        clone = Instantiate(cardObjectPrefab, cardCanvas.transform);

        if (GameObject.Find("P1ScoreText")) P1ScoreText = GameObject.Find("P1ScoreText").GetComponent<Text>();
        if (GameObject.Find("P2ScoreText")) P2ScoreText = GameObject.Find("P2ScoreText").GetComponent<Text>();
        UpdateScore();
        P1ScoreAddText = GameObject.Find("P1ScoreAddText").GetComponent<Text>();
        P1ScoreAddText.gameObject.SetActive(false);
        P2ScoreAddText = GameObject.Find("P2ScoreAddText").GetComponent<Text>();
        P2ScoreAddText.gameObject.SetActive(false);

        if (GameObject.Find("TimerIcon"))
        {
            timerIcon = GameObject.Find("TimerIcon");
            if (GameObject.Find("TimerIcon/Bar")) timerBar = GameObject.Find("TimerIcon/Bar").GetComponent<Image>();
            if (GameObject.Find("TimerIcon/Text")) timerText = GameObject.Find("TimerIcon/Text").GetComponent<Text>();
        }
        audioManager = GetComponent<AudioManager>();
        timeLeft = time;
        if (Data.Singleton.turn == 1) { currentScoreText = P1ScoreText; currentScore = Data.Singleton.P1Score; }
        else { currentScoreText = P2ScoreText; currentScore = Data.Singleton.P2Score; }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0 && timeRunning == true)
        {
            // Count down the time
            timeLeft -= Time.deltaTime;
            // Update the timer
            UpdateTime();
        }
    }

    public void StartPlayer()
    {
        if (Data.Singleton.isGameOver == false)
        {
            ShuffleCards(QuestionsList.unansweredQuestions);
            ResetCard();
            if (Data.Singleton.turn == 1) { mario.GetComponent<Mario>().MoveToMid(midPoint.position); }
            else
            { luigi.GetComponent<Luigi>().MoveToMid(midPoint.position); }
        }  
    }

    public void ChooseACard()
    {
        clone.GetComponent<Button>().interactable = false;
        int randomCard = UnityEngine.Random.Range(0, QuestionsList.unansweredQuestions.Count);
        clone.Find("CardFront/Image").GetComponent<Image>().sprite = questionsList.currentQuestion.factSprite;
        clone.GetComponent<Animator>().Play("HitBlock");
        ChangeScore(questionsList.currentQuestion.points);
        
        if (GetComponent<QuestionsList>().currentQuestion.isBomb == true)
        {
            audioManager.PlaySFX(0);
            StartCoroutine(GameOverRoutine());
        }
        else { audioManager.PlayPointsSound(0); }
    }

    public void ResetCard()
    {
        clone.GetComponent<Animator>().Play("PairSpawn");
        Destroy(clone.gameObject);
        clone = Instantiate(cardObjectPrefab, cardCanvas.transform);
        GetComponent<QuestionsList>().SetCurrentQuestion();
    }

    public List<Question> ShuffleCards(List<Question> questionsToShuffle)
    {
        // Go through all the sprite pairs and shuffle them
        for (index = 0; index < questionsToShuffle.Count; index++)
        {
            // Hold the pair in a temporary variable
            Question tempPair = questionsToShuffle[index];

            // Choose a random index from the sprite pairs list
            int randomIndex = UnityEngine.Random.Range(index, questionsToShuffle.Count);

            // Assign a random sprite pair from the list
            questionsToShuffle[index] = questionsToShuffle[randomIndex];

            // Assign the temporary sprite pair to the random question we chose
            questionsToShuffle[randomIndex] = tempPair;
        }
        return questionsToShuffle;
    }

    void ChangeScore(int changeValue)
    {
        if(Data.Singleton.turn == 1)
        {
            Data.Singleton.P1Score += changeValue;
            currentScore = Data.Singleton.P1Score;
            if (P1ScoreAddText)
            {
                P1ScoreAddText.GetComponent<Text>().text = "+" + changeValue.ToString();
                P1ScoreAddText.gameObject.SetActive(true);
                if (P1ScoreAddText.GetComponent<Animation>()) P1ScoreAddText.GetComponent<Animation>().Play();
            }
        }

        else if (Data.Singleton.turn == 2)
        {
            Data.Singleton.P2Score += changeValue;
            currentScore = Data.Singleton.P2Score;
            if (P2ScoreAddText)
            {
                P2ScoreAddText.GetComponent<Text>().text = "+" + changeValue.ToString();
                P2ScoreAddText.gameObject.SetActive(true);
                if (P2ScoreAddText.GetComponent<Animation>()) P2ScoreAddText.GetComponent<Animation>().Play();
            }
        }
        //Update the score
        UpdateScore();
    }

    void UpdateScore()
    {
        //Update the score text
        if (currentScoreText) currentScoreText.text = currentScore.ToString();
    }

    void UpdateTime()
    {
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
        }
    }

    public void FinishTurn()
    {
        ResetCard();
        if (Data.Singleton.turn == 1) { Data.Singleton.turn = 2; currentScoreText = P2ScoreText; mario.GetComponent<Mario>().MoveBack(); }
        else { Data.Singleton.turn = 1; currentScoreText = P1ScoreText; luigi.GetComponent<Luigi>().MoveBack(); }
    }

    IEnumerator DropTheCoins(Transform spawnPoint)
    {
        if (numberToDrop > 0)
        {
            while (canDropCoins)
            {
                if (numberToDrop > 0)
                    yield return new WaitForSeconds(.1f);
                Rigidbody2D instance = Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity);
                audioManager.PlaySFX(1);
                instance.velocity = Random.insideUnitCircle * 10;
                while (instance.velocity.y < 0)
                    instance.velocity += new Vector2(0f, 10f);

                numberDropped += 1;
                if (numberDropped >= numberToDrop)
                {
                    canDropCoins = false;
                }
            }
        }
    }

    IEnumerator GameOverRoutine()
    {
        Data.Singleton.isGameOver = true;
        MarioCardDraw.ButtonController.DisableButton();
        MarioCardDraw.ButtonController.HideStopButton();
        greenPipe.SetActive(true);
        yield return new WaitForSeconds(3f);
        //timeRunning = true;
        //timerIcon.transform.SetAsLastSibling();
        //audioManager.PlaySFX(2);
        //yield return new WaitForSeconds(1f);
        //audioManager.PlaySFX(2);
        //yield return new WaitForSeconds(1f);
        //audioManager.PlaySFX(2);
        //yield return new WaitForSeconds(1f);
        if (Data.Singleton.turn == 1) {
            numberToDrop = Data.Singleton.P1Score; ChangeScore(-(Data.Singleton.P1Score)); mario.GetComponent<Animator>().SetBool("isDead",true);
            mario.GetComponent<AudioManager>().PlaySFX(3); mario.GetComponent<AudioManager>().PlaySFX(2); Instantiate(mario.GetComponent<Mario>().collisionParticle, mario.GetComponent<Mario>().headPos.position,Quaternion.identity); luigi.GetComponent<Animator>().Play("ThumbsUp");
            StartCoroutine(DropTheCoins(mario.transform));
        }
        else if (Data.Singleton.turn == 2) {
            numberToDrop = Data.Singleton.P2Score; ChangeScore(-(Data.Singleton.P2Score)); luigi.GetComponent<Animator>().SetBool("isDead", true);
            mario.GetComponent<AudioManager>().PlaySFX(3); mario.GetComponent<AudioManager>().PlaySFX(2); Instantiate(luigi.GetComponent<Luigi>().collisionParticle, luigi.GetComponent<Luigi>().headPos.position, Quaternion.identity); mario.GetComponent<Animator>().Play("ThumbsUp");
            StartCoroutine(DropTheCoins(luigi.transform));
        }
        //if (timerIcon.GetComponent<Animation>()) timerIcon.GetComponent<Animation>().Play();
        //audioManager.PlaySFX(0);
        gameOverCanvas.gameObject.SetActive(true);
        FinishTurn();
    }
}
