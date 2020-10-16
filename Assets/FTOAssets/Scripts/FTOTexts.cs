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
        public Sprite[] sprites;
        public string _imagePath;

        void Awake()
        {
            //_imagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + LevelData.Singleton.wordGroupToUse;

             //if (_imagePath == "0")
             //{
             //    _imagePath = "KBA/u1";
             //    Debug.Log("Can't find the image path");
             //}

             //object[] textures = Resources.LoadAll(_imagePath, typeof(Sprite));
             //images = new Sprite[textures.Length];
             //for (int i = 0; i < textures.Length; i++)
             //{
             //    images[i] = (Sprite)textures[i];
             //}

            sprites = LevelDataChanger.instance.LoadSprites();

            texts = new string[sprites.Length];
            for (int i = 0; i < sprites.Length; i++)
            {
                texts[i] = (String)sprites[i].name;
            }

            // If we have a game controller, assign the list of text pairs to it
            if (GetComponent<FTOGameController>()) GetComponent<FTOGameController>().textsList = texts;
        }
	}
}