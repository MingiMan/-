using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;
    [SerializeField] BoxCollider2D boxColl;
    public GameObject playerTarget;
    public float moveSpeed;
    Vector3 targetPosition;

    Vector3 minBound;
    Vector3 maxBound;

    float halfWidth;
    float halfHeight;

    Camera cameraBound;

    [Header("Camera Shake")]
    [SerializeField] Vector2 shake_Offset_1;
    [SerializeField] Vector2 shake_Offset_2;
    [SerializeField] float shakeDuration;
    Vector3 originPos;

    public bool IsCamShake;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        cameraBound = GetComponent<Camera>();
        playerTarget = null;
    }


    private void Update()
    {
        if (playerTarget != null)
        {
            if (!IsCamShake)
            {
                // 플레이어를 따라가는 경우
                targetPosition = new Vector3(playerTarget.transform.position.x, playerTarget.transform.position.y, transform.position.z);
            }
            else
            {
                // 흔들림이 있는 경우
                float posX = Random.Range(-shake_Offset_1.x, shake_Offset_1.x);
                float posY = Random.Range(-shake_Offset_1.y, shake_Offset_1.y);
                Vector3 shakeOffset = new Vector3(posX, posY, 0);
                targetPosition = new Vector3(playerTarget.transform.position.x, playerTarget.transform.position.y, transform.position.z) + shakeOffset;
                targetPosition.z = -10;
            }

            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            float clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
        else
        {
            if (IsCamShake)
            {
                float posX = Random.Range(-shake_Offset_2.x, shake_Offset_2.x);
                float posY = Random.Range(-shake_Offset_2.y, shake_Offset_2.y);

                Vector3 randomPos = originPos + new Vector3(posX, posY, 0);

                randomPos.z = -10;

                if(Vector3.Distance(transform.position, randomPos) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, randomPos, shakeDuration * Time.deltaTime);
                }
            }
        }
    }

    public void SetBound(BoxCollider2D newBound)
    {
        boxColl = newBound;
        minBound = boxColl.bounds.min;
        maxBound = boxColl.bounds.max;
        halfHeight = cameraBound.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    public void ShowCameraShake()
    {
        originPos = transform.position;
        IsCamShake = true;
    }

    public void StopCameraShake()
    {
        StopAllCoroutines();
        IsCamShake = false;
    }
}
