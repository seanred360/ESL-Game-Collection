using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGPlayer : MonoBehaviour
{
    public HealthBarScript playerHealth;
    public Animator anim;

    private void Awake()
    {
        playerHealth = gameObject.GetComponentInChildren<HealthBarScript>();
        anim = GetComponent<Animator>();
    }
}
