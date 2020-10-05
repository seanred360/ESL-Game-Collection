using UnityEngine;
using UnityEngine.UI;

namespace SlicingGame
{
    /// <summary>
    /// The ShowCutBallUI Class just handles what Current and Highest Score is displayed at the top left-hand corner of the screen.  The values
    /// that are shown are determined by the GameController, but WHICH ones to show are handled by this script.  It checks the Game Controllers current
    /// selected game mode and activates that score/highest score ui element.
    /// </summary>
    public class ShowCutBallUI : MonoBehaviour
    {
        public Text[] regularModeCutBallText;          // the Text elements for regular mode
        public Text[] chillModeCutBallText;            // the Text elements for chill mode


        // Use this for initialization
        void Start()
        {
            //In start we call the Method that deactivates all the text, and turns on one of them based on GameController.
            DeactivateAllAndSelectCutTextPerMode();

        }

        /// <summary>
        /// This Method starts by looping through all of the elements in the Text Array and disables them, then it sets the proper text elements active.
        /// </summary>
        private void DeactivateAllAndSelectCutTextPerMode()
        {



            //first loop through all regularmode Text elements in the array
            for (int j = 0; j < regularModeCutBallText.Length; j++)
            {
                //then each element [i] gets set disabled.
                regularModeCutBallText[j].enabled = false;
            }


            //first loop through all relaxMode Text elements in the array
            for (int k = 0; k < chillModeCutBallText.Length; k++)
            {
                //then each element [i] gets set disabled.
                chillModeCutBallText[k].enabled = false;
            }



            //now for turning on one of the sets we just disabled...
            switch (GameController.GameControllerInstance.gameModes)
            {
                //if GameController's gameMode equals GameModes.RegularGameMode then...
                case GameModes.RegularGameMode:

                    //we loop back through an array of the text elements...
                    for (int i = 0; i < regularModeCutBallText.Length; i++)
                    {
                        // and set all of the elements to enabled!
                        regularModeCutBallText[i].enabled = true;
                    }
                    break;

                //if GameController's gameMode equals GameModes.ChillGameMode then...
                case GameModes.ChillGameMode:

                    //we loop back through an array of the text elements...
                    for (int i = 0; i < chillModeCutBallText.Length; i++)
                    {
                        // and set all of the elements to enabled!
                        chillModeCutBallText[i].enabled = true;
                    }

                    break;
                default:
                    //no action if some other action happens... We are all done here :)
                    break;
            }
        }

    }
}
