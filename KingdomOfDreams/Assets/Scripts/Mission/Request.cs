using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Request : MonoBehaviour
{
    public Image imgIcon;
    public Text txtName;
    public Text txtProgress;
    private MissionData data;
    private int goal;
    
    private IngredientInfo info;
    private int itemID;

    private Coroutine routine;

    public bool isAchived;

    public void Init(MissionData data, int itemID,int goal)
    {
        //Debug.LogFormat("<color=red>mission update ingredient info id:{0}</color>", info.id);

        this.itemID = itemID;
        this.data = data;

        //EventManager.instance.onMissionUpdate = (itemID) => {
        //    Debug.LogFormat("<color=cyan>미션 업데이트</color>");
        //    UpdateMissionCount(itemID);
        //};


        //if (EventManager.instance.onMissionUpdate == null)
        //{
        //    EventManager.instance.onMissionUpdate = (itemID) => {
        //        UpdateMissionCount(itemID);
        //    };
        //}
        //EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.MISSION_UPDATE, new EventHandler<int>((type, itemID) =>
        //{
        //    UpdateMissionCount(itemID);
        //}));

        //Debug.LogFormat("<color=red>itemID:{0}</color>", itemID);
        this.info = InfoManager.instance.GetIngredientInfo(itemID);
        this.goal = goal;
        this.txtName.text = DataManager.instance.GetIngredientData(itemID).name;

        this.UpdateMissionProgress();

        var atlas = AtlasManager.instance.GetAtlasByName("inventoryItem");
        this.imgIcon.sprite = atlas.GetSprite(DataManager.instance.GetIngredientData(itemID).sprite_name);

        if (this.routine != null)
        {
            StopCoroutine(this.routine);
            this.routine = StartCoroutine(this.CorUpdateMissionProgress());
        }
    }
    private IEnumerator CorUpdateMissionProgress()
    {
        while (true)
        {
            this.UpdateMissionProgress();
            yield return null;
        }
    }

    private void UpdateMissionProgress( )
    {
        //this.info = InfoManager.instance.GetIngredientInfo(itemID);

        if (info == null)
            this.txtProgress.text = string.Format("0 / {0}", goal);
        else
            this.txtProgress.text = string.Format("{0} / {1}", info.amount, goal);

        if (info != null &&this.data !=null&& info.amount >= goal)
        {
            //Debug.LogFormat("<color=yellow>EventManager.instance.onAchieved:{0}</color>", EventManager.instance.onAchieved);
            //Debug.LogFormat("<color=yellow>this.data:{0}</color>", this.data);
            this.isAchived = true;
            //EventManager.instance.onAchieved(this.data);
            //EventDispatcher.instance.SendEvent<MissionData>((int)LHMEventType.eEventType.ACHIEVE_MISSION, this.data);
        }
    }

    public void UpdateMissionCount(int ingredientID)
    {
        var info = InfoManager.instance.GetIngredientInfo(ingredientID);

        //Debug.LogFormat("<color=red>mission update ingredient info id:{0}</color>", info.id);
        if (info.id == this.itemID)
        {
            this.txtProgress.text = string.Format("{0} / {1}", info.amount, goal);

            if (info.amount >= goal)
            {
                this.isAchived = true;
                //Debug.LogFormat("==UpdateMissionCount achived==");
                //Debug.LogFormat("ingredientID: {0} ", ingredientID);
                //EventManager.instance.onAchieved(this.data);
                //EventDispatcher.instance.SendEvent<MissionData>((int)LHMEventType.eEventType.ACHIEVE_MISSION, this.data);
            }
        }

    }
}
