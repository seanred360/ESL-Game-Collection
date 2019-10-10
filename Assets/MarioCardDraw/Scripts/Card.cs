using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;


public class Card : MonoBehaviour, IPointerClickHandler
{
    public GameController gameController;
    public Image cardBack;
    public static float bottomPos;
    BoxCollider2D boxCollider2d;
    public GameObject positionGetter;
    BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        Vector3 colliderCenter = Vector3.Scale(transform.localScale, boxCollider2D.offset);
        Vector3 colliderPosition = transform.position + colliderCenter;
        Vector3 colliderSize = Vector3.Scale(transform.localScale, boxCollider2D.size);
        float colliderBottom = colliderPosition.y - (colliderSize.y / 2);
        float colliderLeft = colliderPosition.x - (colliderSize.x / 2);
        float colliderRight = colliderPosition.x + (colliderSize.x / 2);
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        bottomPos = colliderBottom;
        //bottomPos = new Vector2(boxCollider2d.offset.x,boxCollider2d.offset.y - (boxCollider2d.size.y/2f));
        //bottomPos = new Vector2(transform.position.x,transform.position.y - transform.position.y/2);
        Debug.Log(bottomPos + "card bot");
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
