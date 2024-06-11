using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Scrollbar;

public class Blood : Props
{
    [SerializeField] GameObject tile;
    [SerializeField] GameObject kotoriBako_3;
    OkOrCancel theOOC;
    public GameObject go_OOC;
    public bool BloodEvent;
    public string boxShake_Sound;

    private void OnEnable()
    {
        tile.gameObject.SetActive(false);
        kotoriBako_3.gameObject.SetActive(false);
        BloodEvent = false;
    }

    protected override void Start()
    {
        base.Start();
        theOOC = FindObjectOfType<OkOrCancel>();
    }

    public override void ShowText()
    {
        if (BloodEvent)
            StartCoroutine(PropText());
    }

    IEnumerator PropText()
    {
        PlayerManager.instance.flag = true;

        theDM.ShowText(textDialogue[0]);

        yield return new WaitUntil(() => !theDM.talking);

        go_OOC.gameObject.SetActive(true);

        theOOC.ShowTwoChoice("네.", "아니오");

        yield return new WaitUntil(() => !theOOC.activated);

        go_OOC.gameObject.SetActive(false);

        if (theOOC.GetResult())
        {
            theAudio.SoundPlay(boxShake_Sound);
            yield return new WaitForSeconds(0.5f);
            PlayerManager.instance.flag = false;
            gameObject.SetActive(false);
            tile.gameObject.SetActive(true);
            kotoriBako_3.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        PlayerManager.instance.flag = false;
    }
}
