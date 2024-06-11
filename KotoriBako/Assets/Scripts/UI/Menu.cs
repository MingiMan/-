using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    Camera cam;
    AudioManager theAudio;
    VolumeManager theVolume;
    BGMManager theBGM;
    FadeManager theFade;
    PlayerManager thePlayer;
    OrderManager theOrder;
    GameManager theGM;

    bool activated;
    bool keyInput;
    bool IsVolumeAcitve;

    int selectFontSize = 25;
    int unSelectFontSize = 20;

    [Header("Sound")]
    public string cancel_Sound;
    public string enter_Sound;
    public string type_Sound;

    [Header("Select")]
    [SerializeField] Text start_Select;
    [SerializeField] Text setting_Select;
    [SerializeField] Text titleExit_Select;
    Text currentSelect;

    private void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        theBGM = FindObjectOfType<BGMManager>();
        theFade = FindObjectOfType<FadeManager>();
        theVolume = FindObjectOfType<VolumeManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theGM = FindObjectOfType<GameManager>();
        cam = Camera.main;
        Initial();
    }

    void Initial()
    {
        currentSelect = start_Select;
        ApplyTextSetting(start_Select, selectFontSize, Color.red);
        ApplyTextSetting(setting_Select, unSelectFontSize, Color.white);
        ApplyTextSetting(titleExit_Select, unSelectFontSize, Color.white);
        gameObject.GetComponent<Canvas>().worldCamera = cam;
        keyInput = false;
        IsVolumeAcitve = false;
        activated = false;
        menu.gameObject.SetActive(false);
    }

    void ApplyTextSetting(Text text, int _fontSize, Color _color)
    {
        text.fontSize = _fontSize;
        text.color = _color;
    }

    private void Update()
    {
        if (!EventManager.isActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X) && !IsVolumeAcitve)
            {
                theAudio.SoundPlay(enter_Sound);
                activated = !activated;

                if (activated)
                {
                    keyInput = true;
                    theOrder.NotMove();
                    menu.SetActive(true);
                }

                else
                {
                    theAudio.SoundPlay(enter_Sound);
                    theOrder.CanMove();
                    menu.SetActive(false);
                    Initial();
                }
            }

            else if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape) && IsVolumeAcitve)
            {

                theAudio.SoundPlay(cancel_Sound);
                menu.gameObject.SetActive(true);
                keyInput = true;
                IsVolumeAcitve = false;
                theVolume.CloseVolume();

            }
            InMenu();
        }
    }

    void InMenu()
    {
        if (menu.activeSelf && activated && keyInput)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                SwitchSelect(false);
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                SwitchSelect(true);
            else if (Input.GetKeyDown(KeyCode.Z))
                ChoiceMenu();
        }
    }

    void SwitchSelect(bool previous)
    {
        theAudio.SoundPlay(type_Sound);
        ApplyTextSetting(currentSelect, unSelectFontSize, Color.white);
        if (previous)
        {
            if (currentSelect == start_Select)
                currentSelect = titleExit_Select;

            else if (currentSelect == titleExit_Select)
                currentSelect = setting_Select;

            else if (currentSelect == setting_Select)
                currentSelect = start_Select;
        }
        else
        {
            if (currentSelect == start_Select)
                currentSelect = setting_Select;
            else if (currentSelect == setting_Select)
                currentSelect = titleExit_Select;
            else if (currentSelect == titleExit_Select)
                currentSelect = start_Select;
        }
        ApplyTextSetting(currentSelect, selectFontSize, Color.red);
    }

    void ChoiceMenu()
    {
        theAudio.SoundPlay(enter_Sound);

        if (currentSelect == start_Select)
            Continue();

        else if (currentSelect == setting_Select)
            OpenVolume();

        else if (currentSelect == titleExit_Select)
            ShowTitle();
    }

    public void Continue()
    {
        activated = false;
        theOrder.CanMove();
        menu.SetActive(false);
        Initial();
    }

    public void OpenVolume()
    {
        keyInput = false;
        IsVolumeAcitve = true;
        menu.gameObject.SetActive(false);
        theVolume.OpenVolume();
    }

    public void ShowTitle()
    {
        StartCoroutine(GoToTitle());
    }

    IEnumerator GoToTitle()
    {
        keyInput = false;
        theFade.FadeOut();
        menu.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        theGM.LoadTitle();
        SceneManager.LoadScene("Title");
        yield return new WaitForSeconds(0.5f);
    }
}
