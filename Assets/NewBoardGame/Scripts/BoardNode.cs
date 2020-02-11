using UnityEngine;

public class BoardNode : MonoBehaviour
{
    public enum NodeType
    {
        normal, stop, plusTwo, minusTwo, starEvent
    }

    public NodeType nodeType;
    public Transform spring, standingTarget;
    public Transform cage;

    private void Awake()
    {
        if(nodeType == NodeType.plusTwo)
        {
            spring = transform.Find("Spring");
            standingTarget = spring.transform.Find("StandingTarget");
            spring.gameObject.SetActive(false);
        }
        if (nodeType == NodeType.stop)
        {
            cage = transform.Find("Cage");
            cage.gameObject.SetActive(false);
        }
    }
}
