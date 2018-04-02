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

    //controls
    bool left = false, right = false, up = false, down = false;
    bool jump = false, attack = false, jumprelease = false;
    void Update()
    {
        //control bindings
        left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        jump = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Z);
        jumprelease = Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Z);
        up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        //hit ground
        if (po.collisions.below || po.collisions.above)
        {
            po.velocity.y = 0;
        }
        if (left && !po.collisions.left)
        {
            po.velocity.x = Mathf.Lerp(po.velocity.x, -moveSpeed, Time.deltaTime * 5f);
        }
        else if (right && !po.collisions.right)
        {
            po.velocity.x = Mathf.Lerp(po.velocity.x, moveSpeed, Time.deltaTime * 5f);
        }
        else
        {
            po.velocity.x = Mathf.Lerp(po.velocity.x, 0f, Time.deltaTime * 10f);
        }
        if (jump && po.collisions.below)
        {
            po.velocity.y = jumpSpeed;
        }
        else if (jumprelease)
        {
            if (po.velocity.y > 0)
            {
                po.velocity.y = po.velocity.y * 0.65f;
            }
        }
        //test gun armature
        if (anim.animation.animations.Count > 0)
        {
            if (armed)
            {
                if (leftarm != null)
                {
                    if (!leftarm.animation.isPlaying && leftarm.animationName != "armed")
                        leftarm.animation.Play("armed");
                }
                if (rightarm != null)
                {
                    if (!rightarm.animation.isPlaying && rightarm.animationName != "armed")
                        rightarm.animation.Play("armed");
                }
                if (leftshoulder != null && rightshoulder != null)
                {
                    if (up)
                    {
                        leftshoulder.offset.rotation = Mathf.Deg2Rad * -85;
                        rightshoulder.offset.rotation = Mathf.Deg2Rad * -45;
                    }
                    else if (down)
                    {
                        leftshoulder.offset.rotation = Mathf.Deg2Rad * 85;
                        leftshoulder.offset.rotation = rightshoulder.offset.rotation = Mathf.Deg2Rad * 45;
                    }
                    else
                    {
                        leftshoulder.offset.rotation = rightshoulder.offset.rotation = Mathf.Deg2Rad * 0;
                    }
                }
            }
            else
            {
                if (leftarm != null)
                {
                    if (!leftarm.animation.isPlaying && leftarm.animationName != "armed")
                        leftarm.animation.Play("unarmed");
                }
                if (rightarm != null)
                {
                    if (!rightarm.animation.isPlaying && rightarm.animationName != "armed")
                        rightarm.animation.Play("unarmed");
                }
                if(leftshoulder != null && rightshoulder != null)
                    leftshoulder.offset.rotation = rightshoulder.offset.rotation = Mathf.Deg2Rad * 0;
            }
            if (po.velocity.x >= 0)
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
                    {
                        anim.animation.FadeIn("run", 0.1f, -1, 0);
                    }
                }
                else
                {
                    if (anim.animation.lastAnimationName != "idle")
                    {
                        anim.animation.FadeIn("idle", 0.1f, -1, 0);
                    }
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
        }
        //physics shit
        po.velocity += Physics2D.gravity * po.gravityModifier * Time.deltaTime;
        po.Move(po.velocity * Time.deltaTime);
    }
}