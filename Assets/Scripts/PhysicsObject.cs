using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{

    public float gravityModifier = 1f;
    public float minGroundNormalY = 0.35f;
    public bool grounded = false, leftwall = false, rightwall = true, jumped = false;
    protected Vector2 groundNormal;
    public Vector2 velocity;
    protected Rigidbody2D rb2d;
    public float minMoveDistance = 0.001f, skinDist = 0.05f;
    public ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        grounded = false; rightwall = false; leftwall = false;

        Vector2 deltaPos = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move;
        move = moveAlongGround * deltaPos.x;

        Move(move, false);

        move = Vector2.up * deltaPos.y;

        Move(move, true);
    }

    public void Move(Vector2 move, bool yMove)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + skinDist);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }
            if (hitBufferList.Count == 0 && yMove||jumped)
            {
                groundNormal = new Vector2(0, 1);
            }
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currNormal = hitBufferList[i].normal;
                if (currNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMove)
                    {
                        groundNormal = currNormal;
                        currNormal.x = 0;
                    }
                }
                else
                {
                    if (move.x < 0)
                    {
                        leftwall = true;
                        //Debug.Log("leftwall");
                    }
                    else if (move.x > 0)
                    {
                        rightwall = true;
                        //Debug.Log("rightwall");
                    }
                }
                float projection = Vector2.Dot(velocity, currNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currNormal;
                }
                float modifiedDistance = hitBufferList[i].distance - skinDist;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        rb2d.position = (Vector2)rb2d.position + move.normalized * distance;
    }
}
