using UnityEngine;
using System.Collections;
using RootMotion.Dynamics;

namespace BoardGame
{
    public class FollowThePath : MonoBehaviour {

        public Transform[] waypoints;
        Animator playerAnim;
        AudioManager audioManager;
        public bool canPlaySFX;
        public GameObject dice, gameManager,player;

        public PuppetMaster puppetMaster;
        public PuppetMaster.StateSettings stateSettings = PuppetMaster.StateSettings.Default;

        public GameObject alertParticle, boomParticle, smileyParticle, fireWorks;

        [SerializeField]
        private float moveSpeed = 1f;
        //[HideInInspector]
        public int waypointIndex = 0;

        public bool moveAllowed = false;
        public bool BackMoveAllowed = false;
        public bool questionAllowed = false;

        private void Awake()
        {
            alertParticle.SetActive(false);
            fireWorks.SetActive(false);
            smileyParticle.SetActive(false);
            boomParticle.SetActive(false);
        }
        // Use this for initialization
        private void Start() {
            canPlaySFX = true;
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            playerAnim = player.GetComponent<Animator>();
            transform.position = waypoints[waypointIndex].transform.position; //put player at the start point
        }

        // Update is called once per frame
        private void Update()
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
            if (moveAllowed)
            {
                player.transform.position = transform.position;
                player.transform.rotation = transform.rotation;
                Move();
            }

            if (BackMoveAllowed)
            {
                player.transform.position = transform.position;
                player.transform.rotation = transform.rotation;
                BackMove();
            }

            if (!moveAllowed && !BackMoveAllowed)
            {
                playerAnim.SetFloat("Forward", 0);
            }

            if (!moveAllowed && !BackMoveAllowed && questionAllowed)
            {
                if (gameManager.GetComponent<GameControl>().ohNoDialogueBox)
                {
                   gameManager.GetComponent<GameControl>().ohNoDialogueBox.SetActive(false);
                }
                gameManager.GetComponent<GameControl>().ohYesDialogueBox.SetActive(true);
                gameManager.GetComponent<QuestionsList>().SetCurrentQuestion();
                audioManager.PlaySFX(10);
                questionAllowed = false;
            }


            // this controls when to play sounds when landing on spaces
            if (transform.position == waypoints[4].transform.position && (canPlaySFX == true) && (moveAllowed == false))
            {
                LandOnBombSpace();
            }
            if (transform.position == waypoints[9].transform.position && (canPlaySFX == true) && (moveAllowed == false))
            {
                LandOnBombSpace();
            }
            if (transform.position == waypoints[14].transform.position && (canPlaySFX == true) && (moveAllowed == false))
            {
                LandOnBombSpace();
            }
            if (transform.position == waypoints[19].transform.position && (canPlaySFX == true) && (moveAllowed == false))
            {
                LandOnBombSpace();
            }
            if (transform.position == waypoints[24].transform.position && (canPlaySFX == true) && (moveAllowed == false))
            {
                LandOnBombSpace();
            }
            if (transform.position == waypoints[29].transform.position && (canPlaySFX == true) && (moveAllowed == false))
            {
                LandOnBombSpace();
            }
            if (transform.position == waypoints[32].transform.position && (canPlaySFX == true) && (moveAllowed == false))
            {
                audioManager.PlaySFX(2);
                fireWorks.SetActive(true);
                canPlaySFX = false;
            }
        }

        public void Move()
        {

            if (waypointIndex <= waypoints.Length - 1)//checks if game is over
            {
                playerAnim.SetFloat("Forward", 1);
                transform.position = Vector3.MoveTowards(transform.position,
                waypoints[waypointIndex].transform.position,
                moveSpeed * Time.deltaTime);

                transform.LookAt(waypoints[waypointIndex].transform.position);
                Vector3 targetPostition = new Vector3(waypoints[waypointIndex].transform.position.x,
                this.transform.position.y, waypoints[waypointIndex].transform.position.z);
                this.transform.LookAt(targetPostition);

                if (transform.position == waypoints[waypointIndex].transform.position)// if the player has moved one space forward
                {
                    audioManager.PlaySFX(3);
                    waypointIndex += 1; // target the next space
                }
            }
            canPlaySFX = true; // stops infinite sound loops on unique spaces with actions
        }

        public void BackMove()
        {
            if (waypointIndex >= 0) //avoid array out of index exception
            {
                if (waypointIndex <= waypoints.Length - 1) // check if gameover
                {
                    playerAnim.SetFloat("Forward", 1);
                    transform.position = Vector3.MoveTowards(transform.position,
                    waypoints[waypointIndex].transform.position,
                    moveSpeed * Time.deltaTime);

                    transform.LookAt(waypoints[waypointIndex].transform.position);
                    Vector3 targetPostition = new Vector3(waypoints[waypointIndex].transform.position.x,
                    this.transform.position.y, waypoints[waypointIndex].transform.position.z);
                    this.transform.LookAt(targetPostition);

                    if (transform.position == waypoints[waypointIndex].transform.position)
                    {
                        audioManager.PlaySFX(3);
                        waypointIndex -= 1;
                    }
                }
                canPlaySFX = true;
            }
        }

        void EnableBombDialogue()
        {
            gameManager.GetComponent<GameControl>().ohNoDialogueBox.SetActive(true);
            gameManager.GetComponent<GameControl>().ohYesDialogueBox.SetActive(false);
            gameManager.GetComponent<GameControl>().CloseDialogueBox();
        }

        void ResurrectPuppet()
        {
            puppetMaster.Resurrect();
        }

        void LandOnBombSpace()
        {
            audioManager.PlaySFX(9);
            audioManager.PlaySFX(4);
            audioManager.PlaySFX(0);
            EnableBombDialogue();
            gameManager.GetComponent<GameControl>().ohNoDialogueBox.SetActive(true);
            puppetMaster.Kill(stateSettings);
            Invoke("ResurrectPuppet", 2);
            Dice.whosTurn *= -1;
            dice.GetComponent<Dice>().BackStartRoll();
            boomParticle.SetActive(true);
            canPlaySFX = false;
        }
    }
}
