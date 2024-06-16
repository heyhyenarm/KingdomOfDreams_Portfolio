using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    //public AudioClip soundClip;  // 재생할 소리 클립

    private Button button;
    //private AudioSource audioSource;

    private void Start()
    {
        // 버튼 컴포넌트 가져오기
        button = GetComponent<Button>();

        // AudioSource 컴포넌트 가져오기
        //audioSource = GetComponent<AudioSource>();

        // 버튼 클릭 시 소리 재생하는 이벤트 추가
        button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        // AudioSource를 통해 소리 재생
        //audioSource.PlayOneShot(soundClip);
        SoundManager.PlayCappedSFX("SFX_UIButtonClick", "UIButton");
    }
}
