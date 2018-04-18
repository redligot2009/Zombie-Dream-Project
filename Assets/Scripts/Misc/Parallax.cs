using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    Vector3 position;
    public Vector2 offset = new Vector3(0,0);
    Transform bg, bg2;
    Sprite bgsprite, bg2sprite;
    BoxCollider2D bgcoll, bg2coll;
    public float parallaxfactor = 0.15f;

    void Start () {
        position = transform.position;
        offset.y = position.y;
        bg = transform.Find("bg");
        bgsprite = bg.GetComponent<Sprite>();
        bgcoll = bg.GetComponent<BoxCollider2D>();
        bg2 = transform.Find("bg2");
        bg2sprite = bg2.GetComponent<Sprite>();
        bg2coll = bg2.GetComponent<BoxCollider2D>();
    }
	void LateUpdate () {
        Vector2 cameraPos = Camera.main.transform.position;
        float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        if (Camera.main != null)
        {
            position.y = Camera.main.transform.position.y + offset.y;
            if (cameraPos.x - cameraHalfWidth < bgcoll.bounds.min.x)
            {
                bg2.transform.position = bg.transform.position - new Vector3(bgcoll.bounds.size.x, 0);
            }
            if (Camera.main.transform.position.x + cameraHalfWidth > bgcoll.bounds.max.x)
            {
                bg2.transform.position = bg.transform.position + new Vector3(bgcoll.bounds.size.x, 0);
            }
            if (Camera.main.transform.position.x - cameraHalfWidth < bg2coll.bounds.min.x)
            {
                bg.transform.position = bg2.transform.position - new Vector3(bg2coll.bounds.size.x, 0);
            }
            if (Camera.main.transform.position.x + cameraHalfWidth > bg2coll.bounds.max.x)
            {
                bg.transform.position = bg2.transform.position + new Vector3(bg2coll.bounds.size.x, 0);
            }
            position.x = cameraPos.x * parallaxfactor;
        }
        transform.position = position;
    }
}
