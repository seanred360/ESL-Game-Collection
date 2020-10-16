using UnityEngine;

public class LevelDataChanger : MonoBehaviour
{
    #region Singleton

    public static LevelDataChanger instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of " + this.name + " found!");
            return;
        }
        instance = this;
    }

    #endregion

    public void ChangeLevelNumber(int number)
    {
        LevelData.Singleton.ChangeLevelNumber(number);
    }

    public void ChangeBookName(string name)
    {
        LevelData.Singleton.bookName = (name);
    }

    public void ChangeWordGroupToUse(string name)
    {
        LevelData.Singleton.wordGroupToUse = (name);
    }

    public void EnableTheButtons()
    {
        ButtonController.EnableButton();
    }
    public void DisableTheButtons()
    {
        ButtonController.DisableButton();
    }

    public void AddWords(bool onOff)
    {
        LevelData.Singleton.addWords = onOff;
    }

    public void AddPhonics(bool onOff)
    {
        LevelData.Singleton.addPhonics = onOff;
    }

    public Sprite[] LoadSprites()
    {
        LevelData.Singleton.ImagePath = LevelData.Singleton.bookName + LevelData.Singleton.numberOfLevel + LevelData.Singleton.wordGroupToUse;
        Debug.Log(LevelData.Singleton.ImagePath);

        if (LevelData.Singleton.ImagePath == "0")
        {
            LevelData.Singleton.ImagePath = "KBA/u1/words";
            Debug.Log("Can't find the image path");
        }

        Object[] loadedSprites = Resources.LoadAll(LevelData.Singleton.ImagePath, typeof(Sprite));
        Sprite[] sprites = new Sprite[loadedSprites.Length];
        for (int i = 0; i < loadedSprites.Length; i++)
        {
            sprites[i] = (Sprite)loadedSprites[i];
            if (sprites[i].name.Length < 2)
            {
                sprites[i] = (Sprite)loadedSprites[i];
            }
        }
        return sprites;
    }
}
