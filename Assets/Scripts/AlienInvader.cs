using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public class AlienInvader : MonoBehaviour
{
    public int point;
    public float bulletSpeed;

    public GameObject bullet;
    private bool dead;

    public Material DeadMaterial;

    // Start is called before the first frame update
    void Start()
    {
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space)) 
        //{ 
        //    OnShoot();
        //}
    }

    public void OnShoot()
    {
        Vector3 spawnPos = gameObject.transform.position + new Vector3(0.0f, 0.0f, -1.0f);
        GameObject obj = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
        obj.GetComponent<Bullet>().OnShoot(new Vector3(0.0f, 0.0f, -1.0f).normalized, bulletSpeed);
    }

    public void OnDead()
    {
        if(dead) Destroy(gameObject);

        transform.parent.gameObject.GetComponent<InvaderRowController>().InvaderDead();
        transform.parent = null;
        GameObject.Find("GlobalController").GetComponent<GlobalController>().IncreasePoint(point);

        //Destroy(gameObject);

        dead = true;

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | 
            RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

        GetComponent<MeshRenderer>().sharedMaterial = DeadMaterial;

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("Rock"))
        {
            Destroy(collider.gameObject);
        }
        else if(dead && collider.CompareTag("AlienInvader"))
        {
            AlienInvader other = collider.GetComponent<AlienInvader>();
            if (!other.dead)
            {
                other.OnDead();
                Destroy(gameObject);
            }
        }

        if (dead) return;
        if (collider.CompareTag("Base"))
        {
            GameObject.Find("GlobalController").GetComponent<GlobalController>().OnGameOver();
        }
        else if (collider.CompareTag("PlayerShip"))
        {
            collider.gameObject.GetComponent<PlayerShip>().OnDead();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (dead) return;
        if (other.CompareTag("RockBase"))
        {
            Destroy(other.gameObject);
        }
    }
}
