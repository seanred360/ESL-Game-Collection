using UnityEngine;
using System;
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
        public Sprite[] loadedSprites;
        string _imagePath;
        public PairImageSound _1,_2,_3,_4,_5,_6,_7,_8,_9,_10,_11,_12,_13,_14;
        int _index;

        void Awake()
        {
            _imagePath = LevelNumber.bookName + LevelNumber.numberOfLevel;
            if (_imagePath == "0")
                _imagePath = "KBA/u11";
            Debug.Log(_imagePath);

            object[] textures = Resources.LoadAll(_imagePath, typeof(Sprite));
            loadedSprites = new Sprite[textures.Length];
            for (int i = 0; i < textures.Length; i++)
            {
                loadedSprites[i] = (Sprite)textures[i];
            }

            //ListResizer.Resize<PairImageSound>(pairsImageSound, loadedSprites.Length);
            _1.image = loadedSprites[1];
            _1.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[1].name);
            _2.image = loadedSprites[2];
            _2.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[2].name);
            _3.image = loadedSprites[3];
            _3.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[3].name);
            _4.image = loadedSprites[4];
            _4.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[4].name);
            _5.image = loadedSprites[5];
            _5.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[5].name);
          
            pairsImageSound.Add(_1);
            pairsImageSound.Add(_2);
            pairsImageSound.Add(_3);
            pairsImageSound.Add(_4);
            pairsImageSound.Add(_5);
        
            if (loadedSprites.Length > 6)
            {
                _6.image = loadedSprites[6];
                _6.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[6].name);
                pairsImageSound.Add(_6);
            }
            if (loadedSprites.Length > 7)
            {
                _7.image = loadedSprites[7];
                _7.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[7].name);
                pairsImageSound.Add(_7);
            }
            if (loadedSprites.Length > 8)
            {
                _8.image = loadedSprites[8];
                _8.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[8].name);
                pairsImageSound.Add(_8);
            }
            if (loadedSprites.Length > 9)
            {
                _9.image = loadedSprites[9];
                _9.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[9].name);
                pairsImageSound.Add(_9);
            }
            if (loadedSprites.Length > 10)
            {
                _10.image = loadedSprites[10];
                _10.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[10].name);
                pairsImageSound.Add(_10);
            }
            if (loadedSprites.Length > 11)
            {
                _11.image = loadedSprites[11];
                _11.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[11].name);
                pairsImageSound.Add(_11);
            }
            if (loadedSprites.Length > 12)
            {
                _12.image = loadedSprites[12];
                _12.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[12].name);
                pairsImageSound.Add(_12);
            }
            if (loadedSprites.Length > 13)
            {
                _13.image = loadedSprites[13];
                _13.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[13].name);
                pairsImageSound.Add(_13);
            }
            if (loadedSprites.Length > 14)
            {
                _14.image = loadedSprites[14];
                _14.sound = Resources.Load<AudioClip>("Sounds/" + loadedSprites[14].name);
                pairsImageSound.Add(_14);
            }

            // If we have a game controller, assign the list of text pairs to it
            if (GetComponent<MGTGameController>()) GetComponent<MGTGameController>().pairsImageSound = pairsImageSound;
        }
    }
}