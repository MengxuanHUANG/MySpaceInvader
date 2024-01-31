using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigidBody;

    public AudioClip explodeKnell;
    public GameObject explodeEffect;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnShoot(Vector3 direction, float force, float lifeTime = 4.0f)
    {
        rigidBody.AddForce(direction * force);

        Invoke("OnDead", lifeTime);
    }

    private void OnDead()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if(collider.CompareTag("Bullet"))
        {
            Destroy(collider.gameObject);
        }
        else if (collider.CompareTag("PlayerShip"))
        {
            collider.gameObject.GetComponent<PlayerShip>().OnDead();
        }
        else if(collider.CompareTag("AlienInvader"))
        {
            collider.gameObject.GetComponent<AlienInvader>().OnDead();
        }
        else if(collider.CompareTag("Rock"))
        {
            Destroy(collider.gameObject);
        }
        AudioSource.PlayClipAtPoint(explodeKnell, gameObject.transform.position);
        Instantiate(explodeEffect, gameObject.transform.position, Quaternion.AngleAxis(-90, Vector3.right));

        Destroy(gameObject);
    }
}
