using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int health = 3;
    public bool dead = false;
    public float hitTimer = 0;
    public float hitTimerDelay = 1f;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
        }
        dead = health <= 0;
    }

    public void Hurt(int damage = 1)
    {
        if (hitTimer <= 0)
        {
            hitTimer = hitTimerDelay;
            health = Mathf.Max(health - damage, 0);
        }
    }
}
