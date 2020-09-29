using System;
using UnityEngine;

namespace ScrambledWordGame.Types
{
    [Serializable]
    public class WordWithHints
    {
        [Tooltip("The word which we need to unscramble")]
        public string word;

        [Tooltip("The text hint that accompanies this word")]
        public string textHint;

        [Tooltip("The image hint that accompanies this word")]
        public Sprite imageHint;
    }
}

