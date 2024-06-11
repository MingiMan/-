using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TransferMap : MonoBehaviour
{
    [Header("DirX or DirY")]
    [SerializeField] private string playerAnimatroDir;
    [SerializeField] private string transferMapName;
    [Header("UP DOWN LEFT RIGHT")]
    [SerializeField] private string direction;
    int playerDir;

    [SerializeField] private Transform targetTransform;
    [SerializeField] private BoxCollider2D targetBound;

    private PlayerManager thePlayer;
    private CameraManager theCamera;
    private FadeManager theFade;
    private OrderManager theOrder;
    private AudioManager theAudio;

    private Animator playerAnimator;

    private enum Direction { UP, DOWN, LEFT, RIGHT }
    private Direction transferDirection;

    public bool IsCameraTarget;

    bool IsCameraShaking;

    private void Awake()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theCamera = FindObjectOfType<CameraManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theAudio = FindObjectOfType<AudioManager>();

        PlayerDir();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerAnimator = collision.gameObject.GetComponent<Animator>();

            if (transferDirection == Direction.UP && playerAnimator.GetFloat("DirY") == 1 ||
                transferDirection == Direction.DOWN && playerAnimator.GetFloat("DirY") == -1 ||
                transferDirection == Direction.LEFT && playerAnimator.GetFloat("DirX") == -1 ||
                transferDirection == Direction.RIGHT && playerAnimator.GetFloat("DirX") == 1)
            {
                StartCoroutine(TransferCoroutine());
            }
        }
    }

    private void PlayerDir()
    {
        switch (direction)
        {
            case "UP":
                transferDirection = Direction.UP;
                playerDir = 1;
                break;
            case "DOWN":
                transferDirection = Direction.DOWN;
                playerDir = -1;
                break;
            case "LEFT":
                transferDirection = Direction.LEFT;
                playerDir = -1;
                break;
            case "RIGHT":
                transferDirection = Direction.RIGHT;
                playerDir = 1;
                break;
            default:
                transferDirection = Direction.UP;
                break;
        }
    }

    private IEnumerator TransferCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        theOrder.SetTransparent("Player");
        theAudio.SoundPlay("openDoor_Sound");
        yield return new WaitForSeconds(0.2f);
        theFade.FadeOut();
        yield return new WaitForSeconds(0.8f);
        if (theCamera.IsCamShake) // 현재 카메라 흔들림이 진행중이라면 잠시 멈추고 다른 맵으로 이동하게 될따 다시 카메라 흔들림을 사용한다.
        {
            theCamera.StopCameraShake();
            IsCameraShaking = true;
        }

        thePlayer.transform.position = targetTransform.position;
        thePlayer.currentMapName = transferMapName;

        if(IsCameraTarget) // 카메라가  플레이어를 따라가는 방들이 있기때문에 그런 부분은 조건문을 한 번 실행을 해줘야 정상적으로 카메라가 움직인다. 
            theCamera.transform.position = new Vector3(targetTransform.transform.position.x, targetTransform.transform.position.y, theCamera.transform.position.z);

        playerAnimator.SetFloat(playerAnimatroDir, playerDir);
        yield return new WaitForSeconds(0.3f);
        theFade.FadeIn();
        theOrder.SetUnTransparent("Player");

        if (IsCameraShaking)
        {
            theCamera.ShowCameraShake();
            IsCameraShaking = false;
        }
        yield return new WaitForSeconds(0.7f);

        if (EventManager.isActive) // 이벤트가 실행중일 때는 플레이어가 못움직이게 한다.
            theOrder.NotMove();
        else
            theOrder.CanMove();
    }
}
