using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public RenderTexture renderTexture;

    private void Start()
    {
        // 첫 번째 카메라 설정
        camera1.targetTexture = renderTexture;
        camera1.enabled = true;

        // 두 번째 카메라 설정
        camera2.enabled = true;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // 두 번째 카메라의 출력을 첫 번째 카메라의 Render Texture에서 가져와 다른 위치에 렌더링
        Graphics.Blit(renderTexture, destination);
    }
}
