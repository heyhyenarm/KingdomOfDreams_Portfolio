using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingRevive : MonoBehaviour
{
    public Camera postCam;
    public Camera worldCam;
    public GameObject followCam;

    void Start()
    {

    }

    //private void Update()
    //{
    //    if (this.followCam != null) this.postCam.transform.position = this.followCam.transform.position;
    //}
    //�ǹ� ��Ȱ ����Ʈ ���μ��� ���̾� ����
    public void PostRevive()
    {
        if (this.worldCam != null)
        {
            //MainCam Culling Mask Nothing���� �ٲٰ� UI, Character �߰�
            this.worldCam.cullingMask = 0;
            this.worldCam.cullingMask |= 1 << LayerMask.NameToLayer("UI");
            this.worldCam.cullingMask |= 1 << LayerMask.NameToLayer("Character");
        }
        //MainCam Culling Mask Nothing���� �ٲٰ� UI, Character �߰�
        Camera.main.cullingMask = 0;
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("UI");
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Character");

        //PostCam Culling Mask : Everything���� �ٲٰ� UI, Character ����
        this.postCam.cullingMask = -1;
        this.postCam.cullingMask = this.postCam.cullingMask & ~(1 << LayerMask.NameToLayer("UI"));
        this.postCam.cullingMask = this.postCam.cullingMask & ~(1 << LayerMask.NameToLayer("Character"));
    }
}
