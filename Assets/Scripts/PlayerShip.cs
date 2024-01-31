using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    // private members
    private Rigidbody rigidBody;
    private Vector3 position;
    
    // public members
    public float moveSpeed;
    public float bulletSpeed;
    public float horizontalLimit;
    public GameObject bullet;

    public float shootCooldownTime;
    private bool cooldown;
    private float cooldownTimer;

    private MainCamera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.GetComponent<MainCamera>();
        rigidBody = GetComponent<Rigidbody>();
        cooldownTimer = 0.0f;
        cooldown = false;
    }

    void FixedUpdate()
    {
        float axis_raw_input = Input.GetAxisRaw("Horizontal");

        if (Math.Abs(axis_raw_input) > 0.01f)
        {
            float new_x = transform.position.x + axis_raw_input * moveSpeed;
            if(Math.Abs(new_x) < horizontalLimit)
            {
                transform.position = new Vector3(new_x, transform.position.y, transform.position.z);
            }
        }

        // cooldown timer
        if(cooldown)
        {
            cooldownTimer += Time.fixedDeltaTime;
            if(cooldownTimer > shootCooldownTime)
            {
                cooldownTimer = 0.0f;
                cooldown = false;
            }
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !cooldown)
        {
            OnShoot();
        }
    }

    public void OnShoot()
    {
        Vector3 spawnPos = gameObject.transform.position + new Vector3(0.0f, 0.0f, 0.7f);
        // instantiate the Bullet

        GameObject obj = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
        obj.GetComponent<Bullet>().OnShoot(new Vector3(0.0f, 0.0f, 1.5f).normalized, bulletSpeed);

        cooldown = true;
    }

    public void OnDead()
    {
        GameObject.Find("GlobalController").GetComponent<GlobalController>().OnPlayerDead();
        Destroy(gameObject);

        mainCamera.StartShake();
    }
}
