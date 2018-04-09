using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class ZombieAnimation : MonoBehaviour {

    public UnityArmatureComponent anim;
    PhysicsObject po;
    GoombaAI goombaAI;

	void Start () {
        po = GetComponent<PhysicsObject>();
        goombaAI = GetComponent<GoombaAI>();
        anim = transform.Find("anim").GetComponent<UnityArmatureComponent>();
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
	}
}
