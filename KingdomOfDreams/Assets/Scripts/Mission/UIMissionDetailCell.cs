using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionDetailCell : MonoBehaviour
{
    public Text txtMissionName;
    public Text txtMissionDescript;

    public List<Request> list = new List<Request>();
    public Transform requestListTrans;
    public GameObject requestPrefab;

    public void Init(int index)
    {
        this.RefreshDetailCell();

        MissionData data = DataManager.instance.GetMissionData(InfoManager.instance.MissionInfos[index].id);

        this.txtMissionName.text = data.name;
        string[] name = data.name.Split(' ');

        if (data.id < 1010)
        {
            if(data.id%2 ==1)
                this.txtMissionDescript.text = string.Format("������ {0}�� ���� �ٽ� ���� ���ؼ� ��Ḧ ��ƿ���", name[0]);
            else
                this.txtMissionDescript.text = string.Format("������� {0}��(��) �츮�� ���ؼ� ��Ḧ ��ƿ���", name[0]);
        }
        else
        {
            this.txtMissionDescript.text = string.Format("������ {0}��(��) �ٽ� ���� ���ؼ� ��Ḧ ��ƿ���", name[0]);
        }

        //Debug.Log("�������� �̼��� �䱸 �����۵�� �ش��ϴ� �䱸 ���� ���� �����ϱ�");
        this.InstantiateRequest(data);

    }

    private void InstantiateRequest(MissionData data)
    {
        Request request = Instantiate(requestPrefab, this.requestListTrans).GetComponent<Request>();
        list.Add(request);
        request.Init(data, data.request_ingredient_id0, data.goal0);

        if (data.request_ingredient_id1 != 0)
        {
            request = Instantiate(requestPrefab, this.requestListTrans).GetComponent<Request>();
            list.Add(request);
            request.Init(data, data.request_ingredient_id1, data.goal1);
        }

        if (data.request_ingredient_id2 != 0)
        {
            request = Instantiate(requestPrefab, this.requestListTrans).GetComponent<Request>();
            list.Add(request);
            request.Init(data, data.request_ingredient_id2, data.goal2);
        }

        if (data.request_ingredient_id3 != 0)
        {
            request = Instantiate(requestPrefab, this.requestListTrans).GetComponent<Request>();
            list.Add(request);
            request.Init(data, data.request_ingredient_id3, data.goal3);
        }

        if (data.request_ingredient_id4 != 0)
        {
            request = Instantiate(requestPrefab, this.requestListTrans).GetComponent<Request>();
            list.Add(request);
            request.Init(data, data.request_ingredient_id4, data.goal4);
        }
    }

    public void RefreshDetailCell()
    {
        foreach(var cell in this.list)
        {
            Destroy(cell.gameObject);
        }
        this.list.Clear();
    }
}
