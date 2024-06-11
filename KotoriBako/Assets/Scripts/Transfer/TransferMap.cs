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
        if (theCamera.IsCamShake) // ���� ī�޶� ��鸲�� �������̶�� ��� ���߰� �ٸ� ������ �̵��ϰ� �ɵ� �ٽ� ī�޶� ��鸲�� ����Ѵ�.
        {
            theCamera.StopCameraShake();
            IsCameraShaking = true;
        }

        thePlayer.transform.position = targetTransform.position;
        thePlayer.currentMapName = transferMapName;

        if(IsCameraTarget) // ī�޶�  �÷��̾ ���󰡴� ����� �ֱ⶧���� �׷� �κ��� ���ǹ��� �� �� ������ ����� ���������� ī�޶� �����δ�. 
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

        if (EventManager.isActive) // �̺�Ʈ�� �������� ���� �÷��̾ �������̰� �Ѵ�.
            theOrder.NotMove();
        else
            theOrder.CanMove();
    }
}
