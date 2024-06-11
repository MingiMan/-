using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;
    [SerializeField] ArrowKeySystem theArrowKeySystem;
    [SerializeField] AmuletFindGame theAmuletFindGame;
    [SerializeField] CirclePatternSystem theCirclePatternSystem;
    [SerializeField] FindCardSystem theFindCardSystem;
    [SerializeField] DamageOnContactSystem theDamageOnContactSystem;

    OrderManager theOrder;
    AudioManager theAudio;
    PlayerManager thePlayer;
    PlayerStatusManager thePlayerStatus;
    [SerializeField] Image timer_linear_image;
    [SerializeField] GameObject timer;
    [SerializeField] GameObject miniGameBg;

    float time_remaining;
    float maxTime;
    bool IsGameRunning;
    public bool IsActivated;
    public string ghostLaugh_Sound;

    MiniGameType currentMiniGame;
    GameObject currentKotoriBako;

    public bool DestroyKotoriBako;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    private void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        thePlayerStatus = FindObjectOfType<PlayerStatusManager>();
    }

    public void PlayMiniGame(MiniGameType _gameType,GameObject _kotoriBako)
    {
        DestroyKotoriBako = false;
        IsActivated = true;
        currentKotoriBako = _kotoriBako;

        if (_gameType == MiniGameType.DamageContactGame)
        {
            theDamageOnContactSystem.ShowGame();
            return;
        }
        miniGameBg.gameObject.SetActive(true);
        theOrder.NotMove();
        currentMiniGame = _gameType;
        switch (_gameType)
        {
            case MiniGameType.AmuletGame:
                theAmuletFindGame.GetComponent<RectTransform>().gameObject.SetActive(true);
                theAmuletFindGame.ShowGame();
                break;

            case MiniGameType.ArrowKeyGame:
                theArrowKeySystem.GetComponent<RectTransform>().gameObject.SetActive(true);
                theArrowKeySystem.ShowGame();
                break;

            case MiniGameType.CirclePatternGame:
                theCirclePatternSystem.GetComponent<RectTransform>().gameObject.SetActive(true);
                theCirclePatternSystem.ShowGame();
                break;
            case MiniGameType.FindCardGame:
                theFindCardSystem.GetComponent<RectTransform>().gameObject.SetActive(true);
                theFindCardSystem.ShowGame();
                break;
        }
    }

    public void PlayTime(float _playTime)
    {
        time_remaining = _playTime;
        maxTime = _playTime;
        IsGameRunning = true;
        StartCoroutine(UpdateTime());
    }

    IEnumerator UpdateTime()
    {
        timer.gameObject.SetActive(true);
        while (time_remaining > 0 && IsGameRunning)
        {
            time_remaining -= Time.deltaTime;
            timer_linear_image.fillAmount = time_remaining / maxTime;
            yield return null;
        }

        if (time_remaining <= 0)
            GameOver();
    }

    public void TimeStop()
    {
        timer.gameObject.SetActive(false);
        IsGameRunning = false;
    }

    public void TimeAgain()
    {
        timer.gameObject.SetActive(true);
        IsGameRunning = true;
    }

    public void MiniGameClear()
    {
        theOrder.NotMove();
        EventManager.isActive = true;
        IsGameRunning = false;
        timer.gameObject.SetActive(false);
        miniGameBg.gameObject.SetActive(false);
        DestroyKotoriBako = true;
        StartCoroutine(DestroyInKotoriBako());

        switch (currentMiniGame)
        {
            case MiniGameType.ArrowKeyGame:
                theArrowKeySystem.GetComponent<RectTransform>().gameObject.SetActive(false);
                PlayerStatusManager.Instance.StageClear();
                break;
            case MiniGameType.AmuletGame:
                theAmuletFindGame.GetComponent<RectTransform>().gameObject.SetActive(false);
                PlayerStatusManager.Instance.StageClear();
                break;
            case MiniGameType.CirclePatternGame:
                theCirclePatternSystem.GetComponent<RectTransform>().gameObject.SetActive(false);
                PlayerStatusManager.Instance.StageClear();
                break;

            case MiniGameType.FindCardGame:
                theFindCardSystem.GetComponent<RectTransform>().gameObject.SetActive(true);
                PlayerStatusManager.Instance.StageClear();
                break;
            case MiniGameType.DamageContactGame:
                PlayerStatusManager.Instance.StageClear();
                break;
        }
        IsActivated = false;
        theOrder.CanMove();
    }

    IEnumerator DestroyInKotoriBako()
    {
        EventManager.isActive = true;
        SpriteRenderer spriteRenderer = currentKotoriBako.GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        theAudio.SoundPlay(ghostLaugh_Sound);
        while (color.a > 0)
        {
            color.a -= 0.02f;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);
        EventManager.isActive = false;
        currentKotoriBako.gameObject.SetActive(false);
        currentKotoriBako = null;
        thePlayer.notMove = false;
        thePlayer.flag = false;
    }

    public void GameOver()
    {
        currentKotoriBako.GetComponent<Animator>().Rebind();
        EventManager.isActive = false;
        theOrder.CanMove();
        IsActivated = false;
        IsGameRunning = false;
        timer.gameObject.SetActive(false);
        miniGameBg.gameObject.SetActive(false);
        thePlayerStatus.HealthDecrease();
        switch (currentMiniGame)
        {
            case MiniGameType.ArrowKeyGame: // 별관
                theArrowKeySystem.GetComponent<RectTransform>().gameObject.SetActive(false);
                break;

            case MiniGameType.AmuletGame:
                theAmuletFindGame.GetComponent<RectTransform>().gameObject.SetActive(false);
                break;
            case MiniGameType.CirclePatternGame: // 별관
                theCirclePatternSystem.GetComponent<RectTransform>().gameObject.SetActive(false);
                break;

            case MiniGameType.FindCardGame:
                theFindCardSystem.GetComponent<RectTransform>().gameObject.SetActive(false);
                break;
        }
    }
}