using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public float gravity;
    public Vector2 velocity;
    public bool isWalkingLeft = true;

    public LayerMask floorMask;

    private bool grounded = false;

    private enum EnemyState
    {
        walking,
        falling,
        dead
    }

    private EnemyState state = EnemyState.falling;

	// Use this for initialization
	void Start ()
    {
        enabled = false;

        Fall();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state != EnemyState.dead)
        {
            Vector3 pos = transform.localPosition;
            Vector3 scale = transform.localScale;
<<<<<<< HEAD
            if(po.grounded) state = EnemyState.walking;
            if (state == EnemyState.falling)
            {
            }
            if (state == EnemyState.walking)
            {
                if (dirx == -1)
                {
                    po.velocity.x = -moveSpeed.x;
=======

            if (state == EnemyState.falling)
            {
                pos.y += velocity.y * Time.deltaTime;

                velocity.y -= gravity * Time.deltaTime;
            }
            if (state == EnemyState.walking)
            {
                if (isWalkingLeft)
                {
                    pos.x -= velocity.x * Time.deltaTime;

>>>>>>> c27e5009df2343f121a4837f3a094d4ec7e4db51
                    scale.x = -1;
                }
                else
                {
<<<<<<< HEAD
                    po.velocity.x = moveSpeed.x;
                    scale.x = 1;
                }
            }
            if (po.leftwall) dirx = 1;
            else if (po.rightwall) dirx = -1;

        }
    }
=======
                    pos.x += velocity.x * Time.deltaTime;

                    scale.x = 1;
                }
            }

            if (velocity.y <= 0)
                pos = CheckGround (pos);

            transform.localPosition = pos;
            transform.localScale = scale;
        }
    }
    // i change this to void but it should be vector 3
   Vector3 CheckGround (Vector3 pos)
    {

        Vector2 originLeft = new Vector2(pos.x- 0.5f + 0.2f, pos.y - .5f);
        Vector2 originMiddle = new Vector2(pos.x, pos.y - .5f);
        Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y - .5f);

        RaycastHit2D groundLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D groundMIddle = Physics2D.Raycast(originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D groundRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

        if (groundLeft.collider != null || groundMIddle.collider != null || groundRight.collider != null)
        {
            RaycastHit2D hitRay = groundLeft;

            if (groundLeft)
            {
                hitRay = groundLeft;
            }
            else if (groundMIddle)
            {
                hitRay = groundMIddle;
            }
            else if (groundRight)
            {
                hitRay = groundRight;
            }

            pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + .5f;

            grounded = true;
            velocity.y = 0;
            state = EnemyState.walking;
        }
        else
        {
            if (state != EnemyState.falling)
            {
                Fall();
            }
        }
        return pos;
    }

    void OnBecameVisible()
    {
        enabled = true;    
    }

    void Fall()
    {
        velocity.y = 0;

        state = EnemyState.falling;

        grounded = false;
    }
>>>>>>> c27e5009df2343f121a4837f3a094d4ec7e4db51
}
