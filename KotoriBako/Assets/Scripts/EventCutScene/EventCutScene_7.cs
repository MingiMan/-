using System.Collections;
using UnityEngine;

public class EventCutScene_7 : Event
{
    [SerializeField] Dialogue[] dialogues;
    [SerializeField] GameObject ghost;
    [SerializeField] Transform[] gardenWalls;
    [SerializeField] Transform transfer_Shrine;
    [SerializeField] Transform eventPos;
    bool flag;

    private void OnEnable()
    {
        flag = false;
        ghost.gameObject.SetActive(false);
        foreach(var wall in gardenWalls)
            wall.transform.gameObject.SetActive(false);
        transfer_Shrine.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !flag)
        {
            flag = true;
            ShowEventScene();
        }
    }

    public override void ShowEventScene()
    {
        StartCoroutine(EventScene());
    }

    IEnumerator EventScene()
    {
        ghost.gameObject.SetActive(true);
        theOrder.PreLoadCharacter();
        EventManager.isActive = true;
        theCam.StopCameraShake();
        theFade.FadeOut();
        yield return new WaitForSeconds(1.5f);
        theOrder.Turn("Player", "DOWN");
        theFade.FadeIn();
        thePlayer.transform.position = eventPos.transform.position;
        theEvent.FadeOutPanel();
        yield return new WaitForSeconds(1f);
        theDM.ShowDialogue(dialogues[0]);
        yield return new WaitUntil(() => !theDM.talking);
        theAudio.SoundPlay("openDoor_Sound");
        theDM.ShowDialogue(dialogues[1]); // ....?
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Turn("Player", "UP");
        theOrder.EventMove("Ghost1", "DOWN", 5);
        theOrder.EventMove("Ghost1", "DOWN", 5);
        theOrder.EventMove("Ghost1", "DOWN", 5);
        yield return new WaitForSeconds(2f);
        theAudio.SoundPlay("ghost_Sound");
        yield return new WaitForSeconds(1f);
        theDM.ShowDialogue(dialogues[2]); // µµ¸Á°¡ÀÚ
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.EventMove("Player", "DOWN", 4);
        theOrder.EventMove("Player", "DOWN", 5);
        theOrder.EventMove("Player", "DOWN", 5);
        theFade.FadeOut();
        gameObject.SetActive(false);
    }
}
