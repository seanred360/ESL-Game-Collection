using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    }

    public void OnPointerClick(PointerEventData CardButton)
    {
        if(GetComponent<Button>().interactable == true && Data.Singleton.isGameOver == false)
        {
            gameController.StartPlayer();
            MarioCardDraw.ButtonController.DisableButton();
            MarioCardDraw.ButtonController.HideStopButton();
        }
    }
}
