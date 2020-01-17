using UnityEngine;
using System;
using ScrambledWordGame.Types;
using System.Collections.Generic;

namespace ScrambledWordGame
{
    /// <summary>
    /// This script displays a list of words, each one can have a text hint and image hint
    /// </summary>
    public class SWGWordsWithHints : MonoBehaviour
    {
        [Tooltip("Should we show the hint when the word is displayed? If we don't show the hint at the start, a hint button will appear, which we can press to show the hint")]
        public bool showHintAtStart = false;

        [Tooltip("Which percentage of the bonus points we lose when we show the hint, for example 0.5 means we lose 50% of the bonus, while 0 means we lose all of the bonus")]
        [Range(0, 1)]
        public float hintBonusLoss = 0.5f;

        [Tooltip("A list of all the words in the game. In this list each word has a text/image hint to accompany it. You can either have a list of word only, or words with hints, but not both")]
        public List<WordWithHints> wordsWithHints;

        public string _imagePath;
        Sprite[] sprites;

        void OnValidate()
        {
            if (showHintAtStart == true) hintBonusLoss = 1;
        }

        void Awake()
        {
            _imagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + LevelData.Singleton.wordGroupToUse;
            Debug.Log(_imagePath);

            if (_imagePath == "0")
            {
                _imagePath = "KBA/u1";
                Debug.Log("Can't find the image path");
            }

            UnityEngine.Object[] loadedSprites = Resources.LoadAll(_imagePath, typeof(Sprite));
            sprites = new Sprite[loadedSprites.Length];
            for (int i = 0; i < loadedSprites.Length; i++)
            {
                sprites[i] = (Sprite)loadedSprites[i];
            }

            for (int i = 0; i < sprites.Length; i++)
            {
                WordWithHints newWord = new WordWithHints();
                newWord.word = sprites[i].name;
                newWord.imageHint = sprites[i];
                newWord.textHint = sprites[i].name;
                wordsWithHints.Add(newWord);
            }
            for (int i = 0; i < wordsWithHints.Count; i++)
            {
                if(wordsWithHints[i].word.Length < 2)
                {
                    wordsWithHints.RemoveAt(i);
                }
            }

            // If we have a game controller, assign the list of words to it
            if (GetComponent<SWGGameController>())
            {
                GetComponent<SWGGameController>().wordsWithHints = wordsWithHints;

                GetComponent<SWGGameController>().showHintAtStart = showHintAtStart;

                GetComponent<SWGGameController>().hintBonusLoss = hintBonusLoss;
            }
        }
	}
}