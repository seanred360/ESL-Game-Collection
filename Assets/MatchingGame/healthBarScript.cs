using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {

    private float maxHealth = 100f;
    Image healthBar;
    public float health;
    public MGPlayer player;
    private Animator anim;


    // Use this for initialization
    void Start()
    {
        anim = player.anim;
        healthBar = GetComponent<Image>();
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / maxHealth;
        anim.Play("Die");
    }

    public void Die()
    {
        anim.SetBool("isDead", true);
    }
}
