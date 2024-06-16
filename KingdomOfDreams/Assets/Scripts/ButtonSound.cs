using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    //public AudioClip soundClip;  // ����� �Ҹ� Ŭ��

    private Button button;
    //private AudioSource audioSource;

    private void Start()
    {
        // ��ư ������Ʈ ��������
        button = GetComponent<Button>();

        // AudioSource ������Ʈ ��������
        //audioSource = GetComponent<AudioSource>();

        // ��ư Ŭ�� �� �Ҹ� ����ϴ� �̺�Ʈ �߰�
        button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        // AudioSource�� ���� �Ҹ� ���
        //audioSource.PlayOneShot(soundClip);
        SoundManager.PlayCappedSFX("SFX_UIButtonClick", "UIButton");
    }
}
