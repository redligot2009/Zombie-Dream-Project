using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float health = 3;
    public bool dead = false;
    public float hitTimer = 0;
    public float healTimer = 0;
    public float healTimerDelay = 0.1f;
    public float hitTimerDelay = 1f;
    public float origHealth = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        origHealth = Mathf.Max(health, origHealth);
        if(healTimer > 0)
        {
            healTimer -= Time.deltaTime;
        }
        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
        }
        dead = health <= 0;
    }

    public void Heal(float healthRestore = 0)
    {
        if(healTimer <= 0)
        {
            healTimer = healTimerDelay;
            health = Mathf.Min(health + healthRestore, origHealth);
        }
    }

    public void Hurt(float damage = 1)
    {
        if (hitTimer <= 0)
        {
            hitTimer = hitTimerDelay;
            health = Mathf.Max(health - damage, 0);
        }
    }
}
