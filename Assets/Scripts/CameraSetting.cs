using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public CinemachineFreeLook freeLookCam; // FreeLookCam
    public CinemachineFreeLook dialogueCamRight; // ���� ī�޶�
    public CinemachineFreeLook dialogueCamLeft; // ���� ī�޶�

    void Update()
    {
        // ī�޶��� ����� ��ġ�� �Ǵ��ϴ� �Լ� ȣ��
        if(Input.GetKeyDown(KeyCode.Z)) CheckCameraSide();
    }

    // ī�޶� �÷��̾��� ������ �ִ��� ������ �ִ��� �Ǵ��ϴ� �޼���
    void CheckCameraSide()
    {
        // �÷��̾�� FreeLookCam ������ ���� ���� ���
        Vector3 directionToPlayer = player.position - freeLookCam.transform.position;

        // �÷��̾��� ���� ���� ���� (�÷��̾��� ������)
        Vector3 playerRight = player.right;

        // ��Ʈ���� ����Ͽ� ����, ���� �Ǵ�
        float dotProduct = Vector3.Dot(directionToPlayer, playerRight);

        // ��Ʈ�� ���� ���� ī�޶� Ȱ��ȭ
        if (dotProduct > 0)
        {
            // �÷��̾��� ������ ��ġ
            dialogueCamRight.gameObject.SetActive(true);
            dialogueCamLeft.gameObject.SetActive(false);
        }
        else
        {
            // �÷��̾��� ������ ��ġ
            dialogueCamRight.gameObject.SetActive(false);
            dialogueCamLeft.gameObject.SetActive(true);
        }
    }

}
