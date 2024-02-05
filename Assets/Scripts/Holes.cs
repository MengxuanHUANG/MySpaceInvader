using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holes : MonoBehaviour
{
    GlobalController globalController;

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
        }
        else if(other.CompareTag("AlienInvader"))
        {
            globalController.IncreasePoint(other.GetComponent<AlienInvader>().point);
        }

        Destroy(other.gameObject);
    }
}
