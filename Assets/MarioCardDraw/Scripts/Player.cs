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
        public float to;
        float jumpPower = 1f;
        int numJumps = 1;
        public float duration = .3f;
        public GameObject collisionParticle;
        public Transform headPos;
        public int playerNumber;
        bool canHitBlock = true;
        public GameObject scoreUI;
        BoxCollider2D boxCollider2D;
        float aspectModifier;

        // Start is called before the first frame update
        void Start()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
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
            if (collision.gameObject.tag == "ButtonTag1" && canHitBlock)
            {
                audioManager.PlaySFX(2);
                gameController.ChooseACard();
                Instantiate(collisionParticle, headPos.position, Quaternion.identity);
                //anim.Play("HitHead");
                canHitBlock = false;
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
            //if (collision.gameObject.tag == "ButtonTag1" && canHitBlock)
            //{
            //    audioManager.PlaySFX(2);
            //    gameController.ChooseACard();
            //    Instantiate(collisionParticle, headPos.position, Quaternion.identity);
            //    anim.Play("HitHead");
            //    canHitBlock = false;
            //}
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
            Vector3 colliderCenter = Vector3.Scale(transform.localScale, boxCollider2D.offset);
            Vector3 colliderPosition = transform.localPosition + colliderCenter;
            Vector3 colliderSize = Vector3.Scale(transform.localScale, boxCollider2D.size);
            float colliderBottom = colliderPosition.y - (colliderSize.y / 2);
            float colliderTop = colliderPosition.y + (colliderSize.y / 2);
            float colliderLeft = colliderPosition.x - (colliderSize.x / 2);
            float colliderRight = colliderPosition.x + (colliderSize.x / 2);
            if (Camera.main.aspect == 4f / 3f) { aspectModifier = .01666667f; }
            if (Camera.main.aspect == 16f / 9f) { aspectModifier = .02222222f; }
            to = (Card.bottomPos) * aspectModifier + 1;
            GetComponent<Rigidbody2D>().DOMoveY(to, duration, false);
            //GetComponent<Rigidbody2D>().DOMoveY(to + GetComponent<BoxCollider2D>().offset.y - GetComponent<BoxCollider2D>().size.y, duration, false);
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
