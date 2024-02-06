using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holes : MonoBehaviour
{
    GlobalController globalController;
    public AudioClip explodeKnell;
    // Start is called before the first frame update
    void Start()
    {
        globalController = transform.parent.GetComponent<GlobalController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            globalController.IncreaseBullet();
            AudioSource.PlayClipAtPoint(explodeKnell, gameObject.transform.position);
        }
        else if(other.CompareTag("AlienInvader"))
        {
            globalController.IncreaseBullet(other.GetComponent<AlienInvader>().point / 10);
            AudioSource.PlayClipAtPoint(explodeKnell, gameObject.transform.position);
        }

        Destroy(other.gameObject);
    }
}
