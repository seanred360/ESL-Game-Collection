using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MarioCardDraw;

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
    public Text P1ScoreText, P2ScoreText, P3ScoreText, P4ScoreText;
    internal Text P1ScoreAddText, P2ScoreAddText, P3ScoreAddText, P4ScoreAddText;
    internal Text currentScoreText;

    AudioManager audioManager;

    public GameObject mario, luigi, peach, goombella;
    public List<Player> players;
    public GameObject greenPipe;
    public Transform midPoint;
    public Rigidbody2D coinPrefab;
    bool canDropCoins = true;
    int numberDropped;
    int numberToDrop;
    public Canvas curtain;
    public Animator initCurtain;

    // Start is called before the first frame update
    void Awake()
    {
        questionsList = GetComponent<QuestionsList>();
        clone = Instantiate(cardObjectPrefab, cardCanvas.transform);
        players.Add(mario.GetComponent<Player>());
        players.Add(luigi.GetComponent<Player>());
        players.Add(peach.GetComponent<Player>());
        players.Add(goombella.GetComponent<Player>());

        //if (GameObject.Find("P1ScoreText")) P1ScoreText = GameObject.Find("P1ScoreText").GetComponent<Text>();
        //if (GameObject.Find("P2ScoreText")) P2ScoreText = GameObject.Find("P2ScoreText").GetComponent<Text>();
        //if (GameObject.Find("P3ScoreText")) P3ScoreText = GameObject.Find("P3ScoreText").GetComponent<Text>();
        //if (GameObject.Find("P4ScoreText")) P4ScoreText = GameObject.Find("P4ScoreText").GetComponent<Text>();
        UpdateScore();
        P1ScoreAddText = GameObject.Find("P1ScoreAddText").GetComponent<Text>();
        P1ScoreAddText.gameObject.SetActive(false);
        P2ScoreAddText = GameObject.Find("P2ScoreAddText").GetComponent<Text>();
        P2ScoreAddText.gameObject.SetActive(false);
        P3ScoreAddText = GameObject.Find("P3ScoreAddText").GetComponent<Text>();
        P3ScoreAddText.gameObject.SetActive(false);
        P4ScoreAddText = GameObject.Find("P4ScoreAddText").GetComponent<Text>();
        P4ScoreAddText.gameObject.SetActive(false);

        audioManager = GetComponent<AudioManager>();
        if (Data.Singleton.turn == 1) { currentScoreText = P1ScoreText; currentScore = Data.P1Score; }
        //else { currentScoreText = P2ScoreText; currentScore = Data.P2Score; }
        //P1ScoreText.GetComponent<Text>().text = Data.P1Score.ToString();
        //P2ScoreText.GetComponent<Text>().text = Data.P2Score.ToString();
        //P3ScoreText.GetComponent<Text>().text = Data.P3Score.ToString();
        //P4ScoreText.GetComponent<Text>().text = Data.P4Score.ToString();
    }

    public void SetNumberOfPlayers( int num)
    {
        Data.numberOfPlayers = num;
        switch (Data.numberOfPlayers)
        { 
            case 1:
                mario.SetActive(true);
                break;
            case 2:
                mario.SetActive(true); luigi.SetActive(true);
                break;
            case 3:
                mario.SetActive(true); luigi.SetActive(true); peach.SetActive(true);
                break;
            case 4:
                mario.SetActive(true); luigi.SetActive(true); peach.SetActive(true);goombella.SetActive(true);
                break;
        }
        initCurtain.SetBool("isPaused", false);
    }

        public void StartPlayer(int whatTurn)
        {
            if (Data.Singleton.isGameOver == false)
            {
                ShuffleCards(QuestionsList.unansweredQuestions);
                ResetCard();
                players[whatTurn - 1].MoveToMid(midPoint.position);
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
            switch (Data.Singleton.turn)
            {
                case 1:
                    Data.P1Score += changeValue;
                    currentScore = Data.P1Score;
                    if (P1ScoreAddText)
                    {
                        P1ScoreAddText.GetComponent<Text>().text = "+" + changeValue.ToString();
                        P1ScoreAddText.gameObject.SetActive(true);
                        if (P1ScoreAddText.GetComponent<Animation>()) P1ScoreAddText.GetComponent<Animation>().Play();
                    }
                    break;
                case 2:
                    Data.P2Score += changeValue;
                    currentScore = Data.P2Score;
                    if (P2ScoreAddText)
                    {
                        P2ScoreAddText.GetComponent<Text>().text = "+" + changeValue.ToString();
                        P2ScoreAddText.gameObject.SetActive(true);
                        if (P2ScoreAddText.GetComponent<Animation>()) P2ScoreAddText.GetComponent<Animation>().Play();
                    }
                    break;
                case 3:
                    Data.P3Score += changeValue;
                    currentScore = Data.P3Score;
                    if (P3ScoreAddText)
                    {
                        P3ScoreAddText.GetComponent<Text>().text = "+" + changeValue.ToString();
                        P3ScoreAddText.gameObject.SetActive(true);
                        if (P3ScoreAddText.GetComponent<Animation>()) P3ScoreAddText.GetComponent<Animation>().Play();
                    }
                    break;
                case 4:
                    Data.P4Score += changeValue;
                    currentScore = Data.P4Score;
                    if (P4ScoreAddText)
                    {
                        P4ScoreAddText.GetComponent<Text>().text = "+" + changeValue.ToString();
                        P4ScoreAddText.gameObject.SetActive(true);
                        if (P4ScoreAddText.GetComponent<Animation>()) P4ScoreAddText.GetComponent<Animation>().Play();
                    }
                    break;
            }
            UpdateScore();
        }

        void UpdateScore()
        {
            //Update the score text
            if (currentScoreText) currentScoreText.text = currentScore.ToString();
        }

        public void FinishTurn()
        {
            ResetCard();
        switch (Data.Singleton.turn)
        {
            case 1:
                Data.Singleton.turn = 2; currentScoreText = P2ScoreText; mario.GetComponent<Player>().MoveBack();
                    break;
            case 2:
                if (Data.numberOfPlayers >= 3)
                {
                    Data.Singleton.turn = 3; currentScoreText = P3ScoreText;
                }
                else {Data.Singleton.turn = 1; currentScoreText = P1ScoreText; }
                luigi.GetComponent<Player>().MoveBack();
                    break;
            case 3:
                if (Data.numberOfPlayers == 4)
                {
                    Data.Singleton.turn = 4; currentScoreText = P4ScoreText;
                }
                else { Data.Singleton.turn = 1; currentScoreText = P1ScoreText; }
                peach.GetComponent<Player>().MoveBack();
                    break;
            case 4:
                Data.Singleton.turn = 1; currentScoreText = P1ScoreText; goombella.GetComponent<Player>().MoveBack();
                    break;
            }
        }

        IEnumerator DropTheCoins(Transform spawnPoint)
        {
            if (numberToDrop > 0)
            {
                canDropCoins = true;
                while (canDropCoins)
                {
                    if (numberToDrop > 0)
                        yield return new WaitForSeconds(.1f);
                    Rigidbody2D instance = Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity);
                    Destroy(instance.gameObject, 6f);
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

        void KillPlayer()
        {
            switch (Data.Singleton.turn)
            {
                case 1:
                    numberToDrop = Data.P1Score; ChangeScore(-(Data.P1Score)); mario.GetComponent<Animator>().SetBool("isDead", true);
                    mario.GetComponent<AudioManager>().PlaySFX(3); mario.GetComponent<AudioManager>().PlaySFX(2); Instantiate(mario.GetComponent<Player>().collisionParticle,
                    mario.GetComponent<Player>().headPos.position, Quaternion.identity);
                    StartCoroutine(DropTheCoins(mario.transform));
                    break;
                case 2:
                    numberToDrop = Data.P2Score; ChangeScore(-(Data.P2Score)); luigi.GetComponent<Animator>().SetBool("isDead", true);
                    mario.GetComponent<AudioManager>().PlaySFX(3); mario.GetComponent<AudioManager>().PlaySFX(2); Instantiate(luigi.GetComponent<Player>().collisionParticle,
                    luigi.GetComponent<Player>().headPos.position, Quaternion.identity);
                    StartCoroutine(DropTheCoins(luigi.transform));
                    break;
                case 3:
                    numberToDrop = Data.P3Score; ChangeScore(-(Data.P3Score)); peach.GetComponent<Animator>().SetBool("isDead", true);
                    mario.GetComponent<AudioManager>().PlaySFX(3); mario.GetComponent<AudioManager>().PlaySFX(2); Instantiate(peach.GetComponent<Player>().collisionParticle,
                    peach.GetComponent<Player>().headPos.position, Quaternion.identity);
                    StartCoroutine(DropTheCoins(peach.transform));
                    break;
                case 4:
                    numberToDrop = Data.P4Score; ChangeScore(-(Data.P4Score)); goombella.GetComponent<Animator>().SetBool("isDead", true);
                    mario.GetComponent<AudioManager>().PlaySFX(3); mario.GetComponent<AudioManager>().PlaySFX(2); Instantiate(goombella.GetComponent<Player>().collisionParticle,
                    goombella.GetComponent<Player>().headPos.position, Quaternion.identity);
                    StartCoroutine(DropTheCoins(goombella.transform));
                    break;
            }
        foreach(Player player in players)
        {
            if(player.GetComponent<Animator>().GetBool("isDead") == false)
            {
                player.GetComponent<Animator>().Play("ThumbsUp");
            }
        }
        }

        IEnumerator GameOverRoutine()
        {
            Data.Singleton.isGameOver = true;
            ButtonController.DisableButton();
            ButtonController.HideStopButton();
            greenPipe.SetActive(true);
            yield return new WaitForSeconds(3f);
            KillPlayer();
            //gameOverCanvas.gameObject.SetActive(true);
            //FinishTurn();
            StartCoroutine(MoveBack());
        }

        IEnumerator MoveBack()
        {
            yield return new WaitForSeconds(3f);
            curtain.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            //Data.Singleton.isAtMid = true;
            Data.Singleton.isGameOver = false;
            foreach (Player player in players)
            {
                player.GetComponent<Animator>().SetBool("isDead", false);
                player.GetComponent<Animator>().Play("Idle");
                //player.MoveBack();
            }
            FinishTurn();
               greenPipe.transform.Find("PiranhaPlantHead").gameObject.SetActive(false);
            greenPipe.SetActive(false);
            yield return new WaitForSeconds(5f);
            curtain.gameObject.SetActive(false);
        }
}
