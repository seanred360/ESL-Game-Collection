using UnityEngine;
using System;
using UnityEngine.UI;

namespace FindTheObject
{
    /// <summary>
    /// This script displays a list of text pairs, each pair contains a First Text and a Second Text
    /// </summary>
    public class FTOTexts : MonoBehaviour
    {
        public string[] texts;
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

            texts = new string[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                texts[i] = (String)images[i].name;
            }

            // If we have a game controller, assign the list of text pairs to it
            if (GetComponent<FTOGameController>()) GetComponent<FTOGameController>().textsList = texts;
        }
	}
}