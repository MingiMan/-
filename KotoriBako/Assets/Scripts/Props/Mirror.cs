using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;

public class Mirror : Props
{
    bool IsEvent;
    [SerializeField] SpriteRenderer blood;
    public static bool GetItem;
    public string washing_Sound;
    public string openBox_Sound;
    [SerializeField] Light2D room_Light;
    [SerializeField] PlayableDirector kotoriBako_1;
    [SerializeField] GameObject key;

    OrderManager theOrder;
    EventManager theEventManager;

    public void OnEnable()
    {
        IsEvent = true;
        GetItem = false;
        kotoriBako_1.enabled = false;
        key.gameObject.SetActive(false);
    }

    public override void ShowText()
    {
        if (IsEvent)
            StartCoroutine(PropText());
    }

    IEnumerator PropText()
    {
        PlayerManager.instance.flag = true;
        theDM.ShowText(textDialogue[0]);
        yield return new WaitUntil(() => !theDM.talking);
        if (GetItem)
        {
            theDM.ShowText(textDialogue[1]); // 수건으로 닦아보자
            yield return new WaitUntil(() => !theDM.talking);
            theEventManager.FadeOutPanel();
            EventManager.isActive = true;
            yield return new WaitForSeconds(1f);
            theAudio.SoundPlay(washing_Sound);
            StartCoroutine(FadeOutBlood());
            yield return new WaitForSeconds(4f);
            theOrder.EventMove("Player", "DOWN", 5);
            theOrder.Turn("Player", "UP");
            yield return new WaitForSeconds(0.5f);
            animor.SetTrigger("IsWorking");
            yield return new WaitForSeconds(1f);
            room_Light.intensity = 0.1f;
            theAudio.SoundPlay(openBox_Sound);
            yield return new WaitForSeconds(2f);
            room_Light.intensity = 0.4f;
            kotoriBako_1.enabled = true;
            IsEvent = false;
            yield return new WaitForSeconds(1f);
            theEventManager.FadeInPanel();
            EventManager.isActive = false;
            key.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        PlayerManager.instance.flag = false;
    }

    IEnumerator FadeOutBlood()
    {
        Color color = blood.color;
        while(color.a > 0)
        {
            color.a -= 0.02f;

            blood.color = color;
            yield return new WaitForSeconds(0.02f);
        }
    }

    protected override void Start()
    {
        base.Start();
        animor = GetComponent<Animator>();
        theOrder = FindObjectOfType<OrderManager>();
        theEventManager = FindObjectOfType<EventManager>();
    }
}
