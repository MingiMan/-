using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    FadeManager theFade;
    PlayerManager thePlayer;
    PlayerStatusManager thePlayerStatusManager;
    CameraManager theCamera;
    Camera cam;
    OrderManager theOrder;
    AudioManager theAudio;
    BGMManager theBgm;

    public void LoadGame()
    {
        StartCoroutine(LoadGameStart());
    }

    public void LoadTitle()
    {
        StartCoroutine(LoadTitleStart());
    }

    public void LoadGameOver()
    {
        StartCoroutine (LoadGameOverStart());
    }

    
    void Initial()
    {
        theFade = FindObjectOfType<FadeManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        cam = FindObjectOfType<Camera>();
        theOrder = FindObjectOfType<OrderManager>();
        theCamera = FindObjectOfType<CameraManager>();
        thePlayerStatusManager = FindObjectOfType<PlayerStatusManager>();
        theAudio= FindObjectOfType<AudioManager>();
        theBgm = FindObjectOfType<BGMManager>();
    }

    IEnumerator LoadGameStart()
    {
        Initial();
        yield return new WaitForSeconds(0.5f);
        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;
        theFade.GetComponent<Canvas>().worldCamera = cam;
        thePlayerStatusManager.GetComponent<Canvas>().worldCamera = cam;
        yield return new WaitForSeconds(2f);
        theFade.FadeIn();
    }

    IEnumerator LoadTitleStart()
    {
        yield return new WaitForSeconds(0.5f);
        Initial();
        thePlayer.GetComponent<Animator>().Rebind();
        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 0f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;
        theOrder.NotMove();
        yield return new WaitForSeconds(2f);
        theFade.FadeIn();
    }

    IEnumerator LoadGameOverStart()
    {   
        Initial();
        StopAllCoroutines();
        theFade.FadeInRed();
        theFade.FadeOut();
        yield return new WaitForSeconds(3f);
        theFade.FadeIn();
        gameObject.transform.position = new Vector3(0, 0, 0);
        theCamera.playerTarget = null;
        theCamera.transform.position = new Vector3(0, 0, theCamera.transform.position.z);
        thePlayerStatusManager.GetComponent<Canvas>().worldCamera = cam;
        theFade.GetComponent<Canvas>().worldCamera = cam;
        thePlayer.currentMapName = "GameOver";
    }
}
