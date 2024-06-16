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

    //Main�� �ϳ��� ��ġ�� ����..?
    public void Init()
    {
        DataManager.instance.LoadMissionData();
        DataManager.instance.LoadDialogueData();
        DataManager.instance.LoadIngredientData();

        Debug.LogFormat("mission path:{0}", InfoManager.instance.missionPath);

        var app = GameObject.FindObjectOfType<App>();
        if (app.isNewbie)
        {
            Debug.Log("�ű�����");
            isFirstMission = true;
            //Debug.LogFormat("Ʈ����? : {0}", isFirstMission);
            InfoManager.instance.MissionInfoInit();
        }
        else
        {
            Debug.Log("��������");
            InfoManager.instance.LoadMissionInfo();
            if (!InfoManager.instance.IsNewbie(InfoManager.instance.ingredientPath))
            {
                InfoManager.instance.LoadIngredientInfos();
            }
        }

        //if (InfoManager.instance.IsMissionNewbie(InfoManager.instance.missionPath))
        //{
        //    Debug.Log("�ű�����");
        //    isFirstMission = true;
        //    //Debug.LogFormat("Ʈ����? : {0}", isFirstMission);
        //    InfoManager.instance.MissionInfoInit();

        //}
        //else
        //{
        //    Debug.Log("��������");
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

        Debug.LogFormat("�̼� ���� �� {0}�� ����", data.reward_dream_count);

        var info = InfoManager.instance.MissionInfos.Find(x => x.id == data.id);

        info.isClear = true;
        InfoManager.instance.SaveMissionInfos();
    }
}
