using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeIcons : MonoBehaviour
{
    public GameObject LifeIcon;

    public LinkedList<GameObject> icons;

    public Vector3 startPos;
    public float interval;

    private Text LifeText;

    // Start is called before the first frame update
    void Start()
    {
        icons = new LinkedList<GameObject>();
        LifeText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(int lifeCount)
    {
        while(icons.Count > 0)
        {
            Destroy(icons.Last.Value);
            icons.RemoveLast();
        }
        icons.Clear();

        Vector3 spawn_pos = startPos;
        for (int i = 0; i < lifeCount; i++)
        {
            GameObject obj = Instantiate(LifeIcon, spawn_pos, Quaternion.identity) as GameObject;
            icons.AddLast(obj);
            spawn_pos.x += interval;
        }
        LifeText.text = icons.Count.ToString();
    }

    public void ReduceLife()
    {
        if(icons.Count > 0)
        {
            Destroy(icons.Last.Value);
            icons.RemoveLast();
            LifeText.text = icons.Count.ToString();
        }
    }
}
