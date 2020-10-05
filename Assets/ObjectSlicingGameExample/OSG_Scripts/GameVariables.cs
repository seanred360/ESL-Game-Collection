using UnityEngine;

/// <summary>
/// Static Class that holds some of the Game/Player Data.  At this Time we save some of these
/// values in PlayerPrefs.  Later I will setup a BinaryFormatter/FileStream to save the 
/// necessary data to a bin file.
/// </summary>
public static class GameVariables
{

    public static int BallsMissed;                                                              //The number of ball missed in a given round(important for regularMode).
    public static int Experience = PlayerPrefs.GetInt(Tags.experience);                         //the players experience which is stored via PlayerPrefs
    public static int Level                                                                     //the players level which is the players experience divided by 500
    {
        get
        {
            return Experience / 500;
        }

        set
        {
            Experience = value * 500;
        }
    }
    public static float splatterQuadSpawnDistance = 55f;                                        //this is the distance from 0,0,0 on the z-axis that the ball splatters are spawned
    public static int RegularModeScore;                                                         //the score var used for each round of regular mode
    public static int RegularModeHighestScore = PlayerPrefs.GetInt(Tags.highestRegularScore);   //the Highest Score achieved in regularMode which is stored via PlayerPrefs
    public static int ChillModeScore;                                                           //the score var used for each round of relax mode
    public static int ChillModeHighestScore = PlayerPrefs.GetInt(Tags.highestChillScore);       //the Highest Score achieved in relaxMode which is stored via PlayerPrefs
    public static float soundVolume = 0.8f;                                                     //global sound volume
    public static int mutedVolume = PlayerPrefs.GetInt("mutedAudio");                           //Saves state of the Muted Audio Boolean

    public static int dojoBGNum = PlayerPrefs.GetInt("BGint");                                  // the int for the selectedBg(we get the last stored val).  We set this value in the UIDojoSelector
    

    public static GameObject screenFaderPrefab =                                                //Reference to the location of ScreenFader Prefab(for editor use only)
Resources.Load("Prefabs/MODFaderPrefab(DontDestroyOnLoad)") as GameObject;


    /*"Prefabs/FaderCanvas(DontDestroyOnLoad)"*/

    //****Notes****
    //The reference to the Screen Fader Prefab(in Resources) is there so
    //that the game can be started from any scene, and the prefab will
    //be created if its needed and missing from scene(it normally lives in scene0)
    //and persists from that point on...  The game SHOULD be started from Scene0
    //for a proper experience.  Things may go wrong if not... 

    //In editor... hard to do.  We just want to check out individual scenes and hit play.  

    //In Release/Build Time.... easy to do... it starts from the beginning every time.

    //Shouldn't be an issue anymore... there is a fade caller on every camera... 6/16/16


}
