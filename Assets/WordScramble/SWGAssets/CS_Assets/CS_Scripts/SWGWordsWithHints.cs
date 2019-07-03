using UnityEngine;
using System;
using ScrambledWordGame.Types;

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
        public WordWithHints[] wordsWithHints;
        
        void OnValidate()
        {
            if (showHintAtStart == true) hintBonusLoss = 1;
        }

        void Awake()
        {
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