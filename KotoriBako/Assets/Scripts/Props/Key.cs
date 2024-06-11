using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyType
{
    BathRoomKey,
    TatamiKey,
    LibraryKey
}

public class Key : Props
{
    [SerializeField] BathRoom_Wall bathRoom_wall;
    [SerializeField] TatamiRoom_Wall tatamiRoom_Wall;
    [SerializeField] Library_Wall Library_Wall;
    public string get_Sound;

    public KeyType keyType;

    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);
    }

    public override void ShowText()
    {
        switch (keyType)
        {
            case KeyType.BathRoomKey:
                StartCoroutine(BathRoomKeyPropText());
                break;
            case KeyType.TatamiKey:
                StartCoroutine(TatamiKeyPropText());
                break;
            case KeyType.LibraryKey:
                StartCoroutine(LibraryKeyPropText());
                break;
        }
    }

    IEnumerator BathRoomKeyPropText()
    {
        PlayerManager.instance.flag = true;
        Mirror.GetItem = true;
        theDM.ShowText(textDialogue[0]);
        theAudio.SoundPlay(get_Sound);
        yield return new WaitUntil(() => !theDM.talking);
        bathRoom_wall.IsEvent = true;
        PlayerManager.instance.flag = false;
        gameObject.SetActive(false);
    }

    IEnumerator TatamiKeyPropText()
    {
        PlayerManager.instance.flag = true;
        theDM.ShowText(textDialogue[1]); 
        theAudio.SoundPlay(get_Sound);
        yield return new WaitUntil(() => !theDM.talking);
        tatamiRoom_Wall.IsEvent = true;
        PlayerManager.instance.flag = false;
        gameObject.SetActive(false);
    }

    IEnumerator LibraryKeyPropText()
    {
        PlayerManager.instance.flag = true;
        theDM.ShowText(textDialogue[2]);
        theAudio.SoundPlay(get_Sound);
        yield return new WaitUntil(() => !theDM.talking);
        Library_Wall.IsEvent = true;
        PlayerManager.instance.flag = false;
        gameObject.SetActive(false);
    }
}
