using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Data
{
    public static Data instance;
    public int turn = 1;
    public int P1Score;
    public int P2Score;
    public bool isJumping;
    public bool isAtMid;
    public bool isWalking;
    public bool isGameOver;

    public static Data Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new Data();
            }
            return instance;
        }
    }
}
