using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TSGameControl : MonoBehaviour
{
    public GameObject trevor;
    public GameObject hitParticle;
    public Camera cam;
    public Animator anim;
    public AudioManager audioManager;
    public float duration, strength, randomness;
    public int vibrato;
    Vector2 screenPos;
    Vector2 worldPos;
    GameObject clone;

    public void HitTrevor()
    {
        StartCoroutine(ShakeScale());
        StartCoroutine(ShakeTitle());
        anim.Play("HitTrevor");

        if (audioManager)
        {
            audioManager.PlaySFX(Random.Range(0, 6));
            audioManager.PlayMusic(Random.Range(0, 30));
        }
        
        screenPos = Input.mousePosition;
        worldPos = cam.ScreenToWorldPoint(screenPos);
        clone = Instantiate(hitParticle, worldPos, Quaternion.identity);
    }

    IEnumerator ShakeTitle()
    {
        Debug.Log("shake it");
        yield return new WaitForSeconds(1f);
        trevor.transform.DOShakeRotation(duration, new Vector3(0, 0, strength), vibrato, randomness, false).SetLoops(0);
    }

    IEnumerator ShakeScale()
    {
        trevor.transform.DOShakeScale(1f, .05f, 10, 90, true).SetLoops(0);
        yield return null;
    }
}
