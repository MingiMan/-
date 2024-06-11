using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class Computer : Props
{
    [SerializeField] PlayableDirector kotoriBako_4;
    [SerializeField] EventCutScene_5 eventCutScene_5;
    [SerializeField] GameObject computerLight;
    EventManager theEventManager;
    public int correct_Number;
    bool IsEvent;


    private void Awake()
    {
        animor = GetComponent<Animator>();
        theEventManager = FindObjectOfType<EventManager>();
    }
    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        kotoriBako_4.enabled = false;
        IsEvent = true;
        computerLight.gameObject.SetActive(true);
    }

    public override void ShowText()
    {
        if (PlayerManager.instance.animor.GetFloat("DirY") == 1 && IsEvent)
        {
            StartCoroutine(PropText());
        }
    }

    IEnumerator PropText()
    {
        foreach (var dialogue in textDialogue)
        {
            PlayerManager.instance.flag = true;
            theDM.ShowText(dialogue);
            yield return new WaitUntil(() => !theDM.talking);
        }
        theDM.ShowPassWardDialogue(correct_Number);
        yield return new WaitUntil(() => !theDM.talking);
        if (theDM.IsCorrect)
        {
            yield return new WaitForSeconds(0.5f);
            eventCutScene_5.ShowEventScene();
            yield return new WaitUntil(() => !eventCutScene_5.isAcitve);
            theEventManager.FadeOutPanel();
            yield return new WaitForSeconds(1f);
            kotoriBako_4.enabled = true;
            animor.SetTrigger("IsWorking");
            computerLight.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            theEventManager.FadeInPanel();
            IsEvent = false;
        }
        yield return new WaitForSeconds(0.5f);
        PlayerManager.instance.flag = false;
    }
}
