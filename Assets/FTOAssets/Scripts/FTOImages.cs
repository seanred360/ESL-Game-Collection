using UnityEngine;
using System;
using UnityEngine.UI;

namespace FindTheObject
{
    /// <summary>
    /// This script displays a list of text pairs, each pair contains a First Text and a Second Text
    /// </summary>
    public class FTOImages : MonoBehaviour
    {
        public Sprite[] sprites;

        public string _imagePath;

        void Awake()
        {
            sprites = LevelDataChanger.instance.LoadSprites();

            //_imagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + LevelData.Singleton.wordGroupToUse;
            //Debug.Log(_imagePath);

            //if (_imagePath == "0")
            //{
            //    _imagePath = "KBA/u1";
            //    Debug.Log("Can't find the image path");
            //}

            //Debug.Log(_imagePath);

            //object[] textures = Resources.LoadAll(_imagePath, typeof(Sprite));
            //sprites = new Sprite[textures.Length];
            //for (int i = 0; i < textures.Length; i++)
            //{
            //    sprites[i] = (Sprite)textures[i];
            //}

            // If we have a game controller, assign the list of text pairs to it
            if (GetComponent<FTOGameController>()) GetComponent<FTOGameController>().imagesList = sprites;
        }
    }
}