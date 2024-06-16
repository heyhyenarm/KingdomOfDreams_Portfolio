using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInProgressMissionCell : MonoBehaviour
{
    public List<Request> requests = new List<Request>();
    public GameObject requestPrefab;

    private MissionData data;
    public int missionId;

    public GameObject requstAchivedGo;

    public void Init(int index)
    {
        MissionData data = DataManager.instance.GetMissionData(InfoManager.instance.MissionInfos[index].id);
        this.data = data;
        this.missionId = data.id;
        //Debug.LogFormat("<color=cyan>data.id:{0}</color>", data.id);
        //Debug.LogFormat("<color=cyan>request id0:{0}, request id1:{1},request id2:{2},request id3:{3},request id4:{4}</color>",data.request_ingredient_id0, data.request_ingredient_id1, data.request_ingredient_id2, data.request_ingredient_id3, data.request_ingredient_id4);

        Request request = Instantiate(requestPrefab, this.transform).GetComponent<Request>();
        requests.Add(request);
        request.Init(data, data.request_ingredient_id0, data.goal0);


        if (data.request_ingredient_id1 != 0)
        {
            request = Instantiate(requestPrefab, this.transform).GetComponent<Request>();
            requests.Add(request);
            request.Init(data, data.request_ingredient_id1, data.goal1);
        }

        if (data.request_ingredient_id2 != 0)
        {
            request = Instantiate(requestPrefab, this.transform).GetComponent<Request>();
            requests.Add(request);
            request.Init(data, data.request_ingredient_id2, data.goal2);
        }

        if (data.request_ingredient_id3 != 0)
        {
            request = Instantiate(requestPrefab, this.transform).GetComponent<Request>();
            requests.Add(request);
            request.Init(data, data.request_ingredient_id3, data.goal3);
        }

        if (data.request_ingredient_id4 != 0)
        {
            request = Instantiate(requestPrefab, this.transform).GetComponent<Request>();
            requests.Add(request);
            request.Init(data, data.request_ingredient_id4, data.goal4);
        }
        //Debug.Log("진행중인 미션의 요구 아이템들과 해당하는 요구 수량 정보 생성하기");
    }

    private void IsAchive()
    {
        int count = 0;
        foreach(Request request in requests)
        {
            if (request.isAchived)
            {
                count++;
            }
            else
                break;
        }
        if (count == requests.Count)
        {
            EventManager.instance.onAchieved(this.data);
        }
    }

    private void Update()
    {
        this.IsAchive();
    }
}
