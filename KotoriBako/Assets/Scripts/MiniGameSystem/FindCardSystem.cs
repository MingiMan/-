using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FindCardSystem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    Sprite[] card;
    [SerializeField]
    Sprite[] differentCard;
    [SerializeField] Transform cards;
    public int totalCard;
    bool mouseInput;
    public float playerTimer;
    int playCount;
    int nextCard;


    void OnEnable()
    {
        playCount = 0;
        nextCard = 0;
        mouseInput = false;
        totalCard = 36;
    }


    public void ShowGame()
    {
        MiniGameManager.Instance.PlayTime(playerTimer);
        CreatGenerateAICard();
    }

    void CreatGenerateAICard()
    {
        foreach (RectTransform childCard in cards.GetComponent<RectTransform>())
        {
            RectTransform childRect = childCard.GetComponent<RectTransform>(); // 버그 해결
            if (childRect != null)
            {
                Destroy(childCard.gameObject);
            }
        }

        int randomCard = Random.Range(0, totalCard);
        for (int i = 0; i < totalCard; i++)
        {
            GameObject obj = new GameObject();
            Image cardObj = obj.AddComponent<Image>();
            cardObj.sprite = card[nextCard];
            cardObj.name = "Card " + i;
            cardObj.transform.SetParent(cards.transform, false);
            if (i == randomCard)
            {
                cardObj.GetComponent<Image>().sprite = differentCard[nextCard];
                cardObj.name = "Joker";
            }
        }
        mouseInput = true;
    }

    void ConfigureGameLevel()
    {
        mouseInput = false;
        playCount++;
        nextCard++;

        foreach (RectTransform childCard in cards.GetComponent<RectTransform>())
        {
            RectTransform childRect = childCard.GetComponent<RectTransform>(); // 버그 해결
            if (childRect != null)
            {
                Destroy(childCard.gameObject);
            }
        }

        switch (playCount)
        {
            case 1:
                CreatGenerateAICard();
                break;

            case 2:
                CreatGenerateAICard();
                break;

            case 3:
                EndGame();
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (mouseInput)
        {
            GameObject clickedCard = eventData.pointerCurrentRaycast.gameObject;
            if (clickedCard.name == "Joker")
                ConfigureGameLevel();
            else
            {
                // 순서 바꾸지마
                MiniGameManager.Instance.GameOver();
                gameObject.SetActive(false);
            }
        }
    }

    public void EndGame()
    {
        MiniGameManager.Instance.MiniGameClear();
        gameObject.SetActive(false);
    }
}
