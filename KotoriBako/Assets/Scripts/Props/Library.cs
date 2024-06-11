using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : Props
{
    public override void ShowText()
    {
        StartCoroutine(PropText());
    }

    IEnumerator PropText()
    {
        PlayerManager.instance.flag = true;
        foreach (var dialogue in textDialogue)
        {
            theDM.ShowText(dialogue);
            yield return new WaitUntil(() => !theDM.talking);
        }
        yield return new WaitForSeconds(0.5f);
        PlayerManager.instance.flag = false;
    }
}
