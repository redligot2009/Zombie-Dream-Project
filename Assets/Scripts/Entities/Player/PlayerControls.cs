using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class PlayerControls : MonoBehaviour
{

    PhysicsObject po;
    public float jumpSpeed = 8f, moveSpeed = 8f;
    //UnityEngine.Transform anim;
    UnityArmatureComponent anim;

    void Start()
    {
        po = GetComponent<PhysicsObject>();
        //anim = GameObject.Find("anim").GetComponent<UnityEngine.Transform>();
        anim = GameObject.Find("anim").GetComponent<UnityArmatureComponent>();
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
                po.velocity.y = po.velocity.y * 0.65f;
            }
        }
        /*if(po.velocity.x > 0)
        {
            anim.eulerAngles = new Vector3(0, 0, 0);
        }
        else if(po.velocity.x < 0)
        {
            anim.eulerAngles = new Vector3(0, 180, 0);
        }*/
        if(po.velocity.x >= 0)
        {
            anim.armature.flipX = false;
        }
        else
        {
            anim.armature.flipX = true;
        }
        if(Mathf.Abs(po.velocity.x) > 0.5f)
        {
            if(anim.animation.lastAnimationName != "run")
                anim.animation.FadeIn("run",0.1f,-1,0);
        }
        else
        {
            if (anim.animation.lastAnimationName != "idle")
                anim.animation.FadeIn("idle", 0.1f, -1, 0);
        }
        //physics shit
        po.velocity += Physics2D.gravity * po.gravityModifier * Time.deltaTime;
        po.Move(po.velocity * Time.deltaTime);
    }
}