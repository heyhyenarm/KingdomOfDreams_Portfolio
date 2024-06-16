using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondMain : MonoBehaviour
{
    [SerializeField]
    private UIStage06Director director;
    public GameObject playerPrefab;
    public CinemachineVirtualCamera followCam;

    public Transform[] spawnPoints;
    public GameObject fishPrefab;
    private float delta;
    private int count;

    private bool isClicked;

    //꿈 조각
    public GameObject[] dreamPiecesPrefab;
    private GameObject dreamPiece;
    public GameObject magicPiecePrefab;
    private GameObject magicPiece;

    private int stageNum;

    private Player player;

    //public GameObject stageReviveVideo;

    //광고 컨트롤러
    //public GoogleAdMobController adController;

    private void Awake()
    {
        var go = GameObject.Find("UIStage06Director");
        go.SetActive(true);
        this.director = go.GetComponent<UIStage06Director>();
        //this.director.shop.uiChestShop.adController = this.adController;
    }

    public void Start()
    {
        var pond_SC = SoundManager.GetSoundConnectionForThisLevel("Pond");
        SoundManager.PlayConnection(pond_SC);

        this.Init();
    }
    public void Init()
    {
        //건물 데이터 연동, 기존일 때 미션 겹쳐나옴, ...
        //this.stageID = 8005;
        //Debug.LogFormat("stage ID: {0}", this.stageID);
        //this.missionMain.Init(this.stageID);

        //플레이어, UI 생성
        this.player = new Player(this.playerPrefab);
        Debug.LogFormat("player: {0}", this.player);
        this.player.State = new NormalState(this.player);

        Debug.Log("player init");
        this.player.mono.Init();
        //this.director.Init(stageID);

        this.player.mono.joystick = this.director.joystick;
        this.followCam.Follow = this.player.mono.transform;
        this.followCam.LookAt = this.player.mono.transform;

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
                case Enums.ePlayerLocation.Pond:
                    this.player.State = new FishingState(this.player);
                    break;
            }
            Debug.LogFormat("IState:{0}", this.player.State);
            this.player.DoAction();

            Invoke("IsClicked", 5.7f);
        });
        //미션 클리어 이벤트
        EventManager.instance.onAchieved = (data) =>
        {
            //낚시터에서는 구현 안함
        };
    }
    //private void LoadAd()
    //{
    //    this.adController.RequestAndLoadRewardedInterstitialAd();
    //}
    public void IsClicked()
    {
        this.count--;
        this.isClicked = false;

        //꿈 조각 확률 생성
        GenerateDreamPieces();
    }
    private void Update()
    {
        //상호작용 버튼 활성화, 비활성화
        if (this.player.mono.isTargeting && this.player.mono.actionTarget != null && this.isClicked == false)
        {
            this.director.btn_Interaction.interactable = true;
            var atlas = AtlasManager.instance.GetAtlasByName("Interaction");
            var sprite = atlas.GetSprite("fishing");
            this.director.icon_Interaction.sprite = sprite;
            this.director.icon_Interaction.gameObject.SetActive(true);
        }
        else
        {
            this.director.btn_Interaction.interactable = false;
            this.director.icon_Interaction.gameObject.SetActive(false);
        }

        if (this.count <= 80)
        {
            SpawnFish();
        }
    }
    public void SpawnFish()
    {
        this.delta += Time.deltaTime;
        if (this.delta > 2f)
        {
            var fishGo = Instantiate(this.fishPrefab);
            fishGo.transform.position = this.spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            this.count++;
            this.delta = 0;
        }
    }
    //꿈 조각 드랍
    private void GenerateDreamPieces()
    {
        float randomValue = Random.value;

        var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;

        if (myLevel >= 1) //마법도구가 있을때
        {
            if (randomValue <= 0.1f) // 10%의 확률로 실행
            {
                dreamPiece = Instantiate(dreamPiecesPrefab[Random.Range(0, dreamPiecesPrefab.Length)], transform.position, transform.rotation);
                dreamPiece.transform.position = this.player.mono.transform.position + new Vector3(0, 3f, 0);
            }
        }
        else if (myLevel == 0)  //마법도구가 없을때
        {
            if (randomValue <= 0.1f) // 10% 확률로 실행
            {
                magicPiece = Instantiate(magicPiecePrefab, transform.position, transform.rotation);
                magicPiece.transform.position = this.player.mono.transform.position + new Vector3(0, 3f, 0); //나무 위치에 마법의 꿈 조각 생성
            }
        }
    }
}
