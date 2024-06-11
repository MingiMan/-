using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EventCutScene_1 : Event
{
    [SerializeField] Dialogue[] dialogues;
    [SerializeField] GameObject kotoriBako;
    [SerializeField] Sprite[] Ghsot_Illustration;
    [SerializeField] GameObject ghost;
    Image currentImage;

    public string drawers_Sound;
    public string serach_Sound;
    public string boxShake_Sound;
    public string openBox_Sound;
    public string ghost_Sound;

    public bool isActive;

    protected override void Start()
    {
        base.Start();
        currentImage = GetComponent<Image>();
        currentImage.enabled = false;
        currentImage.sprite = Ghsot_Illustration[0];
        kotoriBako.gameObject.SetActive(false);
    }


    public override void ShowEventScene()
    {
        isActive = true;
        StartCoroutine(EventScene());
    }

    IEnumerator EventScene()
    {
        theOrder.PreLoadCharacter();
        theEvent.FadeOutPanel();
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 4; i++)
        {
            theDM.ShowDialogue(dialogues[i]);
            yield return new WaitUntil(() => !theDM.talking);
        }

        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "UP");
        yield return new WaitUntil(() => thePlayer.queue.Count == 0);
        yield return new WaitForSeconds(0.5f);

        theAudio.SoundPlay(drawers_Sound);
        yield return new WaitForSeconds(1.3f);

        theAudio.SoundPlay(serach_Sound);
        yield return new WaitForSeconds(1f);

        theDM.ShowDialogue(dialogues[4]); // À½ ÀÌ°Ô ¹¹Áö?
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Move("Player", "DOWN");
        yield return new WaitUntil(() => thePlayer.queue.Count == 0);
        theOrder.Turn("Player", "UP");
        kotoriBako.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.7f);

        theDM.ShowDialogue(dialogues[5]); // .... »óÀÚ?
        yield return new WaitUntil(() => !theDM.talking);

        theAudio.SoundPlay(serach_Sound);
        yield return new WaitForSeconds(0.7f);

        theDM.ShowDialogue(dialogues[6]); // ¼³¸¶... ¼ÕÁÖ¸¦ À§ÇÑ ±ÝµÎ²¨ºñ?
        yield return new WaitUntil(() => !theDM.talking);

        theDM.ShowDialogue(dialogues[7]); // °¨»çÇÕ´Ï´Ù!!
        yield return new WaitUntil(() => !theDM.talking);

        theAudio.SoundPlay(openBox_Sound);
        kotoriBako.GetComponent<Animator>().SetBool("Open", true);
        yield return new WaitForSeconds(0.7f);

        theFade.FadeRed();
        yield return new WaitForSeconds(0.5f);

        theDM.ShowDialogue(dialogues[8]); // À¸¾Æ¾Æ¾Ç
        yield return new WaitUntil(() => !theDM.talking);


        theOrder.Move("Player", "RIGHT");
        theOrder.Move("Player", "RIGHT");
        yield return new WaitUntil(() => thePlayer.queue.Count == 0);

        theOrder.Turn("Player", "LEFT");

        theDM.ShowDialogue(dialogues[9]); // ÀÌ°Ô ¹¹¾ß ¹«½¼ ³¿»õ¾ß
        yield return new WaitUntil(() => !theDM.talking);

        ghost.gameObject.SetActive(true);
        theOrder.PreLoadCharacter();
        yield return new WaitForSeconds(1f);
        ghost.gameObject.GetComponent<Animator>().SetFloat("DirX", -1);
        yield return new WaitForSeconds(0.7f);
        ghost.gameObject.GetComponent<Animator>().SetFloat("DirX", 1);
        yield return new WaitForSeconds(0.7f);
        theDM.ShowDialogue(dialogues[10]); // ...?
        yield return new WaitUntil(() => !theDM.talking);

        theFade.FadeOut();
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(Event_Ghsot_Illustration());  // ±Í½Å »ðÈ­

        theAudio.SoundPlay(ghost_Sound);
        yield return new WaitForSeconds(3f);
        currentImage.enabled = false;

        theFade.FadeIn();
        yield return new WaitForSeconds(1f);
        theDM.ShowDialogue(dialogues[11]); // ±Í..±Í½Å?
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Move("Player", "RIGHT");
        yield return new WaitForSeconds(0.5f);
        theOrder.EventMove("Player", "UP", 4);
        yield return new WaitUntil(() => thePlayer.queue.Count == 0);
        yield return new WaitForSeconds(2f);
        kotoriBako.gameObject.SetActive(false);
        ghost.gameObject.SetActive(false);
        isActive = false;
    }

    IEnumerator Event_Ghsot_Illustration()
    {
        currentImage.enabled = true;

        Color color = currentImage.color;

        yield return new WaitForSeconds(2f);

        while (color.a > 0.8)
        {
            color.a -= 0.01f;
            currentImage.color = color;
            yield return new WaitForSeconds(0.01f);
        }

        currentImage.sprite = Ghsot_Illustration[1];

        while (color.a < 1)
        {
            color.a += 0.01f;
            currentImage.color = color;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
