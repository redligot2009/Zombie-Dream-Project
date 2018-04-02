using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class PlayerControls : MonoBehaviour
{

    PhysicsObject po;
    public float jumpSpeed = 8f, moveSpeed = 8f;
    //UnityEngine.Transform anim;
    UnityArmatureComponent anim, leftarm = null, rightarm = null;
    Vector2 input;
    Bone rightshoulder, leftshoulder;
    void Start()
    {
        po = GetComponent<PhysicsObject>();
        //anim = GameObject.Find("anim").GetComponent<UnityEngine.Transform>();
        anim = transform.Find("anim").GetComponent<UnityArmatureComponent>();
        leftarm = anim.transform.Find("leftarm (leftarm)").GetComponent<UnityArmatureComponent>();
        rightarm = anim.transform.Find("rightarm (rightarm)").GetComponent<UnityArmatureComponent>();
        rightshoulder = rightarm.armature.GetBone("right_shoulder");
        leftshoulder = leftarm.armature.GetBone("left_shoulder");
    }

    bool armed = true;

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
        //test gun armature
        if(armed)
        {
            if (leftarm != null)
            {
                if (leftarm.animationName != "armed")
                    leftarm.animation.Play("armed");
            }
            if (rightarm != null)
            {
                if (rightarm.animationName != "armed")
                    rightarm.animation.Play("armed");
            }
            if(Input.GetKey(KeyCode.W))
            {
                leftshoulder.offset.rotation = Mathf.Deg2Rad * -85;
                rightshoulder.offset.rotation = Mathf.Deg2Rad * -45;
            }
            else if(Input.GetKey(KeyCode.S))
            {
                leftshoulder.offset.rotation = Mathf.Deg2Rad * 85;
                leftshoulder.offset.rotation = rightshoulder.offset.rotation = Mathf.Deg2Rad * 45;
            }
            else
            {
                leftshoulder.offset.rotation = rightshoulder.offset.rotation = Mathf.Deg2Rad * 0;
            }
        }
        else
        {
            if (leftarm != null)
            {
                if (leftarm.animationName != "unarmed")
                    leftarm.animation.Play("unarmed");
            }
            if (rightarm != null)
            {
                if (rightarm.animationName != "unarmed")
                    rightarm.animation.Play("unarmed");
            }
            leftshoulder.offset.rotation = rightshoulder.offset.rotation = Mathf.Deg2Rad * 0;
        }
        if(po.velocity.x >= 0)
        {
            anim.armature.flipX = false;
        }
        else
        {
            anim.armature.flipX = true;
        }
        if (po.collisions.below)
        {
            if (Mathf.Abs(po.velocity.x) > 0.5f)
            {
                if (anim.animation.lastAnimationName != "run")
                    anim.animation.FadeIn("run", 0.1f, -1, 0);
            }
            else
            {
                if (anim.animation.lastAnimationName != "idle")
                    anim.animation.FadeIn("idle", 0.1f, -1, 0);
            }
        }
        else
        {
            if (po.velocity.y >= 0)
            {
                if (anim.animation.lastAnimationName != "jump")
                {
                    anim.animation.FadeIn("jump", 0.1f, -1, 0);
                }
            }
            else
            {
                if (anim.animation.lastAnimationName != "fall")
                {
                    anim.animation.FadeIn("fall", 0.1f, -1, 0);
                }
            }
        }
        //physics shit
        po.velocity += Physics2D.gravity * po.gravityModifier * Time.deltaTime;
        po.Move(po.velocity * Time.deltaTime);
    }
}