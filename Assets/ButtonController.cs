﻿using UnityEngine;
using UnityEngine.UI;

public static class ButtonController
{
    public static GameObject[] objs;

    static void awake()
    {
        objs = GameObject.FindGameObjectsWithTag("ButtonTag1");
    }

    public static void EnableButton()
    {
        objs = GameObject.FindGameObjectsWithTag("ButtonTag1");
        foreach (GameObject ButtonTag1 in objs)
        {
            ButtonTag1.GetComponent<Button>().interactable = true;
        }
    }

    public static void DisableButton()
    {
        objs = GameObject.FindGameObjectsWithTag("ButtonTag1");
        foreach (GameObject ButtonTag1 in objs)
        {
            ButtonTag1.GetComponent<Button>().interactable = false;
        }
    }
}