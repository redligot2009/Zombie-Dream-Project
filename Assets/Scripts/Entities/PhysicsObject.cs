using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PhysicsObject : MonoBehaviour
{
    /*movement stuff*/
    public float gravityModifier = 1f;
    public Vector2 velocity;

    /*box collider*/
    [HideInInspector]
    public BoxCollider2D coll;

    /*slope stuff*/
    public float maxClimbAngle = 80, maxDescendAngle = 80;

    /*ray spacing + collision stuff*/
    public LayerMask collisionMask;
    public float skinDist = 0.05f;
    public float hopThreshold = 0.05f;
    public int horizontalRayCount = 4, verticalRayCount = 4;
    float horizontalRaySpacing, verticalRaySpacing;
    public RayCastOrigins raycastOrigins;
    public CollisionInfo collisions;

    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void Move(Vector2 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        /*
         * Call DescendSlope if falling and
         * before correcting horizontal and 
         * vertical collisions.
         */
        if(velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    public bool CheckHorizontal(LayerMask layer, float padding = 0.0f, float dirx=0)
    {
        float directionX = (dirx==0?Mathf.Sign(velocity.x):dirx);
        float rayLength = coll.bounds.extents.x + skinDist + padding;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = new Vector2(transform.position.x,raycastOrigins.bottomLeft.y);
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, layer);
            Debug.DrawLine(rayOrigin, rayOrigin+new Vector2(0,0.1f),Color.blue);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            if (hit) return true;
        }
        return false;
    }

    public RaycastHit2D CheckHorizontalHit(LayerMask layer, float padding = 0.0f, float dirx=0)
    {
        float directionX = (dirx==0?Mathf.Sign(velocity.x):dirx);
        float rayLength = skinDist + padding;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, layer);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            if (hit) return hit;
        }
        return new RaycastHit2D();
    }

    public bool CheckVertical(LayerMask layer, float padding = 0.0f, float diry = 0)
    {
        float directionY = (diry != 0?Mathf.Sign(velocity.y):diry);
        float rayLength = skinDist + padding;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, layer);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
            if (hit) return true;
        }
        return false;
    }
    public RaycastHit2D CheckVerticalHit(LayerMask layer, float padding = 0.0f, float dirx = 0)
    {
        float directionY = (dirx==0?Mathf.Sign(velocity.y):dirx);
        float rayLength = skinDist + padding;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, layer);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
            if (hit) return hit;
        }
        return new RaycastHit2D();
    }

    public RaycastHit2D CheckBoxHit(LayerMask layer, float padding = 0.0f)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, coll.bounds.size, 0, Vector2.one ,padding,layer);
        if (hit) return hit;
        return new RaycastHit2D();
    }

    void HorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinDist;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    float distanceToSlopeStart = 0;
                    /*If found a new slope*/
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        /*correct velocity.x so that object is
                         correctly aligned along slope on x-axis
                         */
                        distanceToSlopeStart = hit.distance - skinDist;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }
                //check if wall height is too small
                float wallHeight = Mathf.Abs(raycastOrigins.bottomLeft.y - hit.collider.bounds.max.y);
                if (wallHeight >= hopThreshold && (!collisions.climbingSlope && slopeAngle > maxClimbAngle || slopeAngle > maxClimbAngle))
                {
                    velocity.x = (hit.distance - skinDist) * directionX;
                    rayLength = hit.distance;
                    /*stop bouncing up when pressed against 
                    * wall while on slope
                    */
                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinDist;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinDist) * directionY;
                rayLength = hit.distance;

                /*stop bouncing up when pressed against 
                * ceiling while on slope
                */
                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinDist;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinDist) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }
    void ClimbSlope(ref Vector2 velocity, float slopeAngle)
    {
        /*
         * Warning: trigonometry
         * remember SOHCAHTOA
         */
        float moveDistance = Mathf.Abs(velocity.x);
        /* Given move distance,
         * Find y component of final
         * move vector based on ff. formula:
         * sin(angle) = y/d
         * where y = y-component and d = moveDistance
         * by moving stuff around you get the following:
         * 1/sin(angle) = d/y
         * until finally
         * y = d * sin(angle)
        */
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        /* do ff. only if not jumping
         */
        if (velocity.y <= climbVelocityY)
        {
            //modify velocity.y based on slopeAngle
            velocity.y = climbVelocityY;
            //modify velocity.x based on slopeAngle
            /* Given move distance,
             * Find x-component of final
             * move vector based on ff. formula:
             * cos(angle) = x/d
             * where x = x-component and d = moveDistance
             * by moving stuff around you get the following:
             * 1/cos(angle) = d/x
             * until finally
             * x = d * sin(angle)
            */
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        //set ray origin according to direction
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinDist <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }


    /*
     * Finds new bottomleft, bottomright,
     * topleft, and topright points of collider.
     */
    void UpdateRaycastOrigins()
    {
        Bounds bounds = coll.bounds;
        //Gives collision a little bit of space
        bounds.Expand(skinDist * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = coll.bounds;
        bounds.Expand(skinDist * -2f);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RayCastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool climbingSlope, descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
