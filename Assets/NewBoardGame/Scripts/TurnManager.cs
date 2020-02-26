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

            while (rollButton.interactable == true)
            {
                yield return null;
            }

            dice.StartCoroutine(dice.StopRollDice(2f, player));
            yield return new WaitForSeconds(2f);

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
            //stateDrivenCamera.GetComponentInChildren<CinemachineVirtualCamera>().m_Follow = player.transform;
        }
    }
}

