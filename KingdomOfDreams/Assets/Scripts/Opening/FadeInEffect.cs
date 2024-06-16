using UnityEngine;
using UnityEngine.UI;

public class FadeInEffect : MonoBehaviour
{
    public float fadeTime = 2.0f;
    public float delayTime = 5.0f;
    public Image fadeImage;
    public GameObject text;
    public Camera mainCamera;

    private Color fadeColor;
    private float fadeStartTime;
    private bool isFading;
    private bool isDelaying;
    private float delayStartTime;

    private void Awake()
    {
        fadeColor = mainCamera.backgroundColor;
        fadeColor.a = 1f;
        fadeImage.color = fadeColor;
        isFading = false;
        isDelaying = true;
        delayStartTime = Time.timeSinceLevelLoad;
    }
    private void Update()
    {
        if (isDelaying)
        {
            float elapsedTime = Time.timeSinceLevelLoad - delayStartTime;
            if (elapsedTime > delayTime)
            {
                isDelaying = false;
                isFading = true;
                fadeStartTime = Time.timeSinceLevelLoad;
            }
        }
        else if (isFading)
        {
            float elapsedTime = Time.timeSinceLevelLoad - fadeStartTime;
            if (elapsedTime > fadeTime)
            {
                fadeColor.a = 1f;
                mainCamera.backgroundColor = fadeColor;
                isFading = false;
            }
            else
            {
                float alpha = elapsedTime / fadeTime;
                fadeColor.a = 1 - alpha;
                mainCamera.backgroundColor = fadeColor;
                fadeImage.color = fadeColor;
                text.gameObject.SetActive(false);
            }
        }
    }
}