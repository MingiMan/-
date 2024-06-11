using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCutScene_5 : Event
{
    [SerializeField] List<Sprite> Illustrations;
    SpriteRenderer eventCutSprite;
    float initialWaitTime = 0.1f;
    [SerializeField] float timeDecrease = 0.01f;
    [SerializeField] float minimumWaitTime = 0.02f;
    public string footprint_Sound;
    public bool isAcitve = false;

    protected override void Start()
    {
        base.Start();
        eventCutSprite = GetComponent<SpriteRenderer>();
        eventCutSprite.enabled = false;
    }

    public override void ShowEventScene()
    {
        StartCoroutine(EventScene());
    }

    IEnumerator EventScene()
    {
        isAcitve = true;
        float waitTime = initialWaitTime;

        eventCutSprite.enabled = true;
        theAudio.SoundPlay(footprint_Sound);
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < Illustrations.Count; i++)
        {
            eventCutSprite.sprite = Illustrations[i];
            yield return new WaitForSeconds(waitTime);
            waitTime = Mathf.Max(minimumWaitTime, waitTime - timeDecrease);
        }
        yield return new WaitForSeconds(1f);
        theFade.FadeOut();
        yield return new WaitForSeconds(1f);
        eventCutSprite.enabled = false;
        theFade.FadeIn();
        isAcitve = false;
    }
}
