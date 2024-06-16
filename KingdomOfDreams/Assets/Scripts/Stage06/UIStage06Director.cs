using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Unity.VisualScripting;

public class UIStage06Director : MonoBehaviour
{
    private static UIStage06Director instance;

    public Button btn_Interaction;
    public Image icon_Interaction;
    public Button btnInventory;
    public Button btnBook;
    public Button btnMagicTool;
    public VariableJoystick joystick;
    public Transform joyTrans;
    public Button btnDreamland;
    public Button btnShop;
    public Button btnMap;

    //미션
    public UIMissionMini missionMini;
    public UIMissionDetail missionDetail;
    public UIDialogue dialogue;
    public int missionID;
    public float time = 4f;
    private int stageId;


    //인벤토리
    public UIInventory inventory;
    //도감
    public UIBook book;
    public imgExclaimIcon imgExclaim;
    //마법도구
    public UIMagicTool magicTool;
    //조합하기
    public FoodWorkBench foodWorkBench;
    public IronWorkBench ironWorkBench;
    public MeatWorkBench meatWorkBench;
    private GameObject workBenchGo;

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

    //중복처리
    private bool isInitialized = false;

    public UILife uiLife;

    //캐릭터 전달
    public int characterNum;
    public int materialNum;

    //미니맵
    public Button miniMap;
    public Button miniMapBig;
    public GameObject miniStage6;
    public GameObject miniStage7;
    public GameObject miniStage8;

    //클리어 텍스트
    public GameObject uiClear;

    //설명창
    public Button btnExplainUI;
    public UIOffline UIOffline;
    public AutoExplain explainAuto;

    //ticket
    public UITicket ticket;

    private void OnEnable()
    {
        Debug.Log("enable");

    }

    private void OnDisable()
    {
        Debug.Log("disable");
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler<int>((int)LHMEventType.eEventType.MISSION_UPDATE, this.UpdateMissionEventHandler);
    }

    private void Awake()
    {
        //var loading = FindObjectOfType<LoadingSceneController>();
        //if (loading.gameObject.activeSelf == true)
        //{
        //    this.gameObject.SetActive(false);
        //}

        Debug.Log("<color=red>06Director Awake</color>");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }



        this.AddEvent();
    }

    public void AddEvent()
    {
        Debug.Log("<color=red>add event</color>");
        this.btnInventory.onClick.AddListener(() =>
        {
            this.inventory.gameObject.SetActive(true);
        });

        this.btnBook.onClick.AddListener(() =>
        {
            this.book.gameObject.SetActive(true);
            EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.EXCLAIM_ICON_BOOK_ITEM, 0);

        });

        this.btnMagicTool.onClick.AddListener(() =>
        {
            this.magicTool.gameObject.SetActive(true);
        });

        this.btnExplainUI.onClick.AddListener(() =>
        {
            Debug.Log("btnExplainUI click");
            this.btnExplainUI.gameObject.SetActive(false);
        });
        this.explainAuto.btnClose.onClick.AddListener(() =>
        {
            this.explainAuto.gameObject.SetActive(false);
            this.btnExplainUI.gameObject.SetActive(true);

        });


        this.setting.gameObject.SetActive(false);
        this.btnSetting.onClick.AddListener(() =>
        {
            Debug.Log("setting clicked");
            this.setting.gameObject.SetActive(true);
        });
        EventDispatcher.instance.AddEventHandler<int[]>((int)LHMEventType.eEventType.CHARACTER_MATERIAL_CHANGE, new EventHandler<int[]>((type, arr) =>
        {
            Debug.Log("CHARACTER_MATERIAL_CHANGE");
            this.characterNum = arr[0];
            this.materialNum = arr[1];
        }));
        this.miniMap.onClick.AddListener(() =>
        {
            Debug.Log("miniMap clicked");
            if (this.miniMapBig.IsActive()) this.miniMapBig.gameObject.SetActive(false);
            else this.miniMapBig.gameObject.SetActive(true);
        });
        this.miniMapBig.onClick.AddListener(() =>
        {
            Debug.Log("miniMapBig clicked");
            this.miniMapBig.gameObject.SetActive(false);
        });

        this.AddListenerDialogueButtons();

        this.missionMini.onMissionMiniClick = (() =>
        {
            //Debug.LogFormat("detail click count:{0}", this.missionDetail.clickCount);
            this.missionDetail.gameObject.SetActive(true);
        });

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
        //미션 초기화
        Debug.LogFormat("<color=yellow>director Init stageID:{0}</color>", stageId);
        this.stageId = stageId;
        Invoke("MissionInit", 1.5f);

        //꿈 초기화
        this.dream.Init();

        //상점 초기화 
        this.shop.Init();

        if (isInitialized)
        {
            return;
        }

        isInitialized = true;

        Debug.Log("Director Init");
        //조이스틱 생성
        var go = Instantiate(joystick.gameObject, joyTrans);
        this.joystick = go.GetComponent<VariableJoystick>();
        this.joystick.gameObject.SetActive(true);
        //상호작용 버튼 활성화
        this.btn_Interaction.gameObject.SetActive(true);
        //인벤토리 초기화
        this.inventory.Init();
        //오프라인 설명창 초기화
        Debug.Log("<color=pink>디렉터 오프라인 Init</color>");
        this.UIOffline.Init();
        //도감 초기화
        this.book.Init();
        this.imgExclaim.Init();
        //마법도구 초기화
        this.magicTool.Init();
        //조합대 초기화
        this.foodWorkBench.Init();
        this.ironWorkBench.Init();
        this.meatWorkBench.Init();
        Debug.LogFormat("<color=yellow>조합대 Init</color>");


        //맵 초기화
        this.map.Init();
        this.map.gameObject.SetActive(true);
        this.map.uiStage.gameObject.SetActive(false);

        //미니맵 초기화
        this.miniMapBig.gameObject.SetActive(false);

        //this.AddEvent();

        if (this.stageId == 8007)
        {
            this.workBenchGo = GameObject.Find("WorkBench");
        }

        this.uiClear.SetActive(false);

        //티켓 초기화
        this.ticket.Init();
    }

    private void Start()
    {
        Debug.Log("Director Start");

        EventManager.instance.onStage06ClearUI = () =>
        {
            StartCoroutine(instance.map.CStageUIClear(6));
        };
        EventManager.instance.onStage07ClearUI = () =>
        {
            Debug.Log("onStage07ClearUI");
            StartCoroutine(instance.map.CStageUIClear(7));
        };
        EventManager.instance.onStage08ClearUI = () =>
        {
            StartCoroutine(instance.map.CStageUIClear(8));
        };

        this.uiUseDreamPopup.dim.onClick.AddListener(() =>
        {
            this.uiUseDreamPopup.gameObject.SetActive(false);
        });
        this.uiUseDreamPopup.btn_close.onClick.AddListener(() =>
        {
            this.uiUseDreamPopup.gameObject.SetActive(false);
        });

    }
    public void MissionInit()
    {
        this.dialogue.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);

        if (stageId == 8005)
        {
            Debug.LogFormat("<color=red>mission int stage:6</color>");
            this.MissionInit6(stageId);
        }
        if (stageId == 8006)
        {
            Debug.LogFormat("<color=red>mission int stage:7</color>");
            this.MissionInit7(stageId);
        }
        if (stageId == 8007)
        {
            Debug.LogFormat("<color=red>mission int stage:8</color>");
            this.MissionInit8(stageId);
        }
    }
    public void MissionInit6(int stageNum)
    {
        //Debug.LogFormat("Missiondirectorinit");
        //Debug.LogFormat("activeself 밖:{0}", this.dialogue.gameObject.activeSelf);
        if (InfoManager.instance.GetMissionInfo(this.missionID) == null)
        {
            if (InfoManager.instance.DialogueInfo.id == 10048)
            {
                this.DialogueInit();
            }
            else
            {
                InfoManager.instance.DialogueInfo.id = 10049;
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
    public void MissionInit7(int stageNum)
    {
        //Debug.LogFormat("<color=red>mission int missionID:{0}</color>", this.missionID);
        if (InfoManager.instance.GetMissionInfo(this.missionID) == null)
        {
            if (InfoManager.instance.DialogueInfo.id == 10058)
            {
                this.DialogueInit();
            }
            else
            {
                InfoManager.instance.DialogueInfo.id = 10059;
                InfoManager.instance.SaveDialogueInfo();

                this.DialogueInit();
            }
        }

        if (this.missionID <= 1019)
        {
            this.missionID = 1015;
            if (InfoManager.instance.GetMissionInfo(this.missionID) != null
              && InfoManager.instance.GetMissionInfo(this.missionID).isClear
              && InfoManager.instance.GetMissionInfo(++this.missionID).isClear
              && InfoManager.instance.GetMissionInfo(++this.missionID).isClear
              && InfoManager.instance.GetMissionInfo(++this.missionID).isClear
              && InfoManager.instance.GetMissionInfo(++this.missionID).isClear
              && InfoManager.instance.GetMissionInfo(++this.missionID) == null)
            {
                InfoManager.instance.DialogueInfo.id = 10066;
                InfoManager.instance.SaveDialogueInfo();

                this.DialogueInit();
            }
        }

        //Debug.LogFormat("<color=red>1015 mission clear check after missionID:{0}</color>", this.missionID);

        if (InfoManager.instance.GetMissionInfo(this.missionID) != null)
        {
            this.missionMini.Init();
            this.missionDetail.Init(stageId);

        }
    }
    public void MissionInit8(int stageId)
    {
        Debug.LogFormat("<color=red>mission int 8 missionID:{0}</color>", this.missionID);
        if (this.missionID == 1028)
            return;

        if (InfoManager.instance.GetMissionInfo(this.missionID) == null)
        {
            InfoManager.instance.DialogueInfo.id = 10075;
            InfoManager.instance.SaveDialogueInfo();

            this.DialogueInit();
        }
        if (InfoManager.instance.GetMissionInfo(1023)!=null
            && InfoManager.instance.GetMissionInfo(1023).isClear
            && InfoManager.instance.GetMissionInfo(1024) == null)
        {
            InfoManager.instance.DialogueInfo.id = 10078;
            InfoManager.instance.SaveDialogueInfo();

            this.DialogueInit();
        }

        //if (InfoManager.instance.GetMissionInfo(this.missionID)!=null
        //    && InfoManager.instance.GetMissionInfo(this.missionID).isClear
        //    && InfoManager.instance.GetMissionInfo(++this.missionID)!=null
        //    && InfoManager.instance.GetMissionInfo(this.missionID).isClear
        //    && InfoManager.instance.GetMissionInfo(++this.missionID).isClear
        //    && InfoManager.instance.GetMissionInfo(++this.missionID).isClear
        //    && InfoManager.instance.GetMissionInfo(++this.missionID) == null)
        //{
        //    if (InfoManager.instance.DialogueInfo.id == 10081)
        //    {
        //        this.DialogueInit();

        //    }
        //    else
        //    {
        //        InfoManager.instance.DialogueInfo.id = 10080;
        //        InfoManager.instance.SaveDialogueInfo();

        //        this.DialogueInit();
        //    }
        //}

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
        var data = this.GetDialogueData();
        Debug.LogFormat("<color=cyan>다이얼로그 이닛, data id:{0}</color>", data.id);

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
        Debug.LogFormat("<color=yellow>dialogue click ,missionID:{0}</color>", missionID);
        var data = this.GetDialogueData();
        InfoManager.instance.DialogueInfo.id = data.id + 1;
        InfoManager.instance.SaveDialogueInfo();
        if (data.mission_id == 0)
        {
            if ((data.id >= 10050 && data.id <= 10053) ||
                (data.id >= 10062 && data.id <= 10065) ||
                (data.id >= 10067 && data.id <= 10069) ||
                (data.id >= 10079 && data.id <= 10080) ||
                (data.id >= 10091 && data.id <= 10092))
                this.dialogue.gameObject.SetActive(false);
            if (data.id == 10057)
            {
                //7스로 전환
                this.dialogue.gameObject.SetActive(false);
                EventManager.instance.onStage06ClearUI();
            }
            else if (data.id == 10074)
            {
                //8스로 전환
                this.dialogue.gameObject.SetActive(false);
                Invoke("InitBuildingInfo", 0);
                EventManager.instance.onStage07ClearUI();

            }
            else if (data.id == 10090)
            {
                this.dialogue.gameObject.SetActive(false);
                this.workBenchGo.transform.GetChild(0).gameObject.SetActive(true);
                this.workBenchGo.transform.GetChild(1).gameObject.SetActive(true);
                this.workBenchGo.transform.GetChild(2).gameObject.SetActive(true);

            }
            else if (data.id == 10095)
            {
                //성 건설
                Debug.Log("<color=yellow>성 건설 보여주기</color>");
                this.dialogue.gameObject.SetActive(false);
            }
            else
                this.dialogue.Init(this.GetDialogueData());
        }
        else if (data.mission_id != 0)
        {
            if (data.mission_id == 1010 || data.mission_id == 1015)
            {
                for (int i = 0; i < 5; i++)
                {
                    this.AddMission(data.mission_id + i);
                    this.dialogue.gameObject.SetActive(false);
                }
                if (data.mission_id == 1010)
                {
                    this.explainAuto.gameObject.SetActive(true);
                }
            }
            if (data.mission_id == 1020)
            {
                for (int i = 0; i < 3; i++)
                {
                    Debug.LogFormat("<color=red>다이얼로그 id:{0}</color>",data.id);
                    this.AddMission(data.mission_id + i);
                    this.dialogue.Init(GetDialogueData());
                }
            }
            if (data.mission_id == 1024)
            {
                for (int i = 0; i < 3; i++)
                {
                    this.AddMission(data.mission_id + i);
                    this.dialogue.gameObject.SetActive(false);
                }
            }
            if (data.mission_id == 1023)
            {
                this.AddMission(missionID);
                this.dialogue.gameObject.SetActive(false);
            }
            if (data.mission_id == 1027)
            {
                for (int i = 0; i < 2; i++)
                {
                    this.AddMission(data.mission_id + i);
                }
            }
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
