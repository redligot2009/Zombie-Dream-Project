using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class ZombieAnimation : MonoBehaviour {

    public UnityArmatureComponent anim;
    PhysicsObject po;
    GoombaAI goombaAI;
    Health health;

	void Start () {
        po = GetComponent<PhysicsObject>();
        goombaAI = GetComponent<GoombaAI>();
        health = GetComponent<Health>();
        anim = transform.Find("anim").GetComponent<UnityArmatureComponent>();
        anim.animation.FadeIn("run", 0.1f);
	}
	
	void Update () {
		if(goombaAI.dirx == 1)
        {
            anim.armature.flipX = false;
        }
        else
        {
            anim.armature.flipX = true;
        }
        if (health.dead)
        {
            if (anim.animation.GetState("death") == null)
                anim.animation.FadeIn("death", 0.1f, 1);
        }
        else
        {
            if (health.hitTimer > 0)
            {
                if (anim.animation.GetState("hurt") == null)
                {
                    anim.animation.FadeIn("hurt", 0.1f, 1);
                }
            }
            else
            {
                if (anim.animation.GetState("run") == null)
                {
                    anim.animation.FadeIn("run", 0.25f, -1);
                }
            }
        }
	}
}
