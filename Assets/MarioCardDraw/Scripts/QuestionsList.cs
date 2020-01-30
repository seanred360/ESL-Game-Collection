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
            string _imagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + LevelData.Singleton.wordGroupToUse;
            if (_imagePath == "0") { _imagePath = "KBA/u1"; Debug.Log("Can't find the image path"); }

            Object[] loadedSprites = Resources.LoadAll(_imagePath, typeof(Sprite));
            for (int i = 0; i < loadedSprites.Length; i++)
            {
                Question question = new Question();
                question.factSprite = (Sprite)loadedSprites[i];
                question.fact = loadedSprites[i].name;
                question.points = 1;
                questions.Add(question);
            }
            while (questions.Count < 13)
            {
                int rand = Random.Range(0, questions.Count);
                questions.Add(questions[rand]);
            }
            questions.Add(bomb);
        }
        // only add 2 bombs if kid's box version
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

    public static List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }

        return _list;
    }
}
