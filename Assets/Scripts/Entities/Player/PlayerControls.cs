using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class PlayerControls : MonoBehaviour
{

    PhysicsObject po;
    public float jumpSpeed = 8f, moveSpeed = 8f;
    //UnityEngine.Transform anim;
    UnityArmatureComponent anim, leftarm = null, rightarm = null;
    //UnityArmatureComponent righthand;
    Vector2 input;
    Bone rightshoulder, leftshoulder;
    Bone weaponend;

    public float bounceVelocity = 5f;

    public Health health;

    public GameObject bullet;

    void Start()
    {
        po = GetComponent<PhysicsObject>();
        //anim = GameObject.Find("anim").GetComponent<UnityEngine.Transform>();
        anim = transform.Find("anim").GetComponent<UnityArmatureComponent>();
        leftarm = anim.transform.Find("leftarm (leftarm)").GetComponent<UnityArmatureComponent>();
        rightarm = anim.transform.Find("rightarm (rightarm)").GetComponent<UnityArmatureComponent>();
        rightshoulder = rightarm.armature.GetBone("right_shoulder");
        leftshoulder = leftarm.armature.GetBone("left_shoulder");
        weaponend = rightshoulder.armature.GetBone("arm").armature.GetBone("weaponend");
        health = GetComponent<Health>();
    }

    bool armed = true;

    //controls
    bool left = false, right = false, up = false, down = false;
    bool jump = false, attack = false, jumprelease = false;
    bool shoot = false;

    void Shoot()
    {
        GameObject go = Instantiate(bullet);
        go.transform.position = new Vector3(transform.position.x + facing * (po.coll.bounds.size.x / 2), leftarm.transform.position.y);
        go.transform.eulerAngles = new Vector3(0, (facing == 1 ? 0 : 180), -rightshoulder.offset.rotation * Mathf.Rad2Deg);
    }

    int facing = 1;

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
            //gun armature
            if (anim.animation.animations.Count > 0)
            {
                if (armed)
                {
                    if (leftarm != null)
                    {
                        if (leftarm.animation.isCompleted && leftarm.animationName != "armed")
                            leftarm.animation.Play("armed");
                    }
                    if (rightarm != null)
                    {
                        if (rightarm.animation.isCompleted && rightarm.animationName != "armed")
                            rightarm.animation.Play("armed");
                    }
                    //shoot logic
                    if (shoot)
                    {
                        Shoot();
                        if(rightarm.animationName != "shoot")
                            rightarm.animation.FadeIn("shoot",0.05f,1);
                        if(leftarm.animationName != "shoot")
                            leftarm.animation.FadeIn("shoot",0.05f,1);
                    }
                    if (leftshoulder != null && rightshoulder != null)
                    {
                        if (up)
                        {
                            leftshoulder.offset.rotation = Mathf.Deg2Rad * -85;
                            rightshoulder.offset.rotation = Mathf.Deg2Rad * -45;
                        }
                        else if (down)
                        {
                            leftshoulder.offset.rotation = Mathf.Deg2Rad * 85;
                            leftshoulder.offset.rotation = rightshoulder.offset.rotation = Mathf.Deg2Rad * 45;
                        }
                        else
                        {
                            leftshoulder.offset.rotation = rightshoulder.offset.rotation = Mathf.Deg2Rad * 0;
                        }
                    }
                }
                else
                {
                    if (leftarm != null)
                    {
                        if (!leftarm.animation.isPlaying && leftarm.animationName != "armed")
                            leftarm.animation.Play("unarmed");
                    }
                    if (rightarm != null)
                    {
                        if (!rightarm.animation.isPlaying && rightarm.animationName != "armed")
                            rightarm.animation.Play("unarmed");
                    }
                    if (leftshoulder != null && rightshoulder != null)
                        leftshoulder.offset.rotation = rightshoulder.offset.rotation = Mathf.Deg2Rad * 0;
                }
                if (po.velocity.x >= 0)
                {
                    anim.armature.flipX = false;
                    facing = 1;
                }
                else
                {
                    anim.armature.flipX = true;
                    facing = -1;
                }
                //animations player
                if (po.collisions.below)
                {
                    //run
                    if (Mathf.Abs(po.velocity.x) > 0.5f)
                    {
                        if (anim.animation.lastAnimationName != "run")
                        {
                            anim.animation.FadeIn("run", 0.1f, -1, 0);
                        }
                    }
                    else
                    {
                    //idle
                        if (anim.animation.lastAnimationName != "idle")
                        {
                            anim.animation.FadeIn("idle", 0.1f, -1, 0);
                        }
                    }
                }
                else
                {
                    if (po.velocity.y >= 0)
                    {
                    //jump
                        if (anim.animation.lastAnimationName != "jump")
                        {
                            anim.animation.FadeIn("jump", 0.1f, -1, 0);
                        }
                    }
                    else
                    {
                    //fall
                        if (anim.animation.lastAnimationName != "fall")
                        {
                            anim.animation.FadeIn("fall", 0.1f, -1, 0);
                        }
                    }
                }
            }
            //hit toxic waste

            bool hitToxicWaste = po.CheckHorizontal(LayerMask.GetMask("hazard")) || po.CheckVertical(LayerMask.GetMask("hazard"));

            if(hitToxicWaste)
            {
                health.Hurt(100);
            }

            //hit enemy

            RaycastHit2D hitEnemy = po.CheckHorizontalHit(LayerMask.GetMask("enemy"));
            RaycastHit2D hitEnemyBounce = po.CheckVerticalHit(LayerMask.GetMask("enemy"),0.15f);

            //hit enemy damage player
            if(hitEnemy)
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
            else
            {
                //hit enemy damage enemy
                if (hitEnemyBounce && !po.collisions.below)
                {
                    Health enemyHealth = hitEnemyBounce.transform.GetComponent<Health>();
                    if (!enemyHealth.dead)
                    {
                        po.velocity.y = jumpSpeed;
                    }
                }
            }

            //physics shit
            po.velocity += Physics2D.gravity * po.gravityModifier * Time.deltaTime;
            po.Move(po.velocity * Time.deltaTime);
        }
        else
        {
            leftarm.animation.FadeIn("unarmed",0.1f);
            rightarm.animation.FadeIn("unarmed",0.1f);
        }
    }
}