using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonMain : MonoBehaviour
{
    [SerializeField]
    private UIStage06Director director;
    public GameObject playerPrefab;
    public CinemachineVirtualCamera followCam;

    private Player player;
    public Dungeon dungeon;
    public App.eSceneType prevScene;

    private bool isClicked;

    private Enums.eMonsterState monsterState;

    //소리 설정
    private float musicVolume;
    private float otherVolume;
    //플레이어 오디오소스
    AudioSource audioSource;

    //광고 컨트롤러
    //public GoogleAdMobController adController;

    private void Awake()
    {
        var go = GameObject.Find("UIStage06Director");
        go.SetActive(true);
        this.director = go.GetComponent<UIStage06Director>();
        //this.director.shop.uiChestShop.adController = this.adController;
    }
    private void Start()
    {
        var dungeon_SC = SoundManager.GetSoundConnectionForThisLevel("Dungeon");
        SoundManager.PlayConnection(dungeon_SC);

        //var ad = this.director.shop.uiChestShop.chestCells[0]._btnAD.GetComponent<Button>();
        //ad.onClick.AddListener(LoadAd);
    }
    public void Init()
    {
        this.player = new Player(this.playerPrefab);
        //Debug.LogFormat("player: {0}", this.player);
        this.player.State = new NormalState(this.player);

        //Debug.Log("player init");
        this.player.mono.Init();
        this.player.mono.transform.position = new Vector3(-2.56f, 5.26f, 38.54f);

        this.audioSource = this.player.mono.GetComponent<AudioSource>();
        this.audioSource.volume = PlayerPrefs.GetFloat("OtherVolume"); ;

        this.player.mono.joystick = this.director.joystick;
        this.followCam.Follow = this.player.mono.transform;
        this.followCam.LookAt = this.player.mono.transform;

        this.dungeon.Init(prevScene);

        //��ȣ�ۿ� ��ư Ŭ��
        this.director.btn_Interaction.onClick.AddListener(() => {
            this.isClicked = true;
            switch (this.player.mono.location)
            {
                case Enums.ePlayerLocation.Dungeon:
                    this.player.State = new AttackState(this.player);
                    break;
            }
            //Debug.LogFormat("IState:{0}", this.player.State);
            this.player.DoAction();
            Invoke("IsClicked", 1.1f);

        });
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
        if (this.player.mono.isTargeting && this.player.mono.actionTarget != null && !this.isClicked)
        {
            this.monsterState = this.player.mono.actionTarget.GetComponent<AttackingMonster>().state;

            if (this.monsterState != Enums.eMonsterState.Hit && this.monsterState != Enums.eMonsterState.Die
                && this.player.mono.state != Enums.ePlayerState.Attacking_Stun)
            {
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
    }
}
