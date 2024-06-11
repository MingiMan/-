using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;


public class EventCutScene_2 : Event
{
    [SerializeField] Dialogue[] dialogues;
    [SerializeField] TextDialogue[] textDialogues;
    [SerializeField]
    PlayableDirector bookTimeLine;
    public bool isActive;
    public string ghost_Sound;
    public string serach_Sound;
    public string page_Sound;

    protected override void Start()
    {
        bookTimeLine.enabled = false;
        base.Start();
    }

    public override void ShowEventScene()
    {
        isActive = true;
         StartCoroutine(EventScene());
    }

    IEnumerator EventScene()
    {
        thePlayer.GetComponent<Animator>().Rebind();
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.5f);
        theOrder.Turn("Player", "RIGHT");
        yield return new WaitForSeconds(1f);
        theOrder.Turn("Player", "LEFT");
        yield return new WaitForSeconds(0.5f);
        theOrder.Turn("Player", "DOWN");
        theDM.ShowDialogue(dialogues[0]); // 도대체 왜 할머니 집에 저런 게 있는 거야!!!
        yield return new WaitUntil(() => !theDM.talking);
        thePlayer.GetComponent<Animator>().Rebind();
        theDM.ShowDialogue(dialogues[1]); // 갔나?
        yield return new WaitUntil(() => !theDM.talking);
        theAudio.SoundPlay(ghost_Sound);
        yield return new WaitForSeconds(3f);
        theDM.ShowDialogue(dialogues[2]); // 돌아다니는 것 같은데..
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Turn("Player", "LEFT");
        yield return new WaitForSeconds(1f);
        theDM.ShowDialogue(dialogues[3]); // 이 옷장에 도움이 될 만한 건 없나?
        yield return new WaitUntil(() => !theDM.talking);

        for(int i =0; i < 4; i++)
        {
            theOrder.EventMove("Player", "LEFT", 5);
        }

        yield return new WaitForSeconds(1.5f);
        theOrder.Turn("Player", "UP");
        for (int i = 0; i < 2; i++)
        {
            theAudio.SoundPlay(serach_Sound);
            yield return new WaitForSeconds(0.8f);
        }
        bookTimeLine.enabled = true;
        yield return new WaitForSeconds(0.9f);
        theFade.Flash();
        yield return new WaitForSeconds(2f);
        theOrder.Turn("Player", "RIGHT");
        theDM.ShowDialogue(dialogues[4]); // 아야 이건 또 뭐야?
        yield return new WaitUntil(() => !theDM.talking);
        theDM.ShowDialogue(dialogues[5]); // 오래된일지인가?
        yield return new WaitUntil(() => !theDM.talking);
        theAudio.SoundPlay(page_Sound);
        yield return new WaitForSeconds(1);
        theDM.ShowText(textDialogues[0]); // 오키노시마 반란은?
        yield return new WaitUntil(() => !theDM.talking);
        isActive = false;
    }
}
