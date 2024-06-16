using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;



public class Stage08Main : MonoBehaviour
{
    [SerializeField]
    private UIStage06Director director;
    public GameObject playerPrefab;
    public CinemachineVirtualCamera followCam;

    public int stageID;
    private bool isClearStage;
    private bool isClearCBuilding;
    public List<Building> buildings;

    private Player player;
    public GameObject moveEffect;

    public GameObject buildingReviveVideo;
    public GameObject stageReviveVideo;
    public FarmRow[] farmrows;

    public Bee beePrefab;

    private bool isClicked;

    //구역 클리어 후 이동하는 npc들
    public GameObject[] fellingNPCS;
    public GameObject[] fishingNPCS;
    public GameObject[] farmingNPCS;
    public GameObject[] miningNPCS;
    public GameObject[] attackingNPCS;

    public Camera postCam;


    //구역들
    public List<StageArea> stageAreas;

    public int realStageID;

    //미니맵
    public MinimapDirector minimap;
    public Camera miniCam;
    private Vector3 playerPos;

    private bool isVisible;
    private GameObject workBenchGo;

    private bool addComplexMission;

    //포스트 프로세싱
    public PostProcessingRevive post;

    //소리 설정
    private float musicVolume;
    private float otherVolume;
    //비디오 플레이어
    public VideoPlayer videoPlayerBuildings;
    public VideoPlayer videoPlayerStage;
    //플레이어 오디오소스
    AudioSource audioSource;

    //광고 컨트롤러
    //public GoogleAdMobController adController;

    private int clearMissionCount;

    private void Awake()
    {
        //포스트 프로세싱 테스트
        //this.post.PostRevive();

        //var go = GameObject.FindObjectOfType<UIStage06Director>();
        var go = GameObject.Find("UIStage06Director");
        go.SetActive(true);
        this.director = go.GetComponent<UIStage06Director>();
        this.director.map.uiStage.gameObject.SetActive(false);
        //this.director.shop.uiChestShop.adController = this.adController;
    }

    public void Start()
    {
        Debug.Log("<color=green>stage08 maing start</color>");

        var stage08_SC = SoundManager.GetSoundConnectionForThisLevel("Stage08");
        SoundManager.PlayConnection(stage08_SC);

        //if (InfoManager.instance.StageInfos[7].isClear)
        //{
        //    //포스트 프로세싱 레이어 변경
        //    this.post.PostRevive();
        //}

        this.Init();

        //var ad = this.director.shop.uiChestShop.chestCells[0]._btnAD.GetComponent<Button>();
        //ad.onClick.AddListener(LoadAd);

        //벌 생성
        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.APPEAR_BEE, AppearBeeHandler);

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.END_REVIVE_VIDEO, EndReviveVideoHandler);

        EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.CLEAR_MISSION_REFRESH, CountClearMissionHandler);

        EventManager.instance.blackToColor = (buildungNum) =>
        {
            var num = buildungNum - 15;
            Debug.LogFormat("<color>Stage8_{0}</color>", num);
            this.stageAreas[num].clearArea(8, num);
        };


        EventManager.instance.getDream = (amount) =>
        {
            Debug.LogFormat("<color=yellow>get dream {0}</color>", amount);
            GetDream(amount);
        };

        this.workBenchGo = GameObject.Find("WorkBench");

        if (InfoManager.instance.DialogueInfo.id > 10090)
        {
            this.workBenchGo.transform.GetChild(0).gameObject.SetActive(true);
            this.workBenchGo.transform.GetChild(1).gameObject.SetActive(true);
            this.workBenchGo.transform.GetChild(2).gameObject.SetActive(true);
        }

        //다시 로드 시 부활된 구역들 material color 설정
        foreach (Building building in buildings)
        {
            int areaNum = building.id - 4000 - 15;
            Debug.LogFormat("areaNum : {0}, building id : {1}", areaNum, building.id);
            if (building.isClear)
            {
                this.stageAreas[areaNum].clearArea(8, areaNum);
            }
        }
        EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.USE_DREAM, new EventHandler<int>((type, amount) =>
        {
            Debug.LogFormat("use dream {0}", amount);
            this.UseDream(amount);
        }));

    }
    public void OnDestroy()
    {
        //벌 생성 이벤트 핸들러 제거
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.APPEAR_BEE, AppearBeeHandler);
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.END_REVIVE_VIDEO, EndReviveVideoHandler);
        EventDispatcher.instance.RemoveEventHandler<int>((int)LHMEventType.eEventType.CLEAR_MISSION_REFRESH, CountClearMissionHandler);

    }
    //벌 생성 이벤트 핸들러
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
        Debug.LogFormat("<color=magenta>end revive video stageID:{0}</color>", stageID);
        this.director.DialogueInit();
    }
    private void CountClearMissionHandler(short type, int clearMissionCount)
    {
        this.clearMissionCount = clearMissionCount;
    }
    //public void OnDestroy()
    //{
    //    // FarmRow 파괴 시 이벤트 핸들러 제거
    //    EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.END_REVIVE_VIDEO, EndReviveVideoHandler);
    //}

    public void Init()
    {
        this.stageID = 8007;
        Debug.LogFormat("stage ID: {0}", this.stageID);
        //this.missionMain.Init(this.stageID);

        //플레이어, UI 생성
        this.player = new Player(this.playerPrefab);
        Debug.LogFormat("player: {0}", this.player);
        this.player.State = new NormalState(this.player);

        Debug.Log("player init");
        this.player.mono.Init();
        this.player.mono.transform.position = new Vector3(194.3f, 1.5f, -126f);
        this.audioSource = this.player.mono.GetComponent<AudioSource>();
        this.audioSource.volume = PlayerPrefs.GetFloat("OtherVolume"); ;

        //초기 이펙트 위치 설정
        this.moveEffect.transform.position = this.player.mono.transform.position + new Vector3(0f, 0.2f, -0.2f);

        this.director.missionID = 1023;
        List<MissionData> stageMissionDatas = DataManager.instance.GetMissionDatas().FindAll(x => x.stage_id == this.stageID);

        int index = 0;
        foreach (Building building in buildings)
        {
            building.Init(stageMissionDatas[index].resurrection_id);

            building.btn_UseDream.onClick.AddListener(() =>
            {
                //꿈 사용 즉시완료 팝업 띄우기
                Debug.LogFormat("꿈 사용 팝업");
                this.director.uiUseDreamPopup.gameObject.SetActive(true);
                this.director.uiUseDreamPopup.PriceUpdate(building.buildData);
            });
            //building.btn_PlayAD.onClick.AddListener(() =>
            //{
            //    //광고 재생
            //    Debug.LogFormat("광고 재생");
            //    if (this.adController.rewardedInterstitialAd != null)
            //    {
            //        building.buildTime = 0;
            //    }
            //});

            index++;
        }

        this.director.uiUseDreamPopup.btn_use.onClick.AddListener(() =>
        {
            //실제 꿈 사용, 건물 짓기
            //Debug.LogFormat("꿈 사용");
            if(InfoManager.instance.DreamInfo.amount>=this.director.uiUseDreamPopup.price)
            {
                UseDream(this.director.uiUseDreamPopup.price);
                int id = this.director.uiUseDreamPopup.ClosePopup();
                //꿈 개수 업데이트
                var building = this.buildings.Find(x => x.id == id);
                building.BuildObject();
            }
            else
            {
                StartCoroutine(this.director.BuildingAlert());
            }
        });

        //미션 클리어 이벤트
        EventManager.instance.onAchieved = (data) => {
            var building = buildings.Find(x => x.id == data.resurrection_id);
            if (building != null)
            {
                building.isDone = true;
                building.effect.SetActive(true);
            }
        };

        //this.director.AddEvent();
        if(this.realStageID == stageID)
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

        //상호작용 버튼 클릭
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

            //Invoke("IsClicked", 2f);

            this.isClicked = false;
        });
        //미니맵 Init
        Debug.LogFormat("<color=green>playerPos:{0}, player mono:{1}</color>", this.playerPos, this.player.mono);
        this.playerPos = this.player.mono.gameObject.transform.position;
        this.minimap.player.transform.position = new Vector3(playerPos.x, this.minimap.player.transform.position.y, playerPos.z);
        this.miniCam.transform.position = new Vector3(playerPos.x, this.miniCam.transform.position.y, playerPos.z);
        this.director.miniStage6.SetActive(false);
        this.director.miniStage7.SetActive(false);
        this.director.miniStage8.SetActive(true);
        Debug.LogFormat("<color=green>playerPos:{0}, player mono:{1}</color>", this.playerPos, this.player.mono);

        this.NPCMove();

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
    }
    //private void LoadAd()
    //{
    //    this.adController.RequestAndLoadRewardedInterstitialAd();
    //}

    public void IsClicked()
    {
        this.isClicked = false;

    }

    private int workBenchCount;
    private void Update()
    {
        //미니맵 위치 업데이트
        this.playerPos = this.player.mono.gameObject.transform.position;
        this.minimap.player.transform.position = new Vector3(playerPos.x, this.minimap.player.transform.position.y, playerPos.z);
        this.miniCam.transform.position = new Vector3(playerPos.x, this.miniCam.transform.position.y, playerPos.z);

        //상호작용 버튼 활성화, 비활성화
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

        //뛰는 상태면 이펙트 따라 움직임
        int animState = this.player.mono.anim.GetInteger("State");
        if (animState == 1 || animState == 3 || animState == 5 || animState == 7)
            this.MoveEffect();

        //오브젝트 클릭
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
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "Building")
                {
                    var buildingGo = hitInfo.transform.gameObject.GetComponent<Building>();
                    Debug.Log("건물 클릭");
                    //지어진 건물
                    if (buildingGo.isBuild)
                    {
                        Debug.Log("지어진 건물");
                    }
                    //시간 종료 && 안지어진 건물
                    else if (buildingGo.endTime)
                    {
                        //텐트 비활성화, 건물 지어지는 애니메이션
                        buildingGo.BuildObject();
                    }
                    //시간 흐르는 중
                    else if (!buildingGo.isComplete)
                    {
                        //Debug.Log("지어지는 중");
                    }
                    //미션 클리어?
                    else if (buildingGo.isDone)
                    {
                        //Debug.Log("텐트 활성화");
                        //미션 재료 개수 차감
                        SubtractIngredientAmountB(buildingGo);

                        //천막 활성화, 시간 흐름
                        //this.adController.RequestAndLoadRewardedInterstitialAd();
                        buildingGo.StartBuild();
                    }
                }
                else if (hitInfo.transform.gameObject.tag == "FoodWorkBench")
                {
                    this.director.foodWorkBench.gameObject.SetActive(true);
                }
                else if(hitInfo.transform.gameObject.tag == "IronWorkBench")
                {
                    this.director.ironWorkBench.gameObject.SetActive(true);
                }
                else if(hitInfo.transform.gameObject.tag == "MeatWorkBench")
                {
                    this.director.meatWorkBench.gameObject.SetActive(true);
                }
                else if (hitInfo.transform.gameObject.tag == "Fairy")
                {
                    //요정 클릭
                    //Debug.Log("Fairy hit");
                    EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.ENTER_FAIRYSHOP);
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }

        // 카메라의 viewport 영역 내에 오브젝트가 있는지 확인
        if (InfoManager.instance.DialogueInfo.id <= 10091)
        {
            if (this.workBenchCount == 3)
                workBenchCount = 0;

            if (this.workBenchGo != null)
            {
                if (this.workBenchGo.transform.GetChild(workBenchCount).gameObject.activeSelf == true && !this.isVisible)
                {
                    Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.workBenchGo.transform.GetChild(workBenchCount).gameObject.transform.position);
                    bool isObjectVisible = (viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1 && viewportPos.z > 0);
                    
                    if (isObjectVisible)
                    {
                        // 오브젝트가 화면 내에 보이게 되었을 때 실행할 메서드 호출
                        OnObjectVisible();
                        isVisible = true;
                    }

                    workBenchCount++;
                }
            }
        }
        //시장 건설 완료
        if (this.buildings[0].isClear && InfoManager.instance.MissionInfos.FindIndex(x =>x.id ==1024) ==-1 &&InfoManager.instance.DialogueInfo.id<=10077)
        {
            if (InfoManager.instance.DialogueInfo.id != 10077)
            {
                InfoManager.instance.DialogueInfo.id = 10077;
                InfoManager.instance.SaveDialogueInfo();
                this.director.DialogueInit();
            }
            else
            {
                this.director.DialogueInit();
                InfoManager.instance.DialogueInfo.id = 10078;
                InfoManager.instance.SaveDialogueInfo();
            }
        }
        //복합 건물 건설 완료 <= 복합 건물 부활 영상 재생
        if (this.buildings[1].isClear && 
            this.buildings[2].isClear &&
            this.buildings[3].isClear && InfoManager.instance.DialogueInfo.id <= 10081 && !this.isClearCBuilding)
        {
            this.isClearCBuilding = true;

            StartCoroutine(WaitForBuildingReviveVideo(8f));//시간은 영상만든 후 영상 길이만큼 변경
        }

        //스테이지 클리어 조건
        if (this.buildings[4].isClear &&
            this.buildings[5].isClear && InfoManager.instance.DialogueInfo.id < 10093 && !this.isClearStage)
        {
            this.isClearStage = true;

            Debug.LogFormat("스테이지 08 클리어");
            //InfoManager.instance.StageInfos[7].isClear = true;
            //InfoManager.instance.SaveStageInfos();

            StartCoroutine(WaitForStageClearVideo(24f));
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
    private void OnObjectVisible()
    {
        // 화면 내에 보이게 되었을 때 실행할 동작 구현
        Debug.Log("Object is visible!");
        InfoManager.instance.DialogueInfo.id = 10091;
        this.director.DialogueInit();
    }

    private List<IngredientInfo> tempInfo;

    private void SubtractIngredientAmountB(Building building)
    {
        var data = DataManager.instance.GetMissionDatas().FindLast(x => x.resurrection_id == building.id);
        Debug.LogFormat("<color=yellow>data.id:{0}, data.resurrection_id:{1}, building.id:{2}</color>", data.id, data.resurrection_id, building.id);
        //List<IngredientInfo> infos = new List<IngredientInfo>();
        if (data.id == 1023)
        {
            var info0 = InfoManager.instance.GetIngredientInfo(data.request_ingredient_id0);
            info0.amount = info0.amount - data.goal0;
            var info1 = InfoManager.instance.GetIngredientInfo(data.request_ingredient_id1);
            info1.amount = info1.amount - data.goal1;
            var info2 = InfoManager.instance.GetIngredientInfo(data.request_ingredient_id2);
            info2.amount = info2.amount - data.goal2;
            var info3 = InfoManager.instance.GetIngredientInfo(data.request_ingredient_id3);
            info3.amount = info3.amount - data.goal3;
            var info4 = InfoManager.instance.GetIngredientInfo(data.request_ingredient_id4);
            info4.amount = info4.amount - data.goal4;
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

    public void GetReward(int id)
    {
        var data = DataManager.instance.GetMissionData(id);

        Debug.LogFormat("미션 보상 꿈 {0}개 받음", data.reward_dream_count);

        //var info = InfoManager.instance.MissionInfos.Find(x => x.id == data.id);

        var info = InfoManager.instance.GetMissionInfo(id);

        info.isClear = true;
        InfoManager.instance.SaveMissionInfos();
        this.director.missionMini.Init();
        this.director.missionDetail.Init(stageID);

        GetDream(data.reward_dream_count);
        //땅 변함
        if(info.id == 1026) MaterialManager.instance.Change8Ground();
    }
    public void GetDream(int amount)
    {
        //꿈 받음
        InfoManager.instance.DreamAcount(amount);
        StartCoroutine(this.director.dream.CGetDream(amount));

        //꿈 업뎃
        this.director.dream.Init();
    }
    private void UseDream(int amount)
    {
        //꿈으로 구매
        InfoManager.instance.DreamAcount(-amount);
        StartCoroutine(this.director.dream.CGetDream(-amount));

        //꿈 업뎃
        this.director.dream.Init();
    }

    //복합건물 클리어
    private IEnumerator WaitForBuildingReviveVideo(float time)
    {
        this.player.mono.canMove = false;

        //브금 중지
        SoundManager.Pause();

        //볼륨 조절
        this.videoPlayerBuildings.SetDirectAudioVolume(0, PlayerPrefs.GetFloat("SoundVolume"));

        //스테이지 부활 영상 재생
        this.buildingReviveVideo.SetActive(true);
        yield return new WaitForSeconds(time);
        this.buildingReviveVideo.SetActive(false);

        this.player.mono.canMove = true;

        //브금 재생
        SoundManager.UnPause();


        this.director.dialogue.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);

        InfoManager.instance.DialogueInfo.id = 10081;
        InfoManager.instance.SaveDialogueInfo();

        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.END_REVIVE_VIDEO);
    }
    //스테이지 클리어
    private IEnumerator WaitForStageClearVideo(float time)
    {
        this.player.mono.canMove = false;

        //건물 지은 후 약간의 시간 텀
        yield return new WaitForSeconds(3f);

        //브금 중지
        SoundManager.Pause();

        //볼륨 조절
        this.videoPlayerStage.SetDirectAudioVolume(0, PlayerPrefs.GetFloat("SoundVolume"));

        //스테이지 부활 영상 재생
        this.stageReviveVideo.SetActive(true);
        yield return new WaitForSeconds(time);
        this.stageReviveVideo.SetActive(false);

        this.player.mono.canMove = true;

        //브금 재생
        SoundManager.UnPause();

        //포스트 프로세싱 레이어 변경
        this.post.PostRevive();
        this.director.uiClear.SetActive(true);

        //카메라 이동
        Camera.main.transform.position = new Vector3(191.327f, 49f, -93f);
        Camera.main.orthographicSize = 61.63f;
        postCam.transform.position = new Vector3(187.9f, 49f, -93f);
        postCam.orthographicSize = 105.8f;
        postCam.nearClipPlane = -52.5f;


        InfoManager.instance.DialogueInfo.id = 10093;
        InfoManager.instance.SaveDialogueInfo();

        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.END_REVIVE_VIDEO);

        //yield return new WaitForSeconds(10f);
        //씬 전환 이벤트
        //EventManager.instance.onStage08ClearUI();
    }

    public void NPCMove()
    {
        //8스테이지 buildingNum 4015(시장)부터 시작
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
