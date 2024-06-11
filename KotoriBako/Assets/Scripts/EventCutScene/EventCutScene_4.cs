using System.Collections;
using UnityEngine;

public class EventCutScene_4 : Event
{
    [SerializeField] Dialogue[] dialogues;
    [SerializeField] TextDialogue[] textDialogues;
    public bool isActive;
    public string search_Sound;
    public string get_Sound;

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
        isActive = true;
        theFade.FadeIn();
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 9; i++)
        {
            theDM.ShowDialogue(dialogues[i]);
            yield return new WaitUntil(() => !theDM.talking);

            switch (i)
            {
                case 2:
                    theAudio.SoundPlay(search_Sound);
                    yield return new WaitForSeconds(1f);
                    break;
                case 3:
                    theDM.ShowText(textDialogues[0]); // 일지 마지막 장에서 부적과 쪽지가 떨어졌다.
                    yield return new WaitUntil(() => !theDM.talking);
                    break;
                case 4:
                    theDM.ShowText(textDialogues[1]); // 부적을 희득했다.
                    theAudio.SoundPlay(get_Sound);
                    yield return new WaitUntil(() => !theDM.talking);
                    yield return new WaitForSeconds(1f);
                    break;

                case 7:
                    theOrder.EventMove("Player", "RIGHT", 5);
                    theOrder.EventMove("Player", "RIGHT", 5);
                    theOrder.EventMove("Player", "RIGHT", 5);
                    theOrder.EventMove("Player", "RIGHT", 5);
                    yield return new WaitForSeconds(1.5f);
                    theOrder.Turn("Player", "DOWN");
                    yield return new WaitForSeconds(1f);

                    theDM.ShowText(textDialogues[2]);
                    yield return new WaitUntil(() => !theDM.talking);
                    yield return new WaitForSeconds(1f);
                    break;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            theOrder.EventMove("Player", "DOWN", 5);
        }
        yield return new WaitUntil(() => thePlayer.queue.Count == 0);
        yield return new WaitForSeconds(1f);
        theOrder.EventMove("Player", "DOWN", 5);
        theOrder.EventMove("Player", "LEFT", 5);
        yield return new WaitForSeconds(2f);
        for (int i = 9; i < 12; i++)
        {
            theDM.ShowDialogue(dialogues[i]);
            yield return new WaitUntil(() => !theDM.talking);
        }
        isActive = false;
    }
}
