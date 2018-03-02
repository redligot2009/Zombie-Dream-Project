using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    
    PhysicsObject po;
    Animator anim;
    Transform animobj;

	void Start () {
        po = GetComponent<PhysicsObject>();
        animobj = transform.Find("animations");
        anim = animobj.GetComponent<Animator>();
	}
	
	void Update ()
    {
        anim.SetFloat("xspeed", Mathf.Abs(po.velocity.x));
        anim.SetFloat("yspeed", Mathf.Abs(po.velocity.y));
	}
}
