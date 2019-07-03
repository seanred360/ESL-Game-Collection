using UnityEngine;
using System;

namespace TwoOfAKindGame
{
    /// <summary>
    /// This script displays a list of images ( Sprite )
    /// </summary>
    public class TOKPairsText : MonoBehaviour
    {
        public string[] pairsText;
        public Sprite[] images;
        public string _imagePath;

        void Awake()
        {
            _imagePath = LevelNumber.bookName + LevelNumber.numberOfLevel;

            if (_imagePath == "0")
            {
                _imagePath = "KBA/u1";
                Debug.Log("Can't find the image path");
            }

            object[] textures = Resources.LoadAll(_imagePath, typeof(Sprite));
            images = new Sprite[textures.Length];
            for (int i = 0; i < textures.Length; i++)
            {
                images[i] = (Sprite)textures[i];
            }

            pairsText = new string[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                pairsText[i] = (String)images[i].name;
            }

            // If we have a game controller, assign the list of images to it
            if (GetComponent<TOKGameController>()) GetComponent<TOKGameController>().pairsText = pairsText;
        }
	}
}