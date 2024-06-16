using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionMini : MonoBehaviour
{
    public Transform contentTrans;
    public GameObject inProgressMissionPrefab;
    public Button[] btnOpenDetails;
    public System.Action onMissionMiniClick;
    public List<UIInProgressMissionCell> progressMissionCellList=new List<UIInProgressMissionCell>();


    public void Init()
    {

        this.RefreshMissionCell();

        int index = 0;
        foreach (var info in InfoManager.instance.MissionInfos)
        {
            //Debug.LogFormat("<color=magenta>foreach ex info.id:{0}</color>", info.id);
            if (!info.isClear)
            {
                //Debug.LogFormat("<color=magenta>info.id:{0}</color>", info.id);
                GameObject go = Instantiate(inProgressMissionPrefab, this.contentTrans);
                UIInProgressMissionCell cell = go.GetComponent<UIInProgressMissionCell>();
                cell.Init(index);
                this.progressMissionCellList.Add(cell);
            }
            index++;
        }   
        ////실행마다 리스트 생성됨 (수정 필요)
        //for (int i = 0; i < InfoManager.instance.MissionInfos.Count; i++)
        //{
            
        //    //Debug.Log("진행중인 미션 리스트 생성하기");
        //    GameObject go = Instantiate(inProgressMissionPrefab, this.contentTrans);
        //    go.GetComponent<UIInProgressMissionCell>().Init(i);
        //}

        for(int i = 0; i < btnOpenDetails.Length; i++)
        {
            this.btnOpenDetails[i].onClick.AddListener(() =>
            {
                this.onMissionMiniClick();
            });
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.contentTrans.GetComponent<ContentSizeFitter>().transform);

    }

    public void RefreshMissionCell()
    {
        //Debug.LogFormat("<color=magenta>mission mini refresh</color>");
        //Debug.LogFormat("<color=magenta>mission mini refresh count:{0}</color>", progressMissionCellList.Count);

        foreach (var cell in this.progressMissionCellList)
        {
            Destroy(cell.gameObject);
        }
        this.progressMissionCellList.Clear();

    }

    public void ChangeCell(int missionId)
    {
        foreach(var cell in this.progressMissionCellList)
        {
            if (cell.missionId == missionId)
            {
                //Debug.LogFormat("<color=yellow>cell.transform.childCount:{0}</color>", cell.transform.childCount);
                for(int i = 1; i < cell.transform.childCount; i++)
                {
                    Destroy(cell.transform.GetChild(i).gameObject);
                }
                this.progressMissionCellList.Clear();

                cell.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
        

}
