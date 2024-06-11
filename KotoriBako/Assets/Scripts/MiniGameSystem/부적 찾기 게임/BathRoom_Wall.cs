using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BathRoom_Wall : Props
{
    [SerializeField] GameObject bathRoomTransfer;
    [SerializeField] GameObject wall;
    OrderManager theOrder;
    public string stop_Sound;
    public string openBox_Sound;
    bool flag;
    public bool IsEvent;

    private void OnEnable()
    {
        gameObject.SetActive(true);
        bathRoomTransfer.gameObject.SetActive(false);
        wall.gameObject.SetActive(true);
        IsEvent = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !flag)
        {
            if (!IsEvent)
                StartCoroutine(DonGo());
            else
                StartCoroutine(Go());
        }
    }


    IEnumerator DonGo()
    {
        flag = true;
        theOrder.NotMove();
        theDM.ShowText(textDialogue[0]);
        theAudio.SoundPlay(stop_Sound);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.EventMove("Player", "LEFT",5);
        theOrder.CanMove();
        yield return new WaitForSeconds(0.5f);
        flag = false;
    }

    IEnumerator Go()
    {
        wall.gameObject.SetActive(false);
        flag = true;
        theOrder.NotMove();
        theDM.ShowText(textDialogue[0]);
        yield return new WaitUntil(() => !theDM.talking);
        theDM.ShowText(textDialogue[1]);
        theAudio.SoundPlay(openBox_Sound);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.EventMove("Player", "RIGHT", 5);
        bathRoomTransfer.gameObject.SetActive(true);
        theOrder.CanMove();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }


    protected override void Start()
    {
        base.Start();
        theOrder = FindObjectOfType<OrderManager>();
    }

    public override void ShowText()
    {
        throw new System.NotImplementedException(); 
    }
}
