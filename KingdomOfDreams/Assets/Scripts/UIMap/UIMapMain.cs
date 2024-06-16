using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMapMain : MonoBehaviour
{
    public Button btnMap;
    public UIStage uiStage;
    public UIKingdomStage uiKingdomStage;

    public void Init()
    {
        Debug.Log("UIMapMain Init");
        DataManager.instance.LoadStageData();
        this.uiStage.Init();
    }
    void Start()
    {
        //this.uiStage.gameObject.SetActive(true);
        this.uiKingdomStage.gameObject.SetActive(false);

        this.btnMap.onClick.AddListener(() =>
        {
            Debug.Log("btnMap clicked");
            this.uiStage.gameObject.SetActive(true);
        });
        this.uiStage.onClickBack = () =>
        {
            Debug.Log("onClickBack uiStage UIMapMain");
            this.uiStage.gameObject.SetActive(false);
            this.uiKingdomStage.gameObject.SetActive(true);
        };
        this.uiKingdomStage.onClickBack = () =>
        {
            Debug.Log("onClickBack uiKingdomStage UIMapMain");
            this.uiKingdomStage.gameObject.SetActive(false);
        };

        //EventManager.instance.onTutorial01ClearUI = () =>
        //{
        //    StartCoroutine(this.CStageUIClear(1));
        //};

    }
    public IEnumerator CStageUIClear(int stageNum)
    {
        Debug.LogFormat("<color=green>stage : {0} StageUIClear</color>", stageNum);
        this.uiStage.isClearMoving = true;
        var targetStage = this.uiStage.uiStageCells[stageNum].GetComponent<UIStageCell>();
        this.uiStage.nowStage.isClear = true;
        this.uiStage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        this.uiStage.nowStage.GetComponent<UIStageCell>()._btnStageGo.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.5f);

        targetStage._lockGo.SetActive(false);
        yield return new WaitForSeconds(0.3f);

        this.uiStage.SelectStage(targetStage);
        this.uiStage.nowStage = targetStage;

        yield return new WaitForSeconds(3f);

        //this.uiStage.gameObject.SetActive(false);
        Debug.Log("<color=yellow>CStageUIClear done</color>");
        //foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
        //{
        //    Debug.LogFormat("<color>stageInfo.stage : {0}, stageInfo.isClear : {1}</color>", stageInfo.stage, stageInfo.isClear);
        //    if (stageInfo.isClear == false)
        //    {
        //        Debug.LogFormat("stageInfo.stage : {0}", stageInfo.stage);


        //        break;
        //    }
        //}
        for(int i = 0; i < this.uiStage.uiStageCells.Length; i++)
        {
            Debug.LogFormat("stage clear ui to app {0}", i);
            var stageInfo = InfoManager.instance.StageInfos[i];
            if (stageInfo.isClear == false)
            {
                Debug.LogFormat("stageInfo.stage : {0}", stageInfo.stage);
                if (i == 0)
                {
                    EventManager.instance.onTutorial01Clear();
                    //LoadingSceneController.Instance.LoadScene("Tutorial02");
                }
                else if (i == 1)
                {
                    EventManager.instance.onTutorial02Clear();
                    //LoadingSceneController.Instance.LoadScene("Tutorial03");
                }
                else if (i == 2)
                {
                    EventManager.instance.onTutorial03Clear();
                    //LoadingSceneController.Instance.LoadScene("Tutorial04");
                }
                else if (i == 3)
                {
                    EventManager.instance.onTutorial04Clear();
                    //LoadingSceneController.Instance.LoadScene("Tutorial05");
                }
                else if (i == 4)
                {
                    EventManager.instance.onTutorial05Clear();
                    //LoadingSceneController.Instance.LoadScene("Stage06");
                }
                else if (i == 5)
                {
                    EventManager.instance.onStage06Clear();
                    //LoadingSceneController.Instance.LoadScene("Stage07");
                }
                else if (i == 6)
                {
                    //Debug.Log("6");
                    EventManager.instance.onStage07Clear();
                    //LoadingSceneController.Instance.LoadScene("Stage08");
                }
                else if (i == 7)
                {
                    EventManager.instance.onStage08Clear();
                }

                break;
            }
        }
        //EventManager.instance.onTutorial01Clear();
    }

}
