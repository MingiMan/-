using UnityEngine;

public class Bound : MonoBehaviour
{
    [Header("Bound 이름과 transferMapName 이름과 일치해야할 것!")]
    BoxCollider2D bound;
    public string boundName;
    CameraManager theCamera;
    PlayerManager thePlayer;

    [SerializeField] float camSize;
    [SerializeField] float camPosX;
    [SerializeField] float camPosY;


    // 해당 맵을 이동할 때 카메라가 플레이어를 따라다닐지 여부
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
