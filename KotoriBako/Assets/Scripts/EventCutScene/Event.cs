using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour
{
    protected FadeManager theFade;
    protected DialogueManager theDM;
    protected AudioManager theAudio;
    protected EventManager theEvent;
    protected PlayerManager thePlayer;
    protected OrderManager theOrder;
    protected BGMManager theBGM;
    protected CameraManager theCam;
    protected GameManager theGM;


    protected virtual void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        theDM = FindObjectOfType<DialogueManager>();
        theAudio = FindObjectOfType<AudioManager>();
        theEvent = FindObjectOfType<EventManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theBGM = FindObjectOfType<BGMManager>();
        theCam = FindObjectOfType<CameraManager>();
        theGM = FindObjectOfType<GameManager>();
    }

    public abstract void ShowEventScene();
}
