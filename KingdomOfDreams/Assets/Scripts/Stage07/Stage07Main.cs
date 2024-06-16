using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class Stage07Main : MonoBehaviour
{
    [SerializeField]
    private UIStage06Director director;
    public GameObject playerPrefab;
    public CinemachineVirtualCamera followCam;

    public int stageID;
    public bool isClearStage;
    public List<Building> buildings =new List<Building>();

    private Player player;
    public GameObject moveEffect;

    public GameObject stageReviveVideo;
    public FarmRow[] farmrows;

    public Bee beePrefab;

    private bool isClicked;

    //���� Ŭ���� �� �̵��ϴ� npc��
    public GameObject[] fellingNPCS;
    public GameObject[] fishingNPCS;
    public GameObject[] farmingNPCS;
    public GameObject[] miningNPCS;
    public GameObject[] attackingNPCS;

    //������
    public List<StageArea> stageAreas;

    public int realStageID;

    //�̴ϸ�
    public MinimapDirector minimap;
    public Camera miniCam;
    private Vector3 playerPos;

    //private bool addComplexMission;
    //����Ʈ ���μ���
    public PostProcessingRevive post;

    //�Ҹ� ����
    private float musicVolume;
    private float otherVolume;
    //���� �÷��̾�
    public VideoPlayer videoPlayerStage;
    //�÷��̾� ������ҽ�
    AudioSource audioSource;

    //���� ��Ʈ�ѷ�
    //public GoogleAdMobController adController;

    private int clearMissionCount;

    private void Awake()
    {
        //����Ʈ ���μ��� �׽�Ʈ
        //this.post.PostRevive();

        var go = GameObject.Find("UIStage06Director");
        go.SetActive(true);
        this.director = go.GetComponent<UIStage06Director>();
        this.director.map.uiStage.gameObject.SetActive(false);
        //this.director.shop.uiChestShop.adController = this.adController;
    }


    public void Start()
    {
        Debug.LogFormat("<color=magenta>stage07 main start</color>");

        var stage07_SC = SoundManager.GetSoundConnectionForThisLevel("Stage07");
        SoundManager.PlayConnection(stage07_SC);

        this.Init();

        //var ad = this.director.shop.uiChestShop.chestCells[0]._btnAD.GetComponent<Button>();
        //ad.onClick.AddListener(LoadAd);

        //�� ����
        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.APPEAR_BEE, AppearBeeHandler);

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.END_REVIVE_VIDEO, EndReviveVideoHandler);
        //Debug.LogFormat("<color=magenta>end revive video event add</color>");

        EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.CLEAR_MISSION_REFRESH, CountClearMissionHandler);

        EventManager.instance.blackToColor = (areaNum) =>
        {
            var num = areaNum - 10;
            //this.stageAreas[num].clearArea(this.stageID - 8000, num);
            Debug.LogFormat("<color>Stage7_{0}</color>", num);
            if (this.stageAreas != null)
            {
                if (num > 5) this.stageAreas[num - 1].clearArea(7, num);
                else this.stageAreas[num].clearArea(7, num);
                //this.stageAreas[num].clearArea(7, num);
            }
        };

        foreach (Building building in buildings)
        {
            int areaNum = building.id - 4000 - 10;
            Debug.LogFormat("areaNum : {0}, building id : {1}", areaNum, building.id);
            //if (areaNum >= 5)
            //{
            //    if (building.isBuild)
            //    {
            //        this.stageAreas[areaNum-1].clearArea(7, areaNum);
            //    }
            //}
            //else
            //{
            //    if (building.isBuild)
            //    {
            //        this.stageAreas[areaNum].clearArea(7, areaNum);
            //    }
            //}
            if (building.isClear)
            {
                if (areaNum >= 5) this.stageAreas[areaNum - 1].clearArea(7, areaNum);
                else this.stageAreas[areaNum].clearArea(7, areaNum);
            }
        }



    }
    public void OnDestroy()
    {
        //�� ���� �̺�Ʈ �ڵ鷯 ����
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.APPEAR_BEE, AppearBeeHandler);

        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.END_REVIVE_VIDEO, EndReviveVideoHandler);
        EventDispatcher.instance.RemoveEventHandler<int>((int)LHMEventType.eEventType.CLEAR_MISSION_REFRESH, CountClearMissionHandler);

    }
    //�� ���� �̺�Ʈ �ڵ鷯
    private void AppearBeeHandler(short type)
    {
        if (this.player.mono.actionTarget != null)
        {
            var beeGo = Instantiate(this.beePrefab);
            Vector3 pos = this.player.mono.actionTarget.transform.position - this.player.mono.transform.position;
            beeGo.transform.position = this.player.mono.actionTarget.transform.position + pos;
            beeGo.targetPos = this.player.mono.transform;
        }
    }
    private void EndReviveVideoHandler(short type)
    {
        //Debug.LogFormat("<color=magenta>end revive video stageID:{0}</color>", stageID);
        this.director.DialogueInit();
    }
    private void CountClearMissionHandler(short type, int clearMissionCount)
    {
        this.clearMissionCount = clearMissionCount;
    }

    public void Init()
    {
        //�ǹ� ������ ����, ������ �� �̼� ���ĳ���, ...
        this.stageID = 8006;
        Debug.LogFormat("stage ID: {0}", this.stageID);
        //this.missionMain.Init(this.stageID);

        //�÷��̾�, UI ����
        this.player = new Player(this.playerPrefab);
        Debug.LogFormat("player: {0}", this.player);
        this.player.State = new NormalState(this.player);

        Debug.Log("player init");
        this.player.mono.Init();
        this.player.mono.transform.position = new Vector3(192.77f, 2.33f, -34.69f);
        this.audioSource = this.player.mono.GetComponent<AudioSource>();
        this.audioSource.volume = PlayerPrefs.GetFloat("OtherVolume"); ;

        //�ʱ� ����Ʈ ��ġ ����
        this.moveEffect.transform.position = this.player.mono.transform.position + new Vector3(0f, 0.2f, -0.2f);

        this.director.missionID = 1015;
        List<MissionData> stageMissionDatas = DataManager.instance.GetMissionDatas().FindAll(x => x.stage_id == this.stageID);

        int index = 0;
        foreach (Building building in buildings)
        {
            building.Init(stageMissionDatas[index].resurrection_id);

            building.btn_UseDream.onClick.AddListener(() =>
            {
                //�� ��� ��ÿϷ� �˾� ����
                Debug.LogFormat("�� ��� �˾�");
                this.director.uiUseDreamPopup.gameObject.SetActive(true);
                this.director.uiUseDreamPopup.PriceUpdate(building.buildData);
            });
            //building.btn_PlayAD.onClick.AddListener(() =>
            //{
            //    //���� ���
            //    Debug.LogFormat("���� ���");
            //    if (this.adController.rewardedInterstitialAd != null)
            //    {
            //        building.buildTime = 0;
            //    }
            //});

            index++;
        }

        this.director.uiUseDreamPopup.btn_use.onClick.AddListener(() =>
        {
            //���� �� ���, �ǹ� ����
            //Debug.LogFormat("�� ���");
            if (InfoManager.instance.DreamInfo.amount >= this.director.uiUseDreamPopup.price)
            {
                UseDream(this.director.uiUseDreamPopup.price);
                int id = this.director.uiUseDreamPopup.ClosePopup();
                //�� ���� ������Ʈ
                var building = this.buildings.Find(x => x.id == id);
                building.BuildObject();
            }
            else
            {
                StartCoroutine(this.director.BuildingAlert());
            }
        });

        //�̼� Ŭ���� �̺�Ʈ
        EventManager.instance.onAchieved = (data) => {
            //Debug.Log("<color=yellow>onAchieved</color>");
            var building = buildings.Find(x => x.id == data.resurrection_id);
            if (building != null)
            {
                building.isDone = true;
                building.effect.SetActive(true);
            }
        };

        if (this.realStageID == stageID)
            this.director.Init(stageID);

        this.player.mono.joystick = this.director.joystick;
        this.followCam.Follow = this.player.mono.transform;
        this.followCam.LookAt = this.player.mono.transform;

        for (int i = 0; i < farmrows.Length; i++)
        {
            this.farmrows[i].Init();
        }


        //var stageMissionDatas = DataManager.instance.GetMissionDatas().FindAll(x => x.stage_id == this.stageID);

        //var Info0 = InfoManager.instance.MissionInfos.Find(x => x.id == stageMissionDatas[0].id);
        //var Info1 = InfoManager.instance.MissionInfos.Find(x => x.id == stageMissionDatas[1].id);
        //this.npc.Init(stageMissionDatas[0].resurrection_id);
        //this.building.Init(stageMissionDatas[1].resurrection_id);

        //��ȣ�ۿ� ��ư Ŭ��
        this.director.btn_Interaction.onClick.AddListener(() => {
            this.isClicked = true;

            switch (this.player.mono.location)
            {
                case Enums.ePlayerLocation.Plain:
                    this.player.State = new NormalState(this.player);
                    break;

                case Enums.ePlayerLocation.Forest:
                    this.player.State = new FellingState(this.player);
                    break;

                case Enums.ePlayerLocation.Farm:
                    this.player.State = new FarmingState(this.player);
                    break;
            }
            Debug.LogFormat("IState:{0}", this.player.State);
            this.player.DoAction();

            Invoke("IsClicked", 2f);
        });



        this.NPCMove();

        //�̴ϸ� Init
        this.playerPos = this.player.mono.gameObject.transform.position;
        this.minimap.player.transform.position = new Vector3(playerPos.x, this.minimap.player.transform.position.y, playerPos.z);
        this.miniCam.transform.position = new Vector3(playerPos.x, this.miniCam.transform.position.y, playerPos.z);
        this.director.miniStage6.SetActive(false);
        this.director.miniStage7.SetActive(true);
        this.director.miniStage8.SetActive(false);

        this.director.uiClear.SetActive(false);

        this.musicVolume = this.director.setting.otherVolume;
        EventDispatcher.instance.AddEventHandler<float>((int)LHMEventType.eEventType.CHANGE_MUSIC_VOLUME, new EventHandler<float>((type, volume) =>
        {
            Debug.Log("CHANGE_MUSIC_VOLUME");
            this.musicVolume = volume;
        }));
        this.musicVolume = this.director.setting.otherVolume;
        EventDispatcher.instance.AddEventHandler<float>((int)LHMEventType.eEventType.CHANGE_OTHERS_VOLUME, new EventHandler<float>((type, volume) =>
        {
            Debug.Log("CHANGE_OTHERS_VOLUME");
            this.otherVolume = volume;
            this.audioSource.volume = this.otherVolume;
        }));
        //EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.AD_REWARD_PLAY, new EventHandler((type) =>
        //{
        //    Debug.LogFormat("AD on chest");
        //    this.adController.ShowRewardedInterstitialAd();
        //}));
        EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.USE_DREAM, new EventHandler<int>((type, amount) =>
        {
            Debug.LogFormat("use dream {0}", amount);
            this.UseDream(amount);
        }));

    }
    //private void LoadAd()
    //{
    //    this.adController.RequestAndLoadRewardedInterstitialAd();
    //}

    public void IsClicked()
    {
        this.isClicked = false;

    }

    private void Update()
    {
        //�̴ϸ� ��ġ ������Ʈ
        this.playerPos = this.player.mono.gameObject.transform.position;
        this.minimap.player.transform.position = new Vector3(playerPos.x, this.minimap.player.transform.position.y, playerPos.z);
        this.miniCam.transform.position = new Vector3(playerPos.x, this.miniCam.transform.position.y, playerPos.z);

        //��ȣ�ۿ� ��ư Ȱ��ȭ, ��Ȱ��ȭ
        if (this.player.mono.isTargeting && this.player.mono.actionTarget != null && this.isClicked == false)
        {
            this.director.btn_Interaction.interactable = true;
            var atlas = AtlasManager.instance.GetAtlasByName("Interaction");
            switch (this.player.mono.location)
            {
                case Enums.ePlayerLocation.Forest:
                    {
                        var sprite = atlas.GetSprite("Axe");
                        this.director.icon_Interaction.sprite = sprite;
                        break;
                    }
                case Enums.ePlayerLocation.Pond:
                    {
                        var sprite = atlas.GetSprite("fishing");
                        this.director.icon_Interaction.sprite = sprite;
                        break;
                    }
                case Enums.ePlayerLocation.Farm:
                    {
                        var sprite = atlas.GetSprite("Shovel");
                        this.director.icon_Interaction.sprite = sprite;
                        break;
                    }
                case Enums.ePlayerLocation.Mine:
                    {
                        var sprite = atlas.GetSprite("Icon_ItemIcon_Pickax");
                        this.director.icon_Interaction.sprite = sprite;
                        break;
                    }
                case Enums.ePlayerLocation.Dungeon:
                    {
                        var sprite = atlas.GetSprite("Sword");
                        this.director.icon_Interaction.sprite = sprite;
                        break;
                    }
            }
            this.director.icon_Interaction.gameObject.SetActive(true);
        }
        else
        {
            this.director.btn_Interaction.interactable = false;
            this.director.icon_Interaction.gameObject.SetActive(false);
        }

        //�ٴ� ���¸� ����Ʈ ���� ������
        int animState = this.player.mono.anim.GetInteger("State");
        if (animState == 1 || animState == 3 || animState == 5 || animState == 7)
            this.MoveEffect();

        //������Ʈ Ŭ��
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
#if UNITY_EDITOR
            if (hit && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
#elif UNITY_ANDROID // or iOS 
            if (hit && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(0))
#endif
            {
                //Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "Building")
                {
                    var buildingGo = hitInfo.transform.gameObject.GetComponent<Building>();
                    Debug.Log("�ǹ� Ŭ��");
                    //������ �ǹ�
                    if (buildingGo.isBuild)
                    {
                        //Debug.Log("������ �ǹ�");
                    }
                    //�ð� ���� && �������� �ǹ�
                    else if (buildingGo.endTime)
                    {
                        //��Ʈ ��Ȱ��ȭ, �ǹ� �������� �ִϸ��̼�
                        buildingGo.BuildObject();
                    }
                    //�ð� �帣�� ��
                    else if (!buildingGo.isComplete)
                    {
                        //Debug.Log("�������� ��");
                    }
                    //�̼� Ŭ����?
                    else if (buildingGo.isDone)
                    {
                        //Debug.Log("��Ʈ Ȱ��ȭ");
                        //�̼� ��� ���� ����
                        SubtractIngredientAmountB(buildingGo);

                        //õ�� Ȱ��ȭ, �ð� �帧
                        //this.adController.RequestAndLoadRewardedInterstitialAd();
                        buildingGo.StartBuild();
                    }
                }
                else if (hitInfo.transform.gameObject.tag == "Fairy")
                {
                    //���� Ŭ��
                    //Debug.Log("Fairy hit");
                    EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.ENTER_FAIRYSHOP);
                }
                else
                {
                    //Debug.Log("No hitInfo");
                }
            }
            else
            {
                //Debug.Log("No hit");
            }
        }

        if(this.clearMissionCount == 4&&InfoManager.instance.DialogueInfo.id<10065)
        {
            //Debug.Log("<color=yellow>�ǹ� 4ä ����</color>");
            InfoManager.instance.DialogueInfo.id = 10065;
            InfoManager.instance.SaveDialogueInfo();
        }

        if (this.clearMissionCount == 5 && InfoManager.instance.DialogueInfo.id < 10066)
        {
            //Debug.Log("<color=yellow>�ǹ� 5ä ����</color>");
            InfoManager.instance.DialogueInfo.id = 10066;
            InfoManager.instance.SaveDialogueInfo();
        }
        //�Ϲ� �ǹ� ���� ��Ȱ ������ ���� �̼� �ֱ�
        //if (this.buildings[0].isClear &&
        //    this.buildings[1].isClear &&
        //    this.buildings[2].isClear &&
        //    this.buildings[3].isClear &&
        //    this.buildings[4].isClear)
        //{
        //    Debug.Log("<color=yellow>���� ���� �˻�</color>");

        //    if (InfoManager.instance.GetMissionInfo(1020) == null)
        //    {
        //        Debug.Log("<color=yellow>���� �� ���̾�α� ����</color>");

        //        InfoManager.instance.DialogueInfo.id = 10066;
        //        InfoManager.instance.SaveDialogueInfo();
        //        this.director.DialogueInit();
        //    }
        //}

        //�������� Ŭ���� ����
        if (this.buildings[0].isClear &&
            this.buildings[1].isClear &&
            this.buildings[2].isClear &&
            this.buildings[3].isClear &&
            this.buildings[4].isClear &&
            this.buildings[5].isClear &&
            this.buildings[6].isClear &&
            this.buildings[7].isClear &&
            !this.isClearStage)
        {
            this.isClearStage = true;
            StartCoroutine(WaitForStageClearVideo(8f));
            //List<MissionData> stageMissionDatas = DataManager.instance.GetMissionDatas().FindAll(x => x.stage_id == this.stageID);
            //var Info0 = InfoManager.instance.MissionInfos.Find(x => x.id == stageMissionDatas[0].id);
            //var Info1 = InfoManager.instance.MissionInfos.Find(x => x.id == stageMissionDatas[1].id);
            //Debug.LogFormat("info0 clear: {0}", Info0.isClear);
            //Debug.LogFormat("info1 clear: {0}", Info1.isClear);
            //if (Info0.isClear && Info1.isClear)
            //{

            //}
            var stageInfo = InfoManager.instance.StageInfos.Find(x => x.stage == 7);
            if (stageInfo == null)
            {
                this.SaveClearInfos();
            }
        }

        if (this.director != null)
        {
            if (this.player.mono.location != Enums.ePlayerLocation.Dreamland)
            {
                if (this.director.missionMini.progressMissionCellList.Count == 0)
                {
                    this.director.missionMini.gameObject.SetActive(false);
                }
                else
                {
                    this.director.missionMini.gameObject.SetActive(true);
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.director.missionMini.contentTrans.GetComponent<ContentSizeFitter>().transform);

                }
            }
            else
            {
                this.director.missionMini.gameObject.SetActive(false);
            }

        }
    }


    private void SaveClearInfos()
    {
        InfoManager.instance.AddStageInfo(7);
        InfoManager.instance.SaveStageInfos();
        var reward = DataManager.instance.GetStageData(this.stageID).reward_chest_id;
        InfoManager.instance.ChestInfos[reward - 100].amount++;
        Debug.LogFormat("<color=yellow>{0}���� ��{1}��</color>", reward, InfoManager.instance.ChestInfos[reward - 100].amount);
        InfoManager.instance.SaveChestInfos();
    }
    private List<IngredientInfo> tempInfo;

    private void SubtractIngredientAmountB(Building building)
    {
        var data = DataManager.instance.GetMissionDatas().Find(x => x.resurrection_id == building.id);
        //List<IngredientInfo> infos = new List<IngredientInfo>();
        if(data.id <= 1019)
        {
            Debug.LogFormat("<color=cyan>request_ingredient_id0:{0}</color>", data.request_ingredient_id0);
            var info = InfoManager.instance.GetIngredientInfo(data.request_ingredient_id0);
            Debug.LogFormat("<color=cyan>info:{0}</color>", info);
            Debug.LogFormat("<color=cyan>info id:{0}</color>", info.id);
            Debug.LogFormat("<color=cyan>info amount:{0}</color>", info.amount);

            info.amount = info.amount - data.goal0;

        }
        else
        {
            var info0 = InfoManager.instance.GetIngredientInfo(data.request_ingredient_id0);
            info0.amount = info0.amount - data.goal0;
            var info1 = InfoManager.instance.GetIngredientInfo(data.request_ingredient_id1);
            info1.amount = info1.amount - data.goal1;
        }
        InfoManager.instance.SaveIngredientInfos();
        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.REFRESH_UI_INVENTORY);
        this.GetReward(data.id);

    }

    public void GetReward(int missionId)
    {
        var data = DataManager.instance.GetMissionData(missionId);

        Debug.LogFormat("�̼� ���� �� {0}�� ����", data.reward_dream_count);

        //var info = InfoManager.instance.MissionInfos.Find(x => x.id == data.id);

        var info = InfoManager.instance.GetMissionInfo(missionId);

        info.isClear = true;
        InfoManager.instance.SaveMissionInfos();
        this.director.missionMini.Init();
        this.director.missionDetail.Init(stageID);

        GetDream(data.reward_dream_count);
    }
    public void GetDream(int amount)
    {
        //�� ����
        InfoManager.instance.DreamAcount(amount);
        StartCoroutine(this.director.dream.CGetDream(amount));

        //�� ����
        this.director.dream.Init();
    }
    private void UseDream(int amount)
    {
        //������ ����
        InfoManager.instance.DreamAcount(-amount);
        StartCoroutine(this.director.dream.CGetDream(-amount));

        //�� ����
        this.director.dream.Init();
    }


    //�������� Ŭ����
    private IEnumerator WaitForStageClearVideo(float time)
    {
        this.player.mono.canMove = false;

        //�ǹ� ���� �� �ణ�� �ð� ��
        yield return new WaitForSeconds(3f);

        //��� ����
        SoundManager.Pause();


        //���� ����
        this.videoPlayerStage.SetDirectAudioVolume(0, PlayerPrefs.GetFloat("SoundVolume"));

        //�������� ��Ȱ ���� ���
        this.stageReviveVideo.SetActive(true);
        yield return new WaitForSeconds(time);
        //���� ��Ȱ��ȭ �ʿ�?

        this.stageReviveVideo.SetActive(false);

        //��� ���
        SoundManager.UnPause();

        //ī�޶� �̵�
        Camera.main.transform.position = new Vector3(191.7f, 5.3184f, -40.1f);
        Camera.main.orthographicSize = 25.5f;
        //�÷��̾� �̵�
        this.player.mono.transform.position = new Vector3(192.77f, 2.33f, -34.69f);

        //����Ʈ ���μ��� ���̾� ����
        this.post.PostRevive();
        this.director.uiClear.SetActive(true);

        //�� ��ȯ �̺�Ʈ
        this.director.dialogue.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        InfoManager.instance.DialogueInfo.id = 10074;

        InitBuildingInfo();

        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.END_REVIVE_VIDEO);


        //yield return new WaitForSeconds(10f);
    }

    public void InitBuildingInfo()
    {
        //8�������� �ǹ� �������ִ� �� ����
        foreach (Building building in buildings)
        {
            if (building.id >= 4016) //���հǹ��� �ʱ�ȭ
            {
                var info = InfoManager.instance.BuildingInfos.Find(x => x.id == building.id);
                info.BuildingInfoInit();

                InfoManager.instance.SaveBuildingInfos();
            }
        }
    }

    public void NPCMove()
    {
        EventManager.instance.npcMove = (buildingNum) =>
        {
            if (buildingNum == 4010)
            {
                for (int i = 0; i < 3; i++)
                {
                    this.fellingNPCS[i].SetActive(true);
                }
            }
            if (buildingNum == 4011)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.fishingNPCS[i].SetActive(true);
                }
            }
            if (buildingNum == 4012)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.farmingNPCS[i].SetActive(true);
                }
            }
            if (buildingNum == 4013)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.miningNPCS[i].SetActive(true);
                }
            }
            if (buildingNum == 4014)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.attackingNPCS[i].SetActive(true);
                }
            }
        };

    }
    public void MoveEffect()
    {
        this.moveEffect.transform.position = this.player.mono.transform.position + new Vector3(0f, 0.2f, -0.2f);
    }
}