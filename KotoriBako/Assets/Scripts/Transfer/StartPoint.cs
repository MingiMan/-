using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [Header("Grandma,Wardrobe")]
    public string mapName;
    PlayerManager thePlayer;
    [SerializeField]
    string dirName;
    [SerializeField]
    float dirCount;
    EventManager theEvent;
    
    private void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theEvent = FindObjectOfType<EventManager>();    
        if (thePlayer.currentMapName == mapName)
        {
            thePlayer.GetComponent<Animator>().SetFloat(dirName, dirCount);
            thePlayer.transform.position = transform.position;
            theEvent.ShowEvent();
        }
    }
}
