using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class Tutorial05Main : MonoBehaviour
{
    //public PlayerMono playerMono;
    public UITutorial05Director director;
    public GameObject playerPrefab;
    public Inventory inventory;

    public int stageID;
    private bool isClearStage;
    public NPC npc;
    public Building building;

    private Player player;
    public GameObject arrowGo;
    public GameObject targetGo;
    public GameObject tempTargetGo;
    public GameObject moveEffect;

    public GameObject npcReviveVideo;
    public GameObject stageReviveVideo;

    private bool isClicked;

    public StageArea tutorialStage = new StageArea();

    //npc 부활 후 머테리얼
    public Material npcColor;

    public Dungeon dungeon;
    private Enums.eMonsterState monsterState;
    private AttackingMonster target;

    //포스트 프로세싱
    public PostProcessingRevive post;

    //부활 Material
    public ChangeTutorialStage changeTutorialStage;

    //소리 설정
    private float musicVolume;
    private float otherVolume;
    //비디오 플레이어
    public VideoPlayer videoPlayerNPC;
    public VideoPlayer videoPlayerStage;
    //플레이어 오디오소스
    AudioSource audioSource;

    //광고 컨트롤러
    //public GoogleAdMobController adController;

    public void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.END_REVIVE_VIDEO, EndReviveVideoHandler);
    }

    public void Start()
    {
        //this.tutorialStage.clearGround(4);
        var tuto05_SC = SoundManager.GetSoundConnectionForThisLevel("Tutorial05");
        SoundManager.PlayConnection(tuto05_SC);
    }
    public void Init()
    {
        this.tempTargetGo = this.targetGo;

        this.stageID = 8004;
        Debug.LogFormat("stage ID: {0}", this.stageID);
        //this.missionMain.Init(this.stageID);

        //플레이어, UI 생성
        this.player = new Player(this.playerPrefab);
        Debug.LogFormat("player: {0}", this.player);
        this.player.State = new NormalState(this.player);

        Debug.Log("player init");
        this.player.mono.Init();
        this.player.mono.transform.position = new Vector3(-17.93f, -0.55f, 8.14f);
        this.player.mono.transform.rotation = Quaternion.Euler(0, 180, 0);
        this.audioSource = this.player.mono.GetComponent<AudioSource>();
        this.audioSource.volume = PlayerPrefs.GetFloat("OtherVolume"); ;

        this.director.Init(stageID);

        this.player.mono.joystick = this.director.joystick;

        //초기 이펙트 위치 설정
        this.moveEffect.transform.position = this.player.mono.transform.position + new Vector3(0f, 0.2f, -0.2f);

        //this.inventory.Init();

        //var data = DataManager.instance.GetMissionDatas().Find(x => x.stage_id == this.stageID);
        List<MissionData> stageMissionDatas = DataManager.instance.GetMissionDatas().FindAll(x => x.stage_id == this.stageID);

        var Info0 = InfoManager.instance.MissionInfos.Find(x => x.id == stageMissionDatas[0].id);
        var Info1 = InfoManager.instance.MissionInfos.Find(x => x.id == stageMissionDatas[1].id);
        this.npc.Init(stageMissionDatas[0].resurrection_id);
        if (Info0 != null) this.npc.isClear = Info0.isClear;
        this.building.Init(stageMissionDatas[1].resurrection_id);

        this.building.btn_UseDream.onClick.AddListener(() =>
        {
            //꿈 사용 즉시완료 팝업 띄우기
            Debug.LogFormat("꿈 사용 팝업");
            this.director.uiUseDreamPopup.gameObject.SetActive(true);
            this.director.uiUseDreamPopup.PriceUpdate(this.building.buildData);
        });
        //this.building.btn_PlayAD.onClick.AddListener(() =>
        //{
        //    //광고 재생
        //    Debug.LogFormat("광고 재생");
        //    if (this.adController.rewardedInterstitialAd != null)
        //    {
        //        building.buildTime = 0;
        //    }
        //});
        this.director.uiUseDreamPopup.btn_use.onClick.AddListener(() =>
        {
            //실제 꿈 사용, 건물 짓기
            //Debug.LogFormat("꿈 사용");
            if (InfoManager.instance.DreamInfo.amount >= this.director.uiUseDreamPopup.price)
            {
                UseDream(this.director.uiUseDreamPopup.price);
                int id = this.director.uiUseDreamPopup.ClosePopup();
                //꿈 개수 업데이트
                this.director.dream.Init();
                this.building.BuildObject();
            }
            else
            {
                StartCoroutine(this.director.BuildingAlert());
            }
        });

        this.dungeon.Init5();

        //상호작용 버튼 클릭
        this.director.btn_Interaction.onClick.AddListener(() => {
            this.isClicked = true;

            switch (this.player.mono.location)
            {
                case Enums.ePlayerLocation.Plain:
                    this.player.State = new NormalState(this.player);
                    break;

                case Enums.ePlayerLocation.Dungeon:
                    this.player.State = new AttackState(this.player);
                    break;
            }
            Debug.LogFormat("IState:{0}", this.player.State);
            if (this.player.mono.actionTarget != null)
                this.player.DoAction();
            Invoke("IsClicked", 1.1f);
        });

        //미션 클리어 이벤트
        EventManager.instance.onAchieved = (data) =>
        {
            //data id가 npc일 때
            if (data.resurrection_id < 3005
            && !this.npc.isClear)
            {
                this.npc.img.gameObject.SetActive(true);
                this.targetGo = this.npc.gameObject;
                this.npc.isDone = true;
            }
            //data id가 건물 일때
            else if (data.resurrection_id >= 4000)
            {
                this.targetGo = this.building.gameObject;
                this.building.isDone = true;
                this.building.effect.SetActive(true);
            }
        };
        //EventDispatcher.instance.AddEventHandler<MissionData>((int)LHMEventType.eEventType.ACHIEVE_MISSION, new EventHandler<MissionData>((type, data) =>
        //{
        //    //data id가 npc일 때
        //    if (data.resurrection_id < 3005
        //    && !this.npc.isClear)
        //    {
        //        this.npc.img.gameObject.SetActive(true);
        //        this.targetGo = this.npc.gameObject;
        //        this.npc.isDone = true;
        //    }
        //    //data id가 건물 일때
        //    else if (data.resurrection_id >= 4000)
        //    {
        //        this.targetGo = this.building.gameObject;
        //        this.building.isDone = true;
        //    }
        //}));

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
        //상호작용 버튼 활성화, 비활성화
        if (this.player.mono.isTargeting && this.player.mono.actionTarget != null && !this.isClicked)
        {
            this.monsterState = this.player.mono.actionTarget.GetComponent<AttackingMonster>().state;

            if (this.monsterState != Enums.eMonsterState.Hit && this.monsterState != Enums.eMonsterState.Die
                && this.player.mono.state != Enums.ePlayerState.Attacking_Stun)
            {
                this.target = this.player.mono.actionTarget.GetComponent<AttackingMonster>();
                this.director.btn_Interaction.interactable = true;
                var atlas = AtlasManager.instance.GetAtlasByName("Interaction");
                var sprite = atlas.GetSprite("Sword");
                this.director.icon_Interaction.sprite = sprite;
                this.director.icon_Interaction.gameObject.SetActive(true);
            }
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

        //튜토리얼 화살표
        this.ArrowGoUpdate();

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
                //Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "NPC")
                {
                    var npc = hitInfo.transform.gameObject.GetComponent<NPC>();
                    //Debug.Log("NPC 클릭");
                    //미션 클리어?
                    if (npc.isDone && !npc.isClear)
                    {

                        //브금 중지
                        SoundManager.Pause();

                        //느낌표 비활성화
                        Debug.Log("npc 클릭, 느낌표 비활성화");
                        this.npc.img.gameObject.SetActive(false);

                        //Debug.Log("npc revive");
                        this.targetGo = null;
                        //미션 재료 개수 차감
                        this.SubtractIngredientAmountN(npc);
                        StartCoroutine(WaitForNPCReviveVideo(8f));

                        //npc 컬러로 변경
                        //NPC 찾기
                        GameObject NPC = GameObject.Find("NPC");

                        // 자식 오브젝트에서 Renderer 컴포넌트 가져오기
                        Transform npcChildTransform = NPC.transform.Find("npc");
                        if (npcChildTransform != null)
                        {
                            Renderer childRenderer = npcChildTransform.GetComponent<Renderer>();
                            if (childRenderer != null)
                            {
                                childRenderer.material = npcColor;
                            }
                        }

                    }
                    else if (npc.isDone && npc.isClear)
                    {
                        //Debug.Log("미션 클리어 O");
                    }
                    else
                    {
                        //Debug.Log("미션 클리어 X"); 
                    }
                }
                else if (hitInfo.transform.gameObject.tag == "Building")
                {
                    var buildingGo = hitInfo.transform.gameObject.GetComponent<Building>();
                    Debug.Log("건물 클릭");
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
                        this.targetGo = null;

                        //미션 재료 개수 차감
                        SubtractIngredientAmountB(buildingGo);

                        //천막 활성화, 시간 흐름
                        //this.adController.RequestAndLoadRewardedInterstitialAd();
                        buildingGo.StartBuild();
                    }
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
        if (this.npc.isClear && this.building.isClear && !this.isClearStage)
        {

            List<MissionData> stageMissionDatas = DataManager.instance.GetMissionDatas().FindAll(x => x.stage_id == this.stageID);
            var Info0 = InfoManager.instance.MissionInfos.Find(x => x.id == stageMissionDatas[0].id);
            var Info1 = InfoManager.instance.MissionInfos.Find(x => x.id == stageMissionDatas[1].id);
            if (Info0.isClear && Info1.isClear)
            {
                this.isClearStage = true;
                StartCoroutine(WaitForStageClearVideo(8f));
            }
            var stageInfo = InfoManager.instance.StageInfos.Find(x => x.stage == 5);
            if (stageInfo == null)
            {
                this.SaveClearInfos();
            }
        }
    }
    private void SaveClearInfos()
    {
        InfoManager.instance.AddStageInfo(5);
        InfoManager.instance.SaveStageInfos();
        var reward = DataManager.instance.GetStageData(this.stageID).reward_chest_id;
        InfoManager.instance.ChestInfos[reward - 100].amount++;
        Debug.LogFormat("<color=yellow>{0}상자 총{1}개</color>", reward, InfoManager.instance.ChestInfos[reward - 100].amount);
        InfoManager.instance.SaveChestInfos();
    }
    public void ArrowGoUpdate()
    {
        if (targetGo != null)
        {
            //튜토리얼 가이드 화살표
            Vector3 arrow = new Vector3(this.player.mono.transform.position.x, -1f, this.player.mono.transform.position.z) - new Vector3(this.targetGo.transform.position.x, 0, this.targetGo.transform.position.z);
            float distance = Vector3.Distance(new Vector3(this.player.mono.transform.position.x, 0, this.player.mono.transform.position.z), new Vector3(this.targetGo.transform.position.x, 0, this.targetGo.transform.position.z));
            //타겟과 거리가 가까우면 비활성화
            if (distance > 2f)
            {
                this.arrowGo.transform.position = this.player.mono.transform.position - arrow.normalized;
                this.arrowGo.transform.LookAt(new Vector3(this.targetGo.transform.position.x, this.arrowGo.transform.position.y, this.targetGo.transform.position.z));
                this.arrowGo.SetActive(true);
            }
            else this.arrowGo.SetActive(false);
        }
        else this.arrowGo.SetActive(false);
    }

    private List<IngredientInfo> tempInfo;
    private void SubtractIngredientAmountN(NPC npc)
    {
        var data = DataManager.instance.GetMissionDatas().Find(x => x.resurrection_id == npc.id);
        var infoFound = InfoManager.instance.IngredientInfos.Find(x => x.id == data.request_ingredient_id0);
        int difference = infoFound.amount - data.goal0;
        var fishInfo = InfoManager.instance.IngredientInfos.Find(x => x.id == 2004);

        fishInfo.amount = difference;
        Debug.Log("<color=magenta>미션에서 차감</color>");
        InfoManager.instance.SaveIngredientInfos();
        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.REFRESH_UI_INVENTORY);
        this.GetReward(data.id);
    }
    private void SubtractIngredientAmountB(Building building)
    {
        var data = DataManager.instance.GetMissionDatas().Find(x => x.resurrection_id == building.id);
        var infoFound = InfoManager.instance.IngredientInfos.Find(x => x.id == data.request_ingredient_id0);
        int difference = infoFound.amount - data.goal0;

        var fishInfo = InfoManager.instance.IngredientInfos.Find(x => x.id == 2004);
        fishInfo.amount = difference;
        Debug.Log("<color=magenta>미션에서 차감</color>");
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


    //플레이어 부활 영상 시간
    private IEnumerator WaitForNPCReviveVideo(float time)
    {
        this.player.mono.canMove = false;

        //볼륨 조절
        this.videoPlayerNPC.SetDirectAudioVolume(0, this.director.setting.musicVolume);

        //NPC 부활 영상 재생
        this.npcReviveVideo.SetActive(true);
        yield return new WaitForSeconds(time);
        this.npcReviveVideo.gameObject.SetActive(false);
        npc.isClear = true;
        //브금 재생
        SoundManager.UnPause();

        this.director.DialogueInit();
        this.player.mono.canMove = true;
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
        this.videoPlayerStage.SetDirectAudioVolume(0, this.director.setting.musicVolume);

        //스테이지 부활 영상 재생
        this.stageReviveVideo.SetActive(true);

        yield return new WaitForSeconds(time);
        //영상 비활성화 필요?
        //포스트 프로세싱 레이어 변경
        this.post.PostRevive();
        this.changeTutorialStage.clearTutorial(this.stageID - 8000);
        this.director.uiClear.SetActive(true);

        this.stageReviveVideo.SetActive(false);

        //브금 재생
        SoundManager.UnPause();

        this.director.dialogue.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);

        InfoManager.instance.DialogueInfo.id = 10038;
        InfoManager.instance.SaveDialogueInfo();

        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.END_REVIVE_VIDEO);


        //yield return new WaitForSeconds(10f);

        //this.tutorialStage.clearGround(this.stageID - 8000);

    }
    public void MoveEffect()
    {
        this.moveEffect.transform.position = this.player.mono.transform.position + new Vector3(0f, 0.2f, -0.2f);
    }
}
