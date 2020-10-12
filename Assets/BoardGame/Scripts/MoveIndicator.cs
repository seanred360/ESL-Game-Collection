using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class MoveIndicator : MonoBehaviour
    {

        public GameObject player1Indicator;
        public GameObject player2Indicator;
        public GameObject dice;
        public GameObject player1;
        public GameObject player2;

        // Use this for initialization
        void Start()
        {
            player1Indicator.SetActive(false);
            player2Indicator.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Dice.whosTurn == 1 && !player1.GetComponent<FollowThePath>().moveAllowed)
            {
                player1Indicator.SetActive(true);
                player2Indicator.SetActive(false);
            }
            if (Dice.whosTurn == -1 && !player2.GetComponent<FollowThePath>().moveAllowed)
            {
                player2Indicator.SetActive(true);
                player1Indicator.SetActive(false);
            }
        }
    }
}
