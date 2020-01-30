using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData a)
    {
        GameObject.FindGameObjectWithTag("Pause").GetComponent<PauseScript>().Pause();
    }
}

