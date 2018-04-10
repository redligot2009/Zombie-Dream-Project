using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class PlayerControls : MonoBehaviour
{

    PhysicsObject po;
    public float jumpSpeed = 8f, moveSpeed = 8f;
    Vector2 input;
    public float bounceVelocity = 5f;

    public Health health;
    PlayerAnimation playerAnimation;
    public GameObject bullet;

    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        po = GetComponent<PhysicsObject>();
        health = GetComponent<Health>();
    }

    public bool armed = true;

    //controls
    public bool left = false, right = false, up = false, down = false;
    public bool jump = false, attack = false, jumprelease = false;
    public bool shoot = false;
    
    public int facing = 1;

    public void Shoot()
    {
        GameObject go = Instantiate(bullet);
        go.transform.position = new Vector3(playerAnimation.righthand.transform.position.x, playerAnimation.righthand.transform.position.y);
        go.transform.eulerAngles = new Vector3(0, (facing == 1 ? 0 : 180), -playerAnimation.rightshoulder.offset.rotation * Mathf.Rad2Deg);
    }

    void Update()
    {
        //control bindings
        left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        jump = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Z);
        jumprelease = Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Z);
        up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        shoot = Input.GetKeyDown(KeyCode.X);
        if (!health.dead)
        {
            //hit ground
            if (po.collisions.below || po.collisions.above)
            {
                po.velocity.y = 0;
            }
            if (left && !po.collisions.left)
            {
                po.velocity.x = Mathf.Lerp(po.velocity.x, -moveSpeed, Time.deltaTime * 5f);
            }
            else if (right && !po.collisions.right)
            {
                po.velocity.x = Mathf.Lerp(po.velocity.x, moveSpeed, Time.deltaTime * 5f);
            }
            else
            {
                po.velocity.x = Mathf.Lerp(po.velocity.x, 0f, Time.deltaTime * 10f);
            }
            if (jump && po.collisions.below)
            {
                po.velocity.y = jumpSpeed;
            }
            else if (jumprelease)
            {
                if (po.velocity.y > 0)
                {
                    po.velocity.y = po.velocity.y * 0.65f;
                }
            }

            //shoot logic
            if(shoot)
            {
                Shoot();
            }

            //hit toxic waste
            bool hitToxicWaste = po.CheckHorizontal(LayerMask.GetMask("hazard"),0.15f,-1) ||
                po.CheckHorizontal(LayerMask.GetMask("hazard"), 0.1f, 1) ||
                po.CheckVertical(LayerMask.GetMask("hazard"),0.1f,1) ||
                po.CheckVertical(LayerMask.GetMask("hazard"), 0.1f, -1);
            if(hitToxicWaste)
            {
                health.Hurt(100);
                po.velocity.y = 0;
            }

            //hit enemy
            RaycastHit2D hitEnemy = po.CheckHorizontalHit(LayerMask.GetMask("enemy"));
            RaycastHit2D hitEnemyBounce = po.CheckVerticalHit(LayerMask.GetMask("enemy"),0.22f,-1);

            //hit enemy damage enemy
            if (hitEnemyBounce && !po.collisions.below && (po.coll.bounds.min.y-hitEnemyBounce.transform.position.y) >= po.coll.bounds.extents.y - po.skinDist && po.velocity.y <= 0)
            {
                Health enemyHealth = hitEnemyBounce.transform.GetComponent<Health>();
                if (!enemyHealth.dead)
                {
                    po.velocity.y = jumpSpeed;
                    enemyHealth.Hurt();
                }
            }
                //hit enemy damage player
                if (hitEnemy && hitEnemyBounce.collider == null)
                {
                    Health enemyHealth = hitEnemy.transform.GetComponent<Health>();
                    if (!enemyHealth.dead)
                    {
                        if (health.hitTimer <= 0)
                        {
                            if (hitEnemy.point.x >= transform.position.x)
                            {
                                po.velocity.x = -bounceVelocity;
                            }
                            else
                            {
                                po.velocity.x = bounceVelocity;
                            }
                        }
                        health.Hurt();
                    }
                }

            //physics shit
            po.velocity += Physics2D.gravity * po.gravityModifier * Time.deltaTime;
            po.Move(po.velocity * Time.deltaTime);
        }
        else
        {
        }
    }
}