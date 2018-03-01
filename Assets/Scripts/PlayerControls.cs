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

        if (Input.GetKey(KeyCode.A) && !po.leftwall)
        {
            po.velocity.x = Mathf.Lerp(po.velocity.x, -moveSpeed, Time.deltaTime * 5f);
        }
        else if (Input.GetKey(KeyCode.D) && !po.rightwall)
        {
            po.velocity.x = Mathf.Lerp(po.velocity.x, moveSpeed, Time.deltaTime * 5f);
        }
        else
        {
            po.velocity.x = Mathf.Lerp(po.velocity.x, 0f, Time.deltaTime * 10f);
        }
        if (po.grounded && !Input.GetKey(KeyCode.Space)) po.jumped = false;
        else if (Input.GetKey(KeyCode.Space) && po.grounded)
        {
            po.jumped = true;
            po.velocity.y = jumpSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (po.velocity.y > 0)
            {
                po.velocity.y = po.velocity.y * 0.75f;
            }
        }
    }
}
