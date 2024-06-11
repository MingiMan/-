using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropsText : MonoBehaviour
{
    public TextDialogue dialogue;
    OrderManager theOrder;
    DialogueManager theDM;
    PlayerManager thePlayer;

    private void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theDM = FindObjectOfType<DialogueManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    public void ShowText()
    {
        StartCoroutine(PropText());
    }

    IEnumerator PropText()
    {
        thePlayer.flag = true;
        theDM.ShowText(dialogue);
        theOrder.NotMove();
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.CanMove();
        yield return new WaitForSeconds(0.6f);
        thePlayer.flag = false;
    }
}
