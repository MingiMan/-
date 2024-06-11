using System.Collections;
using UnityEngine;

public class MiniGameHandler : MonoBehaviour
{
    public TextDialogue dialogue;
    public MiniGameType miniGameType;
    public GameObject go_OOC;
    MiniGameManager theMiniGame;
    DialogueManager theDM;
    OkOrCancel theOOC;
    OrderManager theOrder;
    AudioManager theAudio;
    PlayerManager thePlayer;
    Animator animor;
    public string openBox_Sound;

    public bool Interaction;

    private void Start()
    {
        theMiniGame = FindObjectOfType<MiniGameManager>();
        theOOC = FindObjectOfType<OkOrCancel>();
        theOrder = FindObjectOfType<OrderManager>();
        theDM = FindObjectOfType<DialogueManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theAudio = FindObjectOfType<AudioManager>();
        animor = GetComponent<Animator>();
    }

    void OnEnable()
    {
        Interaction = true;
    }

    public void ShowMiniGame()
    {
        if(Interaction)
            StartCoroutine(OOCCoroutine());
    }

    IEnumerator OOCCoroutine()
    {
        Interaction = false;
        thePlayer.flag = true;
        theDM.ShowText(dialogue);
        theOrder.NotMove();
        yield return new WaitUntil(() => !theDM.talking);
        EventManager.isActive = true;
        go_OOC.gameObject.SetActive(true);
        theOOC.ShowTwoChoice("한다.", "안한다.");

        yield return new WaitUntil(() => !theOOC.activated);

        if (theOOC.GetResult())
        {
            StartCoroutine(EnterMiniGame());
        }
        else
        {
            StartCoroutine(ExitMiniGame());
        }
    }

    IEnumerator EnterMiniGame()
    {
        go_OOC.gameObject.SetActive(false);
        animor.SetBool("Open", true);
        theAudio.SoundPlay(openBox_Sound);
        yield return new WaitForSeconds(0.8f);
        theMiniGame.PlayMiniGame(miniGameType,this.gameObject);
        yield return new WaitUntil(() => !theMiniGame.IsActivated);
        if (theMiniGame.DestroyKotoriBako)
            Interaction = false;
        else
            Interaction = true;

        EventManager.isActive = false;
        theOrder.CanMove();
        thePlayer.flag = false;
    }

    IEnumerator ExitMiniGame()
    {
        Interaction = true;
        EventManager.isActive = false;
        go_OOC.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        thePlayer.flag = false;
        theOrder.CanMove();
    }
}
