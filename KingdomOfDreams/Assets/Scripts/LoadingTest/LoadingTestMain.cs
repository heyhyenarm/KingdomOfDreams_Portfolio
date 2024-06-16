using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingTestMain : MonoBehaviour
{
    public static readonly LoadingTestMain instance = new LoadingTestMain();

    private string changeSceneName;

    private LoadingTestMain() { }
    private void OnEnable()
    {
        Debug.Log("[LoadingTestMain] onEnable");
    }
    private void OnDisable()
    {
        Debug.Log("[LoadingTestMain] onDisable");
        //SoundManager.SetDisableBGM(true);
        var AS = SoundManager.GetCurrentAudioSource();
        if(AS!=null)
            SoundManager.CrossOut(1f, AS);
        //SoundManager.Crossfade(1f, SoundManager.GetCurrentAudioSource(), SoundManager.GetSoundConnectionForThisLevel(this.changeSceneName));
    }
    private void Awake()
    {
        Debug.Log("[LoadingTestMain] Awake");
    }
    private void Start()
    {
        Debug.Log("[LoadingTestMain] Start");
    }
    public AsyncOperation LoadSceneAdditive(string sceneName)
    {
        Debug.Log("******** LoadSceneAdditive ********");
        this.changeSceneName = sceneName;
        return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
    //IEnumerator LoadSceneProcess(AsyncOperation op)
    //{
    //    op.allowSceneActivation = false;

    //    float timer = 0f;
    //    while (!op.isDone)
    //    {
    //        yield return null;

    //        //if (op.progress < 0.9f)
    //        //{
    //        //    slider.value = op.progress;
    //        //}
    //        //else
    //        //{
    //        //    timer += Time.unscaledDeltaTime;
    //        //    slider.value = Mathf.Lerp(0.9f, 1f, timer);
    //        //    if (slider.value >= 1f)
    //        //    {
    //        //        op.allowSceneActivation = true;
    //        //        yield break;
    //        //    }
    //        //}
    //    }
    //}
}
