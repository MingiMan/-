using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MovingObject
{
    int random_int;
    string direction;

    public float inter_MoveWaitTime;
    float current_interMWT;

    Vector2 playerPos;

    public bool notMove;

    [SerializeField] Transform spawnPos;
    [SerializeField] Transform playerSpawnPos;

    public string[] patternDir;
    string currentDir;

    bool flag;

    private Queue<string> directionQueue;
    private Stack<string> oppositeDirStack;
    FadeManager theFade;
    bool Isopposite;

    private void Awake()
    {
        theFade = FindObjectOfType<FadeManager>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animor = GetComponent<Animator>();
    }

    private void Start()
    {
        queue = new Queue<string>();
        current_interMWT = inter_MoveWaitTime;
        notMove = true;
    }

    void MonsterSpawn()
    {
        if (PlayerManager.instance.currentMapName == "UnderGround" && !flag)
        {
            transform.position = spawnPos.transform.position;
            flag = true;
            notMove = false;
            directionQueue = new Queue<string>(patternDir);
            oppositeDirStack = new Stack<string>();
        }
        else if (PlayerManager.instance.currentMapName != "UnderGround")
        {
            notMove = true;
            flag = false;
        }
    }

    private void Update()
    {
        MonsterSpawn();

        if (!notMove)
            PatternDir();
    }

    private void PatternDir()
    {
        current_interMWT -= Time.deltaTime;

        if (current_interMWT < 0)
        {
            current_interMWT = inter_MoveWaitTime;
            vector.Set(0, 0, vector.z);


            if (!Isopposite)
                currentDir = directionQueue.Dequeue();
            else
                currentDir = oppositeDirStack.Pop();


            switch (currentDir)
            {
                case "RIGHT":
                    vector.x = 1;
                    if (!Isopposite)
                        oppositeDirStack.Push("LEFT");
                    break;
                case "LEFT":
                    vector.x = -1;
                    if (!Isopposite)
                        oppositeDirStack.Push("RIGHT");
                    break;
                case "UP":
                    vector.y = 1;
                    if (!Isopposite)
                        oppositeDirStack.Push("DOWN");
                    break;
                case "DOWN":
                    vector.y = -1;
                    if (!Isopposite)
                        oppositeDirStack.Push("UP");
                    break;
            }

            if (base.CheckCollsion())
                return;

            base.Move(currentDir,5);

            if (directionQueue.Count <= 0 && !Isopposite)
            {
                Isopposite = true;
                directionQueue.Clear();
            }

            if (oppositeDirStack.Count <= 0 && Isopposite)
            {
                directionQueue = new Queue<string>(patternDir);
                Isopposite = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            notMove = true;
            PlayerStatusManager.Instance.HealthDecrease();
            StartCoroutine(DelaySpawn());
        }
    }

    IEnumerator DelaySpawn()
    {
        theFade.FadeOut();
        PlayerManager.instance.notMove = true;
        PlayerManager.instance.transform.position = playerSpawnPos.transform.position;
        PlayerManager.instance.gameObject.GetComponent<Animator>().Rebind();
        PlayerManager.instance.gameObject.GetComponent<Animator>().SetFloat("DirX", -1);
        transform.position = spawnPos.transform.position;
        yield return new WaitForSeconds(3f);
        theFade.FadeIn();
        notMove = false;
        PlayerManager.instance.notMove = false;
    }

    void RandomDirection()
    {
        vector.Set(0, 0, vector.z);
        random_int = Random.Range(0, 4);
        switch (random_int)
        {
            case 0:
                vector.y = 1f;
                direction = "UP";
                break;
            case 1:
                vector.y = -1f;
                direction = "DOWN";
                break;
            case 2:
                vector.x = -1f;
                direction = "LEFT";
                break;
            case 3:
                vector.x = 1f;
                direction = "RIGHT";
                break;
        }
    }

    bool NearPlayer()
    {
        playerPos = PlayerManager.instance.transform.position;

        float distanceX = Mathf.Abs(playerPos.x - this.transform.position.x);
        float distanceY = Mathf.Abs(playerPos.y - this.transform.position.y);

        float thresholdX = (moveSpeed * walkCount) * 5 * 1.01f;
        float thresholdY = (moveSpeed * walkCount) * 5 * 0.5f;
        return (distanceX <= thresholdX && distanceY <= thresholdY) || (distanceY <= thresholdX && distanceX <= thresholdY);
    }

    void ChasePlayer()
    {

        Vector2 directionVector = (playerPos - (Vector2)this.transform.position).normalized;
        vector.Set(0, 0, vector.z);

        if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.y))
        {
            direction = directionVector.x > 0 ? "RIGHT" : "LEFT";
            vector.x = directionVector.x > 0 ? 1 : -1;
        }
        else
        {
            direction = directionVector.y > 0 ? "UP" : "DOWN";
            vector.y = directionVector.y > 0 ? 1 : -1;
        }

        if (!base.CheckCollsion())
            base.Move(direction, 5);


    }

    void Test()
    {
        current_interMWT -= Time.deltaTime;

        if (current_interMWT <= 0 && !notMove)
        {
            current_interMWT = inter_MoveWaitTime;

            if (NearPlayer())
                ChasePlayer();

            else
            {
                RandomDirection();

                if (base.CheckCollsion())
                    return;

                base.Move(direction, 5);
            }
        }
    }
}
