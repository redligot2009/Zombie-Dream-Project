using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class PlayerAnimation : MonoBehaviour {
    
    PhysicsObject po;
    [HideInInspector]
    public UnityArmatureComponent anim, leftarm = null, rightarm = null;
    //UnityArmatureComponent righthand;
    Vector2 input;
    [HideInInspector]
    public Bone rightshoulder;
    [HideInInspector]
    public Bone leftshoulder;
    [HideInInspector]
    public Bone weaponend;
    public UnityEngine.Transform righthand;
    PlayerControls controls;
    MeshRenderer[] renderers;
    public Color hurtTint = Color.red;

    void Start ()
    {
        po = GetComponent<PhysicsObject>();
        controls = GetComponent<PlayerControls>();
        //anim = GameObject.Find("anim").GetComponent<UnityEngine.Transform>();
        anim = transform.Find("anim").GetComponent<UnityArmatureComponent>();
        leftarm = anim.transform.Find("leftarm (leftarm)").GetComponent<UnityArmatureComponent>();
        rightarm = anim.transform.Find("rightarm (rightarm)").GetComponent<UnityArmatureComponent>();
        rightshoulder = rightarm.armature.GetBone("right_shoulder");
        leftshoulder = leftarm.armature.GetBone("left_shoulder");
        weaponend = rightshoulder.armature.GetBone("arm").armature.GetBone("weaponend");
        righthand = rightarm.transform.Find("left hand").GetComponent<UnityEngine.Transform>();
        renderers = anim.GetComponentsInChildren<MeshRenderer>();

    }
	
    void FadeAnimation(string name, UnityArmatureComponent obj=null, int layer=0, float fadeTime=0.1f,int playTimes=-1)
    {
        if (obj == null) obj = anim;
        if (obj != null)
        {
            if (obj.animation.GetState(name) == null)
            {
                obj.animation.FadeIn(name, fadeTime, playTimes, layer);
            }
        }
    }

	void Update ()
    {
        if (controls.health.hitTimer > 0)
        {
            foreach (var renderer in renderers)
            {
                Color c = hurtTint;
                renderer.material.color = Color.Lerp(renderer.material.color,c,Time.deltaTime*5f);
            }
        }
        else
        {
            foreach (var renderer in renderers)
            {
                Color c = Color.white;
                renderer.material.color = Color.Lerp(renderer.material.color, c, Time.deltaTime * 5f);
            }
        }
        //gun armature
        if (!controls.health.dead)
        {
            if (anim.animation.animations.Count > 0)
            {
                if (controls.armed)
                {
                    if (leftarm.animation.isCompleted && leftarm.animationName != "armed")
                        leftarm.animation.Play("armed");
                    if (rightarm.animation.isCompleted && rightarm.animationName != "armed")
                        rightarm.animation.Play("armed");
                    //shoot logic
                    if (controls.shoot)
                    {
                        FadeAnimation("shoot", rightarm, 1, 0.05f,1);
                        FadeAnimation("shoot", leftarm, 1, 0.05f,1);
                    }
                    //shoot rotation logic
                    if (leftshoulder != null && rightshoulder != null)
                    {
                        if (controls.up)
                        {
                            leftshoulder.offset.rotation = Mathf.Deg2Rad * -85;
                            rightshoulder.offset.rotation = Mathf.Deg2Rad * -45;
                        }
                        else if (controls.down)
                        {
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
                    if (leftshoulder != null && rightshoulder != null)
                        leftshoulder.offset.rotation = rightshoulder.offset.rotation = Mathf.Deg2Rad * 0;
                }
                if (po.velocity.x > 0 && controls.right)
                {
                    anim.armature.flipX = false;
                    controls.facing = 1;
                }
                else if(po.velocity.x < 0 && controls.left)
                {
                    anim.armature.flipX = true;
                    controls.facing = -1;
                }
                //animations player
                if (po.collisions.below)
                {
                    //run
                    if (Mathf.Abs(po.velocity.x) > 0.5f)
                    {
                        FadeAnimation("run");
                    }
                    else
                    {
                        //idle
                        FadeAnimation("idle");
                    }
                }
                else
                {
                    if (po.velocity.y >= 0)
                    {
                        //jump
                        FadeAnimation("jump");
                    }
                    else
                    {
                        //fall
                        FadeAnimation("fall");
                    }
                }
            }
        }
        else
        {
            FadeAnimation("death",anim,0,0f,1);
            leftshoulder.offset.rotation = rightshoulder.offset.rotation = Mathf.Deg2Rad * 0;
            FadeAnimation("unarmed", leftarm, 1, 0.1f, 1);
            FadeAnimation("unarmed", rightarm, 1, 0.1f, 1);
        }
    }
}
