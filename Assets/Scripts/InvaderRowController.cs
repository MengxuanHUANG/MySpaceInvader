using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class InvaderRowController : MonoBehaviour
{
    public GlobalController globalController;

    private Vector3 initPosition;

    public int invaderCount;
    public float invaderStep;

    public float verticalGap;

    public float horizontalLimit;
    public float moveLimit;

    public GameObject[] alienInvaders;
    public List<LinkedList<GameObject>> invadersLinkedLists;

    public List<float> leftLimits;
    public List<float> rightLimits;

    public float leftMax;
    public float rightMax;

    public float direction;
    public float moveStep;
    
    private float moveTimer;
    private float moveGap;

    public int remainInvaders;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;

        globalController = GameObject.Find("GlobalController").GetComponent<GlobalController>();

        invadersLinkedLists = new List<LinkedList<GameObject>>();

        for (int i = 0; i < alienInvaders.Length; i++)
        {
            invadersLinkedLists.Add(new LinkedList<GameObject>());
        }
        
        Reset();

        moveGap = 0.8f;
        direction = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(remainInvaders <= 0)
        {
            globalController.OnGameWin();
        }
    }

    private void FixedUpdate()
    {
        if(!globalController.isGameOver)
        {
            CheckBoundary();

            moveTimer += globalController.alienMoveSpeed * Time.fixedDeltaTime;

            float scalar = (float)remainInvaders / (float)(alienInvaders.Length * invaderCount);

            if (moveTimer >= moveGap * scalar)
            {
                Move();
                moveTimer = 0;
            }
        }
    }

    private void Move()
    {
        float new_x = transform.position.x + direction * moveStep;
        float new_z = transform.position.z;
        if (new_x < leftMax || new_x > rightMax)
        {
            direction = -direction;
            new_z -= verticalGap * 0.5f;
        }
        transform.position = new Vector3(transform.position.x + direction * moveStep, transform.position.y, new_z);
    }

    private void CheckBoundary()
    {
        float left_max = -1000.0f;
        float right_max = 1000.0f;
        for (int i = 0; i < alienInvaders.Length; ++i)
        {
            LinkedList<GameObject> invadersLinkedList = invadersLinkedLists[i];

            if (invadersLinkedList.Count > 0 && invadersLinkedList.First.Value == null)
            {
                invadersLinkedList.RemoveFirst();
                leftLimits[i] -= invaderStep;
            }

            if (invadersLinkedList.Count > 0 && invadersLinkedList.Last.Value == null)
            {
                invadersLinkedList.RemoveLast();
                rightLimits[i] += invaderStep;
            }
            left_max = Mathf.Max(left_max, leftLimits[i]);
            right_max = Mathf.Min(right_max, rightLimits[i]);
        }

        leftMax = left_max;
        rightMax = right_max;
    }

    public void Reset()
    {
        transform.position = initPosition;

        leftMax = -moveLimit;
        rightMax = moveLimit;

        invaderStep = 2.0f * horizontalLimit / (invaderCount - 1);
        Vector3 position = new Vector3(-horizontalLimit, transform.position.y, transform.position.z);

        leftLimits.Clear();
        rightLimits.Clear();

        remainInvaders = alienInvaders.Length * invaderCount;

        for (int i = 0; i < alienInvaders.Length; ++i)
        {
            foreach(GameObject invader in invadersLinkedLists[i])
            {
                Destroy(invader);
            }
            invadersLinkedLists[i].Clear();

            leftLimits.Add(-moveLimit);
            rightLimits.Add(moveLimit);

            position.x = -horizontalLimit;

            for (int j = 0; j < invaderCount; j++)
            {
                GameObject invader = Instantiate(alienInvaders[i], position, Quaternion.identity) as GameObject;
                invader.transform.parent = transform;

                invadersLinkedLists[i].AddLast(invader);
                position.x += invaderStep;
            }
            position.z += verticalGap;
        }

        Invoke("InvaderShoot", Random.Range(2, 4));
    }

    public void InvaderDead()
    {
        --remainInvaders;
    }

    private void InvaderShoot()
    {
        if(!globalController.isGameOver) 
        {
            int count = Random.Range(1, Mathf.Min(4, remainInvaders));

            List<GameObject> list = new List<GameObject>();

            foreach (LinkedList<GameObject> linked_list in invadersLinkedLists)
            {
                foreach (GameObject invader in linked_list)
                {
                    list.Add(invader);
                }
            }

            ShuffleList(list);

            for (int i = 0; i < count; ++i)
            {
                if (list[i] != null)
                    list[i].GetComponent<AlienInvader>().OnShoot();
            }

            Invoke("InvaderShoot", Random.Range(3, 5));
        }
    }

    void ShuffleList<T>(List<T> list)
    {
        System.Random random = new System.Random();

        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            // Generate a random index between 0 and i (inclusive)
            int randIndex = random.Next(0, i + 1);

            // Swap elements at randIndex and i
            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }
}
