using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimelinePlaybackManager : MonoBehaviour
{
    [Header("Timeline References")]
    public PlayableDirector[] timelines;

    public bool timelinePlaying = false;
    private float timelineDuration;

    public Dice3d dice;
    public Button rollButton;

    [HideInInspector]
    public PlayerMover currentPlayer;

    public void PlayTimeline(int i)
    {
        if (timelines.Length > 0)
        {
            timelines[i].Play();
        }

        timelinePlaying = true;

        StartCoroutine(WaitForTimelineToFinish(i));
    }

    IEnumerator WaitForTimelineToFinish(int i)
    {
        timelineDuration = (float)timelines[i].duration;

        yield return new WaitForSeconds(timelineDuration);
      
        timelinePlaying = false;
    }

    public void StartEventDiceRoll(int i)
    {
        StartCoroutine(EventDiceRoll(i));
    }

    public IEnumerator EventDiceRoll(int i)
    {
        timelines[i].Pause();
        yield return new WaitForSeconds(1f); // wait for dice animation, prevents a scaling bug

        while (rollButton.interactable == true) ///////// wait for button press
        {
            yield return null;
        }

        int numRolled = dice.StopRollDice();

        yield return new WaitForSeconds(2f);

        if (numRolled == 4 || numRolled == 5 || numRolled == 6)
        {
            timelines[1].Play();
            while (timelines[1].state == PlayState.Playing) { yield return null; }
            currentPlayer.chanceEventSuccess = true;
            print("wall event success");
        }
        else
        {
            timelines[2].Play();
            while (timelines[2].state == PlayState.Playing) { yield return null; }
            currentPlayer.chanceEventSuccess = false;
            print("wall event failed");
        }
        rollButton.interactable = true;
        currentPlayer.eventComplete = true;
        timelines[i].Play();
    }

    public void ClickButton()
    {
        rollButton.interactable = false;
    }
}
