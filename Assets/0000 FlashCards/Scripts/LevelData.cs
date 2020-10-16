
public sealed class LevelData
{
    public static LevelData instance;
    public int numberOfLevel;
    public string bookName;
    public string menuPath;
    public string wordGroupToUse;
    public bool addWords = true;
    public bool addPhonics = true;
    public string ImagePath;

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
    public void ChangeWordGroupToUse(string name)
    {
        wordGroupToUse = (name);
    }

    public void EnableTheButtons()
    {
        ButtonController.EnableButton();
    }
    public void DisableTheButtons()
    {
        ButtonController.DisableButton();
    }
}
