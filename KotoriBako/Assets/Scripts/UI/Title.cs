using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    AudioManager theAudio;
    VolumeManager theVolume;
    BGMManager theBGM;
    FadeManager theFade;
    PlayerManager thePlayer;
    OrderManager theOrder;
    GameManager theGM;
    PlayerStatusManager thePlayerStatus;
    Camera cam;
    bool keyInput;
    int selectFontSize = 30;
    int unSelectFontSize = 25;

    [Header("Sound")]
    public string cancel_Sound;
    public string enter_Sound;
    public string type_Sound;

    [Header("Select")]
    [SerializeField] Text start_Select;
    [SerializeField] Text setting_Select;
    [SerializeField] Text exit_Select;
    Text currentSelect;

    private void Awake()
    {
        theAudio = FindObjectOfType<AudioManager>();
        theBGM = FindObjectOfType<BGMManager>();
        theFade = FindObjectOfType<FadeManager>();
        theVolume = FindObjectOfType<VolumeManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theGM = FindObjectOfType<GameManager>();
        thePlayerStatus = FindObjectOfType<PlayerStatusManager>();
        cam = Camera.main;
        Initial();
    }

    private void OnEnable()
    {
        Initial();
    }

    void Initial()
    {
        currentSelect = start_Select;
        ApplyTextSetting(start_Select, selectFontSize, Color.red);
        ApplyTextSetting(setting_Select, unSelectFontSize, Color.white);
        ApplyTextSetting(exit_Select, unSelectFontSize, Color.white);
        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 0f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;
        gameObject.GetComponent<Canvas>().worldCamera = cam;
        thePlayerStatus.Initalize();
        keyInput = true;
        theBGM.BgmPlay(0);
        theOrder.NotMove();
        theVolume.CloseVolume();
    }

    void ApplyTextSetting(Text text,int _fontSize,Color _color)
    {
        text.fontSize = _fontSize;
        text.color = _color;
    }

    private void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                SwitchSelect(false);
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                SwitchSelect(true);
            else if (Input.GetKeyDown(KeyCode.Z))
                ChoiceSelect();
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape) && !keyInput)
            {
                keyInput = true;
                theAudio.SoundPlay(cancel_Sound);
                theVolume.CloseVolume();
            }
        }
    }

    void SwitchSelect(bool previous)
    {
        theAudio.SoundPlay(type_Sound);
        ApplyTextSetting(currentSelect, unSelectFontSize, Color.white);
        if (previous)
        {
            if (currentSelect == start_Select)
                currentSelect = exit_Select;

            else if (currentSelect == exit_Select)
                currentSelect = setting_Select;

            else if (currentSelect == setting_Select)
                currentSelect = start_Select;
        }
        else
        {
            if (currentSelect == start_Select)
                currentSelect = setting_Select;
            else if (currentSelect == setting_Select)
                currentSelect = exit_Select;
            else if (currentSelect == exit_Select)
                currentSelect = start_Select;
        }
        ApplyTextSetting(currentSelect, selectFontSize, Color.red);
    }

    void ChoiceSelect()
    {
        theAudio.SoundPlay(enter_Sound);    
        if (currentSelect == start_Select)
            StartCoroutine(GoToMainGame());

        else if (currentSelect == setting_Select)
        {
            theAudio.SoundPlay(enter_Sound);
            keyInput = false;
            theVolume.OpenVolume();
        }
        else if (currentSelect == exit_Select)
            StartCoroutine(GoToQuitGame());
    }

    IEnumerator GoToMainGame()
    {
        theFade.FadeOut();
        yield return new WaitForSeconds(2f);
        theGM.LoadGame();
        thePlayer.currentMapName = "Grandma_Room";
        SceneManager.LoadScene("Start");
        theBGM.BgmPlay(3);
    }

    IEnumerator GoToQuitGame()
    {
        theFade.FadeOut();
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
}
