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
    //건물 부활 포스트 프로세싱 레이어 변경
    public void PostRevive()
    {
        if (this.worldCam != null)
        {
            //MainCam Culling Mask Nothing으로 바꾸고 UI, Character 추가
            this.worldCam.cullingMask = 0;
            this.worldCam.cullingMask |= 1 << LayerMask.NameToLayer("UI");
            this.worldCam.cullingMask |= 1 << LayerMask.NameToLayer("Character");
        }
        //MainCam Culling Mask Nothing으로 바꾸고 UI, Character 추가
        Camera.main.cullingMask = 0;
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("UI");
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Character");

        //PostCam Culling Mask : Everything으로 바꾸고 UI, Character 제외
        this.postCam.cullingMask = -1;
        this.postCam.cullingMask = this.postCam.cullingMask & ~(1 << LayerMask.NameToLayer("UI"));
        this.postCam.cullingMask = this.postCam.cullingMask & ~(1 << LayerMask.NameToLayer("Character"));
    }
}
