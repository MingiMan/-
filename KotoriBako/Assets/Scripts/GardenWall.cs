using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenWall : Props
{
    [SerializeField] GameObject wall;
    OrderManager theOrder;
    bool flag;
    public bool IsEvent;

    private void OnEnable()
    {
        gameObject.SetActive(true);
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
                gameObject.SetActive(false);
        }
    }

    IEnumerator DonGo()
    {
        flag = true;
        theOrder.NotMove();
        theDM.ShowText(textDialogue[0]);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.EventMove("Player", "UP", 5);
        theOrder.CanMove();
        yield return new WaitForSeconds(0.5f);
        flag = false;
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
