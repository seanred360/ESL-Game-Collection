using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Dice3d : MonoBehaviour
{
    public SpriteRenderer floatingNumber;
    public Sprite[] numbers;
    public Transform[] sides;
    public Transform currentSide, dice;
    public GameObject smokeParticle;
    int numberRolled = 5;
    AudioSource audio;
    Animator anim;
    public Vector3[] rotations;

    private void Awake()
    {
        currentSide = sides[4];
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        anim.enabled = true;
        smokeParticle.SetActive(true);
        audio.Play();
    }

    public void RollDice(int num) //animator calls this
    {
        floatingNumber.sprite = numbers[num -1];
        currentSide = sides[num -1];
        numberRolled = num;
    }

    public IEnumerator StopRollDice(float waitTime,PlayerMover player)
    {
        audio.Stop();
        anim.enabled = false;
        dice.DORotate(rotations[numberRolled - 1], 0.5f);
        yield return new WaitForSeconds(waitTime);
        player.StartMove(numberRolled, 1, 8f);
    }

    //            // 6 = (270,0,180)
    //            // 5 = (0,0,0)
    //            // 4 = (0,270,0)
    //            // 3 = (0,-270,0)
    //            // 2 = (180,0,180)
    //            // 1 = (90,0,180)
}
