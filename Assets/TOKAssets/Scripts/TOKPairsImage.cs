using UnityEngine;
using System;

namespace TwoOfAKindGame
{
    /// <summary>
    /// This script displays a list of text pairs, each pair contains a First Text and a Second Text
    /// </summary>
    public class TOKPairsImage : MonoBehaviour
    {
        public Sprite[] pairsImage;
        public string _imagePath = "KBA/u1";

        void Awake()
        {
            _imagePath = LevelNumber.bookName + LevelNumber.numberOfLevel;

            Debug.Log(_imagePath);

            object[] textures = Resources.LoadAll(_imagePath, typeof(Sprite));
            pairsImage = new Sprite[textures.Length];
            for (int i = 0; i < textures.Length; i++)
            {
                pairsImage[i] = (Sprite)textures[i];
            }
            // If we have a game controller, assign the list of text pairs to it
            if (GetComponent<TOKGameController>()) GetComponent<TOKGameController>().pairsImage = pairsImage;
        }
    }
}