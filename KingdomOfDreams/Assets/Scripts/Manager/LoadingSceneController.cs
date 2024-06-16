using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneController : MonoBehaviour
{
    private static LoadingSceneController instance;

    public static LoadingSceneController Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<LoadingSceneController>();
                if(obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }

    private static LoadingSceneController Create()
    {
        return Instantiate(Resources.Load<LoadingSceneController>("UILoadingDirector"));
    }

    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        this.slider.onValueChanged.AddListener((val) =>
        {
            text.text = string.Format("·ÎµùÁß...");
        });
    }

    public CanvasGroup canvasGroup;

    public Slider slider;

    public TMP_Text text;


    private string loadSceneName;

    public void LoadScene(string sceneName)
    {
        this.gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess());
    }

    public void LoadSceneInfo(int info)
    {
        this.gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;

        loadSceneName = info.ToString();

        if (loadSceneName == "0")
        {
            loadSceneName = "Tutorial01";
        }
        else if(loadSceneName == "1")
        {
            loadSceneName = "Tutorial02";
        }
        else if(loadSceneName == "2")
        {
            loadSceneName = "Tutorial03";
        }
        else if(loadSceneName == "3")
        {
            loadSceneName = "Tutorial04";
        }
        else if(loadSceneName == "4")
        {
            loadSceneName = "Tutorial05";
        }
        else if(loadSceneName == "5")
        {
            loadSceneName = "Stage06";
        }
        else if(loadSceneName == "6")
        {
            loadSceneName = "Stage07";
        }
        else if(loadSceneName == "7")
        {
            loadSceneName = "Stage08";
        }
        StartCoroutine(LoadSceneProcess());

    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                slider.value = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                slider.value = Mathf.Lerp(0.9f, 1f, timer);
                if (slider.value >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while(timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            this.gameObject.SetActive(false);
        }
    }


}