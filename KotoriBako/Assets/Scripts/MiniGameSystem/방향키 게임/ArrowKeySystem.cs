using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowKeySystem : MonoBehaviour
{
    [Header("UP : 0 DOWN : 1 LEFT : 2 RIGHT : 3")]
    [SerializeField] private List<GameObject> arrows;
    [SerializeField] GridLayoutGroup gridLayOut;
    private Dictionary<int, GameObject> activeArrows = new Dictionary<int, GameObject>();
    private List<int> pattern = new List<int>();

    private AudioManager audioManager;
    private FadeManager fadeManager;

    private int playCount = 0;
    private int patternLength = 5;
    private int currentIndex = 0;
    private bool keyInput;

    [SerializeField] private string correctSound;
    [SerializeField] private string failSound;
    [SerializeField] private float playTime;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        fadeManager = FindObjectOfType<FadeManager>();
        gridLayOut = GetComponent<GridLayoutGroup>();
    }

    private void OnEnable()
    {
        gridLayOut.spacing = new Vector2(50, 50);
        activeArrows.Clear();
        pattern.Clear();
        keyInput = false;
        playCount = 0;
        currentIndex = 0;
        patternLength = 5;
    }

    public void ShowGame()
    {
        MiniGameManager.Instance.PlayTime(playTime);
        GenerateAIPattern(patternLength);
    }

    private void GenerateAIPattern(int patternLength)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        for (int i = 0; i < patternLength; i++)
        {
            int arrowIndex = Random.Range(0, arrows.Count);
            GameObject newObj = Instantiate(arrows[arrowIndex], transform, false);
            newObj.name = "ArrowKey " + i;
            activeArrows.Add(i, newObj);
            pattern.Add(arrowIndex);
        }
        keyInput = true;
    }

    private void Update()
    {
        if (keyInput)
        {
            DetectInput();
        }
    }


    private void DetectInput()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
            CheckInput(0);
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            CheckInput(1);
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            CheckInput(2);
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            CheckInput(3);
    }

    private void CheckInput(int keyIndex)
    {
        if (keyIndex == pattern[currentIndex])
            HandleCorrectInput();
        else
            HandleIncorrectInput();
    }

    private void HandleCorrectInput()
    {
        audioManager.SoundPlay(correctSound);
        HighlightArrow(Color.green);
        currentIndex++;

        if (currentIndex == patternLength)
            StartCoroutine(ResetGame());
    }

    private void HandleIncorrectInput()
    {
        HighlightArrow(Color.red);
        currentIndex = 0;
        keyInput = false;
        audioManager.SoundPlay(failSound);
        MiniGameManager.Instance.TimeStop();
        Invoke(nameof(EndGame), 2f);
    }

    private void HighlightArrow(Color color)
    {
        GameObject obj = activeArrows[currentIndex];
        Image objRenderer = obj.GetComponent<Image>();
        objRenderer.color = color;
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (var obj in activeArrows.Values)
        {
            Destroy(obj);
        }

        activeArrows.Clear();
        pattern.Clear();
        playCount++;
        currentIndex = 0;
        AdjustPatternLength();
    }

    private void AdjustPatternLength()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        switch (playCount)
        {
            case 1:
                patternLength = 8;
                gridLayOut.spacing = new Vector2(83, 50);
                GenerateAIPattern(patternLength);
                break;
            case 2:
                patternLength = 10;
                gridLayOut.spacing = new Vector2(50, 50);
                GenerateAIPattern(patternLength);
                break;
            case 3:
                GameOver();
                break;
            default:
                break;
        }
    }

    private void EndGame()
    {
        MiniGameManager.Instance.GameOver();
        gameObject.SetActive(false);
    }

    private void GameOver()
    {
        gameObject.SetActive(false);
        MiniGameManager.Instance.MiniGameClear();
    }
}
