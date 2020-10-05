using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// The CountdownTimer Class manages the Round Timer(the GameController manages the less feature complete bomb and launch timers).
/// Again.. this Method only handles the round timer.(For ChillMode, or any other mode you add that would need a timer).  The Regular game
/// mode does not include a timer... if you miss 3 balls, or hit a bomb you are dead.
/// </summary>
public class CountdownTimer : MonoBehaviour
{

    public float timeTextStartsFlashing;                // this is the time that the text starts flashing (when timer has almost run out).
    public float timeBetweenTimerTextFlashes;           // after you have set a flash time this sets how much time there is between flashes.
    public float timeLeft = 0f;                         // timeLeft is how much time left on the timer.  This is set via script.
    private bool stop = true;                           // boolean - stop is used when the timer has stopped.
    private int minutes;                                // minutes... how many minutes are on the clock.
    private int seconds;                                // seconds... how many seconds are on the clock.
    public Text uiText;                                 // the text component that will be displaying the timer.
    public Color startColor;                            // the start color of the timer text.
    public Color flashingColor;                         // the color of the text after you have reached the "timeTextStartsFlashing"...
                                                        //      so the timer flashes and changes color.  (i.e. a green 60 sec timer
                                                        //      starts flashing at 10 seconds remaining, and the color is now red, or orange.


    // Use this for initialization
    void Start()
    {
        //set text.color to the color we set for "startColor" in the inspector.
        uiText.color = startColor;
    }


    /// <summary>
    /// This Method is responsible for starting the countdown timer.  It takes a float parameter "Number of Seconds the timer should countdown from";
    /// </summary>
    /// <param name="from"></param>
    public void StartTimer(float from)
    {
        //"stop" boolean becomes false;
        stop = false;
        //"timeLeft" is assigned the float value of "from" (which we passed in).
        timeLeft = from;
        //Start the Coroutine UpdateTimeTextDisplay..
        StartCoroutine(UpdateTimeTextDisplay());
    }


    /// <summary>
    /// This Method is responsible for stopping the countdown timer.
    /// </summary>
    public void StopAndResetTimer()
    {
        //Unity's update method is looking for this boolean to change.  When stop is changed to true, the clock stops.
        stop = true;
        //Stop all coroutines!
        StopAllCoroutines();
    }


    // Update is called once per frame
    void Update()
    {
        //if the stop boolean is true return
        if (stop)
        {
            //return early... no need to reduce time...
            return;
        }
        // we call our ReduceTime method every frame.
        ReduceTime();
    }


    /// <summary>
    /// This is the code that runs in update.  It is responsible for decrementing "timeLeft" by Time.deltaTime every frame.  Then simple 
    /// math is used to deduce the "Minutes" and "Seconds" from the timeLeft variable.  Once timeLeft is approximately zero it stops the countdown.
    /// </summary>
    private void ReduceTime()
    {
        //timeLeft has Time.deltTime subtracted from it every time it runs.
        timeLeft -= Time.deltaTime;
        // we store the number of "minutes" as a integer using Mathf.FloorToInt to return the nearest whole number when our "timeLeft" is divided by 60(the number of seconds in a minute).
        // Example... if timeLeft is 135.33f, then FloorToInt 135.33/60 (which equals 2.2555)... FloorToInt(2.2555) returns int 2.
        minutes = Mathf.FloorToInt(timeLeft / 60);
        // we store the number of "seconds" as a integer using Mathf.FloorToInt to return the nearest whole number.. timeLeft - minutes * 60. I.e. (timeLeft = 135.33) we are not at last minute so 
        //"minutes" will be 2, so timeLeft(float) will be timeLeft - ((2 * 60) which is 120)... so it just FloorToInts(135.33 - 120) which turns  15.33 into 15... making the total 135.33 time 2:15
        seconds = Mathf.FloorToInt(timeLeft - minutes * 60);

        //if seconds is above 59 then seconds becomes 59
        if (seconds > 59)
        {
            seconds = 59;
        }

        //if minutes falls BELOW zero, then we better stop the clock.
        if (minutes < 0)
        {
            //stop
            stop = true;
            //minutes gets 0
            minutes = 0;
            //seconds gets 0
            seconds = 0;
        }
    }

    /// <summary>
    /// This Method will disable the text component IF when called "timeLeft" is equal to 0 or "stop" is set to true.
    /// </summary>
    public void DisableTimerText()
    {
        //if timeLeft is less than or equal to 0 or stop is true.
        if(timeLeft <= 0 || stop == true)
        {
            //uiText set to inactive
            uiText.enabled = false;
        }
    }

    /// <summary>
    /// IEnumerator that updates the Text in the UI Canvas, Including changing its color
    /// when it get close to running out.
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateTimeTextDisplay()
    {
        //while stop is false 
        while (!stop)
        {
            // if timeLeft is greater than timeTextStartsFlashing then... regular countdown behavior(i.e. no flashing/color change)
            if(timeLeft > timeTextStartsFlashing)
            {
                // use string.Format to change the uiText's text to be formatted like a typical "clock" layout.
                uiText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
                // wait for 0.2 seconds
                yield return new WaitForSeconds(0.2f);
            }
            else
            //else if timeLeft is less than timeTextStartFlashing
            {
                //then we want the text to flash, and we want to go ahead and change the text color.

                // use string.Format to change the uiText's text to be formatted like a typical "clock" layout.
                uiText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
                // change the color property to the flashing color (choose a orange,yellow, or red)
                uiText.color = flashingColor;
                //waitForSeconds (timeBetweenTimerTextFlashes)
                yield return new WaitForSeconds(timeBetweenTimerTextFlashes);

                //if timeLeft is less than 0.02f
                if (timeLeft < 0.02f)
                {
                    // If time is up(or really close to it in this case (0.02f)), then we can go ahead and call DisableTimerText();
                    DisableTimerText();

                }

                //after waiting for timeBetweenTimerTextFlashes in the WaitForSeconds above...
                uiText.text = "";
                //then we yield again... and we keep going thru this until "stop" equals true.
                yield return new WaitForSeconds(timeBetweenTimerTextFlashes);
            }

        }
    }
}