using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColliderScript : MonoBehaviour
{

    private SphereCollider sphereCollider;
    public GameObject mario;
    private Animator marioAnimator;
    public GameObject bowser;
    private Animator bowserAnimator;
    private AudioManager audiomanager;


    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        audiomanager = GetComponent<AudioManager>();
        if (mario)//sean stops a bug if i dont use mario in the scene
        {
            marioAnimator = mario.GetComponent<Animator>();//sean
        }
        if (bowser)//sean stops a bug if i dont use mario in the scene
        {
            bowserAnimator = bowser.GetComponent<Animator>();//sean
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audiomanager.PlaySFX(Random.Range(0, audiomanager.SClips.Length));
            if (healthBarScript.health > 0)//sean
            {
                StartCoroutine(GetHit());
            }
            else
                marioAnimator.Play("Unarmed-Death1");
        }
        if (other.gameObject.tag == "Enemy")
        {
            StartCoroutine(GetHit2());
        }

    }

    IEnumerator GetHit()//sean
    {
        yield return new WaitForEndOfFrame();
        int hits = 5;
        int hitNumber = Random.Range(1, hits + 1);
        marioAnimator.SetInteger("Action", hitNumber);
        marioAnimator.SetTrigger("GetHitTrigger");

    }
    IEnumerator GetHit2()//sean
    {
        yield return new WaitForEndOfFrame();
        int hits = 5;
        int hitNumber = Random.Range(1, hits + 1);
        bowserAnimator.SetInteger("Action", hitNumber);
        bowserAnimator.SetTrigger("GetHitTrigger");
    }
}
