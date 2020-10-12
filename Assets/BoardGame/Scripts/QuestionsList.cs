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
        //public Question[] questions; // put questions here
        //public List<Question> questions;
        public List<Question> unansweredQuestions; // send questions to this list

        private Question currentQuestion;
        
        public string _imagePath;
        public Sprite[] loadedSprites;

        [SerializeField]
        public Text factText;
        public Image factImage;

        private void Awake()
        {
            string _imagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + LevelData.Singleton.wordGroupToUse;
            Debug.Log(_imagePath);
            if (_imagePath == "0")
            {
                _imagePath = "KBA/u1";
                Debug.Log("Can't find image path");
            }
            Object[] textures = Resources.LoadAll(_imagePath, typeof(Sprite));
            loadedSprites = new Sprite[textures.Length];
            for (int i = 0; i < textures.Length; i++)
            {
                loadedSprites[i] = (Sprite)textures[i];
                Question blankQuestion = new Question();
                blankQuestion.factSprite = loadedSprites[i];
                blankQuestion.fact = loadedSprites[i].name;
                unansweredQuestions.Add(blankQuestion);
            }
        }

        void Start()
        {
            SetCurrentQuestion();
        }

        public void SetCurrentQuestion()
        {
            if (unansweredQuestions == null || unansweredQuestions.Count == 0) // load the questions typed out into the game list
            {
                for (int i = 0; i < loadedSprites.Length; i++)
                {
                    loadedSprites[i] = (Sprite)loadedSprites[i];
                    Question blankQuestion = new Question();
                    blankQuestion.factSprite = loadedSprites[i];
                    blankQuestion.fact = loadedSprites[i].name;
                    unansweredQuestions.Add(blankQuestion);
                }
            }
            int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
            currentQuestion = unansweredQuestions[randomQuestionIndex];
            unansweredQuestions.RemoveAt(randomQuestionIndex);
            //factImage.sprite = loadedSprites[randomQuestionIndex];
            //factText.text = loadedSprites[randomQuestionIndex].name;
            factText.text = currentQuestion.fact;
            factImage.sprite = currentQuestion.factSprite;
        }
    }
}
