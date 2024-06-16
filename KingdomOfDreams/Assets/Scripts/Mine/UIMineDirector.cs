using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMineDirector : MonoBehaviour
{
    public Button btn_Interaction;
    public Button btnInventory;
    public VariableJoystick joystick;
    public Transform joyTrans;

    //�̼�
    public UIMissionMini missionMini;
    public UIMissionDetail missionDetail;
    public int missionID;
    public float time = 4f;

    //�κ��丮
    public UIInventory inventory;

    //UI ��
    public UIMapMain map;

    private void Awake()
    {
        this.missionID = 1000;
        //Invoke("WaitDialogue", 3f);

        this.btnInventory.onClick.AddListener(() =>
        {
            this.inventory.gameObject.SetActive(true);
        });
    }
    public void Init(int stageId)
    {
        Debug.Log("���� ȣ��");
        //���̽�ƽ ����
        var go = Instantiate(joystick.gameObject, joyTrans);
        this.joystick = go.GetComponent<VariableJoystick>();
        this.joystick.gameObject.SetActive(true);
        //��ȣ�ۿ� ��ư Ȱ��ȭ
        this.btn_Interaction.gameObject.SetActive(true);
        //�κ��丮 �ʱ�ȭ
        this.inventory.Init();

        //�� �ʱ�ȭ
        this.map.Init();
        this.map.gameObject.SetActive(true);

        //�̼� �ʱ�ȭ
        this.MissionInit(DataManager.instance.GetStageData(stageId).id);
    }
    public void MissionInit(int stageNum)
    {
        this.missionMini.onMissionMiniClick = (() =>
        {
            Debug.LogFormat("detail click count:{0}", this.missionDetail.clickCount);
            if (this.missionDetail.clickCount == 0)
                this.missionDetail.Init(stageNum);

            this.missionDetail.gameObject.SetActive(true);
        });
    }

    private void Update()
    {
        if (InfoManager.instance.MissionInfos != null)
        {
            if (InfoManager.instance.MissionInfos.Count != 0)
                this.missionMini.gameObject.SetActive(true);
            if (InfoManager.instance.MissionInfos.Count == 0)
                this.missionMini.gameObject.SetActive(false);
        }
        else
            this.missionMini.gameObject.SetActive(false);

    }

}
