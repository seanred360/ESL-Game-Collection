using UnityEngine;
using System;
using MatchingGameTemplate.Types;

namespace MatchingGameTemplate
{
    /// <summary>
    /// This script displays a list of images ( Sprite )
    /// </summary>
    public class MGTPairsSoundText : MonoBehaviour
    {
        public AudioClip[] pairs;

        void Awake()
        {
            // If we have a game controller, assign the list of images to it
            //if (GetComponent<MGTGameController>()) GetComponent<MGTGameController>().pairs = pairs;
        }
	}
}