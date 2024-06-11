using UnityEngine;

public class CamResolution : MonoBehaviour
{
    void Awake()
    {
        // ���� GameObject�� ������ Camera ������Ʈ�� �������� �ڵ�
        Camera cam = GetComponent<Camera>();

        // ���� ī�޶��� ����Ʈ ������ �������� �ڵ�
        Rect viewportRect = cam.rect;

        // ���ϴ� ���� ���� ������ ����ϴ� �ڵ�
        float screenAspectRatio = Screen.width / Screen.height;
        float targetAspectRatio = (int)4 / 3; 

        // ȭ�� ���� ���� ������ ���� ����Ʈ ������ �����ϴ� �ڵ�
        if (screenAspectRatio < targetAspectRatio)
        {
            // ȭ���� �� '����'�� (���ΰ� �� ��ٸ�) ���θ� �����ϴ� �ڵ�
            viewportRect.height = screenAspectRatio / targetAspectRatio;
            viewportRect.y = (1f - viewportRect.height) / 2f;
        }
        else

        {
            // ȭ���� �� '�д�'�� (���ΰ� �� ��ٸ�) ���θ� �����ϴ� �ڵ�.
            viewportRect.width = targetAspectRatio / screenAspectRatio;
            viewportRect.x = (1f - viewportRect.width) / 2f;
        }

        // ������ ����Ʈ ������ ī�޶� �����ϴ� �ڵ�

        cam.rect = viewportRect;

        Screen.SetResolution(Screen.width, Screen.height, false);
    }
}
