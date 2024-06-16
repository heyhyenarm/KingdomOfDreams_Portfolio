using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionDetailScrollView : MonoBehaviour
{
    public Transform contentTrans;
    public GameObject uiMissionDetailCellPrefab;
    public List<GameObject> missionCellList = new List<GameObject>();

    public void Init()
    {
        this.RefreshMissionCell();

        for (int i = 0; i < InfoManager.instance.MissionInfos.Count; i++)
        {
            if (!InfoManager.instance.MissionInfos[i].isClear)
            {
                //Debug.Log("�������� �̼� ����Ʈ �����ϱ�");
                GameObject go = Instantiate(uiMissionDetailCellPrefab, this.contentTrans);
                go.GetComponent<UIMissionDetailCell>().Init(i);
                missionCellList.Add(go);
            }
        }
    }

    public void RefreshMissionCell()
    {
        //Debug.LogFormat("<color=magenta>mission detail scrollview refresh</color>");
        //Debug.LogFormat("<color=magenta>mission detaiil scrollview refresh count:{0}</color>", this.missionCellList.Count);

        foreach (var cell in this.missionCellList)
        {
            Destroy(cell.gameObject);
        }
        this.missionCellList.Clear();

    }
}
