using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Tutorial01Main : MonoBehaviour
{
    //public PlayerMono playerMono;
    public UITutorial01Director director;
    public GameObject playerPrefab;
    public Inventory inventory;

    public int stageID;
    private bool isClearStage;
    public NPC npc;
    public Building building;

    private Player player;
    public GameObject arrowGo;
    public GameObject targetGo;

    public GameObject npcReviveVideo;
    public GameObject stageReviveVideo;

    private bool isClicked;

    //npc ��Ȱ �� ���׸���
    public Material npcColor;

    //����Ʈ ���μ���
    public PostProcessingRevive post;

    //��Ȱ Material
    public ChangeTutorialStage changeTutorialStage;

    //�Ҹ� ����
    private float musicVolume;
    private float otherVolume;
    //���� �÷��̾�
    public VideoPlayer videoPlayerNPC;
    public VideoPlayer videoPlayerStage;
    //�÷��̾� ������ҽ�
    AudioSource audioSource;

    //���� ��Ʈ�ѷ�
    //public GoogleAdMobController adController;

    public void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.END_REVIVE_VIDEO, EndReviveVideoHandler);

    }
    public void Start()
    {
        //this.tutorialStage.clearGround(0);
        //����Ʈ ���μ��� �׽�Ʈ
        //this.post.PostRevive();

        //SoundManager.SetVolume(0.5f);
        //SoundManager.CrossIn(1f, SoundManager.PlayConnection(SoundManager.GetSoundConnectionForThisLevel("Tutorial01")));
        var tuto01_SC = SoundManager.GetSoundConnectionForThisLevel("Tutorial01");
        SoundManager.PlayConnection(tuto01_SC);
    }
    public void Init()
    {
        this.stageID = 8000;
        Debug.LogFormat("stage ID: {0}", this.stageID);
        //this.missionMain.Init(this.stageID);

        //�÷��̾�, UI ����
        this.player = new Player(this.playerPrefab);
        Debug.LogFormat("player: {0}", this.player);
        this.player.State = new NormalState(this.player);

        Debug.Log("player init");
        this.player.mono.Init();
        this.player.mono.transform.position = new Vector3(-13f, 7.5f, -1.5f);
        this.audioSource = this.player.mono.GetComponent<AudioSource>();
        this.audioSource.volume = PlayerPrefs.GetFloat("OtherVolume"); ;

        var stageMissionDatas = DataManager.instance.GetMissionDatas().FindAll(x => x.stage_id == this.stageID);

        var Info0 = InfoManager.instance.MissionInfos.Find(x => x.id == stageMissionDatas[0].id);
        var Info1 = InfoManager.instance.MissionInfos.Find(x => x.id == stageMissionDatas[1].id);
        this.npc.Init(stageMissionDatas[0].resurrection_id);
        if (Info0 != null) this.npc.isClear = Info0.isClear;
        this.building.Init(stageMissionDatas[1].resurrection_id);

        this.building.btn_UseDream.onClick.AddListener(() =>
        {
            //�� ��� ��ÿϷ� �˾� ����
            Debug.LogFormat("�� ��� �˾�");
            this.director.uiUseDreamPopup.gameObject.SetActive(true);
            this.director.uiUseDreamPopup.PriceUpdate(this.building.buildData);
        });
        //this.building.btn_PlayAD.onClick.AddListener(() =>
        //{
        //    //���� ���
        //    Debug.LogFormat("���� ���");
        //    if (this.adController.rewardedInterstitialAd != null)
        //    {
        //        building.buildTime = 0;
        //    }
        //});
        this.director.uiUseDreamPopup.btn_use.onClick.AddListener(() =>
        {
            //���� �� ���, �ǹ� ����
            //Debug.LogFormat("�� ���");
            if (InfoManager.instance.DreamInfo.amount >= this.director.uiUseDreamPopup.price)
            {
                UseDream(this.director.uiUseDreamPopup.price);
                int id = this.director.uiUseDreamPopup.ClosePopup();
                //�� ���� ������Ʈ
                this.director.dream.Init();
                this.building.BuildObject();
            }
            else
            {
                StartCoroutine(this.director.BuildingAlert());
            }
        });

        //�̼� Ŭ���� �̺�Ʈ
        EventManager.instance.onAchieved = (data) =>
        {
            //data id�� npc�� ��
            if (data.resurrection_id < 3005
            && !this.npc.isClear)
            {
                this.npc.img.gameObject.SetActive(true);
                this.targetGo = this.npc.gameObject;
                this.npc.isDone = true;

            }
            //data id�� �ǹ� �϶�
            else if (data.resurrection_id >= 4000)
            {
                this.targetGo = this.building.gameObject;
                this.building.isDone = true;
                this.building.effect.SetActive(true);
            }
        };

        this.director.Init(stageID);

        this.player.mono.joystick = this.director.joystick;

        //this.inventory.Init();

        //var data = DataManager.instance.GetMissionDatas().Find(x => x.stage_id == this.stageID);

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
            }
            Debug.LogFormat("IState:{0}", this.player.State);
            this.player.DoAction();

            Invoke("IsClicked", 2f);

        });
        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.END_REVIVE_VIDEO, EndReviveVideoHandler);

        EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.USE_DREAM, new EventHandler<int>((type, amount) =>
        {
            Debug.LogFormat("use dream {0}", amount);
            this.UseDream(amount);
        }));

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

    private void EndReviveVideoHandler(short type)
    {
        Debug.LogFormat("<color=magenta>end revive video stageID:{0}</color>", stageID);
        this.director.DialogueInit();
    }

    public void IsClicked()
    {
        this.isClicked = false;

    }
    private void Update()
    {
        //Debug.LogFormat("<color=red>player: {0} </color>", player);
        //Debug.LogFormat("<color=red>player.mono: {0}</color>",player.mono);
        //Debug.LogFormat("<color=red>isTargeting: {0}</color>",player.mono.isTargeting);
        //Debug.LogFormat("<color=red>actionTarget: {0}</color>", player.mono.actionTarget);
        //Debug.LogFormat("<color=red>��ư: {0}</color>",director.btn_Interaction);

        //��ȣ�ۿ� ��ư Ȱ��ȭ, ��Ȱ��ȭ
        if (this.player.mono.isTargeting && this.player.mono.actionTarget != null && this.isClicked == false)
        {
            this.director.btn_Interaction.interactable = true;
            this.director.icon_Interaction.gameObject.SetActive(true);
            var atlas = AtlasManager.instance.GetAtlasByName("Interaction");
            switch(this.player.mono.location)
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
        }
        else
        {
            this.director.btn_Interaction.interactable = false;
            this.director.icon_Interaction.gameObject.SetActive(false);
        }

        //Ʃ�丮�� ȭ��ǥ
        this.ArrowGoUpdate();

        //������Ʈ Ŭ��
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            //UIŬ���� ������Ʈ �ȴ��� UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(0) - ���� �� Ȯ�� �ʿ� [WINDOW = -1, Android = 0]
#if UNITY_EDITOR
            if (hit && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
#elif UNITY_ANDROID // or iOS 
            if (hit && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(0))
#endif
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "NPC")
                {
                    var npc = hitInfo.transform.gameObject.GetComponent<NPC>();
                    //Debug.Log("NPC Ŭ��");
                    //�̼� Ŭ����?
                    if (npc.isDone && !npc.isClear)
                    {
                        //��� ����
                        SoundManager.Pause();

                        //����ǥ ��Ȱ��ȭ
                        Debug.Log("npc Ŭ��, ����ǥ ��Ȱ��ȭ");
                        this.npc.img.gameObject.SetActive(false);

                        //Debug.Log("npc revive");
                        this.targetGo = null;
                        //�̼� ��� ���� ����
                        this.SubtractIngredientAmountN(npc);
                        StartCoroutine(WaitForNPCReviveVideo(8f));

                        //npc �÷��� ����
                        //NPC ã��
                        GameObject NPC = GameObject.Find("NPC");

                        // �ڽ� ������Ʈ���� Renderer ������Ʈ ��������
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
                        //Debug.Log("�̼� Ŭ���� O");
                    }
                    else
                    {
                        //Debug.Log("�̼� Ŭ���� X"); 
                    }
                }
                else if (hitInfo.transform.gameObject.tag == "Building")
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
                        this.targetGo = null;

                        //�̼� ��� ���� ����
                        SubtractIngredientAmountB(buildingGo);

                        //õ�� Ȱ��ȭ, �ð� �帧
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

        //�������� Ŭ���� ����
        if (this.npc.isClear && this.building.isClear && !this.isClearStage)
        {

            List<MissionData> stageMissionDatas = DataManager.instance.GetMissionDatas().FindAll(x => x.stage_id == this.stageID);
            var Info0 = InfoManager.instance.MissionInfos.Find(x => x.id == stageMissionDatas[0].id);
            var Info1 = InfoManager.instance.MissionInfos.Find(x => x.id == stageMissionDatas[1].id);
            Debug.LogFormat("info0 clear: {0}", Info0.isClear);
            Debug.LogFormat("info1 clear: {0}", Info1.isClear);
            if (Info0.isClear && Info1.isClear)
            {
                this.isClearStage = true;
                this.GetRewardChest();
                StartCoroutine(WaitForStageClearVideo(8f));
            }
            var stageInfo = InfoManager.instance.StageInfos.Find(x => x.stage == 1);
            if (stageInfo == null)
            {
                this.SaveClearInfos();
            }
        }
    }
    private void SaveClearInfos()
    {
        InfoManager.instance.AddStageInfo(1);
        InfoManager.instance.SaveStageInfos();
        var reward = DataManager.instance.GetStageData(this.stageID).reward_chest_id;
        InfoManager.instance.ChestInfos[reward - 100].amount++;
        Debug.LogFormat("<color=yellow>{0}���� ��{1}��</color>", reward, InfoManager.instance.ChestInfos[reward - 100].amount);
        InfoManager.instance.SaveChestInfos();
    }
    public void ArrowGoUpdate()
    {
        if (targetGo != null)
        {
            //Ʃ�丮�� ���̵� ȭ��ǥ
            Vector3 arrow = new Vector3(this.player.mono.transform.position.x, -1f, this.player.mono.transform.position.z) - new Vector3(this.targetGo.transform.position.x, 0, this.targetGo.transform.position.z);
            float distance = Vector3.Distance(new Vector3(this.player.mono.transform.position.x, 0, this.player.mono.transform.position.z), new Vector3(this.targetGo.transform.position.x, 0, this.targetGo.transform.position.z));
            //Ÿ�ٰ� �Ÿ��� ������ ��Ȱ��ȭ
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

    private void SubtractIngredientAmountN(NPC npc)
    {
        var data = DataManager.instance.GetMissionDatas().Find(x => x.resurrection_id == npc.id);
        var infoFound = InfoManager.instance.IngredientInfos.Find(x => x.id == data.request_ingredient_id0);
        int difference = infoFound.amount - data.goal0;
        var fishInfo = InfoManager.instance.IngredientInfos.Find(x => x.id == 2000);

        fishInfo.amount = difference;
        Debug.Log("<color=magenta>�̼ǿ��� ����</color>");
        InfoManager.instance.SaveIngredientInfos();
        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.REFRESH_UI_INVENTORY);
        this.GetReward(data.id);
    }
    private void SubtractIngredientAmountB(Building building)
    {
        var data = DataManager.instance.GetMissionDatas().Find(x => x.resurrection_id == building.id);
        var infoFound = InfoManager.instance.IngredientInfos.Find(x => x.id == data.request_ingredient_id0);
        int difference = infoFound.amount - data.goal0;

        var fishInfo = InfoManager.instance.IngredientInfos.Find(x => x.id == 2000);
        fishInfo.amount = difference;
        Debug.Log("<color=magenta>�̼ǿ��� ����</color>");
        InfoManager.instance.SaveIngredientInfos();
        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.REFRESH_UI_INVENTORY);
        this.GetReward(data.id);
    }

    public void GetReward(int id)
    {
        var data = DataManager.instance.GetMissionData(id);

        Debug.LogFormat("�̼� ���� �� {0}�� ����", data.reward_dream_count);

        //var info = InfoManager.instance.MissionInfos.Find(x => x.id == data.id);

        var info = InfoManager.instance.GetMissionInfo(id);

        info.isClear = true;
        InfoManager.instance.SaveMissionInfos();
        this.director.missionMini.Init();
        this.director.missionDetail.Init(stageID);

        GetDream(data.reward_dream_count);
    }
    private void GetDream(int amount)
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


    //�÷��̾� ��Ȱ ���� �ð�
    private IEnumerator WaitForNPCReviveVideo(float time)
    {
        this.player.mono.canMove = false;

        //���� ����
        this.videoPlayerNPC.SetDirectAudioVolume(0, this.director.setting.musicVolume);

        //NPC ��Ȱ ���� ���
        this.npcReviveVideo.SetActive(true);

        yield return new WaitForSeconds(time);
        this.npcReviveVideo.gameObject.SetActive(false);
        npc.isClear = true;
        //��� ���
        SoundManager.UnPause();

        this.director.DialogueInit();
        this.player.mono.canMove = true;

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
        this.videoPlayerStage.SetDirectAudioVolume(0, this.director.setting.musicVolume);
        //�������� ��Ȱ ���� ���
        Debug.Log("<color=red>���ο��� Ŭ���� ���� ���</color>");
        this.stageReviveVideo.SetActive(true);



        yield return new WaitForSeconds(time);
        //���� ��Ȱ��ȭ �ʿ�?
        //����Ʈ ���μ��� ���̾� ����
        this.post.PostRevive();
        this.changeTutorialStage.clearTutorial(this.stageID - 8000);
        this.director.uiClear.SetActive(true);

        this.stageReviveVideo.SetActive(false);

        //��� ���
        SoundManager.UnPause();

        this.director.dialogue.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        InfoManager.instance.DialogueInfo.id = 10009;
        InfoManager.instance.SaveDialogueInfo();
        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.END_REVIVE_VIDEO);
        //this.director.missionDetail.clickCount = 0;
        //yield return new WaitForSeconds(10f);

        
        //�� ��ȯ �̺�Ʈ
    }
    //���� ����
    private void GetRewardChest()
    {
        //���� �׽�Ʈ
        var reward = DataManager.instance.GetStageData(this.stageID).reward_chest_id;
        InfoManager.instance.ChestInfos[reward - 100].amount++;
        Debug.LogFormat("<color=yellow>{0}���� ��{1}��</color>", reward, InfoManager.instance.ChestInfos[reward - 100].amount);
        InfoManager.instance.SaveChestInfos();
        this.director.shop.uiChestShop.chestInit();
    }

}