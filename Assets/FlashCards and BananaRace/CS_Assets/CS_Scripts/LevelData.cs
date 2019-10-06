
public sealed class LevelData
{
    public static LevelData instance;
    public int numberOfLevel;
    public string bookName;
    public string menuPath;
    public bool babyMode = false;

    public static LevelData Singleton
    {
        get
        {
            if(instance == null)
            {
                instance = new LevelData();
            }
            return instance;
        }
    }

    public void ChangeLevelNumber(int number)
    {
        numberOfLevel = (number);
    }

    public void ChangeBookName(string name)
    {
        bookName = (name);
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
}
