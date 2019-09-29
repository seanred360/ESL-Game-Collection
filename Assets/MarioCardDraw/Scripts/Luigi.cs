﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Luigi : MonoBehaviour
{
    GameController gameController;
    public float timeToWalk = 2f;
    Animator anim;
    Vector2 startPos;
    public float to = -2f;
    float jumpPower = 1f;
    int numJumps = 1;
    public float duration = .3f;
    public GameObject collisionParticle;
    public Transform headPos;
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        anim = GetComponent<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        audioManager = GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (CanPlaySFX())
            if (GetComponent<Animator>().GetBool("isWalking") == true)
            {
                audioManager.PlaySFX(0);
            }
    }

    bool CanPlaySFX()
    {
        if (audioManager.SFX.GetComponent<AudioSource>().isPlaying)
        {
            return false;
        }
        else return true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            StopJumping();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Data.Singleton.isJumping = true;
            anim.SetBool("isJumping", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DisableThis")
        {
            audioManager.PlaySFX(2);
            gameController.ChooseACard();
            Instantiate(collisionParticle, headPos.position, Quaternion.identity);
            GetComponent<Animator>().Play("HitHead");
        }
        if (collision.gameObject.tag == "Mid")
        {
            Data.Singleton.isAtMid = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Mid")
        {
            Data.Singleton.isAtMid = false;
        }
    }

    public void MoveToMid(Vector2 destination)
    {
        if (Data.Singleton.isAtMid == false)
        {
            MarioCardDraw.ButtonController.DisableButton();
            MarioCardDraw.ButtonController.HideStopButton();
            GetComponent<Animator>().SetBool("isWalking", true);
            GetComponent<Rigidbody2D>().DOMove(destination, timeToWalk).OnComplete(StopWalking);
        }
        else Jump();
    }

    public void MoveBack()
    {
        if (Data.Singleton.isAtMid == true && Data.Singleton.isGameOver == false)
        {
            MarioCardDraw.ButtonController.DisableButton();
            MarioCardDraw.ButtonController.HideStopButton();
            Data.Singleton.isAtMid = false;
            GetComponent<Animator>().SetBool("isWalking", true);
            GetComponent<Rigidbody2D>().DOMove(startPos, timeToWalk).OnComplete(StopWalkingBack);
        }
    }

    public void Jump()
    {
        MarioCardDraw.ButtonController.DisableButton();
        MarioCardDraw.ButtonController.HideStopButton();
        Data.Singleton.isJumping = true;
        audioManager.PlaySFX(1);
        GetComponent<Animator>().SetBool("isJumping", true);
        GetComponent<Rigidbody2D>().DOMoveY(to, duration, false);
    }

    void StopWalking()
    {
        Data.Singleton.isWalking = false;
        transform.position = gameController.midPoint.position;
        GetComponent<Animator>().SetBool("isWalking", false);
        audioManager.SFX.GetComponent<AudioSource>().Stop();
        Jump();
    }

    void StopWalkingBack()
    {
        Data.Singleton.isWalking = false;
        GetComponent<Animator>().SetBool("isWalking", false);
        audioManager.SFX.GetComponent<AudioSource>().Stop();
        MarioCardDraw.ButtonController.EnableButton();
        MarioCardDraw.ButtonController.ShowStopButton();
    }

    void StopJumping()
    {
        Data.Singleton.isJumping = false;
        GetComponent<Animator>().SetBool("isJumping", false);
        if(Data.Singleton.isGameOver == false)
        {
            MarioCardDraw.ButtonController.EnableButton();
            MarioCardDraw.ButtonController.ShowStopButton();
        }
    }
}
