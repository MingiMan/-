using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public SpriteRenderer white;
    public SpriteRenderer black;
    public Image red;
    Color color;
    WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    public void FadeOut(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(_speed));
    }

    IEnumerator FadeOutCoroutine(float _speed)
    {
        color = black.color;

        while (color.a < 1)
        {
            color.a += _speed;
            black.color = color;
            yield return waitTime;
        }
    }

    public void FadeIn(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(_speed));
    }


    IEnumerator FadeInCoroutine(float _speed)
    {
        color = black.color;

        while (color.a > 0f)
        {
            color.a -= _speed;
            black.color = color;
            yield return waitTime;
        }
    }

    public void FlashOut(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashOutCoroutine(_speed));
    }

    IEnumerator FlashOutCoroutine(float _speed)
    {
        color = white.color;

        while (color.a < 1f)
        {
            color.a += _speed;
            white.color = color;
            yield return waitTime;
        }
    }

    public void Flash(float _speed = 0.1f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine(_speed));
    }

    IEnumerator FlashCoroutine(float _speed)
    {

        color = white.color;

        while (color.a < 1f)
        {
            color.a += _speed;
            white.color = color;
            yield return waitTime;
        }

        while (color.a > 0f)
        {
            color.a -= _speed;
            white.color = color;
            yield return waitTime;
        }
    }

    public void FlashIn(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashInCoroutine(_speed));
    }

    IEnumerator FlashInCoroutine(float _speed)
    {
        color = white.color;

        while (color.a > 0f)
        {
            color.a -= _speed;
            white.color = color;
            yield return waitTime;
        }
    }

    public void FadeRed(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeRedCoroutine(_speed));
    }

    public void FadeOutRed(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutRedCoroutine(_speed));
    }

    public void FadeInRed(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInRedCoroutine(_speed));
    }

    IEnumerator FadeOutRedCoroutine(float _speed)
    {
        color = red.color;

        while (color.a < 0.7f)
        {
            color.a += _speed;
            red.color = color;
            yield return waitTime;
        }
    }

    IEnumerator FadeInRedCoroutine(float _speed)
    {
        color = red.color;

        while (color.a > 0)
        {
            color.a -= _speed;
            red.color = color;
            yield return waitTime;
        }
    }


    IEnumerator FadeRedCoroutine(float _speed)
    {
        color = red.color;

        while (color.a < 0.7f)
        {
            color.a += _speed;
            red.color = color;
            yield return waitTime;
        }

        while (color.a > 0)
        {
            color.a -= _speed;
            red.color = color;
            yield return waitTime;
        }
    }
}
