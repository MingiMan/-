using System.Collections;
using UnityEngine;

public class Scroll : Props
{
    OkOrCancel theOOC;
    Blood blood;
    public GameObject go_OOC;
    public bool ScrollEvent;
    public string boxShake_Sound;

    private void OnEnable()
    {
        gameObject.SetActive(true);
        ScrollEvent = false;
    }

    protected override void Start()
    {
        base.Start();
        theOOC = FindObjectOfType<OkOrCancel>();
        blood = FindObjectOfType<Blood>();
    }
    public override void ShowText()
    {
        if (ScrollEvent)
            StartCoroutine(PropText());
    }

    IEnumerator PropText()
    {
        PlayerManager.instance.flag = true;
        theDM.ShowText(textDialogue[0]);
        yield return new WaitUntil(() => !theDM.talking);
        go_OOC.gameObject.SetActive(true);
        theOOC.ShowTwoChoice("네.", "아니오");
        yield return new WaitUntil(() => !theOOC.activated);
        go_OOC.gameObject.SetActive(false);
        if (theOOC.GetResult())
        {
            theAudio.SoundPlay(boxShake_Sound);
            yield return new WaitForSeconds(0.5f);
            ScrollEvent = false;
            blood.BloodEvent = true;
            PlayerManager.instance.flag = false;
            gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);
        PlayerManager.instance.flag = false;
    }
}
