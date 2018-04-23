using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadUI : MonoBehaviour {

    public Transform target;
    PlayerControls controls;
	void Start () {
        controls = target.GetComponent<PlayerControls>();
	}
	
	void Update () {
        float ratio = Mathf.Max((controls.reloadTime > 0 ? (controls.reloadTimer / controls.reloadTime) : 0),0);
        transform.localScale = new Vector3(ratio,transform.localScale.y);
	}
}
