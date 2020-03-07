using System.Collections;
using UnityEngine;
using NBG;
using System;

public class PlayerMover : MonoBehaviour
{
    public BoardRoute currentRoute;
    public Transform currentNode;
    int currentNodeIndex;
    public int numberRolled;
    public bool isMoving, finishedTurn = false;
    PlayerLauncher playerLauncher;
    PlayerEffectsHandler playerEffectsHandler;
    Animator anim;
    TurnManager turnManager;
    public bool chanceEventSuccess;
    public bool eventComplete;


    private void Awake()
    {
        turnManager = GameObject.FindObjectOfType<TurnManager>();
        anim = GetComponent<Animator>();
        currentRoute = GameObject.FindObjectOfType<BoardRoute>();
        playerLauncher = GetComponent<PlayerLauncher>();
        playerEffectsHandler = GetComponent<PlayerEffectsHandler>();
        currentNode = currentRoute.childNodeList[currentNodeIndex];
    }

    private void Update()
    {
        currentNode = currentRoute.childNodeList[currentNodeIndex];
    }

    public void StartMove(int remainingMoves, int dir, float movementSpeed)
    {
        StartCoroutine(MovePlayer(remainingMoves,dir,movementSpeed));
    }

    IEnumerator MovePlayer(int remainingMoves, int dir, float movementSpeed)
    {
        //if (isMoving)
        //{
        //    yield break;
        //}
        //isMoving = true;
        //finishedTurn = false;
        anim.SetBool("isWalking", true);

        UpdateNodeIndex(1, dir);

        Vector3 nextPos = currentRoute.childNodeList[currentNodeIndex].position;
        transform.LookAt(nextPos);

        //yield until reached next node////////////////////////////////////////////////////////////////////
        while (HasNotArrived(nextPos, movementSpeed)) { yield return null; }

        yield return new WaitForSeconds(0.01f);
        remainingMoves--;

        BoardNode curBoardNode = currentNode.GetComponent<BoardNode>();
        if (curBoardNode.nodeType == BoardNode.NodeType.stop && curBoardNode.isCompleted == false) { yield return StartCoroutine(EventNodeRoutine(remainingMoves, dir, movementSpeed)); yield break; }

        if( remainingMoves > 0 ) { StartCoroutine(MovePlayer( remainingMoves, dir, movementSpeed )); }
        else
        { FinishMove(dir); }
            //BoardNode curBoardNode = currentNode.GetComponent<BoardNode>();
            ////stop node logic///////////////////////////////////////////////////////////////////////////////////////////////
            //if ( curBoardNode.nodeType == BoardNode.NodeType.stop && curBoardNode.isCompleted == false )
            //{
            //    anim.SetBool("isWalking", false);
            //    //anim.SetBool("isIdle", true);
            //    isMoving = false;
            //    curBoardNode.timelinePlaybackManager.currentPlayer = this;
            //    curBoardNode.timelinePlaybackManager.timelines[0].Play();

            //    while (!eventComplete) { yield return null; }
            //    eventComplete = false; /////////reset value for next time

            //    yield return new WaitForSeconds(1f);

            //    /////// event fail
            //    if (chanceEventSuccess == false) { StartCoroutine(MovePlayer(1, -1, 3f)); yield break; }

            //    ////// event success
            //    else { curBoardNode.roadBlock.gameObject.SetActive(false); curBoardNode.isCompleted = true;}
            //}

            ////star event logic//////////////////////////////////////////////////////////////////////////////////////
            //if (currentNode.GetComponent<BoardNode>().nodeType == BoardNode.NodeType.starEvent)
            //{
            //    yield return StartCoroutine(StarEvent());
            //    remainingMoves += 1;
            //}
        

        //anim.SetBool("isWalking", false);
        //isMoving = false;
        //StartCoroutine(BonusRollMove(dir));
    }

    IEnumerator EventNodeRoutine(int remainingMoves, int dir, float movementSpeed)
    {
        BoardNode curBoardNode = currentNode.GetComponent<BoardNode>();
        //stop node logic///////////////////////////////////////////////////////////////////////////////////////////////
        if (curBoardNode.nodeType == BoardNode.NodeType.stop && curBoardNode.isCompleted == false)
        {
            //anim.SetBool("isWalking", false);
            isMoving = false;
            curBoardNode.timelinePlaybackManager.currentPlayer = this;
            curBoardNode.timelinePlaybackManager.timelines[0].Play();

            while (!eventComplete) { yield return null; }
            eventComplete = false; /////////reset value for next time

            yield return new WaitForSeconds(1f);

            /////// event fail
            if (chanceEventSuccess == false) { StartCoroutine(MovePlayer(1, -1, 3f)); }

            ////// event success
            else { curBoardNode.roadBlock.gameObject.SetActive(false); curBoardNode.isCompleted = true; StartCoroutine(MovePlayer(remainingMoves, dir, movementSpeed)); }
        }
    }

    private void FinishMove(int dir)
    {
        anim.SetBool("isWalking", false);
        isMoving = false;
        StartCoroutine(BonusRollMove(dir));
    }

    IEnumerator BonusRollMove(int dir)
    {
        yield return new WaitForEndOfFrame();
        BoardNode.NodeType currentNodeType = currentNode.GetComponent<BoardNode>().nodeType;
        switch (currentNodeType)
        {
            case BoardNode.NodeType.normal:
                break;
            case BoardNode.NodeType.stop:
                break;
            case BoardNode.NodeType.plusTwo:
                playerLauncher.StartLaunchRoutine(currentRoute.childNodeList[currentNodeIndex + 2]);
                UpdateNodeIndex(2, dir);
                Debug.Log("+++++ 2 roll node");
                break;
            case BoardNode.NodeType.minusTwo:
                playerEffectsHandler.ActivateTornado(8f);
                yield return new WaitForSeconds(2f);
                StartCoroutine(MovePlayer(2, -1, 3f));
                Debug.Log("----- 2 roll node");
                break;
            default:
                break;
        }
        while (isMoving) { yield return null; }
        finishedTurn = true;
    }

    IEnumerator StarEvent()
    {
        var starTimeline = currentNode.GetComponent<BoardNode>().timelinePlaybackManager;
        starTimeline.PlayTimeline(0);
        anim.SetBool("isTalking", true);
        while (starTimeline.timelinePlaying) { yield return null; }
        anim.SetBool("isTalking", false);
    }

    bool HasNotArrived(Vector3 arrivalPos, float movementSpeed)
    {
        //move until we arrive, then return true
        return arrivalPos != (transform.position = Vector3.MoveTowards(transform.position, arrivalPos, movementSpeed * Time.deltaTime));
    }

    void UpdateNodeIndex(int moveNumber, int dir) //keep track of which node play is at
    {
        currentNodeIndex += moveNumber * dir;
        currentNodeIndex %= currentRoute.childNodeList.Count;
    }
}
