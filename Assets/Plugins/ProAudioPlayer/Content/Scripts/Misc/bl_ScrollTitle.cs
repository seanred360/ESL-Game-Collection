using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bl_ScrollTitle : MonoBehaviour {

    public string m_Text = "";
    [Space(5)]
    public Text mText = null;
    public Vector2 StartPosition;
    public Vector2 MiddlePosition;
    public Vector2 FinalPosition;
    [Space(5)]
    public MoveType m_MoveType = MoveType.Snapp;
    [Range(1, 100)]
    public float ScrollSpeed = 100f;
    public float WaitForNextPos = 5f;

    protected bool isDone = false;
    private int state = 0;
    protected int CurrentText = 0;
    protected bool mAvaible = true;
    protected float _timeStartedLerping;


    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {

        if (mText == null)
            return;


        Vector2 p = mText.rectTransform.anchoredPosition;
        if (state == 0 && mAvaible)
        {
            if (m_MoveType == MoveType.Snapp)
            {
                p = Vector2.MoveTowards(p, StartPosition, Time.deltaTime * ((ScrollSpeed * 5) * Time.timeScale));
                if (p == StartPosition && mAvaible)
                {
                    StartCoroutine(NextState(true, 0.0f));
                }
            }
            else if (m_MoveType == MoveType.Lerp)
            {
                float timeSinceStarted = Time.time - _timeStartedLerping;
                float percentageComplete = timeSinceStarted / (ScrollSpeed / 10);
                p = Vector2.Lerp(p, StartPosition, percentageComplete);
                if (percentageComplete >= 1.0f && mAvaible)
                {
                    StartCoroutine(NextState(true, 0.0f));
                }
            }

        }
        else if (state == 1 && mAvaible)
        {
            if (m_MoveType == MoveType.Snapp)
            {
                p = Vector2.MoveTowards(p, MiddlePosition, Time.deltaTime * ((ScrollSpeed * 5) * Time.timeScale));
                if (p == MiddlePosition && mAvaible)
                {
                    StartCoroutine(NextState(false, WaitForNextPos));
                }
            }
            else if (m_MoveType == MoveType.Lerp)
            {
                float timeSinceStarted = Time.time - _timeStartedLerping;
                float percentageComplete = timeSinceStarted / (ScrollSpeed / 10);
                p = Vector2.Lerp(p, MiddlePosition, percentageComplete);
                if (percentageComplete >= 1.0f && mAvaible)
                {
                    StartCoroutine(NextState(false, WaitForNextPos));
                }
            }

        }
        else if (state == 2 && mAvaible)
        {
            if (m_MoveType == MoveType.Snapp)
            {
                p = Vector2.MoveTowards(p, FinalPosition, Time.deltaTime * (ScrollSpeed * 5));
                if (p == FinalPosition && mAvaible)
                {
                    StartCoroutine(NextState(false, 0.5f));
                }
            }
            else if (m_MoveType == MoveType.Lerp)
            {
                float timeSinceStarted = Time.time - _timeStartedLerping;
                float percentageComplete = timeSinceStarted / (ScrollSpeed / 10);
                p = Vector2.Lerp(p, FinalPosition, percentageComplete);
                if (percentageComplete >= 1.0f && mAvaible)
                {
                    StartCoroutine(NextState(false, 0.5f));
                }
            }

        }
        mText.rectTransform.anchoredPosition = p;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    public void ChangeText(string t)
    {
        mText.text = t;
    }
    /// <summary>
    /// 
    /// </summary>
    void ResetPosition()
    {
        mText.rectTransform.anchoredPosition = StartPosition;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator NextState(bool first, float t)
    {
        mAvaible = false;
        _timeStartedLerping = Time.time;
        if (!first)
        {
            yield return new WaitForSeconds(t);
        }
        if (state == 2)
        {
            ResetPosition();
        }
        state = (state + 1) % 3;
        mAvaible = true;

    }

    [System.Serializable]
    public enum MoveType
    {
        Lerp,
        Snapp,
    }
}