using UnityEngine;

/// <summary>
/// Simple Class that Disables a GameObject after delay time.
/// </summary>
public class DisableGameObject : MonoBehaviour
{

    public float delay;                 //The amount of time to wait for disabling the GameObject(can also be 0...)

    // Use this for initialization
    void OnEnable()
    {
        //once this game has started and this object is enabled we start the invoke method that will disable the object.
        Invoke("DisableObj", delay);
    }

    /// <summary>
    /// Simple Method that sets gameObject inactive.
    /// </summary>
    void DisableObj()
    {
        gameObject.SetActive(false);
    }

    // This function is called when the behaviour becomes disabled or inactive
    public void OnDisable()
    {
        CancelInvoke();
    }
}
