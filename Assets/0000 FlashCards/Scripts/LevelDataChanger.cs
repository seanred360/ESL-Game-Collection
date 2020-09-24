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

    public void BabyModeOn(bool onOff)
    {
        LevelData.Singleton.babyMode = onOff;
    }

    public void AddWords(bool onOff)
    {
        LevelData.Singleton.addWords = onOff;
    }

    public void AddPhonics(bool onOff)
    {
        LevelData.Singleton.addPhonics = onOff;
    }
}
