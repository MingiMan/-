using System.Collections;
using UnityEngine;

public class WashingMachine : Props
{
    EventManager theEventManager;
    [SerializeField] GameObject bloods;
    [SerializeField] Tub tub;
    public string washMachin_Sound;
    bool IsAcitve;

    public GameObject go_OOC;
    OkOrCancel theOOC;
    OrderManager theOrder;

    bool flag;

    public string footprint_Sound;

    private void OnEnable()
    {
        foreach (Transform child in bloods.transform)
        {
            child.gameObject.SetActive(false);
        }
        IsAcitve = true;
        flag = false;
    }

    private void Update()
    {
        if (PlayerManager.instance.currentMapName == "BathRoom" && IsAcitve)
        {
            if (!flag) 
            {
                WashingMachineOn();
                flag = true;
            }
        }
        else
        {
            if (flag)
            {
                WashingMachineOff();
                flag = false;
            }
        }
    }


    public override void ShowText()
    {
        if (PlayerManager.instance.animor.GetFloat("DirY") == 1 && IsAcitve)
        {
            StartCoroutine(PropEvent());
        }
    }

    IEnumerator PropEvent()
    {
        EventManager.isActive = true;
        foreach (var dialogue in textDialogue)
        {
            PlayerManager.instance.flag = true;
            theDM.ShowText(dialogue);
            yield return new WaitUntil(() => !theDM.talking);
        }
        go_OOC.gameObject.SetActive(true);
        theOOC.ShowTwoChoice("²ö´Ù", "¾È²ö´Ù.");
        yield return new WaitUntil(() => !theOOC.activated);
        go_OOC.gameObject.SetActive(false);

        if (theOOC.GetResult())
        {
            theEventManager.FadeOutPanel();
            yield return new WaitForSeconds(1f);
            theOrder.EventMove("Player", "DOWN", 5);
            theOrder.Turn("Player", "UP");
            theFade.FlashOut();
            yield return new WaitForSeconds(0.5f);
            theFade.FlashIn();
            yield return new WaitForSeconds(0.5f);
            theFade.FadeRed();
            yield return new WaitForSeconds(1f);
            PlayerManager.instance.notMove = true;
            IsAcitve = false;
            WashingMachineOff();

            theAudio.SoundPlay(footprint_Sound);
            yield return new WaitForSeconds(0.4f);
            foreach (Transform child in bloods.transform)
            {
                child.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.3f);
            }
            tub.TubAction();
            yield return new WaitForSeconds(1f);
            theFade.FadeInRed();
            yield return new WaitForSeconds(1f);
            theEventManager.FadeInPanel();
        }
        yield return new WaitForSeconds(0.5f);
        PlayerManager.instance.notMove = false;
        EventManager.isActive = false;
        PlayerManager.instance.flag = false;
    }

    protected override void Start()
    {
        base.Start();
        animor = GetComponent<Animator>();
        theOOC = FindObjectOfType<OkOrCancel>();
        theOrder = FindObjectOfType<OrderManager>();
        theEventManager = FindObjectOfType<EventManager>();
    }

    public void WashingMachineOn()
    {
        theAudio.SoundPlay(washMachin_Sound);
        animor.SetBool("IsWorking", true);
    }

    public void WashingMachineOff()
    {
        theAudio.SoundStop(washMachin_Sound);
        animor.SetBool("IsWorking", false);
    }
}
