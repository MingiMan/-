using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TatamiBoundary : MonoBehaviour
{
    [SerializeField]  DamageOnContactSystem ThedamageOnContactSystem;
    [SerializeField] GameObject eventCollider;
    [SerializeField] GameObject eventMap;
    [SerializeField] GameObject kotorimako;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ThedamageOnContactSystem.timerText.gameObject.SetActive(false);
            StartCoroutine(MiniGameEnd());
        }
    }

    IEnumerator MiniGameEnd()
    {
        yield return new WaitForSeconds(2f);
        MiniGameManager.Instance.MiniGameClear();
        ThedamageOnContactSystem.TimeOver();
        eventCollider.gameObject.SetActive(false);
        eventMap.gameObject.SetActive(false);
        kotorimako.gameObject.SetActive(false);
    }
}
