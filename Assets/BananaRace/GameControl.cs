using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    private static GameObject player1, player2;
    public static GameObject player1Watch, player2Watch;

    public static int diceSideThrown = 1;
    public static int BackDiceSideThrown = 1;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    public static bool gameOver = false;
    public static bool p1Finished = false;
    public static bool p2Finished = false;

    // Use this for initialization
    void Start () {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        player1Watch = GameObject.Find("Player1Watch");
        player2Watch = GameObject.Find("Player2Watch");

        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (player1.GetComponent<FollowThePath>().waypointIndex > 
            player1StartWaypoint + diceSideThrown)
        {
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if (player1.GetComponent<FollowThePath>().waypointIndex < //if the next space is lower than the dice number then stop
            player1StartWaypoint - BackDiceSideThrown
            || player1.GetComponent<FollowThePath>().waypointIndex < 0)//stop if the player is at the start
        {
            player1.GetComponent<FollowThePath>().BackMoveAllowed = false; // tell player 1 to stop moving
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex + 1; // save the location player 1 landed on
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex >
            player2StartWaypoint + diceSideThrown)
        {
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex < //if the next space is lower than the dice number then stop
         player2StartWaypoint - BackDiceSideThrown
         || player2.GetComponent<FollowThePath>().waypointIndex < 0)//stop if the player is at the start)
        {
            player2.GetComponent<FollowThePath>().BackMoveAllowed = false; // tell player 1 to stop moving
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex + 1; // save the location player 1 landed on
        }

        if (player1.GetComponent<FollowThePath>().waypointIndex == 
            player1.GetComponent<FollowThePath>().waypoints.Length && !p1Finished)
        {
            p1Finished = true;
            player1Watch.GetComponent<StopWatch>().Player1Finished();
            gameOver = true;
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex ==
            player2.GetComponent<FollowThePath>().waypoints.Length && !p2Finished)
        {
            p2Finished = true;
            player2Watch.GetComponent<StopWatch>().Player2Finished();
            gameOver = true;
        }
    }

    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove) {
            case 1:
                player1.GetComponent<FollowThePath>().moveAllowed = true;
                if (player1.GetComponent<FollowThePath>().waypointIndex < GameControl.player1StartWaypoint) //if player is looking back
                {
                    player1.GetComponent<FollowThePath>().waypointIndex += 2; //look forward
                }
                break;
            case 2:
                player2.GetComponent<FollowThePath>().moveAllowed = true;
                if (player2.GetComponent<FollowThePath>().waypointIndex < GameControl.player2StartWaypoint) //if player is looking back
                {
                    player2.GetComponent<FollowThePath>().waypointIndex += 2; //look forward
                }
                break;
        }
    }

    public static void BackMovePlayer(int playerToMove) // give player permission to move
    {
        switch (playerToMove)
        {
            case 1:
                player1.GetComponent<FollowThePath>().BackMoveAllowed = true;
                if (player1.GetComponent<FollowThePath>().waypointIndex > GameControl.player1StartWaypoint) //if player is looking ahead
                {
                    player1.GetComponent<FollowThePath>().waypointIndex -= 2; //look back
                }
                break;
            case 2:
                player2.GetComponent<FollowThePath>().BackMoveAllowed = true;
                if (player2.GetComponent<FollowThePath>().waypointIndex > GameControl.player2StartWaypoint) //if player is looking ahead
                {
                    player2.GetComponent<FollowThePath>().waypointIndex -= 2; //look back
                }
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
