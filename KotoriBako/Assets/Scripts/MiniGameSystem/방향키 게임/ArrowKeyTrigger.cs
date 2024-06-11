using System.Collections.Generic;
using UnityEngine;

public class ArrowKeyTrigger : MonoBehaviour
{
    public string[] arrows;
    Queue<string> arrow;
    bool isEvent;
    bool flag;
    [SerializeField] Scroll scroll;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !flag)
        {
            if (PlayerManager.instance.directionChanged)
            {
                arrow = new Queue<string>(arrows);
                isEvent = true;
                flag = true;
            }
        }
    }

    private void OnEnable()
    {
        isEvent = false;
        flag = false;
    }

    private void Update()
    {
        if (isEvent && PlayerManager.instance.directionChanged)
        {
            if (arrow.Peek() == PlayerManager.instance.currentDir)
            {
                PlayerManager.instance.directionChanged = false;
                arrow.Dequeue();
                if (arrow.Count <= 0)
                {
                    scroll.ScrollEvent = true;
                    isEvent = false;
                    arrow.Clear();
                }
            }
            else
            {
                arrow.Clear();
                isEvent = false;
                flag = false;
                PlayerManager.instance.directionChanged = false;
            }
        }
    }
}
