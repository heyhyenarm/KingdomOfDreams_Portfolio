using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial05Director : MonoBehaviour
{
    public Button btn_Interaction;
    public Image icon_Interaction;
    public Button btnInventory;
    public Button btnBook;
    public VariableJoystick joystick;
    public Transform joyTrans;

    //�̼�
    public UIMissionMini missionMini;
    public UIMissionDetail missionDetail;
    public UIDialogue dialogue;
    public int missionID;
    public float time = 4f;
    private int stageId;

    //�κ��丮
    public UIInventory uiInventory;

    //����
    public UIBook book;
    public imgExclaimIcon imgExclaim;

    //UI ��
    public UIMapMain map;

    //UI ����
    public UIShop shop;

    //Setting
    public Button btnSetting;
    public UISetting setting;

    //UI ��
    public UIDream dream;

    //�ǹ� �� ���
    public UIUseDreamPopup uiUseDreamPopup;

    //Ŭ���� �ؽ�Ʈ
    public GameObject uiClear;

    //����ò
    public Image imgExplain;
    public Button btnExplain;

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler<int>((int)LHMEventType.eEventType.MISSION_UPDATE, this.UpdateMissionEventHandler);
    }

    private void Awake()
    {
        this.missionID = 1008;
        //Invoke("WaitDialogue", 3f);

        this.btnInventory.onClick.AddListener(() =>
        {
            this.uiInventory.gameObject.SetActive(true);
        });

        this.btnBook.onClick.AddListener(() =>
        {
            this.book.gameObject.SetActive(true);
            EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.EXCLAIM_ICON_BOOK_ITEM, 0);
        });

        this.btnExplain.onClick.AddListener(() =>
        {
            this.imgExplain.gameObject.SetActive(false);
        });

        this.setting.gameObject.SetActive(false);
        this.btnSetting.onClick.AddListener(() =>
        {
            Debug.Log("setting clicked");
            this.setting.gameObject.SetActive(true);
        });


        EventManager.instance.onTutorial05ClearUI = () =>
        {
            StartCoroutine(this.map.CStageUIClear(5));
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
        //    Debug.LogFormat("<color=cyan>�̼� ������Ʈ</color>");
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
        Debug.LogFormat("<color=cyan>�̼� ������Ʈ</color>");
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
        Debug.Log("���� ȣ��");
        //���̽�ƽ ����
        var go = Instantiate(joystick.gameObject, joyTrans);
        this.joystick = go.GetComponent<VariableJoystick>();
        this.joystick.gameObject.SetActive(true);
        //��ȣ�ۿ� ��ư Ȱ��ȭ
        this.btn_Interaction.gameObject.SetActive(true);
        //�κ��丮 �ʱ�ȭ
        this.uiInventory.Init();
        //���� �ʱ�ȭ
        this.book.Init();
        this.imgExclaim.Init();
        //�������� �ʱ�ȭ
        //this.magicTool.Init();
        //�� �ʱ�ȭ
        this.map.Init();
        this.map.gameObject.SetActive(true);

        //�̼� �ʱ�ȭ
        this.stageId = stageId;
        Invoke("MissionInit", 1f);


        //���� �ʱ�ȭ 
        this.shop.Init();

        //�� �ʱ�ȭ
        this.dream.Init();

        this.uiClear.SetActive(false);


    }
    public void MissionInit()
    {
        //Debug.LogFormat("Missiondirectorinit");
        //Debug.LogFormat("activeself ��:{0}", this.dialogue.gameObject.activeSelf);
        if (InfoManager.instance.GetMissionInfo(this.missionID) == null)
        {
            InfoManager.instance.DialogueInfo.id = 10035;
            InfoManager.instance.SaveDialogueInfo();

            this.DialogueInit();
        }

        if (InfoManager.instance.GetMissionInfo(this.missionID) != null &&
           InfoManager.instance.GetMissionInfo(this.missionID).isClear == true &&
           InfoManager.instance.GetMissionInfo(++this.missionID) == null)
        {
            InfoManager.instance.DialogueInfo.id = 10036;
            InfoManager.instance.SaveDialogueInfo();

            this.DialogueInit();
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
        Debug.LogFormat("<color=cyan>���̾�α� �̴�</color>");
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
            if (data.id == 10042)
            {
                this.dialogue.gameObject.SetActive(false);
                EventManager.instance.onTutorial05ClearUI();
                InfoManager.instance.SaveDialogueInfo();

            }
            if (data.id == 10096)
            {
                InfoManager.instance.DialogueInfo.id = 10036;
                InfoManager.instance.SaveDialogueInfo();
                this.dialogue.gameObject.SetActive(false);
                this.imgExplain.gameObject.SetActive(true);
            }
            else
                this.dialogue.Init(this.GetDialogueData());
        }
        else
        {
            this.dialogue.gameObject.SetActive(false);
            this.AddMission(data.mission_id);
            InfoManager.instance.SaveDialogueInfo();

        }

        yield break;
    }

    public void AddMission(int id)
    {
        Debug.LogFormat("<color=cyan>director addmision and save</color>");
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

    //public void WaitDialogue()
    //{
    //    this.dialogue.Init(missionID);
    //    this.dialogue.gameObject.SetActive(true); // GameObject Ȱ��ȭ
    //    Invoke("CloseDialogue", 3f);
    //}
    //public void CloseDialogue()
    //{
    //    this.dialogue.gameObject.SetActive(false);
    //    this.AddMission(missionID);
    //}
    private void OnApplicationQuit()
    {
        Debug.Log("<color=green>���� �������</color>");
        // ���� ���� �� ���� ���� ũ��, ���� ����
        float soundVolume = this.setting.musicVolume;
        float otherVolume = this.setting.otherVolume;

        // 1 : on, -1 : off
        int vibtationOnOff = this.setting.vibration;

        PlayerPrefs.SetFloat(this.setting.SoundVolume, soundVolume);
        PlayerPrefs.SetFloat(this.setting.OtherVolume, otherVolume);
        PlayerPrefs.SetInt(this.setting.VibrationOnOff, vibtationOnOff);
        PlayerPrefs.Save();

        Debug.Log("���� ���� ũ��: " + soundVolume.ToString());
        Debug.Log("���� ȿ���� ũ��: " + otherVolume.ToString());
        Debug.Log("���� ���� OnOff: " + vibtationOnOff.ToString());
    }
    public GameObject alert;
    public IEnumerator BuildingAlert()
    {
        this.alert.SetActive(true);

        yield return new WaitForSeconds(2f);

        this.alert.SetActive(false);
    }
}