using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CirclePatternSystem : MonoBehaviour
{
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject BG;
    [SerializeField] Canvas CirclePatternCanvas;
    [SerializeField] Dictionary<int, CircleIdentifier> circles;
    [SerializeField] GridLayoutGroup gridLayOut;
    [SerializeField] GameObject go;
    [SerializeField] private float playTime;
    List<CircleIdentifier> lines;
    GameObject lineOnEdit;
    RectTransform lineOnEditRcTs;
    CircleIdentifier circleOnEdit;
    Dictionary<int, List<int>> directConnections = new Dictionary<int, List<int>>()
{
    {0, new List<int>{1, 3, 4}},
    {1, new List<int>{0, 2, 3, 4, 5}},
    {2, new List<int>{1, 4, 5}},
    {3, new List<int>{0, 1, 4, 6, 7}},
    {4, new List<int>{0, 1, 2, 3, 5, 6, 7, 8}},
    {5, new List<int>{1, 2, 4, 7, 8}},
    {6, new List<int>{3, 4, 7}},
    {7, new List<int>{3, 4, 5, 6, 8}},
    {8, new List<int>{4, 5, 7}}
};
    List<int> pattern;
    int patternLength;
    int playCount;
    int circleCount;
    const int MAX_ATTEMPTS = 100;
    bool enables;
    bool unLocking;
    public bool activated;

    bool bugFix;

    Color defalutColor = new Color(0.65882f, 0.44314f, 0.1451f, 1);


    private void OnEnable()
    {
        playCount = 0;
        patternLength = 6;
        circleCount = 9;
        gridLayOut.spacing = new Vector2(25, 90);
        unLocking = false;
        enables = false;
        bugFix = false;
        directConnections = new Dictionary<int, List<int>>()
{
    {0, new List<int>{1, 3, 4}},
    {1, new List<int>{0, 2, 3, 4, 5}},
    {2, new List<int>{1, 4, 5}},
    {3, new List<int>{0, 1, 4, 6, 7}},
    {4, new List<int>{0, 1, 2, 3, 5, 6, 7, 8}},
    {5, new List<int>{1, 2, 4, 7, 8}},
    {6, new List<int>{3, 4, 7}},
    {7, new List<int>{3, 4, 5, 6, 8}},
    {8, new List<int>{4, 5, 7}}
};
        pattern = new List<int>();
        circles = new Dictionary<int, CircleIdentifier>();
        lines = new List<CircleIdentifier>();
        pattern = new List<int>();
    }

    public void ShowGame()
    {
        go.gameObject.SetActive(true);
        BG.gameObject.SetActive(true);

        for (int i = 0; i < circleCount; i++)
        {
            var circle = go.transform.GetChild(i);
            var identifier = circle.GetComponent<CircleIdentifier>();
            var animor = circle.GetComponent<Animator>();
            animor.enabled = false;
            identifier.gameObject.SetActive(true);
            identifier.id = i;
            circles.Add(i, identifier);
        }
        activated = true;
        GenerateAIPattern(patternLength);
    }

    private void Update()
    {
        if (!enables)
            return;

        if (unLocking)
        {
            // Vector3 mousePos = canvas.transform.InverseTransformPoint(Input.mousePosition); <- Screen Overlay 일때 사용
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CirclePatternCanvas.GetComponent<RectTransform>(), Input.mousePosition, CirclePatternCanvas.worldCamera, out localPoint);
            Vector3 mousePos = new Vector3(localPoint.x, localPoint.y, circleOnEdit.transform.localPosition.z);
            lineOnEditRcTs.sizeDelta = new Vector2(lineOnEditRcTs.sizeDelta.x, Vector3.Distance(mousePos, circleOnEdit.transform.localPosition)); // 원의 중심값
            lineOnEditRcTs.rotation = Quaternion.FromToRotation(Vector3.up, (mousePos - lineOnEditRcTs.localPosition)).normalized;
        }
    }


    public void OnMouseEnterCircle(CircleIdentifier circle)
    {
        if (!enables)
            return;


        if (unLocking) // 내가 누른원을 기준으로 연결하여 다른 원을 선택할때
        {
            lineOnEditRcTs.sizeDelta = new Vector2(lineOnEditRcTs.sizeDelta.x, Vector3.Distance(circleOnEdit.transform.localPosition, circle.transform.localPosition));
            lineOnEditRcTs.rotation = Quaternion.FromToRotation(Vector3.up, (circle.transform.localPosition - circleOnEdit.transform.localPosition).normalized);
            TrySetLineEdit(circle);
        }
    }

    public void OnMouseDownCircle(CircleIdentifier circle)
    {
        if (!enables)
            return;

        unLocking = true;
        TrySetLineEdit(circle);
    }

    public void OnMouseUpCircle(CircleIdentifier circle)
    {
        if (!enables)
            return;

        if (lines.Count != pattern.Count)
        {
            activated = false;
            StartCoroutine(Release());
            return;
        }
        Destroy(lines[lines.Count - 1].gameObject);
        lines.RemoveAt(lines.Count - 1);

        //foreach (var line in lines)
        //{
        //    EnableCircleColorFade(line.gameObject.GetComponent<Animator>(), true);
        //}

        for (int i = 0; i < pattern.Count; i++)
        {
            if (circles.ContainsKey(pattern[i]))
                EnableCircleColorFade(circles[pattern[i]].gameObject.GetComponent<Animator>(), true);
        }


        StartCoroutine(Release());

        unLocking = false;
    }

    // AI 패턴 생성
    void GenerateAIPattern(int patternLength)
    {
        pattern.Clear();

        for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
        {
            int lastLine = Random.Range(0, directConnections.Count);
            pattern.Add(lastLine);

            for (int i = 1; i < patternLength; i++)
            {
                List<int> possibleNextLines = directConnections[lastLine];

                List<int> availableNextLines = new List<int>(possibleNextLines);

                availableNextLines.RemoveAll(node => pattern.Contains(node));

                if (availableNextLines.Count == 0)
                    availableNextLines.AddRange(possibleNextLines);

                int nextLine = availableNextLines[Random.Range(0, availableNextLines.Count)];
                pattern.Add(nextLine);
                lastLine = nextLine;
            }

            if (pattern.Distinct().Count() == pattern.Count)
            {
                StartCoroutine(StartCircleAnimation());
                return; // 버그 해결
            }
            else
            {
                pattern.Clear();
            }
        }
    }

    // Circle 생성
    void GenerateAICricle()
    {
        circles.Clear();
        for (int i = 0; i < circleCount; i++)
        {
            var circle = go.transform.GetChild(i);
            var identifier = circle.GetComponent<CircleIdentifier>();
            identifier.gameObject.SetActive(true);
            identifier.id = i;
            circles.Add(i, identifier);
        }
    }


    IEnumerator StartCircleAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < pattern.Count; i++)
        {
            if (circles.ContainsKey(pattern[i]))
            {
                EnableCircleColorFade(circles[pattern[i]].gameObject.GetComponent<Animator>(), true);
                yield return new WaitForSeconds(0.5f);
            }
        }

        foreach (var circleEmpty in circles.Values)
        {
            UnEnableCircleColorFade(circleEmpty.gameObject.GetComponent<Animator>());
        }
        MiniGameManager.Instance.PlayTime(playTime);
        enables = true;
    }


    void TrySetLineEdit(CircleIdentifier circle)
    {
        if (!enables)
            return;

        foreach (var line in lines)
        {
            if (line.id == circle.id)
                return;
        }

        lineOnEdit = CreateLine(circle.transform.localPosition, circle.id);
        lineOnEditRcTs = lineOnEdit.GetComponent<RectTransform>();
        circleOnEdit = circle;
        CheckPattern();

    }

    GameObject CreateLine(Vector3 pos, int id)
    {
        var line = GameObject.Instantiate(linePrefab, CirclePatternCanvas.transform);
        line.transform.localPosition = pos;
        var lineIdf = line.AddComponent<CircleIdentifier>();
        lineIdf.id = id;
        lines.Add(lineIdf);
        return line;
    }

    void CheckPattern()
    {
        if (lines.Count <= pattern.Count)
        {
            if (pattern[lines.Count - 1] == circleOnEdit.id)
            {
                EnableCircleColorFade(circleOnEdit.gameObject.GetComponent<Animator>(), true);
                // EnableCircleColorFade(lineOnEdit.gameObject.GetComponent<Animator>(), true);
            }
            else
            {
                EnableCircleColorFade(circleOnEdit.gameObject.GetComponent<Animator>(), false);
                // EnableCircleColorFade(lineOnEdit.gameObject.GetComponent<Animator>(), false);
                activated = false;
                StartCoroutine(Release());
            }
        }
        else
        {
            EnableCircleColorFade(circleOnEdit.gameObject.GetComponent<Animator>(), false);
            // EnableCircleColorFade(lineOnEdit.gameObject.GetComponent<Animator>(), false);
            activated = false;
            StartCoroutine(Release());
        }
    }

    // 순서 건드리지 말아요
    void HandleGameProgress()
    {
        playCount++;
        circles.Clear();

        if (playCount <= 3 && activated)
        {
            ConfigureGameLevel();
            if (!bugFix)
            {
                SetDirectConnections();
                GenerateAICricle();
                GenerateAIPattern(patternLength);
            }
        }
        else
        {
            GameOver();
        }
    }

    void ConfigureGameLevel()
    {
        switch (playCount)
        {
            case 1:
                circleCount = 12;
                patternLength = 7;
                gridLayOut.spacing = new Vector2(25, 45);
                break;
            case 2:
                circleCount = 15;
                patternLength = 8;
                gridLayOut.spacing = new Vector2(25, 25);
                break;

            case 3:
                bugFix = true;
                EndGame();
                break;
        }
    }

    IEnumerator Release()
    {
        enables = false;
        MiniGameManager.Instance.TimeStop();
        yield return new WaitForSeconds(3f);

        foreach (var circle in circles)
        {
            circle.Value.GetComponent<UnityEngine.UI.Image>().color = defalutColor;
            circle.Value.GetComponent<Animator>().enabled = false;
        }

        foreach (var line in lines)
            Destroy(line.gameObject);


        lines.Clear();
        lineOnEdit = null;
        lineOnEditRcTs = null;
        circleOnEdit = null;
        HandleGameProgress();
    }

    void EnableCircleColorFade(Animator animor, bool _fade)
    {
        animor.enabled = true;
        animor.Play("Empty");
        animor.SetBool("Fade", _fade);
    }

    void UnEnableCircleColorFade(Animator animor)
    {
        animor.Play("Empty");
        animor.GetComponent<UnityEngine.UI.Image>().color = defalutColor;
        animor.enabled = false;
    }

    public void GameOver()
    {
        BG.gameObject.SetActive(false);
        go.gameObject.SetActive(false);
        gameObject.SetActive(false);
        MiniGameManager.Instance.GameOver();

    }

    public void EndGame()
    {
        BG.gameObject.SetActive(false);
        go.gameObject.SetActive(false);
        gameObject.SetActive(false);
        MiniGameManager.Instance.MiniGameClear();
    }

    void SetDirectConnections()
    {
        if (playCount == 1)
        {
            directConnections = new Dictionary<int, List<int>>()
    {
        {0, new List<int>{1, 3, 4}},
        {1, new List<int>{0, 2, 3, 4, 5}},
        {2, new List<int>{1, 4, 5}},
        {3, new List<int>{0, 1, 4, 6, 7}},
        {4, new List<int>{0, 1, 2, 3, 5, 6, 7, 8}},
        {5, new List<int>{1, 2, 4, 7, 8}},
        {6, new List<int>{3, 4, 7,9,10}},
        {7, new List<int>{3, 4, 5, 6, 8,9,10,11}},
        {8, new List<int>{4, 5, 7,10,11}},
        {9, new List<int>{6,7,10}},
        {10, new List<int>{6,7,8,9,11}},
        {11, new List<int>{7,8,10}},
    };
        }
        if (playCount == 2)
        {
            directConnections = new Dictionary<int, List<int>>()
    {
        {0, new List<int>{1, 3, 4}},
        {1, new List<int>{0, 2, 3, 4, 5}},
        {2, new List<int>{1, 4, 5}},
        {3, new List<int>{0, 1, 4, 6, 7}},
        {4, new List<int>{0, 1, 2, 3, 5, 6, 7, 8}},
        {5, new List<int>{1, 2, 4, 7, 8}},
        {6, new List<int>{3, 4, 7,9,10}},
        {7, new List<int>{3, 4, 5, 6, 8,9,10,11}},
        {8, new List<int>{4, 5, 7,10,11}},
        {9, new List<int>{6,7,10,12}},
        {10, new List<int>{6,7,8,9,11,12,13,14}},
        {11, new List<int>{7,8,10,13,14}},
        {12, new List<int>{9,10,13}},
        {13, new List<int>{9,10,11,12,14}},
        {14, new List<int>{10,11,13}},
    };
        }
    }

}
