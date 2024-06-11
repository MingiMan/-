using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class EventCutScene_3 : Event
{
    [SerializeField] TextDialogue[] textDialogues;
    [SerializeField] List<Sprite> Illustrations;
    [SerializeField] SpriteRenderer eventCutSprite;
    [SerializeField] Animator illustrationEvent;
    private int currentIllustrationIndex = 0;  // ÇöÀç »ðÈ­ ÀÎµ¦½º
    public bool isActive;
    public string page_Sound;

    protected override void Start()
    {
        eventCutSprite.enabled = false;
        illustrationEvent.enabled = false;
        base.Start();
    }

    void UpDateIllustration()
    {
        if (currentIllustrationIndex > Illustrations.Count)
        {
            currentIllustrationIndex = 0;
            return;
        }
        eventCutSprite.sprite = Illustrations[currentIllustrationIndex];
        illustrationEvent.enabled = true;
        illustrationEvent.Rebind();
        eventCutSprite.enabled = true;
        currentIllustrationIndex++;
    }



    public override  void ShowEventScene()
    {
        StartCoroutine(EventScene());
    }

    IEnumerator EventScene()
    {
        isActive = true;
        theFade.FadeOut();
        yield return new WaitForSeconds(2f);
        theBGM.BgmPlay(2);

        for (int i = 0; i < textDialogues.Length; i++)
        {
            UpDateIllustration();
            theDM.ShowText(textDialogues[i]);
            yield return new WaitUntil(() => !theDM.talking);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
            theAudio.SoundPlay(page_Sound);
        }
        yield return new WaitForSeconds(0.5f);
        theFade.FadeOut();
        yield return new WaitForSeconds(1.5f);
        eventCutSprite.enabled = false;
        isActive = false;
    }
}
