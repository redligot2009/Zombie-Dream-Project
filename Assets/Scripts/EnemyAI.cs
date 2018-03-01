using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int dirx = -1;

    public Vector2 moveSpeed = new Vector2(5,5);
    PhysicsObject po;

    private enum EnemyState
    {
        walking,
        falling,
        dead
    }

    private EnemyState state = EnemyState.falling;
    
	void Start ()
    {
        po = GetComponent<PhysicsObject>();
	}
	
	void Update ()
    {
        if (state != EnemyState.dead)
        {
            Vector3 pos = transform.localPosition;
            Vector3 scale = transform.localScale;
            if(po.grounded) state = EnemyState.walking;
            if (state == EnemyState.falling)
            {
            }
            if (state == EnemyState.walking)
            {
                if (dirx == -1)
                {
                    po.velocity.x = -moveSpeed.x;
                    scale.x = -1;
                }
                else
                {
                    po.velocity.x = moveSpeed.x;
                    scale.x = 1;
                }
            }
            if (po.leftwall) dirx = 1;
            else if (po.rightwall) dirx = -1;

        }
    }
}
