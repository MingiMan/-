using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingObject
{
    static public PlayerManager instance;
    public string currentMapName;

    [SerializeField]
    GameObject playerLight;
    [SerializeField]
    float playerRunSpeed;
    float applyRunSpeed;
    bool applyRunFlag;
    public bool canMove;
    public string walkSound_1;
    public string walkSound_2;
    public string grass_Sound;

    AudioManager theAudio;

    public bool notMove = false;
    public float raycastDistance;
    public bool flag;
    public LayerMask gameLayer;

    public string currentDir;
    public bool directionChanged = false;


    string currentSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        canMove = true;
        queue = new Queue<string>();
        animor = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        theAudio = FindObjectOfType<AudioManager>();
        playerLight.gameObject.SetActive(false);
        currentSound = walkSound_1;
    }

    private void Update()
    {
        if (currentMapName == "Corridor_1" || currentMapName == "Corridor_2" || currentMapName == "Annex_Corridor" ||
            currentMapName == "UnderGround")
            playerLight.gameObject.SetActive(true);
        else
            playerLight.gameObject.SetActive(false);

        PropInteraction();
    }
    private void FixedUpdate()
    {
        if (EventManager.isActive)
            return;

        MoveInput();
    }


    private void MoveInput()
    {
        directionChanged = false;
        if (canMove && !notMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }
    }

    IEnumerator MoveCoroutine()
    {
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0 && !notMove)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = playerRunSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            if (vector.x != 0)
                vector.y = 0;

            animor.SetFloat("DirX", vector.x);
            animor.SetFloat("DirY", vector.y);


            bool checkCollsionFlag = base.CheckCollsion();

            if (checkCollsionFlag)
                break;

            StringDir();

            animor.SetBool("Walking", true);

            theAudio.SoundPlay(currentSound);

            // boxCollider2D.offset = new Vector2(vector.x, vector.y);

            while (currentWalkCount < walkCount)
            {

                transform.Translate(vector.x * (moveSpeed + applyRunSpeed), vector.y * (moveSpeed + applyRunSpeed), 0);

                if (applyRunFlag)
                    currentWalkCount++;

                currentWalkCount++;

                if (currentWalkCount == 12)
                    boxCollider2D.offset = Vector2.zero;

                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
        }
        animor.SetBool("Walking", false);
        canMove = true;
    }

    public void StringDir()
    {
        if (vector.x == 1)
            ChangeDirection("RIGHT");
        else if (vector.x == -1)
            ChangeDirection("LEFT");
        else if (vector.y == 1)
            ChangeDirection("UP");
        else if (vector.y == -1)
            ChangeDirection("DOWN");
    }

    private void PropInteraction()
    {
        Debug.DrawRay(transform.position, vector.normalized * raycastDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, vector.normalized, raycastDistance, gameLayer);

        if (hit.collider != null && Input.GetKeyUp(KeyCode.Z) && !flag)
        {
            MiniGameHandler obj = hit.collider.gameObject.GetComponent<MiniGameHandler>();
            if (obj != null)
                obj.ShowMiniGame();
        }
        if (hit.collider != null && Input.GetKeyUp(KeyCode.Z) && !flag)
        {
            Props obj = hit.collider.gameObject.GetComponent<Props>();
            if (obj != null)
                obj.ShowText();
        }

    }
    public void ChangeDirection(string newDir)
    {
        currentDir = newDir;
        directionChanged = true;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Grass":
                currentSound = grass_Sound;
                break;
            case "Wood":
                currentSound = walkSound_1;
                break;
        }
    }
}
