using UnityEngine;

public class BoardNode : MonoBehaviour
{
    public enum NodeType
    {
        normal, stop, plusTwo, minusTwo, starEvent, riverEvent
    }

    public NodeType nodeType;
    public Transform spring, standingTarget;
    public Transform roadBlock;
    public TimelinePlaybackManager timelinePlaybackManager;
    public bool isCompleted;

    private void Awake()
    {
        if(timelinePlaybackManager == null) { timelinePlaybackManager = GetComponent<TimelinePlaybackManager>(); }

        if(nodeType == NodeType.plusTwo)
        {
            spring = transform.Find("Spring");
            standingTarget = spring.transform.Find("StandingTarget");
            spring.gameObject.SetActive(false);
        }
        if (nodeType == NodeType.stop)
        {
            roadBlock = transform.Find("RoadBlock");
        }
    }
}
