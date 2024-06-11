using System.Collections;
using UnityEngine;

public class Tub : Props
{
    public GameObject go_OOC;
    OkOrCancel theOOC;
    EventManager theEventManager;
    [SerializeField] GameObject kotoriBako;
    public bool tubEvent;
    public string bloodWater_Sound;
    public string waterOut_Sound_Sound;

    private void OnEnable()
    {
        kotoriBako.gameObject.SetActive(false);
    }

    public override void ShowText()
    {
        if (tubEvent)
            StartCoroutine(EventProp());
    }


    IEnumerator EventProp()
    {
        EventManager.isActive = true;
        PlayerManager.instance.flag = true;
        foreach (var dialogue in textDialogue)
        {
            theDM.ShowText(dialogue);
            yield return new WaitUntil(() => !theDM.talking);
        }

        go_OOC.gameObject.SetActive(true);
        theOOC.ShowTwoChoice("»«´Ù", "¾È»«´Ù.");
        yield return new WaitUntil(() => !theOOC.activated);
        go_OOC.gameObject.SetActive(false);

        if (theOOC.GetResult())
        {
            theEventManager.FadeOutPanel();
            yield return new WaitForSeconds(1f);
            EventManager.isActive = true;
            kotoriBako.gameObject.SetActive(true);
            TubClear();
            yield return new WaitForSeconds(5f);
            theEventManager.FadeInPanel();
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);
        EventManager.isActive = false;
        PlayerManager.instance.flag = false;

    }

    protected override void Start()
    {
        base.Start();
        animor = GetComponent<Animator>();
        theOOC = FindObjectOfType<OkOrCancel>();
        theEventManager = FindObjectOfType<EventManager>();
    }

    public void TubClear()
    {
        animor.SetTrigger("IsClear");
        theAudio.SoundPlay(waterOut_Sound_Sound);
        tubEvent = false;
    }

    public void TubAction()
    {
        animor.SetTrigger("IsWorking");
        theAudio.SoundPlay(bloodWater_Sound);
        tubEvent = true;
    }
}
