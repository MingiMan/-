using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    public AudioClip[] clips;
    public AudioSource source;

    WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        source = GetComponent<AudioSource>();
    }

    public void BgmPlay(int _playMusicTrack)
    {
        // source.volume = 1;
        source.clip = clips[_playMusicTrack];
        source.Play();
    }

    public void SetVolumn(float _volume)
    {
        source.volume = _volume;
    }

    public void Pause()
    {
        source.Pause();
    }

    public void UnPause()
    {
        source.UnPause();
    }
    public void BgmStop()
    {
        source.Stop();
    }

    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        for (float i = 1.0f; i >= 0.0f; i -= 0.01f)
        {
            source.volume = i;
            yield return waitTime;
            // 반복문 내에서 new가 자주 호출된다면 따로 선언을 해줘서 넣어준다.
        }
    }

    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0.1f; i <= 1f; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
            // 반복문 내에서 new가 자주 호출된다면 따로 선언을 해줘서 넣어준다.
        }
    }
}
