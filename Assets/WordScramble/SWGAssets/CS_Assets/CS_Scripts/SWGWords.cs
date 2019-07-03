using UnityEngine;
using System;
using ScrambledWordGame.Types;

namespace ScrambledWordGame
{
    /// <summary>
    /// This script displays a list of words
    /// </summary>
    public class SWGWords : MonoBehaviour
    {
        internal SWGGameController gameController;

        public string[] words;

        internal int index;

        void Awake()
        {
            // If we have a game controller, assign the list of words to it
            if ( GetComponent<SWGGameController>() ) gameController = GetComponent<SWGGameController>();

            // If we have an old words list, assign the words from it to the new words-with-hints list
            if ( gameController && words.Length > 0 )
            {
                // Set the length of words-with-hints list based on the length of the words list
                gameController.wordsWithHints = new WordWithHints[words.Length];

                // Go through all the words in the old list and assign the words to the new list
                for (index = 0; index < gameController.wordsWithHints.Length; index++) gameController.wordsWithHints[index] = new WordWithHints();

                // Go through all the words in the old list and assign the words to the new list
                for (index = 0; index < gameController.wordsWithHints.Length; index++) gameController.wordsWithHints[index].word = words[index];
            }
        }
	}
}