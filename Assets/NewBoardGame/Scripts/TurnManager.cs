using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

namespace NBG
{
    public class TurnManager : MonoBehaviour
    {
        public Dice3d dice;
        public GameObject RollPhaseUI;
        public PlayerMover[] players;
        public Button rollButton;
        public ShowModelController showModelController;
        int currentPlayerIndex;
        public CinemachineStateDrivenCamera stateDrivenCamera;
        public CinemachineVirtualCamera[] vCams;

        private void Start()
        {
            vCams = stateDrivenCamera.GetComponentsInChildren<CinemachineVirtualCamera>();
            if (players.Length <= 0) { players = GameObject.FindObjectsOfType<PlayerMover>(); }
            ChangeCameraTarget(players[0]);
            StartCoroutine(StartDicePhase(players[0]));
        }

        public void ClickButton()
        {
            rollButton.interactable = false;
        }

        IEnumerator StartDicePhase(PlayerMover player)
        {
            ChangeCameraTarget(player);
            RollPhaseUI.SetActive(true);
            showModelController.EnableModel(players[currentPlayerIndex].name);
            yield return new WaitForSeconds(1f); // wait for dice animation, prevents a scaling bug

            while (rollButton.interactable == true)/////////// wait for button press
            {
                yield return null;
            }

            int numRolled = dice.StopRollDice(); ////// stop the dice movement

            yield return new WaitForSeconds(2f); ////// wait then hide UI and move player
            player.StartMove(numRolled, 1, 8f);
            rollButton.interactable = true;
            RollPhaseUI.SetActive(false);
            while (players[currentPlayerIndex].finishedTurn == false) { yield return null; }

            yield return new WaitForSeconds(3f);
            FinishTurn();
        }

        private void FinishTurn()
        {
            currentPlayerIndex++;
            currentPlayerIndex %= players.Length;
            StartCoroutine(StartDicePhase(players[currentPlayerIndex]));
        }

        void ChangeCameraTarget(PlayerMover player)
        {
            foreach(CinemachineVirtualCamera cam in vCams)
            {
                cam.m_Follow = player.transform;
            }
            
            stateDrivenCamera.m_AnimatedTarget = player.GetComponent<Animator>();
        }

        public void TogglePlayerEventCompleteBool()
        {
           if (players[currentPlayerIndex].eventComplete == false) { players[currentPlayerIndex].eventComplete = true; }
           else { players[currentPlayerIndex].eventComplete = false; }
        }

        //public IEnumerator EventDiceRoll(PlayerMover player)
        //{
        //    RollPhaseUI.SetActive(true);
        //    showModelController.EnableModel(players[currentPlayerIndex].name);
        //    yield return new WaitForSeconds(1f); // wait for dice animation, prevents a scaling bug

        //    while (rollButton.interactable == true)///////// wait for button press
        //    {
        //        yield return null;
        //    }

        //    int numRolled = dice.StopRollDice();

        //    if (numRolled == 4 || numRolled == 5 || numRolled == 6)
        //    {
        //        player.chanceEventSuccess = true;
        //        print("get out of jail");
        //    }
        //    else { player.chanceEventSuccess = false; print("stuck in jail");  }

        //    while (rollButton.interactable == true)
        //    {
        //        yield return null;
        //    }
        //}
    }
}

