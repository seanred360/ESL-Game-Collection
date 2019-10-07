using UnityEngine;
using DG.Tweening;

namespace MarioCardDraw
{
    public class Player : MonoBehaviour
    {
        GameController gameController;
        public float timeToWalk = 2f;
        Animator anim;
        AudioManager audioManager;
        Vector2 startPos;
        public float to = -2f;
        float jumpPower = 1f;
        int numJumps = 1;
        public float duration = .3f;
        public GameObject collisionParticle;
        public Transform headPos;
        public int playerNumber;
        bool canHitBlock = true;
        public GameObject scoreUI;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            audioManager = GetComponent<AudioManager>();
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            scoreUI.SetActive(true);
        }

        private void Update()
        {
            if (CanPlaySFX())
                if (anim.GetBool("isWalking") == true)
                {
                    audioManager.PlaySFX(0);
                }
        }

        bool CanPlaySFX()
        {
            if (audioManager.SFX.GetComponent<AudioSource>().isPlaying)
            {
                return false;
            }
            else return true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                StopJumping();
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                Data.Singleton.isJumping = true;
                anim.SetBool("isJumping", true);
                Data.Singleton.canClick = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "ButtonTag1" && canHitBlock)
            {
                audioManager.PlaySFX(2);
                gameController.ChooseACard();
                Instantiate(collisionParticle, headPos.position, Quaternion.identity);
                //anim.Play("HitHead");
                canHitBlock = false;
            }
            if (collision.gameObject.tag == "Mid")
            {
                Data.Singleton.isAtMid = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Mid")
            {
                Data.Singleton.isAtMid = false;
            }
        }

        public void MoveToMid(Vector2 destination)
        {
            if (Data.Singleton.isAtMid == false)
            {
                startPos = transform.position;// remembers where we began so we can move back later
                ButtonController.DisableButton();
                ButtonController.HideStopButton();
                anim.SetBool("isWalking", true);
                GetComponent<Rigidbody2D>().DOMove(destination, timeToWalk).OnComplete(StopWalking);
            }
            else Jump();
        }

        public void MoveBack()
        {
            if (Data.Singleton.isAtMid == true && Data.Singleton.isGameOver == false)
            {
                ButtonController.DisableButton();
                ButtonController.HideStopButton();
                Data.Singleton.isAtMid = false;
                anim.SetBool("isWalking", true);
                GetComponent<Rigidbody2D>().DOMove(startPos, timeToWalk).OnComplete(StopWalkingBack);
            }
        }

        public void Jump()
        {
            ButtonController.DisableButton();
            ButtonController.HideStopButton();
            Data.Singleton.isJumping = true;
            audioManager.PlaySFX(1);
            anim.SetBool("isJumping", true);
            GetComponent<Rigidbody2D>().DOMoveY(to, duration, false);
        }

        void StopWalking()
        {
            Data.Singleton.isWalking = false;
            //transform.position = gameController.midPoint.position;
            anim.SetBool("isWalking", false);
            audioManager.SFX.GetComponent<AudioSource>().Stop();
            Jump();
        }

        void StopWalkingBack()
        {
            Data.Singleton.isWalking = false;
            anim.SetBool("isWalking", false);
            audioManager.SFX.GetComponent<AudioSource>().Stop();
            ButtonController.EnableButton();
            ButtonController.ShowStopButton();
        }

        void StopJumping()
        {
            Data.Singleton.isJumping = false;
            canHitBlock = true;
            anim.SetBool("isJumping", false);
            if (Data.Singleton.isGameOver == false)
            {
                ButtonController.EnableButton();
                ButtonController.ShowStopButton();
            }
        }
    }
}
