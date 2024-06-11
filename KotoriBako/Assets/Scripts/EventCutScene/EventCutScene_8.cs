using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventCutScene_8 : Event
{
    [SerializeField] Dialogue[] dialogues;
    [SerializeField] GameObject snim;
    [SerializeField] GameObject kotorBako;
    [SerializeField] Transform eventPos;
    bool flag;

    private void OnEnable()
    {
        flag = false;
        snim.gameObject.SetActive(false);
        kotorBako.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
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
        theOrder.Turn("Player", "UP");
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 8; i++)
            theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(3f);
        theDM.ShowDialogue(dialogues[0]); // 헉, 허억….
        yield return new WaitUntil(() => !theDM.talking);
        theDM.ShowDialogue(dialogues[1]); // 거기 누구 아무도 없습니까
        yield return new WaitUntil(() => !theDM.talking);
        yield return new WaitForSeconds(0.5f);
        theAudio.SoundPlay("openDoor_Sound");
        snim.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        theOrder.PreLoadCharacter();
        for(int i = 0; i < 2; i++)
            theOrder.Move("Snim", "DOWN");
        yield return new WaitForSeconds(2f);
        for (int i = 2; i < 8; i++)
        {
            theDM.ShowDialogue(dialogues[i]); 
            yield return new WaitUntil(() => !theDM.talking);
            if(i == 5)
            {
                yield return new WaitForSeconds(0.5f);
                theAudio.SoundPlay("fallBook_Sound"); // 상자 놓는소리
                kotorBako.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
            }
        }
        yield return new WaitForSeconds(1f);
        theFade.FadeOut();
        yield return new WaitForSeconds(5f);
        thePlayer.currentMapName = "Grandma_Room";
        thePlayer.transform.position = eventPos.transform.position;
        theOrder.Turn("Player", "DOWN");
        theCam.playerTarget = null;
        theCam.transform.position = new Vector3(20, 65, theCam.transform.position.z);
        yield return new WaitForSeconds(2f);
        theFade.FadeIn();
        yield return new WaitForSeconds(1f);
        theOrder.Move("Player", "RIGHT");
        yield return new WaitForSeconds(0.5f);
        theOrder.Move("Player", "LEFT");
        yield return new WaitForSeconds(0.5f);
        theOrder.Turn("Player", "DOWN");
        yield return new WaitForSeconds(1f);
        for (int i = 8; i < 12 ; i++)
        {
            theDM.ShowDialogue(dialogues[i]);
            yield return new WaitUntil(() => !theDM.talking);
        }
        theFade.FadeOut();
        yield return new WaitForSeconds(1f);
        theGM.LoadTitle();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Title");
    }
}
