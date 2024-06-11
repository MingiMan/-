using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    BGMManager theBGM;
    Camera cam;

    [SerializeField] SpriteRenderer upPanel;
    [SerializeField] SpriteRenderer downPanel;

    [SerializeField] EventCutScene_1 EventCutScene_1;
    [SerializeField] EventCutScene_2 EventCutScene_2;
    [SerializeField] EventCutScene_3 EventCutScene_3;
    [SerializeField] EventCutScene_4 EventCutScene_4;

    OrderManager theOrder;
    PlayerManager thePlayer;
    float speed = 0.02f;
    static public bool isActive;

    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theBGM = FindObjectOfType<BGMManager>();
        cam = Camera.main;
        gameObject.GetComponent<Canvas>().worldCamera = cam;
    }

    public void ShowEvent()
    {
        StartCoroutine(StartEventScene());
    }

    IEnumerator StartEventScene()
    {
        theOrder.NotMove();
        thePlayer.notMove = true;
        isActive = true;
        yield return new WaitForSeconds(3f);
        FadeOutPanel();
        EventCutScene_1.ShowEventScene();
        yield return new WaitUntil(() => EventCutScene_1.isActive == false);
        EventCutScene_2.ShowEventScene();
        yield return new WaitUntil(() => EventCutScene_2.isActive == false);
        EventCutScene_3.ShowEventScene();
        yield return new WaitUntil(() => EventCutScene_3.isActive == false);
        EventCutScene_4.ShowEventScene();
        yield return new WaitUntil(() => EventCutScene_4.isActive == false);
        FadeInPanel();
        theBGM.FadeOutMusic();
        yield return new WaitForSeconds(2F);
        isActive = false;
        theOrder.CanMove();
        theBGM.BgmPlay(1);
        theBGM.FadeInMusic();
        PlayerStatusManager.Instance.helathParent.gameObject.SetActive(true);
        PlayerStatusManager.Instance.stageCountParent.gameObject.SetActive(true);
    }

    public void FadeOutPanel()
    {
        StartCoroutine(ShowFadeOutPanel());
    }

    public void FadeInPanel()
    {
        StartCoroutine(ShowFadeInPanel());
    }

    IEnumerator ShowFadeOutPanel()
    {
        Color upPanelColor = upPanel.color;
        Color downPanelColor = downPanel.color;

        while (downPanelColor.a < 1)
        {
            upPanelColor.a += speed;
            downPanelColor.a += speed;
            upPanel.color = upPanelColor;
            downPanel.color = downPanelColor;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator ShowFadeInPanel()
    {
        Color upPanelColor = upPanel.color;
        Color downPanelColor = downPanel.color;

        while (downPanelColor.a > 0)
        {
            upPanelColor.a -= speed;
            downPanelColor.a -= speed;
            upPanel.color = upPanelColor;
            downPanel.color = downPanelColor;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
