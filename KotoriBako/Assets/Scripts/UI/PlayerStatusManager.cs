using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStatusManager : MonoBehaviour
{
    public static PlayerStatusManager Instance;
    GameManager theGM;
    OrderManager theOrder;
    BGMManager theBGM;
    AudioManager theAudio;
    DialogueManager theDM;
    [SerializeField] Dialogue[] dialogues;
    public int playerHealth;
    public int stageCount;


    public Transform helathParent;
    public Transform stageCountParent;

    [SerializeField] Sprite health;
    [SerializeField] Sprite amulet;

    private List<GameObject> currentHealth;
    private List<GameObject> currentAmulet;

    int currentHelathCount;
    int currentAmuletCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        Initalize();
    }

    public void Initalize()
    {
        theGM = FindObjectOfType<GameManager>();
        theOrder = FindObjectOfType<OrderManager>();    
        theBGM = FindObjectOfType<BGMManager>();
        theAudio = FindObjectOfType<AudioManager>();
        currentHealth = new List<GameObject>();
        currentAmulet = new List<GameObject>();

        currentHelathCount = playerHealth;
        currentAmuletCount = stageCount;

        helathParent.gameObject.SetActive(false);
        stageCountParent.gameObject.SetActive(false);

        // 목숨 생성
        GenerateHealth();
        // 부적생성
        GenerateAmulet();
    }

    void GenerateHealth()
    {
        currentHealth.Clear();

        foreach (Transform child in helathParent)
            GameObject.Destroy(child.gameObject);

        for (int i = 0; i < playerHealth; i++)
        {
            currentHealth.Add(new GameObject("Health" + i));
            Image obj = currentHealth[i].AddComponent<Image>();
            obj.sprite = health;

            currentHealth[i].transform.SetParent(helathParent, false);
        }

    }

    void GenerateAmulet()
    {
        currentAmulet.Clear();

        foreach (Transform child in stageCountParent)
            GameObject.Destroy(child.gameObject);

        for (int i = 0; i < stageCount; i++)
        {
            currentAmulet.Add(new GameObject("Amulet" + i));
            Image obj = currentAmulet[i].AddComponent<Image>();
            obj.sprite = amulet;
            currentAmulet[i].transform.SetParent(stageCountParent, false);
        }
    }

    // 스테이지를 클리어할 때마다 부적이 감소
    public void StageClear()
    {
        currentAmuletCount -= 1;
        Destroy(currentAmulet[currentAmuletCount]);
        currentAmulet.RemoveAt(currentAmuletCount);

        if (currentAmuletCount == 0)
        {
            EventCutScene_6 eventCutScene_6 = FindObjectOfType<EventCutScene_6>();
            eventCutScene_6.ShowEventScene();
        }
    }


    // 원령에게 부딧히면 목숨 감소
    public void HealthDecrease()
    {
        if (currentHelathCount == 0)
            return;

        theAudio.SoundPlay("damage_Sound");
        currentHelathCount -= 1;
        Destroy(currentHealth[currentHelathCount]);
        currentHealth.RemoveAt(currentHelathCount);

        if (currentHelathCount == 0)
        {
            StopAllCoroutines();
            currentHelathCount = 0;
            theBGM.Pause();
            theOrder.NotMove();
            StartCoroutine(GoToGameOverScene());
        }
    }

    IEnumerator GoToGameOverScene()
    { 
        helathParent.gameObject.SetActive(false);
        stageCountParent.gameObject.SetActive(false);
        theGM.LoadGameOver();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameOver");
    }
}
