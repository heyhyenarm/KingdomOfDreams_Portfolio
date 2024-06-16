using UnityEngine;

public class AudioSwitcher : MonoBehaviour
{
    public AudioSource mainAudioSource;
    public AudioClip clip1; //메인 브금
    public AudioClip clip2; //컬러1 브금
    public AudioClip clip3; //컬러2 브금
    public AudioClip clip4; //컬러3 브금
    public AudioClip clip5; //컬러4 브금

    public void SwitchAudioSource()
    {
        if (mainAudioSource.clip == clip1) //메인 브금
        {
            mainAudioSource.clip = clip2; //컬러1 브금
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
