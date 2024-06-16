using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private float masterVolume = 1f;

    private void Awake()
    {
        this.masterVolume = AudioListener.volume;
    }

    public void SetMasterVolume(float volume)
    {
        this.masterVolume = volume;
        //AudioListener.volume = masterVolume;
        SoundManager.SetVolume(this.masterVolume);
    }
}
