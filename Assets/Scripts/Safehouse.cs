using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Rigidbody2D))]
public class Safehouse : MonoBehaviour {

    public string nextSceneName;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
