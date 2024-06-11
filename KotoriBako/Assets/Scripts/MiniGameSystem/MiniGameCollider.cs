using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameCollider : MonoBehaviour
{
    DamageOnContactSystem theDamageOnContactSystem;
    bool flag;


    private void Start()
    {
        theDamageOnContactSystem = FindObjectOfType<DamageOnContactSystem>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !flag)
        {
            flag = true;
            theDamageOnContactSystem.OnDamage();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && flag)
        {
            Invoke("OnFlag", 1f);
        }
    }

    void OnFlag()
    {
        flag = false;
    }
}
