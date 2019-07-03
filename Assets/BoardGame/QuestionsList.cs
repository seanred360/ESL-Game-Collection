
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

namespace BoardGame
{
    public class QuestionsList : MonoBehaviour
    {
        public Question[] questions; // put questions here
        private static List<Question> unansweredQuestions; // send questions to this list

        private Question currentQuestion;

        public string _imagePath;
        public Sprite[] loadedSprites;

        [SerializeField]
        public Text factText;
        public Image factImage;

        private void Awake()
        {
             _imagePath = LevelNumber.bookName + LevelNumber.numberOfLevel;
            if(_imagePath == "0")
            _imagePath = "KBA/u1";
            Debug.Log(_imagePath);

            Object[] textures = Resources.LoadAll(_imagePath, typeof(Sprite));
            loadedSprites = new Sprite[textures.Length];
            for (int i = 0; i < textures.Length; i++)
            {
                loadedSprites[i] = (Sprite)textures[i];
            }
        }

        void Start()
        {
            if (unansweredQuestions == null || unansweredQuestions.Count == 0) // load the questions typed out into the game list
            {
                unansweredQuestions = questions.ToList<Question>();
            }
            SetCurrentQuestion();
        }

        public void SetCurrentQuestion()
        {
            int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
            currentQuestion = unansweredQuestions[randomQuestionIndex];

            factImage.sprite = loadedSprites[randomQuestionIndex];
            factText.text = loadedSprites[randomQuestionIndex].name;
            //factText.text = currentQuestion.fact;
            //factImage.sprite = currentQuestion.factSprite;
        }
    }
}
