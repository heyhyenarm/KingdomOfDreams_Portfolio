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
        // ù ��° ī�޶� ����
        camera1.targetTexture = renderTexture;
        camera1.enabled = true;

        // �� ��° ī�޶� ����
        camera2.enabled = true;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // �� ��° ī�޶��� ����� ù ��° ī�޶��� Render Texture���� ������ �ٸ� ��ġ�� ������
        Graphics.Blit(renderTexture, destination);
    }
}
