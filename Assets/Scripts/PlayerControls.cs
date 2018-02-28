﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    PhysicsObject physObj;
    public float jumpSpeed = 8f, moveSpeed = 8f;

    void Start()
    {
        physObj = GetComponent<PhysicsObject>();
    }
    int grounds = 0;
    void Update()
    {

        if (Input.GetKey(KeyCode.A) && !physObj.leftwall)
        {
            physObj.velocity.x = Mathf.Lerp(physObj.velocity.x, -moveSpeed, Time.deltaTime * 5f);
        }
        else if (Input.GetKey(KeyCode.D) && !physObj.rightwall)
        {
            physObj.velocity.x = Mathf.Lerp(physObj.velocity.x, moveSpeed, Time.deltaTime * 5f);
        }
        else
        {
            physObj.velocity.x = Mathf.Lerp(physObj.velocity.x, 0f, Time.deltaTime * 10f);
        }
        if (physObj.grounded && !Input.GetKey(KeyCode.Space)) physObj.jumped = false;
        else if (Input.GetKey(KeyCode.Space) && physObj.grounded)
        {
            physObj.jumped = true;
            physObj.velocity.y = jumpSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (physObj.velocity.y > 0)
            {
                physObj.velocity.y = physObj.velocity.y * 0.5f;
            }
        }
        if (physObj.grounded) grounds = 0;
        if (!physObj.grounded) grounds += 1;
        if (grounds == 1 || physObj.velocity.y > 0 && !physObj.jumped && !physObj.grounded) physObj.Move(new Vector2(0, 4f * Time.deltaTime * -1f), true);
    }
}
