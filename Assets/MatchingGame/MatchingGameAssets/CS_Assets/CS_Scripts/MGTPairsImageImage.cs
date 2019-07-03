using UnityEngine;
using System;
using MatchingGameTemplate.Types;

namespace MatchingGameTemplate
{
    /// <summary>
    /// This script displays a list of text pairs, each pair contains a First Text and a Second Text
    /// </summary>
    public class MGTPairsImageImage : MonoBehaviour
    {
        public PairImageImage[] pairsImageImage;

        void Awake()
        {
            // If we have a game controller, assign the list of text pairs to it
            if (GetComponent<MGTGameController>()) GetComponent<MGTGameController>().pairsImageImage = pairsImageImage;
        }
	}
}