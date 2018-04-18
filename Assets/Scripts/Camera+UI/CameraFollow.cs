using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public Transform leftBounds;
    public Transform rightBounds;
    public Transform aboveBounds;
    public Transform belowBounds;

    public float smoothDampTime = 0.15f;
   
    private Vector2 smoothDampVelocity = Vector2.zero;

    private float camWidth, camHeight, levelMinX, levelMinY, levelMaxX, levelMaxY;

    void Start ()
    {
        camHeight = Camera.main.orthographicSize * 2;
        camWidth = camHeight * Camera.main.aspect;

        float leftBoundsWidth = leftBounds.GetComponentInChildren<BoxCollider2D>().bounds.size.x / 2f;
        float RightBoundsWidth = rightBounds.GetComponentInChildren<BoxCollider2D>().bounds.size.x / 2f;
        float belowBoundsHeight = belowBounds.GetComponentInChildren<BoxCollider2D>().bounds.size.y / 2f;
        float aboveBoundsHeight = aboveBounds.GetComponentInChildren<BoxCollider2D>().bounds.size.y / 2f;

        levelMinX = leftBounds.position.x + leftBoundsWidth + (camWidth / 2f);
        levelMaxX = rightBounds.position.x - RightBoundsWidth - (camWidth / 2f);
        levelMinY = belowBounds.position.y + belowBoundsHeight + (camHeight / 2f);
        levelMaxY = aboveBounds.position.y - aboveBoundsHeight - (camHeight / 2f);
        if (target)
        {
            float targetX = Mathf.Max(levelMinX, Mathf.Min(levelMaxX, target.position.x));
            float targetY = Mathf.Max(levelMinY, Mathf.Min(levelMaxY, target.position.y));
            transform.position = new Vector3(targetX, targetY, transform.position.z);
        }
    }
	
    public bool InCameraBounds(float x, float y)
    {
        return (x >= transform.position.x - camWidth/2 && x <= transform.position.x + camWidth/2) &&
            (y >= transform.position.y - camHeight/2 && y <= transform.position.y + camHeight/2);
    }

	void Update ()
    {
        if (target)
        {
            float targetX = Mathf.Max(levelMinX, Mathf.Min(levelMaxX, target.position.x));
            float targetY = Mathf.Max(levelMinY, Mathf.Min(levelMaxY, target.position.y));
            // So that the camera follows the player smoothly
            float x = Mathf.SmoothDamp(transform.position.x, targetX, ref smoothDampVelocity.x, smoothDampTime);
            float y = Mathf.SmoothDamp(transform.position.y, targetY, ref smoothDampVelocity.y, smoothDampTime);
            transform.position = new Vector3(x, y, transform.position.z);
        }	
	}
}