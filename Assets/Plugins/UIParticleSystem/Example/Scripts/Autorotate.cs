using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autorotate : MonoBehaviour
{
    public Vector3 rotationSpeed;
    public bool localSpace;

    private void Awake()
    {
        Trans = transform;
    }

    void Update()
    {
        if(localSpace)
        {
            Trans.localRotation = Trans.localRotation * Quaternion.Euler(rotationSpeed * Time.deltaTime);
        }
        else
        {
            Trans.rotation = Trans.rotation * Quaternion.Euler(rotationSpeed * Time.deltaTime);
        }
    }

    public Transform Trans { get; private set; }
}
