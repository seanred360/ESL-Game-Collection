using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class VocabPicLoader : MonoBehaviour
{

    string _wordsImagePath;
    string _phonicsImagePath;
    public GameObject buttonPrefab;
    public Transform wordsButtonHolder, phonicsButtonHolder;
    public Sprite[] words, phonics;

    PlaySoundTouch playSoundTouch;

    private void Awake()
    {
        playSoundTouch = GetComponent<PlaySoundTouch>();

        _wordsImagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + "/words";
        _phonicsImagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + "/phonics";

        if (_wordsImagePath == "0")
        {
            _wordsImagePath = "KBA/u1";
            Debug.Log("Can't find the image path");
        }

        #region create vocab word buttons

        if (wordsButtonHolder == null && GameObject.Find("WordGroup")) wordsButtonHolder = GameObject.Find("WordGroup").transform;

        Object[] loadedSprites = Resources.LoadAll(_wordsImagePath, typeof(Sprite));
        words = new Sprite[loadedSprites.Length];
        for (int i = 0; i < loadedSprites.Length; i++)
        {
            words[i] = (Sprite)loadedSprites[i];
        }

        for (int i = 0; i < words.Length; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(wordsButtonHolder, true);
            newButton.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            newButton.GetComponent<Image>().sprite = words[i];
            newButton.gameObject.name = newButton.GetComponent<Image>().sprite.name;
            newButton.GetComponent<PlaySoundTouch>().sound = (Resources.Load<AudioClip>("Sounds/" + newButton.GetComponent<Image>().sprite.name));
        }

        #endregion

        #region create phonics word buttons

        if (phonicsButtonHolder == null && GameObject.Find("PhonicsGroup")) phonicsButtonHolder = GameObject.Find("PhonicsGroup").transform;

        Object[] loadedSprites2 = Resources.LoadAll(_phonicsImagePath, typeof(Sprite));
        phonics = new Sprite[loadedSprites2.Length];
        for (int i = 0; i < loadedSprites2.Length; i++)
        {
            phonics[i] = (Sprite)loadedSprites2[i];
        }

        for (int i = 0; i < phonics.Length; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(phonicsButtonHolder, true);
            newButton.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            newButton.GetComponent<Image>().sprite = phonics[i];
            newButton.gameObject.name = newButton.GetComponent<Image>().sprite.name;
            newButton.GetComponent<PlaySoundTouch>().sound = (Resources.Load<AudioClip>("Sounds/" + newButton.GetComponent<Image>().sprite.name));
        }

        #endregion
    }
}
