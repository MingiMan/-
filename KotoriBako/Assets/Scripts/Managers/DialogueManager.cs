using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public Text text;
    public SpriteRenderer sprite;
    public SpriteRenderer dialogueWindow;
    public GameObject talk_Cursor;

    OrderManager theOrder;

    List<string> listSentences;
    List<Sprite> listSprites;
    List<Sprite> listDialogueWindows;

    int count; // 대화 진행 상황

    public Animator animorSprite;
    public Animator animorDialogueWindow;

    AudioManager theAudio;

    public string typeSound;
    public string enterSound;
    public bool talking = false;
    bool keyActivated = false;

    bool onlyText = false;

    [Header("PassWard")]
    public bool IsCorrect;
    public string correct_Sound;
    public string fail_Sound;
    public string cancle_Sound;
    bool IsPassward;
    int[] passwordDigits = new int[3] { 0, 0, 0 };
    int correctNumber;
    int selectedDigit = 0;


    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowText(TextDialogue propsDialogue)
    {
        talking = true;
        onlyText = true;
        for (int i = 0; i < propsDialogue.sentences.Length; i++)
        {
            listSentences.Add(propsDialogue.sentences[i]);
            listDialogueWindows.Add(propsDialogue.dialogueWindow[i]);
        }
        StartCoroutine(StartTextCoroutine());

    }

    IEnumerator StartTextCoroutine()
    {
        animorDialogueWindow.SetBool("Appear", true);
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i];
            if (i % 7 == 1)
            {
                theAudio.SoundPlay(typeSound);
            }
            yield return new WaitForSeconds(0.01f);
        }
        keyActivated = true;
        TalkCursorActive();
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        talking = true;
        onlyText = false;
        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            listSentences.Add(dialogue.sentences[i]);
            listSprites.Add(dialogue.sprites[i]);
            listDialogueWindows.Add(dialogue.dialogueWindow[i]);
        }

        animorSprite.SetBool("Appear", true);
        animorDialogueWindow.SetBool("Appear", true);
        StartCoroutine(StartDialogueCoroutine());
    }

    public void ShowPassWardDialogue(int _correctNumber)
    {
        listSentences.Add("0 0 0");
        theOrder.NotMove();
        passwordDigits = new int[3] { 0, 0, 0 };
        selectedDigit = 0;
        IsPassward = true;
        talking = true;
        IsCorrect = false;
        animorDialogueWindow.SetBool("Appear", true);
        correctNumber = _correctNumber;
        EventManager.isActive = true;
        UpdatePasswordDisplay();
    }

    private void UpdatePasswordDisplay()
    {
        text.text = $"{passwordDigits[0]} {passwordDigits[1]} {passwordDigits[2]}";
    }



    IEnumerator StartDialogueCoroutine()
    {
        if (count > 0)
        {
            if (listDialogueWindows[count] != listDialogueWindows[count - 1])
            {
                animorSprite.SetBool("Change", true);
                animorDialogueWindow.SetBool("Appear", false);
                yield return new WaitForSeconds(0.2f);
                dialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
                sprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
                animorDialogueWindow.SetBool("Appear", true);
                animorSprite.SetBool("Change", false);
            }
            else
            {
                if (listSprites[count] != listSprites[count - 1])
                {
                    animorSprite.SetBool("Change", true);
                    yield return new WaitForSeconds(0.1f);
                    sprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
                    animorSprite.SetBool("Change", false);
                }
                else
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        else
        {
            dialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
            sprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
        }


        // 0 번째문장에 총길이를 반복하겠다.
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i]; // 1글자 씩 출력

            if (i % 7 == 1)
            {
                theAudio.SoundPlay(typeSound);
            }
            yield return new WaitForSeconds(0.01f);
        }

        keyActivated = true;
        TalkCursorActive();
    }


    public void ExitDialogue()
    {
        text.text = "";
        count = 0;
        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        animorDialogueWindow.SetBool("Appear", false);
        animorSprite.SetBool("Appear", false);
        talking = false;
        IsPassward = false;
        theOrder.CanMove();
    }

    private void Start()
    {
        count = 0;
        text.text = "";
        theAudio = FindObjectOfType<AudioManager>();
        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();
        theOrder = FindObjectOfType<OrderManager>();
        TalkCursorUnActive();
    }

    private void Update()
    {
        if (talking && keyActivated && !IsPassward)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                keyActivated = false;
                count++;
                text.text = "";
                theAudio.SoundPlay(enterSound);

                if (count == listSentences.Count)
                {
                    TalkCursorUnActive();
                    StopAllCoroutines();
                    ExitDialogue();
                }
                else
                {
                    TalkCursorUnActive();
                    StopAllCoroutines();
                    if (onlyText)
                        StartCoroutine(StartTextCoroutine());
                    else
                        StartCoroutine(StartDialogueCoroutine());
                }
            }
        }
        else if (IsPassward && talking)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                passwordDigits[selectedDigit] = (passwordDigits[selectedDigit] + 1) % 10;
                theAudio.SoundPlay(typeSound);
                UpdatePasswordDisplay();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                passwordDigits[selectedDigit] = (passwordDigits[selectedDigit] + 9) % 10;
                theAudio.SoundPlay(typeSound);
                UpdatePasswordDisplay();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                theAudio.SoundPlay(typeSound);
                selectedDigit = (selectedDigit + 1) % 3;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                theAudio.SoundPlay(typeSound);
                selectedDigit = (selectedDigit + 2) % 3;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(CorrectAnswer());
            }

            else if (Input.GetKeyDown(KeyCode.X))
            {
                EventManager.isActive = false;
                TalkCursorUnActive();
                StopAllCoroutines();
                ExitDialogue();
                theAudio.SoundPlay(cancle_Sound);
            }
        }
    }

    IEnumerator CorrectAnswer()
    {
        theAudio.SoundPlay(enterSound);
        yield return new WaitForSeconds(0.5f);

        if (passwordDigits[0] == (correctNumber / 100) % 10
                && passwordDigits[1] == (correctNumber / 10) % 10
                && passwordDigits[2] == (correctNumber % 10))
        {
            IsCorrect = true;
            EventManager.isActive = false;
            TalkCursorUnActive();
            StopAllCoroutines();
            ExitDialogue();
            theAudio.SoundPlay(correct_Sound);
        }
        else
        {
            IsCorrect = false;
            EventManager.isActive = false;
            TalkCursorUnActive();
            StopAllCoroutines();
            ExitDialogue();
            theAudio.SoundPlay(fail_Sound);
        }
    }

    public void TalkCursorActive()
    {
        talk_Cursor.gameObject.SetActive(true);
        theOrder.NotMove();
    }

    public void TalkCursorUnActive()
    {
        talk_Cursor.gameObject.SetActive(false);
        talk_Cursor.GetComponent<Animator>().Rebind();
    }
}
