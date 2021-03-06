﻿using System.Collections;
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

    public AudioSource audioSource;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("enemy"))
        {
            foreach (var point in collision.contacts)
            {
                Health enemyHealth = point.collider.transform.GetComponent<Health>();
                if (point.collider.GetComponent<BoxCollider2D>().bounds.max.y - 0.25f < po.raycastOrigins.bottomLeft.y)
                {
                    if (!enemyHealth.dead)
                    {
                        po.velocity.y = jumpSpeed;
                        enemyHealth.Hurt();
                    }
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log(LayerMask.LayerToName(collision.gameObject.layer));
        if(collision.gameObject.layer == LayerMask.NameToLayer("enemy"))
        {
            Health enemyHealth = collision.transform.GetComponent<Health>();
            if (collision.collider.GetComponent<BoxCollider2D>().bounds.max.y >= po.raycastOrigins.bottomLeft.y-0.25f)
            {
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
                        if (health.hitTimer <= 0)
                        {
                            hurtSoundAudioSource.Play();
                        }
                        health.Hurt();
                        if (!health.dead)
                            camShake.ShakeCamera(0.1f, 0.1f);
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
    public bool shoot = false, shootDown = false, shootUp = false, reload = false;
    
    public int facing = 1;

    public float shootTimer = 0;
    public float shootDelay = 0.1f;

    public float reloadTime = 1f;
    public float reloadTimer = 0;
    AudioSource reloadSoundAudioSource, hurtSoundAudioSource;

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
        camShake.ShakeCamera(currentWeapon.cameraShakeIntensity, 0.05f);
        if(!audioSource.isPlaying && currentWeapon.loopShot || !currentWeapon.loopShot) audioSource.Play();
    }

    public void SetCurrentWeapon()
    {
        weaponDamage = currentWeapon.damage;
        shootDelay = currentWeapon.shootDelay;
        reloadTime = currentWeapon.reloadTime;
        clipSize = currentWeapon.clipSize;
        weaponRecoil = currentWeapon.recoilVelocity;
        audioSource.clip = currentWeapon.shotSound;
        if (currentWeapon.loopShot) audioSource.loop = true;
    }
    
    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        po = GetComponent<PhysicsObject>();
        health = GetComponent<Health>();
        camShake = Camera.main.transform.GetComponent<CameraShake>();
        audioSource = GetComponent<AudioSource>();
        reloadSoundAudioSource = GameObject.Find("reloadSound").GetComponent<AudioSource>();
        hurtSoundAudioSource = GameObject.Find("hurtSound").GetComponent<AudioSource>();
    }

    void Update()
    {
        //control bindings
        left = /*Input.GetKey(KeyCode.A) ||*/ Input.GetKey(KeyCode.LeftArrow);
        right = /*Input.GetKey(KeyCode.D) ||*/ Input.GetKey(KeyCode.RightArrow);
        jump = /*Input.GetKey(KeyCode.Space) ||*/ Input.GetKey(KeyCode.Z);
        jumprelease = /*Input.GetKeyUp(KeyCode.Space) ||*/ Input.GetKeyUp(KeyCode.Z);
        up = /*Input.GetKey(KeyCode.W) ||*/ Input.GetKey(KeyCode.UpArrow);
        down = /*Input.GetKey(KeyCode.S) ||*/ Input.GetKey(KeyCode.DownArrow);
        shoot = Input.GetKeyDown(KeyCode.X);
        shootDown = Input.GetKey(KeyCode.X);
        shootUp = Input.GetKeyUp(KeyCode.X);
        reload = Input.GetKey(KeyCode.R);
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

            if(currBullet > clipSize || currBullet > 1 && reload)
            {
                currBullet = 1;
                reloadTimer = reloadTime;
                reloadSoundAudioSource.Play();
            }
            if(currentWeapon != null)
            {
                SetCurrentWeapon();
                armed = true;
            }
            if (armed)
            {
                if ((shoot && currentWeapon.triggerType == WeaponObject.TriggerType.MANUAL ||
                    shootDown && currentWeapon.triggerType == WeaponObject.TriggerType.AUTOMATIC)
                    && shootTimer <= 0 && reloadTimer <= 0)
                {
                    shootTimer = shootDelay;
                    //audioSource.clip = currentWeapon.releaseSound;
                    audioSource.volume = 1;
                    Shoot();
                }
                if (!shootDown && currentWeapon.triggerType == WeaponObject.TriggerType.AUTOMATIC || reloadTimer > 0 || health.dead || GameManager.GamePaused)
                {
                    audioSource.volume = Mathf.Lerp(audioSource.volume,0,Time.deltaTime*10f);
                    //audioSource.clip = currentWeapon.releaseSound;
                    //audioSource.PlayOneShot(currentWeapon.releaseSound);
                }
            }

            //timers
            if (shootTimer > 0)
            {
                shootTimer -= Time.deltaTime;
            }
            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;
            }

            /*End Weapons Logic*/

            //hit toxic waste
            bool hitToxicWaste = po.CheckHorizontal(LayerMask.GetMask("hazard"),0.15f,-1) ||
                po.CheckHorizontal(LayerMask.GetMask("hazard"), 0.1f, 1) ||
                po.CheckVertical(LayerMask.GetMask("hazard"),0.1f,1) ||
                po.CheckVertical(LayerMask.GetMask("hazard"), 0.1f, -1);
            if(hitToxicWaste)
            {
                camShake.ShakeCamera(0.1f, 0.1f);
                if(health.hitTimer <= 0 && !health.dead)
                {
                    hurtSoundAudioSource.Play();
                }
                health.Hurt(100);
                po.velocity.y = 0;
            }
            //physics shit
            po.velocity += Physics2D.gravity * po.gravityModifier * Time.deltaTime;
            po.Move(po.velocity * Time.deltaTime);
        }
        else
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0, Time.deltaTime * 10f);
            GameManager.isDead = true;
        }
    }
}