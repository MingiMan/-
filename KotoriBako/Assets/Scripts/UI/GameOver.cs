using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    OrderManager theOrder;
    DialogueManager theDM;
    BGMManager theBgm;
    FadeManager theFade;
    GameManager theGM;
    CameraManager theCamera;
    Camera cam;
    [SerializeField] Dialogue[] dialogues;
    [SerializeField] Image gameOverImage;

    [SerializeField] Vector2 shake_Offset;
    [SerializeField] float shakeDuration;

    public string gameOver1_Sound;
    public string gameOver2_Sound;

    private void Start()
    {
        cam = Camera.main;
        theDM = FindObjectOfType<DialogueManager>();
        theBgm = FindObjectOfType<BGMManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theFade = FindObjectOfType<FadeManager>();
        theGM = FindObjectOfType<GameManager>();
        theCamera = FindObjectOfType<CameraManager>();
        Initialize();
    }

    void Initialize()
    {
        gameObject.GetComponent<Canvas>().worldCamera = cam;
        gameOverImage.gameObject.SetActive(false);
        theGM.transform.position = new Vector3(0, 0, 0);
        theCamera.playerTarget = null;
        theCamera.transform.position = new Vector3(0, 0, theCamera.transform.position.z);
        StartCoroutine(GameOverEvent());
    }   

    IEnumerator GameOverEvent()
    {
        theFade.FadeInRed();
        theBgm.BgmPlay(4);
        theOrder.Turn("Player", "DOWN");
        yield return new WaitForSeconds(1f);
        theFade.FadeIn();
        yield return new WaitForSeconds(0.5f);
        theOrder.Turn("Player", "LEFT");
        yield return new WaitForSeconds(0.5f);
        theOrder.Turn("Player", "RIGHT");
        yield return new WaitForSeconds(0.5f);
        theOrder.Turn("Player", "DOWN");
        yield return new WaitForSeconds(1f);
        theDM.ShowDialogue(dialogues[0]); // ....
        yield return new WaitUntil(() => !theDM.talking);   
        theDM.ShowDialogue(dialogues[1]); // ³¡³µ³ª?
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => !theDM.talking);
        yield return new WaitForSeconds(0.5f);
        theGM.gameObject.SetActive(false);
        gameOverImage.gameObject.SetActive(true);
        StartCoroutine(ImageShakeCoroutine());
        theBgm.BgmPlay(5);
        yield return new WaitForSeconds(2f);
        theFade.FadeOut();
        yield return new WaitForSeconds(1f);
        theGM.gameObject.SetActive(true);
        theGM.LoadTitle();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Title");
    }


    IEnumerator ImageShakeCoroutine()
    {
        Vector2 originPos = gameOverImage.transform.position;
        while (true)
        {
            float posX = Random.Range(-shake_Offset.x, shake_Offset.x);
            float posY = Random.Range(-shake_Offset.y, shake_Offset.y);

            Vector2 randomPos = originPos + new Vector2(posX, posY);
            
            while(Vector2.Distance(gameOverImage.transform.position, randomPos) > 0.1f)
            {
                gameOverImage.transform.position = Vector2.MoveTowards(gameOverImage.transform.position, randomPos, shakeDuration * Time.deltaTime);
                yield return null;
            }
        }
    }
}
