using UnityEngine;
using UnityEngine.UI;

namespace BoardGame
{
    public class GameControl : MonoBehaviour
    {

        private static GameObject whoWinsTextShadow, player1MoveText, player2MoveText, player1, player2;
        public GameObject ohNoDialogueBox, optionsCanvas, ohYesDialogueBox, audioManager;

        public static int BackDiceSideThrown, diceSideThrown, player1StartWaypoint, player2StartWaypoint = 0;

        public static bool gameOver = false;

        // Use this for initialization
        void Start()
        {
            audioManager.GetComponent<AudioManager>().PlaySFX(6);
            whoWinsTextShadow = GameObject.Find("WhoWinsText");
            player1MoveText = GameObject.Find("Player1MoveText");
            player2MoveText = GameObject.Find("Player2MoveText");

            player1 = GameObject.Find("Player1");
            player2 = GameObject.Find("Player2");

            player1.GetComponent<FollowThePath>().moveAllowed = false;
            player2.GetComponent<FollowThePath>().moveAllowed = false;

            player1MoveText.gameObject.SetActive(true);
            player2MoveText.gameObject.SetActive(false);
            ohNoDialogueBox.gameObject.SetActive(false);
            ohYesDialogueBox.gameObject.SetActive(false);
            optionsCanvas.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (player1.GetComponent<FollowThePath>().waypointIndex > //if the next space is higher than the dice number then stop
                player1StartWaypoint + diceSideThrown)
            {
                player1.GetComponent<FollowThePath>().moveAllowed = false; // tell player 1 to stop moving
                player1MoveText.gameObject.SetActive(false);
                player2MoveText.gameObject.SetActive(true);
                player1.GetComponent<FollowThePath>().questionAllowed = true;
                player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1; // save the location player 1 landed on
            }

            if (player1.GetComponent<FollowThePath>().waypointIndex < //if the next space is lower than the dice number then stop
              player1StartWaypoint - BackDiceSideThrown
              || player1.GetComponent<FollowThePath>().waypointIndex < 0)//stop if the player is at the start
            {
                player1.GetComponent<FollowThePath>().BackMoveAllowed = false; // tell player 1 to stop moving
                player1MoveText.gameObject.SetActive(false);
                player2MoveText.gameObject.SetActive(true);
                player1.GetComponent<FollowThePath>().questionAllowed = true;
                player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex + 1; // save the location player 1 landed on
            }

            if (player2.GetComponent<FollowThePath>().waypointIndex >
                player2StartWaypoint + diceSideThrown) // check if the player finished moving forward
            {
                player2.GetComponent<FollowThePath>().moveAllowed = false; // tell player 1 to stop moving
                player2MoveText.gameObject.SetActive(false);
                player1MoveText.gameObject.SetActive(true);
                player2.GetComponent<FollowThePath>().questionAllowed = true;
                player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1; // save the location player 1 landed on
            }

            if (player2.GetComponent<FollowThePath>().waypointIndex < //if the next space is lower than the dice number then stop
             player2StartWaypoint - BackDiceSideThrown
             || player2.GetComponent<FollowThePath>().waypointIndex < 0)//stop if the player is at the start)
            {
                player2.GetComponent<FollowThePath>().BackMoveAllowed = false; // tell player 1 to stop moving
                player2MoveText.gameObject.SetActive(false);
                player1MoveText.gameObject.SetActive(true);
                player2.GetComponent<FollowThePath>().questionAllowed = true;
                player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex + 1; // save the location player 1 landed on
            }

            if (player1.GetComponent<FollowThePath>().waypointIndex == //check is player 1 wins
                player1.GetComponent<FollowThePath>().waypoints.Length && gameOver == false)
            {
                whoWinsTextShadow.gameObject.SetActive(true);
                whoWinsTextShadow.GetComponent<Text>().text = "Player 1 Wins"; // write on the screen player 1 wins
                gameOver = true;
            }

            if (player2.GetComponent<FollowThePath>().waypointIndex == //check is player 2 wins
                player2.GetComponent<FollowThePath>().waypoints.Length && gameOver == false)
            {
                whoWinsTextShadow.gameObject.SetActive(true);
                player1MoveText.gameObject.SetActive(false);
                player2MoveText.gameObject.SetActive(false);
                whoWinsTextShadow.GetComponent<Text>().text = "Player 2 Wins"; // write on the screen player 2 wins
                gameOver = true;
            }
        }

        public static void MovePlayer(int playerToMove) // give player permission to move
        {

            switch (playerToMove)
            {
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

        public void CloseDialogueBox()
        {
            ohNoDialogueBox.SetActive(false);
            ohYesDialogueBox.SetActive(false);
            ohYesDialogueBox.GetComponent<Button>().interactable = false;
            ohNoDialogueBox.GetComponent<Button>().interactable = false;
            Invoke("EnableDialogueButtonClose", 5);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void EnableDialogueButtonClose()
        {
            ohYesDialogueBox.GetComponent<Button>().interactable = true;
            ohNoDialogueBox.GetComponent<Button>().interactable = true;
            Debug.Log("can close the button");
        }
    }
}
