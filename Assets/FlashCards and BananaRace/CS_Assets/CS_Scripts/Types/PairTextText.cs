using System;
using UnityEngine;

namespace MatchingGameTemplate.Types
{
    [Serializable]
    public class PairTextText
    {
        [Tooltip("The first text of the pair")]
        public string firstText;

        [Tooltip("The second text of the pair")]
        public string secondText;
    }
}

