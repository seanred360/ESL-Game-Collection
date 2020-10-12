using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame{
    public class CameraPos : MonoBehaviour {

        public GameObject movingPlayer;
        public Transform cameraPositionP1;
        public Transform cameraPositionP2;
        public float speed = 1;

        // Update is called once per frame
        void Update() {
            if (movingPlayer)
                // make the camara look directly at the player
                transform.forward = movingPlayer.transform.position - transform.position;
            //transform.position = cameraPosition.position; 
            if ((cameraPositionP1.gameObject.activeSelf == true) || (cameraPositionP2.gameObject.activeSelf == true))
            {
                if (Dice.whosTurn == 1)
                {
                    // Move our position a step closer to the target.
                    float step = speed * Time.deltaTime; // calculate distance to move
                    transform.position = Vector3.MoveTowards(transform.position, cameraPositionP1.position, step);
                }

                if (Dice.whosTurn == -1)
                {
                    // Move our position a step closer to the target.
                    float step = speed * Time.deltaTime; // calculate distance to move
                    transform.position = Vector3.MoveTowards(transform.position, cameraPositionP2.position, step);
                }
            }
        }
    }
}
