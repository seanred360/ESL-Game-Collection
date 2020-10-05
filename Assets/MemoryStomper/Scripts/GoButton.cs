using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoButton : MonoBehaviour {

    Animation anim;
    AnimationState state;
 
    public void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        StartCoroutine(PerformAnimRoutine());
    }

    private IEnumerator PerformAnimRoutine()
    {
        //here we're going to use PlayQueued to get the animstate, with that we can determine some info about the anim
        state = anim.PlayQueued("CanvasVictory", QueueMode.PlayNow, PlayMode.StopSameLayer); //0 from Play is StopSameLayer... use the enums, they're more explicit

        yield return new WaitForSeconds(state.length);

        gameObject.SetActive(false);
    }
}
