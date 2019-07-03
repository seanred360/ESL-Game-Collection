using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BoardGame
{
    public class Dice : MonoBehaviour
    {

        public Sprite[] diceSides;
        private Image rend;
        public static int whosTurn = 1;
        private bool coroutineAllowed = true;
        public GameObject playerOne;
        public GameObject playerTwo;
        public GameObject yourMoveParticle;

        // Use this for initialization
        private void Start()
        {
            rend = GetComponent<Image>();
            rend.sprite = diceSides[1];
        }

        public void StartRoll()
        {
            if (!GameControl.gameOver && coroutineAllowed)
                StartCoroutine("RollTheDice");
        }

        public void BackStartRoll()
        {
            if (!GameControl.gameOver && coroutineAllowed)
                StartCoroutine("BackRollTheDice");
        }

        private IEnumerator RollTheDice()
        {
            this.gameObject.GetComponent<Button>().interactable = false;
            coroutineAllowed = false;
            int randomDiceSide = 0;
            for (int i = 0; i <= 20; i++)
            {
                GetComponent<AudioSource>().Play();
                //randomDiceSide = 3;
                randomDiceSide = Random.Range(0, diceSides.Length);
                rend.sprite = diceSides[randomDiceSide];
                yield return new WaitForSeconds(0.05f);
            }

            GetComponent<AudioSource>().Stop();

            GameControl.diceSideThrown = randomDiceSide + 1;
            if (whosTurn == 1)
            {
                GameControl.MovePlayer(1);// tells player it can move
                GameObject.Find("Main Camera").GetComponent<CameraPos>().movingPlayer = playerOne;
            }
            else if (whosTurn == -1)
            {
                GameControl.MovePlayer(2);// tells player it can move
                GameObject.Find("Main Camera").GetComponent<CameraPos>().movingPlayer = playerTwo;
            }
            whosTurn *= -1;//changes whos turn it is
            coroutineAllowed = true;// rolling dice is allowed again 
        }

        private IEnumerator BackRollTheDice()
        {
            this.gameObject.GetComponent<Button>().interactable = false;
            yield return new WaitForSeconds(3f);
            coroutineAllowed = false;
            int BackRandomDiceSide = 0;
            for (int i = 0; i <= 20; i++)
            {
                //GetComponent<AudioSource>().Play();
                BackRandomDiceSide = Random.Range(0, diceSides.Length);
                //rend.sprite = diceSides[BackRandomDiceSide];
                yield return new WaitForSeconds(0.05f);
            }

            GetComponent<AudioSource>().Stop();

            GameControl.BackDiceSideThrown = BackRandomDiceSide + 1;
            if (whosTurn == 1)
            {
                GameControl.BackMovePlayer(1);// tells player it can move
                GameObject.Find("Main Camera").GetComponent<CameraPos>().movingPlayer = playerOne;
            }
            else if (whosTurn == -1)
            {
                GameControl.BackMovePlayer(2);// tells player it can move
                GameObject.Find("Main Camera").GetComponent<CameraPos>().movingPlayer = playerTwo;
            }
            whosTurn *= -1;//changes whos turn it is
            coroutineAllowed = true;// rolling dice is allowed again
        }

        public void EnableDicePress()
        {
            this.gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
