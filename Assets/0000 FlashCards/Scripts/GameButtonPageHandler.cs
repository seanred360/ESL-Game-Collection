using UnityEngine;
using UnityEngine.UI;

public class GameButtonPageHandler : MonoBehaviour
{
    public GameObject[] pages;
    public Button[] pageButtons;

    public void ChangePage(int pgNum)
    {
        for( int i = 0; i < pages.Length; i++ )
        {
            if( i + 1 != pgNum )
            {
                pages[i].SetActive(false);
            }
            else { pages[i].SetActive(true); }
        }
        EnableButtons();
    }

    void EnableButtons()
    {
        foreach(Button butt in pageButtons)
        {
            butt.interactable = true;
        }
    }
}
