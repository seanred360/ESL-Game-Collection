using UnityEngine;
using UnityEngine.EventSystems;

public class ResumeButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData b)
    {
        GameObject.FindGameObjectWithTag("Pause").GetComponent<PauseScript>().Unpause();
    }
}
