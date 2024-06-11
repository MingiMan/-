using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towel : Props
{
    public string get_Sound;
    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);    
    }

    public override void ShowText()
    {
        StartCoroutine(PropText());
    }

    IEnumerator PropText()
    {
        PlayerManager.instance.flag = true;
        Mirror.GetItem = true;
        theDM.ShowText(textDialogue[0]);
        theAudio.SoundPlay(get_Sound);
        yield return new WaitUntil(() => !theDM.talking);
        PlayerManager.instance.flag = false;
        gameObject.SetActive(false);
    }
}
