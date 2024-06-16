using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial03Director : MonoBehaviour
{
    public Button btn_Interaction;
    public Image icon_Interaction;
    public Button btnInventory;
    public VariableJoystick joystick;
    public Transform joyTrans;

    //미션
    public UIMissionMini missionMini;
    public UIMissionDetail missionDetail;
    public UIDialogue dialogue;
    public int missionID;
    public float time = 4f;
    private int stageId;

    //인벤토리
    public UIInventory inventory;

    //UI 맵
    public UIMapMain map;

    //UI 상점
    public UIShop shop;

    //Setting
    public Button btnSetting;
    public UISetting setting;

    //UI 꿈
    public UIDream dream;

    //건물 꿈 사용
    public UIUseDreamPopup uiUseDreamPopup;

    //클리어 텍스트
    public GameObject uiClear;

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler<int>((int)LHMEventType.eEventType.MISSION_UPDATE, this.UpdateMissionEventHandler);
    }

    private void Awake()
    {
        this.missionID = 1004;
        //Invoke("WaitDialogue", 3f);

        this.btnInventory.onClick.AddListener(() =>
        {
            this.inventory.gameObject.SetActive(true);
        });

        this.setting.gameObject.SetActive(false);
        this.btnSetting.onClick.AddListener(() =>
        {
            Debug.Log("setting clicked");
            this.setting.gameObject.SetActive(true);
        });

        EventManager.instance.onTutorial03ClearUI = () =>
        {
            StartCoroutine(this.map.CStageUIClear(3));
        };

        this.uiUseDreamPopup.dim.onClick.AddListener(() =>
        {
            this.uiUseDreamPopup.gameObject.SetActive(false);
        });
        this.uiUseDreamPopup.btn_close.onClick.AddListener(() =>
        {
            this.uiUseDreamPopup.gameObject.SetActive(false);
        });

        this.AddListenerDialogueButtons();

        this.missionMini.onMissionMiniClick = (() =>
        {
            //Debug.LogFormat("detail click count:{0}", this.missionDetail.clickCount);
            this.missionDetail.gameObject.SetActive(true);
        });

        //EventManager.instance.onMissionUpdate = (itemID) => {
        //    Debug.LogFormat("<color=cyan>미션 업데이트</color>");
        //    for (int i = 0; i < this.missionMini.progressMissionCellList.Count; i++)
        //    {
        //        for (int j = 0; j < this.missionMini.progressMissionCellList[i].requests.Count; j++)
        //        {
        //            this.missionMini.progressMissionCellList[i].requests[j].UpdateMissionCount(itemID);
        //        }
        //    }
        //    this.missionDetail.Init(stageId);
        //};


        EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.MISSION_UPDATE, this.UpdateMissionEventHandler);

    }

    private void UpdateMissionEventHandler(short type, int itemID)
    {
        Debug.LogFormat("<color=cyan>미션 업데이트</color>");
        for (int i = 0; i < this.missionMini.progressMissionCellList.Count; i++)
        {
            for (int j = 0; j < this.missionMini.progressMissionCellList[i].requests.Count; j++)
            {
                this.missionMini.progressMissionCellList[i].requests[j].UpdateMissionCount(itemID);
            }
        }
        this.missionDetail.Init(stageId);
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
        this.stageId = stageId;
        Invoke("MissionInit", 1.5f);

        //상점 초기화 
        this.shop.Init();

        //꿈 초기화
        this.dream.Init();

        this.uiClear.SetActive(false);
    }
    public void MissionInit()
    {
        //Debug.LogFormat("Missiondirectorinit");
        //Debug.LogFormat("activeself 밖:{0}", this.dialogue.gameObject.activeSelf);
        if (InfoManager.instance.GetMissionInfo(this.missionID) == null)
        {
            if (InfoManager.instance.DialogueInfo.id == 10019)
            {
                this.DialogueInit();
            }
            else
            {
                InfoManager.instance.DialogueInfo.id = 10020;
                InfoManager.instance.SaveDialogueInfo();

                this.DialogueInit();
            }

        }

        if (InfoManager.instance.GetMissionInfo(this.missionID) != null &&
           InfoManager.instance.GetMissionInfo(this.missionID).isClear == true &&
           InfoManager.instance.GetMissionInfo(++this.missionID) == null)
        {
            if (InfoManager.instance.DialogueInfo.id == 10021)
            {
                this.DialogueInit();
            }
            else
            {
                InfoManager.instance.DialogueInfo.id = 10022;
                InfoManager.instance.SaveDialogueInfo();

                this.DialogueInit();
            }


        }

        if (InfoManager.instance.GetMissionInfo(this.missionID) != null)
        {
            this.missionMini.Init();
            this.missionDetail.Init(stageId);

        }
    }

    private DialogueData GetDialogueData()
    {
        var dialogueId = InfoManager.instance.DialogueInfo.id;
        var data = DataManager.instance.GetDialogueData(dialogueId);
        return data;
    }

    public void DialogueInit()
    {
        Debug.LogFormat("<color=cyan>다이얼로그 이닛</color>");
        var data = this.GetDialogueData();
        this.dialogue.Init(data);
        this.dialogue.gameObject.SetActive(true);

    }
    private void AddListenerDialogueButtons()
    {
        this.dialogue.GetComponent<Button>().onClick.AddListener(() =>
        {
            StartCoroutine(DelayedMethod());

        });
    }

    IEnumerator DelayedMethod()
    {
        yield return new WaitForSeconds(0.1f);
        var data = this.GetDialogueData();
        InfoManager.instance.DialogueInfo.id = data.id + 1;
        InfoManager.instance.SaveDialogueInfo();

        if (data.mission_id == 0)
        {
            if (data.id == 10027)
            {
                this.dialogue.gameObject.SetActive(false);
                EventManager.instance.onTutorial03ClearUI();

            }
            else
                this.dialogue.Init(this.GetDialogueData());
        }
        else
        {
            this.dialogue.gameObject.SetActive(false);
            this.AddMission(data.mission_id);

        }

        yield break;
    }

    public void AddMission(int id)
    {
        Debug.LogFormat("director addmision and save");
        InfoManager.instance.AddMissionInfo(id);
        InfoManager.instance.SaveMissionInfos();

        this.missionMini.Init();
        this.missionDetail.Init(stageId);

    }

    private void Update()
    {
        if (this.missionMini.progressMissionCellList.Count == 0)
        {
            this.missionMini.gameObject.SetActive(false);
        }
        else
        {
            this.missionMini.gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.missionMini.contentTrans.GetComponent<ContentSizeFitter>().transform);
        }

    }


    private void OnApplicationQuit()
    {
        Debug.Log("<color=green>설정 저장시작</color>");
        // 게임 종료 시 현재 사운드 크기, 진동 저장
        float soundVolume = this.setting.musicVolume;
        float otherVolume = this.setting.otherVolume;

        // 1 : on, -1 : off
        int vibtationOnOff = this.setting.vibration;

        PlayerPrefs.SetFloat(this.setting.SoundVolume, soundVolume);
        PlayerPrefs.SetFloat(this.setting.OtherVolume, otherVolume);
        PlayerPrefs.SetInt(this.setting.VibrationOnOff, vibtationOnOff);
        PlayerPrefs.Save();

        Debug.Log("종료 사운드 크기: " + soundVolume.ToString());
        Debug.Log("종료 효과음 크기: " + otherVolume.ToString());
        Debug.Log("종료 진동 OnOff: " + vibtationOnOff.ToString());
    }
    public GameObject alert;
    public IEnumerator BuildingAlert()
    {
        this.alert.SetActive(true);

        yield return new WaitForSeconds(2f);

        this.alert.SetActive(false);
    }
}
