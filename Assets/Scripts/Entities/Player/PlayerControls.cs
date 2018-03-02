using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    PhysicsObject po;
    public float jumpSpeed = 8f, moveSpeed = 8f;

    void Start()
    {
        po = GetComponent<PhysicsObject>();
    }

    void Update()
    {
        if (po.collisions.below || po.collisions.above)
        {
            po.velocity.y = 0;
        }
        if (Input.GetKey(KeyCode.A) && !po.collisions.left)
        {
            po.velocity.x = Mathf.Lerp(po.velocity.x, -moveSpeed, Time.deltaTime * 5f);
        }
        else if (Input.GetKey(KeyCode.D) && !po.collisions.right)
        {
            po.velocity.x = Mathf.Lerp(po.velocity.x, moveSpeed, Time.deltaTime * 5f);
        }
        else
        {
            po.velocity.x = Mathf.Lerp(po.velocity.x, 0f, Time.deltaTime * 10f);
        }
        if (Input.GetKey(KeyCode.Space) && po.collisions.below)
        {
            po.velocity.y = jumpSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (po.velocity.y > 0)
            {
                po.velocity.y = po.velocity.y * 0.5f;
            }
        }
        //physics shit
        po.velocity += Physics2D.gravity * po.gravityModifier * Time.deltaTime;
        po.Move(po.velocity * Time.deltaTime);
    }
}