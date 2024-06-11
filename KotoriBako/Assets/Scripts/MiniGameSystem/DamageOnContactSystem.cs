using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageOnContactSystem : MonoBehaviour
{
    DialogueManager theDM;
    OrderManager theOrder;
    FadeManager theFade;
    PlayerManager thePlayer;
    public Text timerText;
    [SerializeField] Dialogue[] dialogues;
    [SerializeField] TextDialogue text;
    [SerializeField] GameObject eventCollider;
    [SerializeField] GameObject eventMap;
    [SerializeField] GameObject eventBoundary;
    public float playTime;
    float currentPlayTime;
    public bool IsGameRunning;

    private void Awake()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theFade = FindObjectOfType<FadeManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    private void OnEnable()
    {
        timerText.gameObject.SetActive(false);
        eventMap.gameObject.SetActive(false);
        eventCollider.gameObject.SetActive(false);
        IsGameRunning = false;
        timerText.text = " ";
        timerText.text = playTime.ToString();
        eventBoundary.gameObject.SetActive(false);
    }

    private void Update()
    {
        ShowTimer();
    }

    public void ShowGame()
    {
        StartCoroutine(EventScene());
    }

    IEnumerator EventScene()
    {
        EventManager.isActive = false;
        theDM.ShowDialogue(dialogues[0]);
        yield return new WaitUntil(() => !theDM.talking);
        theFade.FadeOutRed();
        yield return new WaitForSeconds(1f);
        eventMap.gameObject.SetActive(true);
        eventCollider.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        theFade.FadeInRed();
        yield return new WaitForSeconds(1f);
        thePlayer.GetComponent<Animator>().SetFloat("DirY", -1);
        yield return new WaitForSeconds(0.5f);
        theDM.ShowDialogue(dialogues[1]); // 빨리 탈출해야 겠어
        yield return new WaitUntil(() => !theDM.talking);
        theDM.ShowText(text); // 제한 시간안에 장애물을 건드리지 않고 탈출하세요.
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.CanMove();
        IsGameRunning = true;
        currentPlayTime = playTime;
        timerText.gameObject.SetActive(true);
        eventBoundary.gameObject.SetActive(true);
    }

    public void OnDamage()
    {
        StartCoroutine(PlayerDamage());
    }

    IEnumerator PlayerDamage()
    {
        theFade.FadeRed();
        PlayerStatusManager.Instance.HealthDecrease();
        yield return new WaitForSeconds(0.5f);
        currentPlayTime -= 5f;
    }

    void ShowTimer()
    {
        if (IsGameRunning)
        {
            currentPlayTime -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(currentPlayTime / 60f); // 분
            int seconds = Mathf.FloorToInt(currentPlayTime % 60f); // 초

            timerText.text = string.Format("{0 : 00}:{1 : 00}", minutes, seconds);

            if (currentPlayTime < 15f)
                timerText.color = Color.red;

            if (currentPlayTime < 0)
            {
                TimeOver();
                return;
            }
        }
    }

    public void TimeOver()
    {
        IsGameRunning = false;
        timerText.gameObject.SetActive(false);
        currentPlayTime = playTime;
        eventBoundary.gameObject.SetActive(false);
        eventCollider.gameObject.SetActive(false);
        MiniGameManager.Instance.IsActivated = false;
        theFade.FadeInRed();
    }
}
