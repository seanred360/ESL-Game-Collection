using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBarScript : MonoBehaviour {

    private float maxHealth = 100f;
    Image healthBar;
    public static float health;
    public GameObject mario1;
    private Animator marioAnimator;


    // Use this for initialization
    void Start() {
        marioAnimator = mario1.GetComponent<Animator>();
        healthBar = GetComponent<Image>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update() {
        healthBar.fillAmount = health / maxHealth;
    }
}
