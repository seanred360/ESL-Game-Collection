using UnityEngine;

/// <summary>
/// Tag Class handles all of the Tags in the scene... There were quite a few entries made to the ProjectSettings Tags/Layers, and
/// I got tired of writing all of the strings, and was worried about some of them being misspelled.  This way I can create a static
/// constant read-only string once, make sure it IS right, and then I can have code completion.  So now if I use GameObject.FindGameObjectWithTag
/// I can pass Tags.Player instead of "Player".  Code completion is your friend!
/// 
/// </summary>
public static class Tags
{
    ///////////////////////////////////////////////////////////
    ///////// Scene, Player, & GameObject References //////////
    ///////////////////////////////////////////////////////////

    public static readonly string playerTag = "Player";                                                 // Tags.playerTag is "Player" ()
    public static readonly string ballTag = "Ball";                                                     // Tags.ballTag is "Ball" (all the ball are tagged in the scene)
    public static readonly string bombTag = "Bomb";                                                     //

    public static readonly string comboNumTag = "ComboNumberText";                                      // Tags.comboNumTag is "ComboNumberText" ()
    public static readonly string comboCanvasTag = "ComboCanvas";                                       // Tags.comboCanvasTag is "ComboCanvas" ()
    public static readonly string comboTextLocations = "PossibleComboTextLocations";                    // Tags.comboTextLocations is "PossibleComboTextLocations" (anchors for "ComboTextPopUp")

    public static readonly string bottomBallLaunchers = "BottomBallLaunchers";                          // Tags.bottomBallLaunchers is "BottomBallLaunchers" (the launchers at the bottom of the dojo)

    public static readonly string sideBallLaunchers = "SideBallLaunchers";                              // Tags.sideBallLaunchers is "SideBallLaunchers" (the launchers at the sides of the dojo(frenzy))

    public static readonly string cameraTag = "MainCamera";                                             // Tags.cameraTag is "MainCamera" (reference to the main camera)
    public static readonly string gameControllerTag = "GameController";                                 // Tags.GameController is "GameController" (ref to the Game Controller in the scene)




    public static readonly string playerMenuScoreText = "MenuScoreText";                                // Tags.playerMenuScoreText is "MenuScoreText" (the ref to the Menu Canvas Score Text Component)
    public static readonly string playerMenuLevelText = "MenuLevelText";                                // Tags.playerMenuLevelText is "MenuLevelText" (the ref to the Menu Canvas Level Text Component)


    public static readonly string ballTweenIcon = "BallTweenIcon";                                      // Tags.ballTweenIcon is "BallTweenIcon" (tag used to get the ball icon in top left corner's Animator)
    public static readonly string cutBallAnimTrigger = "CutBall";                                       // Tags.cutBallAnimTrigger is "CutBall" (tag used to trigger the pineapple Animators tween clip)

    public static readonly string GameMusicAudio = "GameMusic";                                         // Tags.GameMusicAudio is "GameMusic" (reference to the scene's Music AudioSource)
    public static readonly string GameSfxAudio = "GameSfx";                                             // Tags.GameSfxAudio is "GameSfx" (reference to the scene's Sfx AudioSource)

    public static readonly string BallPools = "ColorBallPools";                                         // Tags.BallPools is the "BallsPool" (reference to the pools that contain the colored balls for launching)
    public static readonly string OtherPools = "OtherPools";                                            // Tags.OtherPools is the "OthersPool" (reference to the pools that contain the Bombs and PowerUps for launching)

    public static readonly string DojoChnager = "DojoChanger";                                          // Tags.DojoChanger is the "DojoChanger" (reference to the DojoChange GO in each Scene)

    public static readonly string comboAnimStringToHash = "PlayComboTween";                             // the string the animator calls to trigger the combo text tween.

    ///////////////////////////////////////////////////////////
    ////////////// PlayerPrefs Strings & Keys /////////////////
    ///////////////////////////////////////////////////////////

    public static readonly string experience = "playerExperience";                                      // Tags.experience is "playerExperience" (reference to the "experience" text component)

    public static readonly string highestRegularScore = "highestRegularModeScore";                      // Tags.highestRegularScore is "highestRegularModeScore" (reference to the GameVariables.highestRegularScore)
    public static readonly string highestChillScore = "highestChillModeScore";                          // Tags.highestChillScore is "highestChillModeScore" (reference to the GameVariables.highestChillScore)


}

