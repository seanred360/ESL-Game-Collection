using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Data
{
    public static Data instance;
    public int turn = 1;
    public static int P1Score, P2Score, P3Score, P4Score;
    public bool isJumping;
    public bool isAtMid;
    public bool isWalking;
    public bool isGameOver;
    public bool canClick = true;
    public static int numberOfPlayers;

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
