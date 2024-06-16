using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionDetail : MonoBehaviour
{
    public Button btnCloseDetail;
    public int clickCount;

    public Slider sliderOverallProgress;
    public Text txtOverallProgress;

    public int ClearMissionCount;
    private int stageMissionCount;
    public UIMissionDetailScrollView scrollview;

    private int stageId;

    private void Start()
    {
        this.btnCloseDetail.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
        });

    }
    public void Init(int stageNum)
    {
        this.CheckStageProgress(stageNum);
        this.CheckClearMission();

        this.scrollview.Init();
    }

    private void CheckStageProgress(int stageNum)
    {
        //Debug.Log(stageNum);

        List<MissionData> missionDatas = DataManager.instance.GetMissionDatas();
        this.stageMissionCount = 0;
        foreach (MissionData data in missionDatas)
        {
            //Debug.LogFormat("id: {0}, stage:{1}",data.id,data.stage_id);
            //Debug.LogFormat("data.stage:{0}, stageNum:{1}", data.stage_id, stageNum);
            if (data.stage_id == stageNum)
            {
                this.stageMissionCount++;
            }         
        }
        //Debug.LogFormat("stagemissioncount:{0}", StageMissionCount);

    }

    public void CheckClearMission()
    {
        this.ClearMissionCount = 0;

        foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
        {
            // Debug.LogFormat("stageinfo:{0}", stageInfo.stage);
            if (!stageInfo.isClear)
            {
                this.stageId = stageInfo.stage + 8000;
                break;
                //Debug.LogFormat("stageid:{0}", this.stageId);
            }
        }

        foreach (MissionData missionData in DataManager.instance.GetMissionDatas())
        {
            if (missionData.stage_id == this.stageId)
            {
                //Debug.LogFormat("stageID:{0}", missionData.stage_id);
                //Debug.LogFormat("missioninfo:{0}", InfoManager.instance.GetMissionInfo(1000).isClear);
                //Debug.LogFormat("missioninfo:{0}", InfoManager.instance.GetMissionInfo(missionData.id));
                if (InfoManager.instance.GetMissionInfo(missionData.id) != null && InfoManager.instance.GetMissionInfo(missionData.id).isClear)
                {
                    this.ClearMissionCount++;
                    EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.CLEAR_MISSION_REFRESH,this.ClearMissionCount);
                }
            }
        }

        this.ShowProgress();
    }

    private void ShowProgress()
    {
        this.sliderOverallProgress.minValue = 0;
        this.sliderOverallProgress.maxValue = this.stageMissionCount;
        this.txtOverallProgress.text = string.Format("{0} / {1}", this.ClearMissionCount, this.stageMissionCount);
        this.sliderOverallProgress.value = ClearMissionCount;
    }
}
