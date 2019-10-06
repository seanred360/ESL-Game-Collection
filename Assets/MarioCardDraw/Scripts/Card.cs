using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;


public class Card : MonoBehaviour, IPointerClickHandler
{
    public GameController gameController;
    public Image cardBack;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (Data.Singleton.turn == 1) { cardBack.color = Color.red; }
        else { cardBack.color = Color.green; }
        switch(Data.Singleton.turn)
        {
            case 1:
                cardBack.color = Color.red;
                break;
            case 2:
                cardBack.color = Color.green;
                break;
            case 3:
                cardBack.color = Color.magenta;
                break;
            case 4:
                cardBack.color = Color.yellow;
                break;
        }
    }

    public void OnPointerClick(PointerEventData CardButton)
    {
        if(GetComponent<Button>().interactable == true && Data.Singleton.isGameOver == false && Data.Singleton.canClick)
        {
            gameController.StartPlayer(Data.Singleton.turn);
            ButtonController.DisableButton();
            ButtonController.HideStopButton();
            Data.Singleton.canClick = false;
        }
    }
}
