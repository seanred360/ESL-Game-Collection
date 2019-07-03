using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{

    public GameObject lv1;
    public GameObject lv2;
    public GameObject lv3;
    public GameObject lv4;
    public GameObject lv5;
    public GameObject lv6;
    public GameObject lv7;
    public GameObject lv8;
    public GameObject lv9;
    public GameObject lv10;
    public GameObject lv11;
    public GameObject lv12;
    public GameObject lv13;
    public GameObject lv14;
    public GameObject lv15;
    public GameObject lv16;
    public GameObject lv17;
    public GameObject lv18;
    public GameObject lv19;
    public GameObject lv20;
    public GameObject lv21;
    public GameObject lv22;
    public GameObject lv23;
    public GameObject lv24;
    public GameObject lv25;
    public GameObject lv26;
    public GameObject lv27;
    public GameObject lv28;
    public GameObject lv29;
    public GameObject lv30;
    public GameObject lv31;
    public GameObject lv32;
    public GameObject currentLevel;
    public int levelnum;

    private void Awake()
    {
        levelnum = LevelNumber.numberOfLevel;
        switch (levelnum)
        {
            case 1:
                currentLevel = lv1;
                break;
            case 2:
                currentLevel = lv2;
                break;
            case 3:
                currentLevel = lv3;
                break;
            case 4:
                currentLevel = lv4;
                break;
            case 5:
                currentLevel = lv5;
                break;
            case 6:
                currentLevel = lv6;
                break;
            case 7:
                currentLevel = lv7;
                break;
            case 8:
                currentLevel = lv8;
                break;
            case 9:
                currentLevel = lv9;
                break;
            case 10:
                currentLevel = lv10;
                break;
            case 11:
                currentLevel = lv11;
                break;
            case 12:
                currentLevel = lv12;
                break;
            case 13:
                currentLevel = lv13;
                break;
            case 14:
                currentLevel = lv14;
                break;
            case 15:
                currentLevel = lv15;
                break;
            case 16:
                currentLevel = lv16;
                break;
            case 17:
                currentLevel = lv17;
                break;
            case 18:
                currentLevel = lv18;
                break;
            case 19:
                currentLevel = lv19;
                break;
            case 20:
                currentLevel = lv20;
                break;
            case 21:
                currentLevel = lv21;
                break;
            case 22:
                currentLevel = lv22;
                break;
            case 23:
                currentLevel = lv23;
                break;
            case 24:
                currentLevel = lv24;
                break;
            case 25:
                currentLevel = lv25;
                break;
            case 26:
                currentLevel = lv26;
                break;
            case 27:
                currentLevel = lv27;
                break;
            case 28:
                currentLevel = lv28;
                break;
            case 29:
                currentLevel = lv29;
                break;
            case 30:
                currentLevel = lv30;
                break;
            case 31:
                currentLevel = lv31;
                break;
            case 32:
                currentLevel = lv32;
                break;
 
            default:
                currentLevel = lv1;
                break;
        }
        currentLevel.gameObject.SetActive(true);
}
}
