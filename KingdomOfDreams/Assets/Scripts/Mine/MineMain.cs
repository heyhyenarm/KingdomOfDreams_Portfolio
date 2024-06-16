using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineMain : MonoBehaviour
{
    [SerializeField]
    private UIStage06Director director;
    public GameObject playerPrefab;
    public CinemachineVirtualCamera followCam;

    public int stageID;

    private Player player;

    private bool isClicked;

    //���� ��Ʈ�ѷ�
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
        var mine_SC = SoundManager.GetSoundConnectionForThisLevel("Mine");
        SoundManager.PlayConnection(mine_SC);

        this.Init();
    }
    public void Init()
    {
        //�÷��̾�, UI ����
        this.player = new Player(this.playerPrefab);
        this.player.State = new NormalState(this.player);

        this.player.mono.Init();

        this.player.mono.joystick = this.director.joystick;
        this.followCam.Follow = this.player.mono.transform;
        this.followCam.LookAt = this.player.mono.transform;

        //��ȣ�ۿ� ��ư Ŭ��
        this.director.btn_Interaction.onClick.AddListener(() => {
            this.isClicked = true;
            switch (this.player.mono.location)
            {
                case Enums.ePlayerLocation.Mine:
                    this.player.State = new MiningState(this.player);
                    break;
            }
            Debug.LogFormat("IState:{0}", this.player.State);
            this.player.DoAction();

            Invoke("IsClicked", 2f);
        });

        //�̼� Ŭ���� �̺�Ʈ
        EventManager.instance.onAchieved = (data) => {

        };
    }
    private void Update()
    {
        //��ȣ�ۿ� ��ư Ȱ��ȭ, ��Ȱ��ȭ
        if (this.player.mono.isTargeting && this.player.mono.actionTarget != null && this.isClicked == false)
        {
            this.director.btn_Interaction.interactable = true;
            var atlas = AtlasManager.instance.GetAtlasByName("Interaction");
            var sprite = atlas.GetSprite("Icon_ItemIcon_Pickax");
            this.director.icon_Interaction.sprite = sprite;
            this.director.icon_Interaction.gameObject.SetActive(true);
        }
        else
        {
            this.director.btn_Interaction.interactable = false;
            this.director.icon_Interaction.gameObject.SetActive(false);
        }
    }
    //private void LoadAd()
    //{
    //    this.adController.RequestAndLoadRewardedInterstitialAd();
    //}

    public void IsClicked()
    {
        this.isClicked = false;
    }
}
