using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager instance;

    AudioManager theAudio;
    BGMManager theBgm;

    [SerializeField] GameObject volumeSetting;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Text sfxCount;
    [SerializeField] Text bgmCount;

    int minTextSize = 0;
    int maxTextSize = 100;

    public bool IsActive;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        sfxSlider.onValueChanged.AddListener(SFXValue);
        bgmSlider.onValueChanged.AddListener(BGMValue);
    }

    private void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        theBgm = FindObjectOfType<BGMManager>();
        volumeSetting.gameObject.SetActive(false);
        sfxSlider.value = 1;
        bgmSlider.value = 1;
    }


    public void OpenVolume()
    {
        volumeSetting.gameObject.SetActive(true);
        IsActive = true;
    }

    public void BGMValue(float value)
    {
        theBgm.SetVolumn(value);
        UpdateBGMTextSize(value);
    }

    public void SFXValue(float value)
    {
        theAudio.SetVolumn(value);
        UpdateSFXTextSize(value);
    }

    void UpdateBGMTextSize(float value)
    {
        float newSize = Mathf.Lerp(minTextSize, maxTextSize, value);
        int countSize = (int)newSize;
        bgmCount.text = countSize.ToString() + "%";
    }

    private void UpdateSFXTextSize(float value)
    {
        float newSize = Mathf.Lerp(minTextSize, maxTextSize, value);
        int countSize = (int)newSize;
        sfxCount.text = countSize.ToString() + "%";
    }

    public void CloseVolume()
    {
        IsActive = false;
        volumeSetting.gameObject.SetActive(false);
    }
}
