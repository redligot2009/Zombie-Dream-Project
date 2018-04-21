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

    public WeaponObject currentWeapon;

    CameraShake camShake;

    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        po = GetComponent<PhysicsObject>();
        health = GetComponent<Health>();
        camShake = Camera.main.transform.GetComponent<CameraShake>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log(LayerMask.LayerToName(collision.gameObject.layer));
        if(collision.gameObject.layer == LayerMask.NameToLayer("enemy"))
        {
            if (collision.collider.GetComponent<BoxCollider2D>().bounds.max.y >= po.raycastOrigins.bottomLeft.y-0.15f)
            {
                Health enemyHealth = collision.transform.GetComponent<Health>();
                if (!enemyHealth.dead)
                {
                    if (health.hitTimer <= 0)
                    {
                        if (collision.transform.position.x >= transform.position.x)
                        {
                            po.velocity.x = -bounceVelocity;
                        }
                        else
                        {
                            po.velocity.x = bounceVelocity;
                        }
                        health.Hurt();
                        if (!health.dead)
                            camShake.ShakeCamera(1f, 0.05f);
                    }
                }
            }
        }
    }

    public bool armed = true;

    //controls
    [HideInInspector]
    public bool left = false, right = false, up = false, down = false;
    [HideInInspector]
    public bool jump = false, attack = false, jumprelease = false;
    [HideInInspector]
    public bool shoot = false, shootDown = false;
    
    public int facing = 1;

    public float shootTimer = 0;
    public float shootDelay = 0.1f;
    public float reloadTime = 1f;
    public float reloadTimer = 0;
    public float weaponDamage = 1f;
    public float weaponRecoil = 0f;
    public int clipSize = 6;
    public int currBullet = 1;
    Vector2 recoilVelocity = Vector2.zero;

    public void Shoot()
    {
        GameObject go = Instantiate(bullet);
        Bullet _bullet = go.GetComponent<Bullet>();
        _bullet.damage = weaponDamage;
        go.transform.position = new Vector3(playerAnimation.righthand.transform.position.x, playerAnimation.righthand.transform.position.y);
        go.transform.eulerAngles = new Vector3(0, (facing == 1 ? 0 : 180), -playerAnimation.rightshoulder.offset.rotation * Mathf.Rad2Deg);
        go.transform.position += 0.33f * go.transform.up;
        go.transform.position += go.transform.right*0.75f;
        currBullet++;
        po.velocity.x -= facing * weaponRecoil * (Mathf.Abs(po.velocity.x) > 2f ? 0.5f : 1);
        if (camShake.shakeDuration <= 0.01f)
        {
            camShake.ShakeCamera(currentWeapon.cameraShakeIntensity, 0.005f);
        }
    }

    void SetCurrentWeapon()
    {
        shootDelay = currentWeapon.shootDelay;
        reloadTime = currentWeapon.reloadTime;
        clipSize = currentWeapon.clipSize;
        weaponRecoil = currentWeapon.recoilVelocity;
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
        shootDown = Input.GetKey(KeyCode.X);
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
                po.velocity.x = Mathf.SmoothStep(po.velocity.x, 0f, Time.deltaTime * 10f);
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
            /*Weapons Logic*/

            //timers
            if (shootTimer > 0)
            {
                shootTimer -= Time.deltaTime;
            }
            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;
            }
            if(currBullet > clipSize)
            {
                currBullet = 1;
                reloadTimer = reloadTime;
            }
            if(currentWeapon != null)
            {
                SetCurrentWeapon();
                armed = true;
            }
            if (armed)
            {
                if ((shoot&&currentWeapon.triggerType == WeaponObject.TriggerType.MANUAL || 
                    shootDown && currentWeapon.triggerType == WeaponObject.TriggerType.AUTOMATIC) 
                    && shootTimer <= 0 && reloadTimer <= 0)
                {
                    shootTimer = shootDelay;
                    Shoot();
                }
            }

            /*End Weapons Logic*/

            //hit toxic waste
            bool hitToxicWaste = po.CheckHorizontal(LayerMask.GetMask("hazard"),0.15f,-1) ||
                po.CheckHorizontal(LayerMask.GetMask("hazard"), 0.1f, 1) ||
                po.CheckVertical(LayerMask.GetMask("hazard"),0.1f,1) ||
                po.CheckVertical(LayerMask.GetMask("hazard"), 0.1f, -1);
            if(hitToxicWaste)
            {
                camShake.ShakeCamera(0.5f, 0.005f);
                health.Hurt(100);
                po.velocity.y = 0;
            }

            //hit enemy
            RaycastHit2D hitEnemy = po.CheckHorizontalHit(LayerMask.GetMask("enemy"),0,1);
            if (!hitEnemy) hitEnemy = po.CheckHorizontalHit(LayerMask.GetMask("enemy"), -1);
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
            //physics shit
            po.velocity += Physics2D.gravity * po.gravityModifier * Time.deltaTime;
            po.Move(po.velocity * Time.deltaTime);
        }
        else
        {
            GameManager.isDead = true;
        }
    }
}