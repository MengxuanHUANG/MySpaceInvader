using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public class AlienInvader : MonoBehaviour
{
    public int point;
    public float bulletSpeed;

    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        { 
            OnShoot();
        }
    }

    public void OnShoot()
    {
        Vector3 spawnPos = gameObject.transform.position + new Vector3(0.0f, 0.0f, -1.0f);
        GameObject obj = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
        obj.GetComponent<Bullet>().OnShoot(new Vector3(0.0f, 0.0f, -1.0f).normalized, bulletSpeed);
    }

    public void OnDead()
    {
        transform.parent.gameObject.GetComponent<InvaderRowController>().InvaderDead();
        GameObject.Find("GlobalController").GetComponent<GlobalController>().IncreasePoint(point);

        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("Base"))
        {
            GameObject.Find("GlobalController").GetComponent<GlobalController>().OnGameOver();
        }
        else if(collider.CompareTag("RockBase"))
        {
            Destroy(collider.gameObject);
        }
        else if (collider.CompareTag("PlayerShip"))
        {
            collider.gameObject.GetComponent<PlayerShip>().OnDead();
        }
    }
}
