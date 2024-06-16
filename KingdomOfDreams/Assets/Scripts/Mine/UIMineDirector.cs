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

    //미션
    public UIMissionMini missionMini;
    public UIMissionDetail missionDetail;
    public int missionID;
    public float time = 4f;

    //인벤토리
    public UIInventory inventory;

    //UI 맵
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
        Debug.Log("디렉터 호출");
        //조이스틱 생성
        var go = Instantiate(joystick.gameObject, joyTrans);
        this.joystick = go.GetComponent<VariableJoystick>();
        this.joystick.gameObject.SetActive(true);
        //상호작용 버튼 활성화
        this.btn_Interaction.gameObject.SetActive(true);
        //인벤토리 초기화
        this.inventory.Init();

        //맵 초기화
        this.map.Init();
        this.map.gameObject.SetActive(true);

        //미션 초기화
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
