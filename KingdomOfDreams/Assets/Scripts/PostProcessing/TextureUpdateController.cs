using UnityEngine;
using UnityEngine.UI;

public class TextureUpdateController : MonoBehaviour
{
    public Camera targetCamera; // �ؽ��ĸ� �Կ��� ī�޶�
    public RenderTexture targetTexture; // �Կ��� �ؽ��İ� ����� RenderTexture
    public float updateInterval = 0.0333f; // ������Ʈ �ֱ� (�� ����)

    private float timer = 0f;
    private RawImage rawImage;

    private void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= updateInterval)
        {
            // ī�޶�κ��� �ؽ��ĸ� �������Ͽ� ������Ʈ
            targetCamera.targetTexture = targetTexture;
            targetCamera.Render();
            targetCamera.targetTexture = null;

            // �ؽ��ĸ� Raw Image�� ���� ����
            rawImage.texture = targetTexture;

            timer = 0f;
        }
    }
}
