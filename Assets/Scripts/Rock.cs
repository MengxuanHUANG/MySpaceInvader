using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public int width;
    public int height;

    public Vector3 initOffset;

    public GameObject rockObj;
    public List<GameObject> rocks;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        InitRocks();
    }
    private void InitRocks()
    {
        Vector3 pos = transform.position + initOffset;

        foreach(GameObject rock in rocks)
        {
            Destroy(rock);
        }

        rocks.Clear();

        for (int i = 0; i < height; ++i)
        {
            Vector3 local_pos = pos;
            for(int j = 0; j < width; ++j) 
            {
                GameObject obj = Instantiate(rockObj, local_pos, Quaternion.identity) as GameObject;
                obj.transform.parent = transform;
                rocks.Add(obj);
                local_pos.x += 0.1f;
            }
            pos.z += 0.1f;
        }
    }
}
