using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNumber : MonoBehaviour
{

    public static int numberOfLevel;
    public static string bookName;
    public static string menuPath;
    public static bool babyMode = false;

    [Tooltip("The tag of the music source")]
    public string objectTag = "LevelNumber";

    [Tooltip("The time this instance of the music source has been in the game")]
    internal float instanceTime = 0;

    void Start()
    {
        //Don't destroy this object when loading a new scene
        DontDestroyOnLoad(transform.gameObject);
    }

    public void ChangeLevelNumber(int number)
    {
        numberOfLevel = (number);
        Debug.Log(numberOfLevel);
    }

    public void ChangeBookName(string name)
    {
        bookName = (name);
        if (bookName == "KBA/u")
            menuPath = "KBAMainMenu";
        if (bookName == "KBB/u")
            menuPath = "KBBMainMenu";
        Debug.Log(menuPath);
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
        babyMode = onOff;
    }
}
