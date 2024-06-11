using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kettle : Props
{
    [SerializeField] GameObject eventBlood;
    public string rumbling_Sound;
    EventManager theEventManager;

    bool IsEvent;

    protected override void Start()
    {
        base.Start();
        theEventManager = FindObjectOfType<EventManager>();
    }

    private void OnEnable()
    {
        eventBlood.gameObject.SetActive(false);
        IsEvent = true;

    }

    public override void ShowText()
    {
        if(IsEvent)
            StartCoroutine(PropText());
    }

    IEnumerator PropText()
    {
        PlayerManager.instance.flag = true;
        theDM.ShowDialogue(dialogues[0]);
        yield return new WaitUntil(() => !theDM.talking);
        theDM.ShowDialogue(dialogues[1]);
        yield return new WaitUntil(() => !theDM.talking);
        theEventManager.FadeOutPanel();
        yield return new WaitForSeconds(1f);
        theAudio.SoundPlay(rumbling_Sound);
        eventBlood.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        theDM.ShowDialogue(dialogues[2]);
        yield return new WaitUntil(() => !theDM.talking);
        theEventManager.FadeInPanel();
        yield return new WaitForSeconds(1f);
        PlayerManager.instance.flag = false;
        IsEvent = false;
    }
}
