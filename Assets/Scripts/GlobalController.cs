using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    [Header("Global")]
    public bool isGameOver;

    [Header("Player Ship")]
    private GameObject curPlayerShip;

    public int InitLife;
    private int RemainingLife;
    public int PlayerScore;
    public Text scoreGUI;

    public Vector3 PlayerShipInitPosition;
    public GameObject PlayerShip;

    public int InitBulletCount;
    private int bulletCount;
    private Text BulletText;

    [Header("Alien Invader")]
    public GameObject AlienUFO;
    private GameObject currentUFO;

    public Vector3 UFOInitPos;

    public GameObject[] AlienInvaders;
    public float alienMoveSpeed;

    [Header("Rocks")]
    public GameObject Rock;
    public int rockCount;
    public Vector3 rockInitPos;
    public float rockInterval;
    private List<GameObject> rockList;

    // private memebers
    private LifeIcons lifeIcons;
    private Text GameOverText;
    
    // Start is called before the first frame update
    void Start()
    {
        lifeIcons = GameObject.FindObjectOfType<LifeIcons>();
        GameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        rockList = new List<GameObject>();
        scoreGUI = GameObject.Find("PlayerScore").GetComponent<Text>(); 
        BulletText = GameObject.Find("RemainingBullet").GetComponent<Text>();
        OnGameStart();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            Reset();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnGameStart()
    {
        Reset();
    }

    public void OnGameOver()
    {
        // TODO: Display "GameOver"
        if(!isGameOver)
        {
            isGameOver = true;
            GameOverText.text = "Game Over";

            Invoke("BackToStartScreen", 5.0f);
        }
    }

    public void OnGameWin()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            GameOverText.text = "Game Win";

            Invoke("BackToStartScreen", 5.0f);
        }
    }

    public void Reset()
    {
        ClearAll();

        CancelInvoke("BackToStartScreen");

        isGameOver = false;
        GameOverText.text = "";
        GameObject.Find("InvaderController").GetComponent<InvaderRowController>().Reset();

        PlayerScore = 0;
        RemainingLife = InitLife;

        scoreGUI.text = PlayerScore.ToString();

        //Initiate PlayerShip
        InitPlayerShip();

        bulletCount = InitBulletCount;
        BulletText.text = bulletCount.ToString();

        lifeIcons.Reset(InitLife);

        // Inititiate Rocks
        InitRocks();

        CancelInvoke("GenUFO");
        float randTime = Random.Range(7.0f, 15.0f);
        Invoke("GenUFO", randTime);
    }

    private void ClearAll()
    {
        // Clear bullets
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        GameObject[] invaders = GameObject.FindGameObjectsWithTag("AlienInvader");

        foreach (GameObject invader in invaders)
        {
            Destroy(invader);
        }
    }

    private void InitPlayerShip()
    {
        if(curPlayerShip != null)
        {
            Destroy(curPlayerShip);
        }
        curPlayerShip = Instantiate(PlayerShip, PlayerShipInitPosition, Quaternion.identity) as GameObject;
    }

    private void InitRocks()
    {
        for (int i = 0; i < rockList.Count; i++)
        {
            if (rockList[i] != null)
            {
                Destroy(rockList[i]);
            }
        }
        rockList.Clear();

        Vector3 rock_pos = rockInitPos;
        for (int i = 0; i < rockCount; i++) 
        {
            GameObject obj = Instantiate(Rock, rock_pos, Quaternion.identity) as GameObject;
            rockList.Add(obj);
            rock_pos.x += rockInterval;
        }
    }

    public void IncreasePoint(int point)
    {
        PlayerScore += point;

        scoreGUI.text = PlayerScore.ToString();
    }

    public void OnPlayerDead()
    {
        RemainingLife -= 1;
        lifeIcons.ReduceLife();

        if (RemainingLife <= 0)
        {
            OnGameOver();
        }
        else
        {
            InitPlayerShip();
        }
    }

    private void GenUFO()
    {
        if (!isGameOver)
        {
            float sign = Random.RandomRange(-1.0f, 1.0f) > 0 ? 1 : -1;
            Vector3 pos = UFOInitPos;
            pos.x *= sign;
            currentUFO = Instantiate(AlienUFO, pos, Quaternion.identity) as GameObject;
            currentUFO.GetComponent<Rigidbody>().AddForce(new Vector3(sign * 200.0f, 0.0f, 0.0f));

            Invoke("DestroyUFO", 4);

            float randTime = Random.Range(15.0f, 30.0f);
            Invoke("GenUFO", randTime);
        }
    }
    private void DestroyUFO()
    {
        if (currentUFO != null)
        {
            Destroy(currentUFO);
        }
    }

    private void BackToStartScreen()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void IncreaseBullet()
    {
        ++bulletCount;
        BulletText.text = bulletCount.ToString();
    }

    public bool DecreaseBullet()
    {
        if (bulletCount <= 0) return false;
        
        --bulletCount;
        BulletText.text = bulletCount.ToString();
        return true;
    }
    public void Restart()
    {
        Reset();
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void PlayShipShoot()
    {
        if(curPlayerShip != null)
        {
            curPlayerShip.GetComponent<PlayerShip>().OnShoot();
        }
    }
}
