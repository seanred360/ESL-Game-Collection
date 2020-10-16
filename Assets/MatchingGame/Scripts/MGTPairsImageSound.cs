using UnityEngine;
using MatchingGameTemplate.Types;
using System.Collections.Generic;

namespace MatchingGameTemplate
{
    /// <summary>
    /// This script displays a list of text pairs, each pair contains a First Text and a Second Text
    /// </summary>
    public class MGTPairsImageSound : MonoBehaviour
    {
        public List<PairImageSound> pairsImageSound = new List<PairImageSound>();
        //public PairImageSound[] pairsImageSound;
        public Sprite[] sprites;
        public PairImageSound _1,_2,_3,_4,_5,_6,_7,_8,_9,_10,_11,_12,_13,_14;

        void Awake()
        {
            sprites = LevelDataChanger.instance.LoadSprites();

            //_imagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + LevelData.Singleton.wordGroupToUse;
            //if (_imagePath == "0")
            //    _imagePath = "KBA/u11";

            //object[] textures = Resources.LoadAll(_imagePath, typeof(Sprite));
            //loadedSprites = new Sprite[textures.Length];
            //for (int i = 0; i < textures.Length; i++)
            //{
            //    loadedSprites[i] = (Sprite)textures[i];
            //}

            //ListResizer.Resize<PairImageSound>(pairsImageSound, loadedSprites.Length);
            _1.image = sprites[1];
            _1.sound = Resources.Load<AudioClip>("Sounds/" + sprites[1].name);
            _2.image = sprites[2];
            _2.sound = Resources.Load<AudioClip>("Sounds/" + sprites[2].name);
            _3.image = sprites[3];
            _3.sound = Resources.Load<AudioClip>("Sounds/" + sprites[3].name);
            _4.image = sprites[4];
            _4.sound = Resources.Load<AudioClip>("Sounds/" + sprites[4].name);
            _5.image = sprites[5];
            _5.sound = Resources.Load<AudioClip>("Sounds/" + sprites[5].name);
          
            pairsImageSound.Add(_1);
            pairsImageSound.Add(_2);
            pairsImageSound.Add(_3);
            pairsImageSound.Add(_4);
            pairsImageSound.Add(_5);
        
            if (sprites.Length > 6)
            {
                _6.image = sprites[6];
                _6.sound = Resources.Load<AudioClip>("Sounds/" + sprites[6].name);
                pairsImageSound.Add(_6);
            }
            if (sprites.Length > 7)
            {
                _7.image = sprites[7];
                _7.sound = Resources.Load<AudioClip>("Sounds/" + sprites[7].name);
                pairsImageSound.Add(_7);
            }
            if (sprites.Length > 8)
            {
                _8.image = sprites[8];
                _8.sound = Resources.Load<AudioClip>("Sounds/" + sprites[8].name);
                pairsImageSound.Add(_8);
            }
            if (sprites.Length > 9)
            {
                _9.image = sprites[9];
                _9.sound = Resources.Load<AudioClip>("Sounds/" + sprites[9].name);
                pairsImageSound.Add(_9);
            }
            if (sprites.Length > 10)
            {
                _10.image = sprites[10];
                _10.sound = Resources.Load<AudioClip>("Sounds/" + sprites[10].name);
                pairsImageSound.Add(_10);
            }
            if (sprites.Length > 11)
            {
                _11.image = sprites[11];
                _11.sound = Resources.Load<AudioClip>("Sounds/" + sprites[11].name);
                pairsImageSound.Add(_11);
            }
            if (sprites.Length > 12)
            {
                _12.image = sprites[12];
                _12.sound = Resources.Load<AudioClip>("Sounds/" + sprites[12].name);
                pairsImageSound.Add(_12);
            }
            if (sprites.Length > 13)
            {
                _13.image = sprites[13];
                _13.sound = Resources.Load<AudioClip>("Sounds/" + sprites[13].name);
                pairsImageSound.Add(_13);
            }
            if (sprites.Length > 14)
            {
                _14.image = sprites[14];
                _14.sound = Resources.Load<AudioClip>("Sounds/" + sprites[14].name);
                pairsImageSound.Add(_14);
            }

            // If we have a game controller, assign the list of text pairs to it
            if (GetComponent<MGTGameController>()) GetComponent<MGTGameController>().pairsImageSound = pairsImageSound;
        }
    }
}