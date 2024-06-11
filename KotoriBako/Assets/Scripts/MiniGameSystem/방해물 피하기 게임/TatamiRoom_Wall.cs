using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TatamiRoom_Wall : Props
{
    [SerializeField] GameObject[] tatamiTransfer;
    [SerializeField] GameObject wall;
    OrderManager theOrder;
    public string stop_Sound;
    public string openBox_Sound;
    bool flag;
    public bool IsEvent;

    private void OnEnable()
    {
        gameObject.SetActive(true);

        foreach (GameObject obj in tatamiTransfer)
            obj.gameObject.SetActive(false);

        wall.gameObject.SetActive(true);
        IsEvent = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !flag)
        {
            if (collision.gameObject.GetComponent<Animator>().GetFloat("DirY") == 1)
            {
                if (!IsEvent)
                    StartCoroutine(DonGo());
                else
                    StartCoroutine(Go());
            }
        }
    }


    IEnumerator DonGo()
    {
        flag = true;
        theOrder.NotMove();
        theDM.ShowText(textDialogue[0]);
        theAudio.SoundPlay(stop_Sound);
        yield return new WaitUntil(() => !theDM.talking);
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
        theOrder.EventMove("Player", "UP", 5);
        foreach (GameObject obj in tatamiTransfer)
            obj.gameObject.SetActive(true);

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
