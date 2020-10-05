using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SlicingGame
{
    /// <summary>
    /// GenericUIElementFade Class handles the round start images.  The 60,90, seconds, and go!! text Fading.  A AnimationCurveMover
    /// handles the sliding, but this class handles the fade.
    /// </summary>
    public class GenericUIElementFade : MonoBehaviour
    {

        public Sprite sixtyImage;                                       //the sprite that is the "60" from the UI atlas
        public Sprite ninetyImage;                                      //the sprite that has the "90" from the UI atlas

        public Image secondsTextImage;                                  //the image that has the "Seconds" word from the UI atlas
        public Image amtTextImage;                                      //the image that has the "amt" from the UI atlas (60 or 90 from above)
        public Image goTextImage;                                       //the image that had the "Go" word from the UI atlas

        [Header("FadeOut Variables for 'Seconds' and '#Amt' Text")]
        public float fadeOutDelayTime;                                  //fade out delay for "seconds,and amt"
        public float fadeOutCompletedValue;                             //fade out completed value (0)
        public float fadeOutSpeed;                                      //the speed to fade out.
        [Header("FadeOut Variables for'Go' Text")]
        public float fadeOutDelayTimeForGoText;                         //fade out delay for "Go"
        public float fadeOutCompletedValueForGoText;                    //fade out complete value (0)
        public float fadeOutSpeedForGoText;                             //the speed to fade out
        private Color zeroAlphaColor = new Color(0, 0, 0, 0);              //a zeroAlpha color (may be used for making a sprite/image invisible.

        // Use this for initialization
        void Start()
        {
            //first we base the image/sprite type based on the gameMode we are in...
            switch (GameController.GameControllerInstance.gameModes)
            {
                //if we are in RegularGameMode mode...
                case GameModes.RegularGameMode:
                    //there is no need for a "seconds" sprite... regular has no time!
                    secondsTextImage.sprite = null;
                    //no need for the "amt" of seconds either...
                    amtTextImage.sprite = null;
                    //the "seconds" image color gets assigned the zeroAlphaColor.
                    secondsTextImage.color = zeroAlphaColor;
                    //the "amt" image color gets assigned the zeroAlphaColor too
                    amtTextImage.color = zeroAlphaColor;

                    // call this method that only fades out the "Go!!" image
                    FadeOutGoImageOnly();
                    break;


                //if we are in ChillGameMode mode...
                case GameModes.ChillGameMode:
                    //in relax mode we just have to change that amtTextImage.sprite get the "Ninety"
                    amtTextImage.sprite = ninetyImage;
                    // call FadeOutImages
                    FadeOutImages();

                    break;
                default:
                    //do nothing
                    break;
            }


        }


        /// <summary>
        /// This method will Fade Out all the Images.
        /// </summary>
        public void FadeOutImages()
        {
            //We fade out all of the images by StartCoroutine (Fade ( ) ) these all have really long variable names.  We have done this enough
            //that it should be pretty clear.

            //Start the Coroutine to fade the "secondsTextImage" AFTER "fadeOutDelayTime", TO "fadeOutCompletedValue", AT "fadeOutSpeed"
            StartCoroutine(FadeRoundStartText(secondsTextImage, fadeOutDelayTime, fadeOutCompletedValue, fadeOutSpeed));

            //Start the Coroutine to fade the "amtTextImage" AFTER "fadeOutDelayTime", TO "fadeOutCompletedValue", AT "fadeOutSpeed"
            StartCoroutine(FadeRoundStartText(amtTextImage, fadeOutDelayTime, fadeOutCompletedValue, fadeOutSpeed));

            //Start the Coroutine to fade the "goTextImage" AFTER "fadeOutDelayTimeForGoText", TO "fadeOutCompletedValueForGoText", AT "fadeOutSpeedForGoText"
            StartCoroutine(FadeRoundStartText(goTextImage, fadeOutDelayTimeForGoText, fadeOutCompletedValueForGoText, fadeOutSpeedForGoText));

        }

        /// <summary>
        /// This method will fade out only the "Go!!" Image.
        /// </summary>
        public void FadeOutGoImageOnly()
        {
            //Start the Coroutine to fade the "goTextImage" AFTER "fadeOutDelayTimeForGoText", TO "fadeOutCompletedValueForGoText", AT "fadeOutSpeedForGoText"
            StartCoroutine(FadeRoundStartText(goTextImage, fadeOutDelayTimeForGoText, fadeOutCompletedValueForGoText, fadeOutSpeedForGoText));
        }



        /// <summary>
        /// This is the fade method for the GenericUIElementFade Class... this Fade method is just like the other Fade Methods.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="initFadeDelay"></param>
        /// <param name="aValue"></param>
        /// <param name="aTime"></param>
        /// <returns></returns>
        IEnumerator FadeRoundStartText(Image img, float initFadeDelay, float aValue, float aTime)
        {
            //we wait the amount of the initFadeDelay
            yield return new WaitForSeconds(initFadeDelay);

            //we create a new Color named tempColor and assign it the passed img's color.
            Color tempColor = img.color;

            //we create a float and name it alpha we assign it the value of the tempColors alpha.
            float alpha = tempColor.a;

            //we loop from 0 to 1 at the rate of Time.deltaTime divided by aTime...
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                //every iteration we Mathf.Lerp the alpha and aValue(we called with), by t which is increasing to 1.  Then we assign that value to tempColor alpha
                tempColor.a = Mathf.Lerp(alpha, aValue, t);

                //img.color get assigned or new tempColor
                img.color = tempColor;

                //as a precaution (due to some experiences on lesser mobile devices) once the alpha is 1/10 of the way down we just set it to 0.
                if (img.color.a <= 0.1f)
                {
                    //tempColors alpha gets assigned 0f
                    tempColor.a = 0f;

                    //img.color is assigned tempColor (for the few that may be learning... the "Color" includes the alpha in this case. R,G,B,A
                    img.color = tempColor;
                }
                //yield return null... we are done.
                yield return null;
            }
        }

    }
}
