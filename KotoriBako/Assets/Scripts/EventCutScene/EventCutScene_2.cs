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
        theDM.ShowDialogue(dialogues[0]); // ����ü �� �ҸӴ� ���� ���� �� �ִ� �ž�!!!
        yield return new WaitUntil(() => !theDM.talking);
        thePlayer.GetComponent<Animator>().Rebind();
        theDM.ShowDialogue(dialogues[1]); // ����?
        yield return new WaitUntil(() => !theDM.talking);
        theAudio.SoundPlay(ghost_Sound);
        yield return new WaitForSeconds(3f);
        theDM.ShowDialogue(dialogues[2]); // ���ƴٴϴ� �� ������..
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Turn("Player", "LEFT");
        yield return new WaitForSeconds(1f);
        theDM.ShowDialogue(dialogues[3]); // �� ���忡 ������ �� ���� �� ����?
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
        theDM.ShowDialogue(dialogues[4]); // �ƾ� �̰� �� ����?
        yield return new WaitUntil(() => !theDM.talking);
        theDM.ShowDialogue(dialogues[5]); // �����������ΰ�?
        yield return new WaitUntil(() => !theDM.talking);
        theAudio.SoundPlay(page_Sound);
        yield return new WaitForSeconds(1);
        theDM.ShowText(textDialogues[0]); // ��Ű��ø� �ݶ���?
        yield return new WaitUntil(() => !theDM.talking);
        isActive = false;
    }
}
