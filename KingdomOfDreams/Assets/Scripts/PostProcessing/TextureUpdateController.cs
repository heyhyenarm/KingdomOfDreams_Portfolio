using UnityEngine;
using UnityEngine.UI;

public class TextureUpdateController : MonoBehaviour
{
    public Camera targetCamera; // 텍스쳐를 촬영할 카메라
    public RenderTexture targetTexture; // 촬영된 텍스쳐가 적용될 RenderTexture
    public float updateInterval = 0.0333f; // 업데이트 주기 (초 단위)

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
            // 카메라로부터 텍스쳐를 렌더링하여 업데이트
            targetCamera.targetTexture = targetTexture;
            targetCamera.Render();
            targetCamera.targetTexture = null;

            // 텍스쳐를 Raw Image에 직접 적용
            rawImage.texture = targetTexture;

            timer = 0f;
        }
    }
}
