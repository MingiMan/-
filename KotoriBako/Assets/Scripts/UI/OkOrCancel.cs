using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OkOrCancel : MonoBehaviour
{
    AudioManager theAudio;
    OrderManager theOrder;
    public string type_Sound;
    public string enter_Sound;
    public string cancel_Sound;
    public GameObject up_Panel;
    public GameObject down_Panel;


    public Text up_Text;
    public Text down_Text;

    public bool activated;
    bool keyInput;
    bool result = true;

    private void Awake()
    {
        theAudio = FindObjectOfType<AudioManager>();
        theOrder = FindObjectOfType<OrderManager>();
    }

    public void Selected()
    {
        theAudio.SoundPlay(type_Sound);
        result = !result;

        if (result)
        {
            up_Panel.gameObject.SetActive(false);
            down_Panel.gameObject.SetActive(true);
        }
        else
        {
            up_Panel.gameObject.SetActive(true);
            down_Panel.gameObject.SetActive(false);
        }
    }

    public void ShowTwoChoice(string _upText, string _downText)
    {
        theOrder.NotMove();
        EventManager.isActive = true;
        activated = true;
        result = true;
        up_Text.text = _upText;
        down_Text.text = _downText;

        up_Panel.gameObject.SetActive(false);
        down_Panel.gameObject.SetActive(true);

        StartCoroutine(ShowTwoChoiceCoroutine());
    }

    public bool GetResult()
    {
        return result;
    }

    // 중복 실행 방지
    IEnumerator ShowTwoChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
        keyInput = true;
    }

    private void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Selected();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Selected();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                theAudio.SoundPlay(enter_Sound);
                keyInput = false;
                activated = false;
                theOrder.CanMove();
                Invoke("WaitForIsActive", 0.5f);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                theAudio.SoundPlay(cancel_Sound);
                keyInput = false;
                activated = false;
                result = false;
                theOrder.CanMove();
                Invoke("WaitForIsActive", 0.5f);
            }
        }
    }

    void WaitForIsActive()
    {
        EventManager.isActive = false;
    }
}
