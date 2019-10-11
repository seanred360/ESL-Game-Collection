using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class QuestionsList : MonoBehaviour
{
    public List<Question> questions; // put questions here
    public static List<Question> unansweredQuestions; // send questions to this list

    public Question currentQuestion;
    public Question bomb;
    GameController gameController;
    public bool isKidsBeginner;

    [SerializeField]
    public Text factText;
    public Image factImage;

    private void Awake()
    {
        if(isKidsBeginner)
        {
            string _imagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel;
            if (_imagePath == "0") { _imagePath = "KBA/u1"; Debug.Log("Can't find the image path"); }
            Object[] textures = Resources.LoadAll(_imagePath, typeof(Sprite));
            for (int i = 0; i < textures.Length; i++)
            {
                Question question = new Question();
                question.factSprite = (Sprite)textures[i];
                question.fact = textures[i].name;
                question.points = 1;
                questions.Add(question);
            }
        }
        questions.Add(bomb);
        questions.Add(bomb);
    }

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        factText = gameController.clone.Find("CardFront/Text").GetComponent<Text>();
        factImage = gameController.clone.Find("CardFront/Image").GetComponent<Image>();
        if (unansweredQuestions == null || unansweredQuestions.Count == 0) // load the questions typed out into the game list
        {
            unansweredQuestions = questions.ToList<Question>();
        }
        SetCurrentQuestion();
    }

    public void SetCurrentQuestion()
    {
        if (unansweredQuestions.Count < 1)
        {
            unansweredQuestions = questions.ToList<Question>();
            gameController.ShuffleCards(unansweredQuestions);
        }
        factText = gameController.clone.Find("CardFront/Text").GetComponent<Text>();
        factImage = gameController.clone.Find("CardFront/Image").GetComponent<Image>();
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];
        factText.text = currentQuestion.fact;
        factImage.sprite = currentQuestion.factSprite;

        unansweredQuestions.RemoveAt(randomQuestionIndex);
    }
}
