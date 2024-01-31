using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodeEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Dead", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}
