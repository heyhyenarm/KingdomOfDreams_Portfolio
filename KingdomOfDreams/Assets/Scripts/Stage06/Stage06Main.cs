using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class Stage06Main : MonoBehaviour
{
    [SerializeField]
    private UIStage06Director director;
    public GameObject playerPrefab;
    public CinemachineVirtualCamera followCam;

    public int stageID;
    private bool isClearStage;
    public List<Building> buildings;

    private Player player;
    public GameObject moveEffect;

    public GameObject stageReviveVideo;
    public FarmRow[] farmrows;

    public Bee beePrefab;

    private bool isClicked;

    //구역 클리어 후 이동하는 npc들
    //public GameObject[] fellingNPCS;
    //public GameObject[] fishingNPCS;
    //public GameObject[] farmingNPCS;
    //public GameObject[] miningNPCS;
    //public GameObject[] attackingNPCS;

    //구역들
    public List<StageArea> stageAreas;

    public int realStageID;

    //미니맵
    public MinimapDirector minimap;
    public Camera miniCam;
    private Vector3 playerPos;

    //마법진
    public GameObject magicCircleGo;

    //포스트 프로세싱
    public PostProcessingRevive post;

    //소리 설정
    private float musicVolume;
    private float otherVolume;
    //비디오 플레이어
    public VideoPlayer videoPlayerClear;
    //플레이어 오디오소스
    AudioSource audioSource;

    //광고 컨트롤러
    //public GoogleAdMobController adController;

    private void Awake()
    {
        var go = GameObject.Find("UIStage06Director");
        go.SetActive(true);
        this.director = go.GetComponent<UIStage06Director>();
        //if (realStageID != 8005)
        //{
        //    this.director.gameObject.SetActive(false);
        //}

        //test
        //this.post.PostRevive();

    }
    public void OnDestroy()
    {
        //벌 생성 이벤트 핸들러 제거
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.APPEAR_BEE, AppearBeeHandler);
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.END_REVIVE_VIDEO, EndReviveVideoHandler);

    }

    public void Start()
    {
        Debug.LogFormat("<color=red>06 main start</color>");

        //material test
        //for (int i = 0; i < this.stageAreas.Count; i++)
        //{
        //    Debug.LogFormat("<color=yellow>area{0} 변함</color>", i);
        //    this.stageAreas[i].clearArea(6, i);
        //}
        var stage06_SC = SoundManager.GetSoundConnectionForThisLevel("Stage06");
        SoundManager.PlayConnection(stage06_SC);

        this.Init();

        //var ad = this.director.shop.uiChestShop.chestCells[0]._btnAD.GetComponent<Button>();
        //ad.onClick.AddListener(LoadAd);

        //Debug.Log("<color>6 main start</color>");
        //this.player.mono.ChangeCharacter(this.director.characterNum);
        //this.player.mono.SetMaterial(this.player.mono.playerMaterials[this.director.materialNum]);

        //벌 생성
        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.APPEAR_BEE, AppearBeeHandler);

        EventManager.instance.blackToColor = (areaNum) =>
        {
            var num = areaNum - 5;
            //this.stageAreas[num].clearArea(this.stageID - 8000, num);
            //Debug.LogFormat("<color>Stage6_{0}</color>", num);
            this.stageAreas[num].clearArea(6, num);
        };

        foreach (Building building in buildings)
        {
            int areaNum = building.id - 4000 - 5;
            if (building.isClear) this.stageAreas[areaNum].clearArea(6, areaNum);
        }

        EventManager.instance.getDream = (amount) =>
        {
            //Debug.LogFormat("<color=yellow>get dream {0}</color>", amount);
            this.GetDream(amount);
        };

    }
    //private void LoadAd()
    //{
    //    this.adController.RequestAndLoadRewardedInterstitialAd();
    //}

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
    public void Init()
    {
        //건물 데이터 연동, 기존일 때 미션 겹쳐나옴, ...
        this.stageID = 8005;
        //Debug.LogFormat("stage ID: {0}", this.stageID);
        //this.missionMain.Init(this.stageID);

        //플레이어, UI 생성
        this.player = new Player(this.playerPrefab);
        //Debug.LogFormat("player: {0}", this.player);
        this.player.State = new NormalState(this.player);

        //Debug.Log("player init");
        this.player.mono.Init();
        this.player.mono.transform.position = new Vector3(-3.79f, 0, -48.48f);
        //this.player.mono.ChangeCharacter(this.director.characterNum);
        //this.player.mono.SetMaterial(this.player.mono.playerMaterials[this.director.materialNum]);
        this.audioSource = this.player.mono.GetComponent<AudioSource>();
        this.audioSource.volume = PlayerPrefs.GetFloat("OtherVolume"); ;

        //초기 이펙트 위치 설정
        this.moveEffect.transform.position = this.player.mono.transform.position + new Vector3(0f, 0.2f, -0.2f);

        this.director.missionID = 1010;
        List<MissionData> stageMissionDatas = DataManager.instance.GetMissionDatas().FindAll(x => x.stage_id == this.stageID);

        //foreach (var info in InfoManager.instance.StageInfos)
        //{
        //    if (!info.isClear)
        //    {
        //        if (info.stage == 5)
        //            this.director.missionID = 1010;
        //        if (info.stage == 6)
        //            this.director.missionID = 1015;
        //        if (info.stage == 7)
        //            this.director.missionID = 1023;
        //    }
        //}


        int index = 0;
        foreach (Building building in buildings)
        {
            building.Init(stageMissionDatas[index].resurrection_id);

            building.btn_UseDream.onClick.AddListener(() =>
            {
                //꿈 사용 즉시완료 팝업 띄우기
                //Debug.LogFormat("꿈 사용 팝업");
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
            if (InfoManager.instance.DreamInfo.amount >= this.director.uiUseDreamPopup.price)
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
        //EventDispatcher.instance.AddEventHandler<MissionData>((int)LHMEventType.eEventType.ACHIEVE_MISSION, new EventHandler<MissionData>((type, data) =>
        //{
        //    var building = buildings.Find(x => x.id == data.resurrection_id);
        //    building.isDone = true;
        //}));

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
            //Debug.LogFormat("IState:{0}", this.player.State);
            this.player.DoAction();

            Invoke("IsClicked", 2f);
        });



        //npc move 이벤트
        //this.NPCMove();

        //미니맵 Init
        this.playerPos = this.player.mono.gameObject.transform.position;
        this.minimap.player.transform.position = new Vector3(playerPos.x, this.minimap.player.transform.position.y, playerPos.z);
        this.miniCam.transform.position = new Vector3(playerPos.x, this.miniCam.transform.position.y, playerPos.z);
        this.director.miniStage6.SetActive(true);
        this.director.miniStage7.SetActive(false);
        this.director.miniStage8.SetActive(false);

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.END_REVIVE_VIDEO, EndReviveVideoHandler);

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

    private void EndReviveVideoHandler(short type)
    {
        //Debug.LogFormat("<color=magenta>end revive video stageID:{0}</color>", stageID);
        this.director.DialogueInit();
    }

    public void IsClicked()
    {
        this.isClicked = false;

    }

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
                    //Debug.Log("건물 클릭");
                    //지어진 건물
                    if (buildingGo.isBuild)
                    {
                        //Debug.Log("지어진 건물");
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
                else if (hitInfo.transform.gameObject.tag == "Fairy")
                {
                    //요정 클릭
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

        //스테이지 클리어 조건
        if (this.buildings[0].isClear &&
            this.buildings[1].isClear &&
            this.buildings[2].isClear &&
            this.buildings[3].isClear &&
            this.buildings[4].isClear &&
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
            var stageInfo = InfoManager.instance.StageInfos.Find(x => x.stage == 6);
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
        InfoManager.instance.AddStageInfo(6);
        InfoManager.instance.SaveStageInfos();
        var reward = DataManager.instance.GetStageData(this.stageID).reward_chest_id;
        InfoManager.instance.ChestInfos[reward - 100].amount++;
        //Debug.LogFormat("<color=yellow>{0}상자 총{1}개</color>", reward, InfoManager.instance.ChestInfos[reward - 100].amount);
        InfoManager.instance.SaveChestInfos();
    }

    private List<IngredientInfo> tempInfo;

    private void SubtractIngredientAmountB(Building building)
    {
        var data = DataManager.instance.GetMissionDatas().Find(x => x.resurrection_id == building.id);
        var info = InfoManager.instance.GetIngredientInfo(data.request_ingredient_id0);

        info.amount = info.amount - data.goal0;
        InfoManager.instance.SaveIngredientInfos();
        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.REFRESH_UI_INVENTORY);
        this.GetReward(data.id);
    }

    public void GetReward(int id)
    {
        var data = DataManager.instance.GetMissionData(id);

        //Debug.LogFormat("미션 보상 꿈 {0}개 받음", data.reward_dream_count);

        //var info = InfoManager.instance.MissionInfos.Find(x => x.id == data.id);

        var info = InfoManager.instance.GetMissionInfo(id);

        info.isClear = true;
        InfoManager.instance.SaveMissionInfos();
        this.director.missionMini.Init();
        this.director.missionDetail.Init(stageID);

        GetDream(data.reward_dream_count);
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


    //스테이지 클리어
    private IEnumerator WaitForStageClearVideo(float time)
    {
        this.player.mono.canMove = false;

        //건물 지은 후 약간의 시간 텀
        yield return new WaitForSeconds(3f);

        //브금 중지
        SoundManager.Pause();


        //볼륨 조절
        this.videoPlayerClear.SetDirectAudioVolume(0, PlayerPrefs.GetFloat("SoundVolume"));

        //스테이지 부활 영상 재생
        this.stageReviveVideo.SetActive(true);
        yield return new WaitForSeconds(time);
        //영상 비활성화 필요?
        //포스트 프로세싱 레이어 변경
        this.post.PostRevive();
        this.director.uiClear.SetActive(true);

        this.stageReviveVideo.SetActive(false);

        //브금 재생
        SoundManager.UnPause();

        //카메라 이동
        Camera.main.transform.position = new Vector3(-6.3f, 5.494f, -24.1f);
        Camera.main.orthographicSize = 22.25f;
        //플레이어 이동
        this.player.mono.transform.position = new Vector3(-3.79f, 0, -48.48f);

        this.director.dialogue.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);

        InfoManager.instance.DialogueInfo.id = 10054;
        InfoManager.instance.SaveDialogueInfo();

        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.END_REVIVE_VIDEO);


        //yield return new WaitForSeconds(10f);

    }

    //public void NPCMove()
    //{
    //    EventManager.instance.npcMove = (buildingNum) =>
    //    {
    //        if (buildingNum == 4005)
    //        {
    //            for (int i = 0; i < 3; i++)
    //            {
    //                this.fellingNPCS[i].SetActive(true);
    //            }
    //        }
    //        if (buildingNum == 4006)
    //        {
    //            for (int i = 0; i < 3; i++)
    //            {
    //                this.fishingNPCS[i].SetActive(true);
    //            }
    //        }
    //        if (buildingNum == 4007)
    //        {
    //            for (int i = 0; i < 3; i++)
    //            {
    //                this.farmingNPCS[i].SetActive(true);
    //            }
    //        }
    //        if (buildingNum == 4008)
    //        {
    //            for (int i = 0; i < 3; i++)
    //            {
    //                this.miningNPCS[i].SetActive(true);
    //            }
    //        }
    //        if (buildingNum == 4009)
    //        {
    //            for (int i = 0; i < 3; i++)
    //            {
    //                this.attackingNPCS[i].SetActive(true);
    //            }
    //        }
    //    };

    //}
    public void MoveEffect()
    {
        this.moveEffect.transform.position = this.player.mono.transform.position + new Vector3(0f, 0.2f, -0.2f);
    }
}
