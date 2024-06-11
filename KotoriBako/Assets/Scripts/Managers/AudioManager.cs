using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {

        public string name;

        private AudioSource source;
        public AudioClip clip;

        public float Volumn;
        public bool loop;

        public void SetSource(AudioSource _source)
        {
            source = _source;
            source.clip = clip;
            source.loop = loop;
            source.volume = Volumn;
        }

        public void SoundPlay()
        {
            source.Play();
        }

        public void Stop()
        {
            source.Stop();
        }

        public void SetLoop()
        {
            source.loop = true;
        }

        public void SetLoopCancel()
        {
            source.loop = false;
        }

        public void SetVolumn()
        {
            source.volume = Volumn;
        }
    }

    [SerializeField]
    public Sound[] sounds;
    static public AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObj = new GameObject(sounds[i].name);
            sounds[i].SetSource(soundObj.AddComponent<AudioSource>());
            soundObj.transform.SetParent(transform);
        }
    }

    public void SoundPlay(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].SoundPlay();
                return;
            }
        }
    }

    public void SoundStop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }

    public void StopLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].SetLoopCancel();
                return;
            }
        }
    }

    public void SoundPlay(string _name, float _Volumn)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Volumn = _Volumn;
                sounds[i].SetVolumn();
                return;
            }
        }
    }

    public void SetVolumn(float _Volumn)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Volumn = _Volumn;
            sounds[i].SetVolumn();
        }
    }
}
