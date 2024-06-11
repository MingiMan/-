using UnityEngine;

public class Bound : MonoBehaviour
{
    [Header("Bound �̸��� transferMapName �̸��� ��ġ�ؾ��� ��!")]
    BoxCollider2D bound;
    public string boundName;
    CameraManager theCamera;
    PlayerManager thePlayer;

    [SerializeField] float camSize;
    [SerializeField] float camPosX;
    [SerializeField] float camPosY;


    // �ش� ���� �̵��� �� ī�޶� �÷��̾ ����ٴ��� ����
    public bool isCameraPlayerTarget;

    private void Start()
    {
        bound = gameObject.GetComponent<BoxCollider2D>();
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && thePlayer.currentMapName == boundName)
        {
            theCamera.GetComponent<Camera>().orthographicSize = camSize;
            if (isCameraPlayerTarget)
            {   
                theCamera.playerTarget = thePlayer.gameObject;
                theCamera.moveSpeed = 3;
            }
            else
            {
                theCamera.playerTarget = null;
                theCamera.moveSpeed = 0;
                theCamera.transform.position = new Vector3(camPosX, camPosY, theCamera.transform.position.z);
            }
            theCamera.SetBound(bound);
        }
    }
}
