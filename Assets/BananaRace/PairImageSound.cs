using System;
using UnityEngine;

namespace MatchingGameTemplate.Types
{
    [Serializable]
    public class PairImageSound
    {
        [Tooltip("The image of the pair")]
        public Sprite image;

        [Tooltip("The sound of the pair")]
        public AudioClip sound;
    }
}

