using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SlicingGame
{
    /// <summary>
    /// The BallDestroyCombo Class handles the Visibility,Tween, and Count of the Destroyed Ball "Combo" UI display.
    /// </summary>
    public class BallDestroyCombo : MonoBehaviour
    {
        private Text ballComboNum;                                                  // the text component we would be feeding the combo number if we don't use the number sprites
        private CanvasGroup comboTweenCanvasGroup;                                  // the canvas group reference so we can use it to fade alpha
        private Animator comboAnim;                                                 // animator reference for the "combo text tween"
        private int tweenHash = Animator.StringToHash(Tags.comboAnimStringToHash);  // the value to activate the Tween of the Combo Text
        private AnimatorStateInfo baseStateInfo;                                    // reference to the animator state
        private RectTransform comboRect;                                            // rect of the combo texts/sprites
        private RectTransform comboRectParent;                                      // parent rect with this script on it.
        public float comboAmountOfTime = 0.5f;                                      // the amount of time that you have to swipe at least 3 balls in order to get a combo
        public int ballsDestroyedInTime;                                            // the number of balls we destroyed in this half second block.
        public List<Transform> comboTextLocations;                                  // list of transforms of empty gameObjects placed in a sort of grid around the scene view.(where the "Combo Text" can be displayed)
        public Transform selectedComboTextPoint;                                    // the transform that was closest to the last cut ball. and the anchor position where we will place to combo text.

        public bool useImagesForComboNum;                                           // boolean that determines whether sprites or a text component are used to display the numeric portion of the Combo Text
                                                                                    //Inspector Formatting Region...
        #region
        [Space(15, order = 0)]
        [Header("Below is only Used With Non-Text Component Combo Numbers.", order = 1)]
        [Space(15, order = 1)]
        [Header("If not using the 'Text' UI Component for the Ball", order = 2)]
        [Space(-5, order = 2)]
        [Header("'#', then this is an array of the likely ''Combo", order = 3)]
        [Space(-5, order = 3)]
        [Header("Number Sprites'' from our Atlas ie.'(Number 3-9)'", order = 4)]
        [Space(-5, order = 4)]
        [Space(15, order = 5)]

        #endregion

        public Sprite[] comboNumberSpritesFromAtlas;                        // Numbers 3 - 9 of the UI Atlas
        public Image ballComboNumImageReference;                            // reference to the image component that we will feed the desired "Number Sprite" to
        private float _internalComboTimerStartAmount;                       // the value we first put on the timer.

        // Use this for pre-initialization
        void Awake()
        {

            //initialize the list of Transforms we declared above.
            comboTextLocations = new List<Transform>();

            //our ComboRect element is the First Child of the gameObject this script is on... we get the RectTransform component of that child.
            comboRect = transform.GetChild(0).GetComponent<RectTransform>();

            //because we will need to do some simple math to get the ComboText RectTransform aligned we will also
            //get the ComboRects Parent(the object this script is attached to).
            comboRectParent = GetComponent<RectTransform>();

            //Get a reference to the UI text element that will display the number of destroyed balls
            // that are cut within the half second combo periods
            ballComboNum = GameObject.FindGameObjectWithTag(Tags.comboNumTag).GetComponent<Text>();
            //Get a CanvasGroup reference.  the "Canvas Group" is the component who's alpha we fade.  Easier to fade the parent
            //of the ComboText object(s) ( because we may uses meshes/textures/UI texts... etc)
            comboTweenCanvasGroup = GetComponent<CanvasGroup>();
            //Get a reference to the Animator component so that we can play the ComboText tween animation.
            comboAnim = GetComponentInChildren<Animator>();

            //if we intend to use atlas images for numbers 3,4,5,6,7,8,9 Combos, then get a reference to our Image Component
            //and disable the Text Component because we don't need both.
            if (useImagesForComboNum)
            {
                // lol... this feels really, really bad, BUT I am trying to keep Find/Tag usage to a minimum... if you use any
                //part of this for a release title, please change this, or note that the Combo Canvas ordering CANNOT be changed
                //at least the top of the hierarchy... we may change this before template submission.  Several GetChilds....
                ballComboNumImageReference = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();

                //set the initial 'source' image to the first sprite in our ComboNumberSpritesFromAtlas Array...
                ballComboNumImageReference.sprite = comboNumberSpritesFromAtlas[0];

                //lets go ahead and just deactivate the text component on our original "Text.text" version of the "# Hit Combo" system.
                //We don't want it showing through our Sprite Numbers.  If the above boolean is checked then you must have it setup to use the images,
                //or you will be stuck without a working combo system.  Refer to the working copy of the game scene and look at what's happening.
                ballComboNum.enabled = false;

                //Below we will write a Method Named "ChooseCorrectNumberSprite()" and then we will use the same boolean from above so that both
                //systems will work together... just requiring a boolean to be tick in the inspector.  I am only doing this because the atlas' "numbers" 
                //"style" matches that of the "Combo" and "Hit" text sprites.  By the way... TextMeshPro is an incredible asset.  You should give it a look.
                //I am doing this because i wanted more control over the text visuals, and so I am doing this the quick and dirty way.  BUT.. with the above
                //mentioned asset you get a lot of control/ability you cannot get with the built-in color/styles of text components.
            }



        }

        // Use this for initialization
        void Start()
        {
            //set our internal timer var to the "comboAmountOfTime...
            _internalComboTimerStartAmount = comboAmountOfTime;
            //add all the "Empty" GameObjects that are our "9 potential ComboText Anchors" to a GameObject array
            GameObject[] itemsForList = GameObject.FindGameObjectsWithTag(Tags.comboTextLocations);
            //loop through all of those "anchors" in the gameobject array.
            for (int i = 0; i < itemsForList.Length; i++)
            {
                //create a Transform var for each element in the GO array.
                Transform trans = itemsForList[i].GetComponent<Transform>();
                //then we add the transform that we create for every element, and add it to the List.
                comboTextLocations.Add(trans);
            }

            //Because our ComboText start enabled, and visible... the first thing we need to do is to fade it out...
            StartCoroutine(FadeComboText(0f, 0f, 0.5f)); // 0 = immediately, 0 = fade alpha to 0, and 0.5f means do all of this over a half second.
        }


        /// <summary>
        /// This method is responsible for taking the last ball we chopped in our ball combo "period"... It sorts through
        /// the list of ComboTextAnchors we have in the list<> and fines the one closest to our ball.  When you slice a number 
        /// of balls, the combo text appears close to the last cut ball. So we set 9 points in our game area 3x3 grid, so that 
        /// we can have the combo text close to that location, and so the ComboText is predictable, and never off screen.
        /// </summary>
        /// <param name="posOfBall"></param>
        public void SortDistanceToComboTextAnchorLocation(Vector3 posOfBall)
        {

            ///Original Form of the Sort Method within region...  This is just another in-line way to do the sort.
            #region
            //comboTextLocations.Sort(delegate (Transform t1, Transform t2) {
            //    return Vector3.Distance(t1.transform.position, transform.position).CompareTo(Vector3.Distance(t2.transform.position, transform.position));
            //});
            #endregion

            //This uses the basic List<T> Sort Method... It compares 2 values.  when this method is called it compares the distance between our first anchor
            //position and our balls position, and it stores it in "distanceTo1".  Then it does the same with "distanceTo2" but with another list entry, then 
            //it uses .CompareTo to compare distanceTo1 and distanceTo2... See next comment to see what the CompareTo returns.
            comboTextLocations.Sort(delegate (Transform t1, Transform t2)
           {
            //if the list entries are not null...
            if (t1 != null && t2 != null)
               {
                //new floats that represent the distance between point one and the last ball chopped in the combo stretch..(described above)
                float distanceTo1 = Vector3.Distance(t1.position, posOfBall);
                   float distanceTo2 = Vector3.Distance(t2.position, posOfBall);

                //return an int(which is how the list of anchors are sorted/sifted.  All of the points are checked(two at a time), and moved around in the list.
                // if t1 is closer to the ball, than t2, then -1 is returned to the caller.  the rest of the return possibilities are commented below, but this
                //is how this is sorted.  There are so many ways that this can be done, and you can learn a lot more by looking up List(T).Sort Method 
                //(IComparer(T)) on the MSDN.

                return distanceTo1.CompareTo(distanceTo2);// t1 is closer = returns -1 // if t1 is same dist = returns 0 // if t1 is further = returns 1 //
            }
               else
            // if there are any missing elements or problems... return 0
            {
                   return 0;
               }
           });//end of delegate sort Method
        }

        /// <summary>
        /// This Method Changes the HitCombo's Number image to the correct Number image from our sprite array.
        /// </summary>
        /// <param name="numOfDestroyedBalls"></param>
        public void ChooseCorrectNumberSprite(int numOfDestroyedBalls)
        {
            if (ballComboNumImageReference != null)
            {
                if (comboNumberSpritesFromAtlas.Length == 7)
                {
                    //Debug.Log("Yep, everything is set up properly in the inspector...");
                    //Debug.Log("there are 7 entries in the array, so because or combo Sprite function /n is only for 3-9 ball we should be good...");
                    //Debug.Log("go ahead and change sprite to" + "=" + " " + numOfDestroyedBalls.ToString());
                    ballComboNumImageReference.sprite = comboNumberSpritesFromAtlas[numOfDestroyedBalls - 3];
                }
                //Debug.Log("image ref wasn't null");
            }

        }


        // Update is called once per frame
        void Update()
        {
            //call this method every frame to reduce the combo timer(and reset it to zero
            // when necessary,  It also increments the number of ball destroyed within that
            //time.  See method for more details.
            CheckTheCountdownTimer();

        }


        /// <summary>
        /// This method is responsible for checking if the time on the combo clock is below zero.  If it is below
        /// zero then it sets it back to half a second.  If the timer has run out; it zeros out all balls recorded 
        /// to the "ballDestoyedInTime" variable.
        /// </summary>
        public void CheckTheCountdownTimer()
        {
            //is the timer still running and above 0? or is it less than or equal to 0?
            if (comboAmountOfTime <= 0f)
            {
                //time is up so lets reset the combo clock..
                ResetComboTimer();
            }
            else
            {
                //else, clearly the timer is still above 0... reduce time since last frame,
                //and lets do this all again.
                comboAmountOfTime -= Time.deltaTime;
            }
        }

        /// <summary>
        /// This is a super short Method that just resets the combo timer, and zero's out the "ballsDestroyedInTime" Count.
        /// This is in a separate method from above so that we can invoke this method after a second or so passes if we want to... that way it doesn't
        /// have a chance to achieve combos back to back to back... etc.  Later on I removed the invoke, bc it didn't seem to be an issue.  So this
        /// Method just gets called normally, when it is time to reset the clock, and 0 the "balls cut" variable.
        /// </summary>
        public void ResetComboTimer()
        {
            //if the time has run out we zero out the number of destroyed ball...
            //this round is over... better luck next time ;-P
            ballsDestroyedInTime = 0;
            // the timer is reset to half a second.
            comboAmountOfTime = _internalComboTimerStartAmount;
        }

        /// <summary>
        /// This method is responsible for counting the balls that are destroyed within a half second of each other.  Once it
        /// does that it, if the number is 3 or above it displays our "ComboText", and puts it at 1 of the 9 anchors that we placed
        /// in the scene.  It will choose the anchor that is closest to the last chopped ball(in this combo time-frame.
        /// </summary>
        /// <param name="ballPosition"></param>
        public void CheckTimeAndRecordBall(Vector3 ballPosition)
        {
            // if the timer is above 0
            if (comboAmountOfTime > 0f)
            {
                //we must still be counting down so increase the number of destroyed balls.
                ballsDestroyedInTime++;

                // if we have destroyed at least 3 other balls
                if (ballsDestroyedInTime >= 3)
                {
                    //call the sort Method and give it the eligible ball pos((Method Parameter)the last ball that achieved the "combo")
                    SortDistanceToComboTextAnchorLocation(ballPosition);

                    //after the sort method has been called the ComboText anchor point that is closest to the eligible ball
                    // (That brings that anchor to be at position 0 in the list when the sort completes)

                    //so now... selectedComboTextPoint = comboTextLocations[0]
                    selectedComboTextPoint = comboTextLocations[0];

                    //call the method that turns the world space coord(the comboText anchor) into a screen-space
                    //coord.  (so that we can display the UI Images/Text at an appropriate location) The Location
                    //that is relative to the anchor location, from out camera view-port.
                    SetComboTextToBallPosition(selectedComboTextPoint.position);

                    //call the method that fades the comboText elements into view
                    ActivateAndTweenComboText();
                }
            }
            ////call the method that fades the comboText elements into view
            //ActivateAndTweenComboText();
        }


        /// <summary>
        /// This Method is responsible for changing the coords of a ComboText anchor point into screen-space coords
        /// </summary>
        /// <param name="aPos"></param>
        public void SetComboTextToBallPosition(Vector3 aPos)
        {
            //new vector2 that stores the value of the aPos that we sent to this Method.
            Vector2 pos = aPos;

            //new vector2 that stores the conversion of WorldToViewportPoint passed our parameter position("pos")
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(pos);

            //new vector2 then you use the ComboTextRects Parent RectTransform to deduce the necessary shift to
            //place the ComboTextRect in the right location.  For the Canvas 0,0 is center, but the function
            // WorldToViewportPoint uses "view-port"(i.e. 0,0 is bottom left).  So we need to subtract the height &
            // width of the canvas * 0.5 to get the proper coord.
            Vector2 WorldObject_ScreenPosition = new Vector2(
                ((ViewportPosition.x * comboRectParent.sizeDelta.x) - (comboRectParent.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * comboRectParent.sizeDelta.y) - (comboRectParent.sizeDelta.y * 0.5f)));

            //so now we give the ComboTextRext the anchor location that we adjusted above.
            comboRect.anchoredPosition = WorldObject_ScreenPosition;

            // so now the ComboText Element(s) are in the correct point, and from a vantage point down the 
            // z-axis it will appear that the ComboText is at one of our 9 "anchor" locations. :)
        }


        /// <summary>
        /// This method is responsible for activating, and tweening the "ComboText".  If in the half a second the comboTimer is running
        /// we destroyed 3 or more balls, then we will activate the tween animation, and fade in the ComboText UI element.  Using
        /// our Fade coroutine.
        /// </summary>
        public void ActivateAndTweenComboText()
        {
            //have we destroyed 3 or more balls, and less than 10(we only have numbers 3-9 from a sprite standpoint.. would have to use text component or make additional
            //sprites to go higher... we aren't worried about that right now...)
            if (ballsDestroyedInTime > 2 && ballsDestroyedInTime < 10)
            {
                //if so then we check the current animator state(which should be idle)
                baseStateInfo = comboAnim.GetCurrentAnimatorStateInfo(0);

                //if that animator state does not equal the "tweenHash"(which is the StringToHash "PlayComboTween"
                // which we store in our Tags class.
                if (baseStateInfo.fullPathHash != tweenHash)
                {
                    //if the state isn't "playTween".. then we will play the tween
                    comboAnim.SetTrigger(tweenHash);
                }
                //give bonus pointsif the player gets a combo... Can be gamemode specific.
                switch (GameController.GameControllerInstance.gameModes)
                {
                    case GameModes.RegularGameMode:
                        GameVariables.RegularModeScore += 2;//give a two point bonuse when a combo is achieved
                        break;

                    case GameModes.ChillGameMode:
                        GameVariables.ChillModeScore += 2;//give a two point bonuse when a combo is achieved
                        break;
                    default:
                        break;
                }
                //stop all coroutines for good measure
                StopAllCoroutines();
                //start our fade coroutine immediately, and change alpha to 1, in 0.5 seconds...

                // which sets our ComboText to visible.
                StartCoroutine(FadeComboText(0f, 1f, 0.5f));


                if (!useImagesForComboNum)
                {
                    // make the ballComboNum.text Text UI Element to the var amount "ballsDestroyedInTime"... .ToString because we need a string for the text components.text property.
                    ballComboNum.text = ballsDestroyedInTime.ToString();
                }
                else
                {
                    //else we use our number sprites... so we call ChooseCorrectNumberSprite(and pass the number we need).  This method will know which number we want.
                    ChooseCorrectNumberSprite(ballsDestroyedInTime);
                }


                //Invoke our DeactivateComboText Method in 4 seconds... which will stop all coroutines(after they should have already finished anyway)
                // and it starts the fade out coroutine.
                Invoke("DeactivateComboText", 4);
            }

        }


        /// <summary>
        /// This method starts a coroutine that fades the "ComboText" to Transparent
        /// </summary>
        public void DeactivateComboText()
        {
            //stop all coroutines for good measure ;)
            StopAllCoroutines();

            //Start Coroutine to fade the ComboText Alpha to Zero
            StartCoroutine(FadeComboText(0f, 0f, 0.5f));
            //Debug.Log("done fading combo text!");
        }


        /// <summary>
        /// This Coroutine is responsible for fading the ComboText.  First parameter is how long to wait before 
        /// starting the fade.  Second parameter is WHAT value to fade to(ie.. 1 opaque, 0 transparent).  The Third
        /// parameter is how long the fade should take.  This method is used to both fade in, and out.
        /// </summary>
        /// <param name="initFadeDelay"></param>
        /// <param name="aValue"></param>
        /// <param name="aTime"></param>
        /// <returns></returns>
        IEnumerator FadeComboText(float initFadeDelay, float aValue, float aTime)
        {
            //wait if there is a wait time...
            yield return new WaitForSeconds(initFadeDelay);
            //record the current alpha value of our ComboText Canvas
            float alpha = comboTweenCanvasGroup.alpha;

            //for loop the time it takes t to equal 1f.  we increment t by time.delta / our parameter "aTime" every iteration
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                //every pass we Lerp the alpha close to the desired value by the increment amount(t)
                comboTweenCanvasGroup.alpha = Mathf.Lerp(alpha, aValue, t);
                //return null - loop again until t = value.
                yield return null;

            }
            //now that we are done moving towards "aValue", lets set the canvas group alpha to "aValue"...
            //A lot of the time it gets really close to 0, but because of precision issues with floating point
            //numbers it could leave a slight "Ghosting" effect where you can still see some of the combo text,
            //and we don't want that.  So what ever value we wanted to lerp the alpha to, we set manually at the end
            //to solve the problem.
            comboTweenCanvasGroup.alpha = aValue;
        }


    }
}