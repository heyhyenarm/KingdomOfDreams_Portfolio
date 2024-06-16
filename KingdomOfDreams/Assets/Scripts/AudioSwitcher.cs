using UnityEngine;

public class AudioSwitcher : MonoBehaviour
{
    public AudioSource mainAudioSource;
    public AudioClip clip1; //���� ���
    public AudioClip clip2; //�÷�1 ���
    public AudioClip clip3; //�÷�2 ���
    public AudioClip clip4; //�÷�3 ���
    public AudioClip clip5; //�÷�4 ���

    public void SwitchAudioSource()
    {
        if (mainAudioSource.clip == clip1) //���� ���
        {
            mainAudioSource.clip = clip2; //�÷�1 ���
        }
        else if(mainAudioSource.clip == clip2)
        {
            mainAudioSource.clip = clip3;
        }
        else if(mainAudioSource.clip == clip3)
        {
            mainAudioSource.clip = clip4;
        }
        else if(mainAudioSource.clip == clip4)
        {
            mainAudioSource.clip = clip5;
        }

        mainAudioSource.Play();
    }
}
