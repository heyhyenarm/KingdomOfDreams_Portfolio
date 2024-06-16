using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class Tuto02MissionMain : MonoBehaviour
{
    public UITutorial02Director director;
    public bool isFirstMission;

    //private float delta;

    //Main도 하나로 합치기 가능..?
    public void Init()
    {
        DataManager.instance.LoadMissionData();
        DataManager.instance.LoadDialogueData();
        DataManager.instance.LoadIngredientData();

        Debug.LogFormat("mission path:{0}", InfoManager.instance.missionPath);

        var app = GameObject.FindObjectOfType<App>();
        if (app.isNewbie)
        {
            Debug.Log("신규유저");
            isFirstMission = true;
            //Debug.LogFormat("트루임? : {0}", isFirstMission);
            InfoManager.instance.MissionInfoInit();
        }
        else
        {
            Debug.Log("기존유저");
            InfoManager.instance.LoadMissionInfo();
            if (!InfoManager.instance.IsNewbie(InfoManager.instance.ingredientPath))
            {
                InfoManager.instance.LoadIngredientInfos();
            }
        }

        //if (InfoManager.instance.IsMissionNewbie(InfoManager.instance.missionPath))
        //{
        //    Debug.Log("신규유저");
        //    isFirstMission = true;
        //    //Debug.LogFormat("트루임? : {0}", isFirstMission);
        //    InfoManager.instance.MissionInfoInit();

        //}
        //else
        //{
        //    Debug.Log("기존유저");
        //    InfoManager.instance.LoadMissionInfo();
        //    if (!InfoManager.instance.IsMissionNewbie(InfoManager.instance.ingredientPath))
        //    {
        //        InfoManager.instance.LoadIngredientInfos();
        //    }          
        //}
    }
    public void GetReward(int id)
    {
        var data = DataManager.instance.GetMissionData(id);

        Debug.LogFormat("미션 보상 꿈 {0}개 받음", data.reward_dream_count);

        var info = InfoManager.instance.MissionInfos.Find(x => x.id == data.id);

        info.isClear = true;
        InfoManager.instance.SaveMissionInfos();
    }
}
