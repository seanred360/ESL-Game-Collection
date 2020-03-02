using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventHandler : MonoBehaviour
{
    public UnityEvent EndDialogue;

    public void InvokeEndDialggue()
    {
        EndDialogue.Invoke();
    }
}
