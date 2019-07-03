using UnityEngine;
using System;
using UnityEngine.UI;

namespace ScrambledImageGame
{
    /// <summary>
    /// This script displays a list of images
    /// </summary>
    public class SIGImages : MonoBehaviour
    {
        public Texture2D[] images;
        public Sprite[] loadedSprites;
        public string _imagePath = "KBA/u1";

        void Awake()
        {
            _imagePath = LevelNumber.bookName + LevelNumber.numberOfLevel;
            if (_imagePath == "0")
                _imagePath = "KBA/u1";
            Debug.Log(_imagePath);

            object[] sprites = Resources.LoadAll(_imagePath, typeof(Sprite));
            loadedSprites = new Sprite[sprites.Length];
            for (int i = 0; i < sprites.Length; i++)
            {
                loadedSprites[i] = (Sprite)sprites[i];
            }

            object[] textures2d = loadedSprites;
            images = new Texture2D[textures2d.Length];
            for(int i = 0; i < textures2d.Length; i++)
            {
                images[i] = (Texture2D)textureFromSprite(loadedSprites[i]);
            } 
        }

        public static Texture2D textureFromSprite(Sprite sprite)
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                             (int)sprite.textureRect.y,
                                                             (int)sprite.textureRect.width,
                                                             (int)sprite.textureRect.height);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }
    }
}