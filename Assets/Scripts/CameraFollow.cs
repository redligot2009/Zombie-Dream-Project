using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class CameraFollow : MonoBehaviour
{

    public GameObject player;

    Rigidbody2D rb2d, player_rb2d;

    // Use this for initialization

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        player_rb2d = player.GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame

    void LateUpdate()
    {
        //float y = Mathf.Clamp(player.transform.position.x, yMin, yMax);

       transform.position = Vector3.Lerp(transform.position,new Vector3(player_rb2d.transform.position.x, player_rb2d.transform.position.y,transform.position.z),Time.deltaTime*10f);

    }

}