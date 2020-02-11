using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Umbrella : MonoBehaviour
{
    Vector2 endMove;
    PhonicsLettersController gameManager;

    //public void OnPointerClick(PointerEventData umbrella)
    //{
    //    if (GetComponent<Button>().interactable == true)
    //    {
    //        audioSource.Play();
    //        endMove.x = Random.Range(10, 30);
    //        endMove.y = Random.Range(10, 30);
    //        transform.DOMove(endMove, 1f).OnComplete(DestroyThis);
    //    }
    //}

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<PhonicsLettersController>();
    }

    private void OnMouseDown()
    {
        gameManager.audioManager.PlaySFX(0);
        gameManager.audioManager.PlaySFX(2);
        endMove.x = 20 * Random.Range(-1,1);
        endMove.y = 20 * Random.Range(-1, 1);
        if (endMove == Vector2.zero) endMove = new Vector2(10, 10);
        Debug.Log(endMove);
        transform.DOMove(endMove, 2f).OnComplete(DestroyThis);
        transform.DORotate(new Vector3(0, 0, 359), 1f);
    }

    void DestroyThis()
    {
        gameManager.wordObjects.Remove(gameObject);
        Destroy(gameObject);
    }
}
