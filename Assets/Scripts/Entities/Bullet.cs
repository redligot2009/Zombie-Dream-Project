using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public LayerMask tileLayers;
    public float lifeTime = 2f;
    float lifeTimer = 0;
    public float damage = 1;

    CameraFollow cam;

    public float moveSpeed = 5f;

    void Start()
    {
        lifeTimer = lifeTime;
        cam = Camera.main.GetComponent<CameraFollow>();
    }

    bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(Contains(tileLayers, collision.gameObject.layer))
        {
            Destroy(gameObject);
        }
    }
	
	void Update () {
        transform.Translate(new Vector3(moveSpeed * Time.deltaTime,0));
        if (lifeTimer > 0)
            lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0 || !cam.InCameraBounds(transform.position.x, transform.position.y))
        {
            Destroy(gameObject);
        }
	}
}
