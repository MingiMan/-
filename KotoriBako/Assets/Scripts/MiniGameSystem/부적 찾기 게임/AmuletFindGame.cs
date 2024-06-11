using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AmuletFindGame : MonoBehaviour , IPointerDownHandler
{
    AudioManager theAudio;

    [SerializeField] GameObject amuletes;
    [SerializeField] GameObject show_Amulet;
    [SerializeField] GameObject timer;
    [SerializeField] GridLayoutGroup gridLayOut;
    [SerializeField] Sprite[] amulet_Image;
    [SerializeField] Image show_Timer;
    [SerializeField] Text amulet_Timer_Text;

    List<Sprite> random_Image;
    HashSet<int> usedIndices;
    bool mouseInput;
    int playCount;
    int amuletCount;
    public float amulet_Timer;
    public float playTime;
    float maxTime;
    bool IsTime;

    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    void OnEnable()
    {
        usedIndices = new HashSet<int>();
        random_Image = new List<Sprite>();
        IsTime = false;
        playCount = 0;
        amuletCount = 4;
        amulet_Timer = 4;
        maxTime = amulet_Timer;
        gridLayOut.spacing = new Vector2(33, 40);
        amuletes.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
        foreach (Transform child in amuletes.transform)
            Destroy(child.gameObject);
    }

    public void ShowGame()
    {
        SetAmuletImage();
        RandomAmulet();
    }

    void SetAmuletImage()
    {
        usedIndices.Clear();
        random_Image.Clear();

        for (int i = 0; i < amuletCount; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, amulet_Image.Length);
            } while (usedIndices.Contains(randomIndex));

            random_Image.Add(amulet_Image[randomIndex]);
            usedIndices.Add(randomIndex);
        }
    }

    void RandomAmulet()
    {
        show_Amulet.GetComponent<Image>().sprite = null;
        int randomIndex = Random.Range(0, amuletCount);
        Sprite showAmulet = random_Image[randomIndex];
        show_Amulet.GetComponent<Image>().sprite = showAmulet;

        show_Amulet.gameObject.SetActive(true);
        maxTime = amulet_Timer;
        timer.gameObject.SetActive(true);
        IsTime = true;
    }

    void ConfigureGameLevel()
    {
        mouseInput = false;
        playCount++;
        amuletes.gameObject.SetActive(false);

        foreach (Transform child in amuletes.transform)
            Destroy(child.gameObject);

        switch (playCount)
        {
            case 1:
                amuletCount = 6;
                amuletes.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -25);
                gridLayOut.spacing = new Vector2(100, 40);
                ShowGame();
                break;
            case 2:
                amuletCount = 8;
                amuletes.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -25);
                gridLayOut.spacing = new Vector2(33, 40);
                ShowGame();
                break;
            case 3:
                EndGame();
                break;
            default:
                break;
        }
    }

    private void ShowAmuletTime()
    {
        amulet_Timer -= Time.deltaTime;
        amulet_Timer_Text.text = "" + (int)amulet_Timer;
        show_Timer.fillAmount = amulet_Timer / maxTime;

        if (amulet_Timer <= 0)
        {
            amulet_Timer = 4;
            show_Amulet.gameObject.SetActive(false);
            timer.gameObject.SetActive(false);
            IsTime = false;
            CreatGenerateAIAmulet();
        }
    }
    void CreatGenerateAIAmulet()
    {
        for (int i = 0; i < amuletCount; i++)
        {
            GameObject newObj = new GameObject();
            Image obj = newObj.AddComponent<Image>();
            obj.sprite = random_Image[i];
            obj.transform.SetParent(amuletes.transform, false);
        }
        amuletes.gameObject.SetActive(true);
        mouseInput = true;
        MiniGameManager.Instance.PlayTime(playTime);
    }

    void Update()
    {
        if (IsTime)
            ShowAmuletTime();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (mouseInput)
        {
            GameObject clickAmulet = eventData.pointerCurrentRaycast.gameObject;
            Image clickAmuletImage = clickAmulet.GetComponent<Image>();
            if (clickAmuletImage.sprite == show_Amulet.GetComponent<Image>().sprite)
            {
                MiniGameManager.Instance.TimeStop();
                ConfigureGameLevel();
            }
            else
            {
                gameObject.SetActive(false);
                MiniGameManager.Instance.GameOver();
            }
        }
    }

    void EndGame()
    {
        gameObject.SetActive(false);
        MiniGameManager.Instance.MiniGameClear();
    }
}
