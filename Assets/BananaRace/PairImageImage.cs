using System;
using UnityEngine;

namespace MatchingGameTemplate.Types
{
    [Serializable]
    public class PairImageImage
    {
        [Tooltip("The first image of the pair")]
        public Sprite firstImage;

        [Tooltip("The second image of the pair")]
        public Sprite secondImage;
    }
}

