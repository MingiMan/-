using System.Collections;
using UnityEngine;

public class EventCutScene_6 : Event
{
    [SerializeField] Dialogue[] dialogues;
    [SerializeField] EventCutScene_7 eventCutScene_7;
    public bool isActive;

    protected override void Start()
    {
        base.Start();
    }

    public override void ShowEventScene()
    {
        StartCoroutine(EventScene());
    }

    IEnumerator EventScene()
    {
        EventManager.isActive = true;
        eventCutScene_7.gameObject.SetActive(true);
        theEvent.FadeOutPanel();
        yield return new WaitForSeconds(1f);
        theDM.ShowDialogue(dialogues[0]);
        yield return new WaitUntil(() => !theDM.talking);
        theCam.ShowCameraShake();
        yield return new WaitForSeconds(1f);
        theDM.ShowDialogue(dialogues[1]);
        yield return new WaitUntil(() => !theDM.talking);
        theDM.ShowDialogue(dialogues[2]);
        yield return new WaitUntil(() => !theDM.talking);
        theEvent.FadeInPanel();
        yield return new WaitForSeconds(0.5f);
        EventManager.isActive = false;
    }
}
