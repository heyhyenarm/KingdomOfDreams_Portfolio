using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DreamlandMain: MonoBehaviour
{
    [SerializeField]
    private UIDreamlandDirector dreamlandDirector;
    private UIStage06Director director;
    public GameObject playerPrefab;
    public CinemachineVirtualCamera followCam;

    public int stageID;

    private Player player;
    public GameObject moveEffect;
    //public Dreamland dreamland;
    public App.eSceneType prevScene;

    public GameObject[] dreamPiecesPrefab;
    private GameObject dreamPiece;
    public GameObject magicPiecePrefab;
    private GameObject magicPiece;

    public Transform[] pieceTranses;
    private int totalCount = 40;

    public Text txtDreamladnMessage;

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.END_DREAMLAND_TIME, EndDreamlandTimeEventHandler);
    }

    private void Awake()
    {
        this.director = GameObject.FindObjectOfType<UIStage06Director>();

        var dreamland_SC = SoundManager.GetSoundConnectionForThisLevel("Dreamland");
        SoundManager.PlayConnection(dreamland_SC);
    }
    private IEnumerator Start()
    {
        while(true)
        {
            if (this.player.mono.anim.GetInteger("State") == 1)
            {
                DreamlandParticle particle = ObjectPooling.GetObject();
                particle.transform.position = this.player.mono.transform.position + new Vector3(0f, 0.2f, -0.2f);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void Init()
    {
        this.CleanUpUI();


        this.player = new Player(this.playerPrefab);
        //Debug.LogFormat("player: {0}", this.player);
        this.player.State = new NormalState(this.player);

        //Debug.Log("player init");
        this.player.mono.Init();
        this.player.mono.transform.position = new Vector3(50, 1, 40);

        this.player.mono.joystick = this.director.joystick;
        this.followCam.Follow = this.player.mono.transform;
        this.followCam.LookAt = this.player.mono.transform;

        //조각 생성
        this.CreateDreamPieces();
        //타이머 0부터 시작
        this.dreamlandDirector.Init();

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.END_DREAMLAND_TIME, EndDreamlandTimeEventHandler);

    }

    private void EndDreamlandTimeEventHandler(short type)
    {
        StartCoroutine(this.EndTime());
    }

    private IEnumerator EndTime()
    {
        this.player.mono.canMove = false;
        this.player.mono.PlayAnimation(Enums.ePlayerState.Idle);
        yield return null;

        this.dreamlandDirector.PlusPiece();
        this.dreamlandDirector.imgResult.gameObject.SetActive(true);

        yield return YieldCache.WaitForSeconds(2f);
        this.ResetUI();
        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.EXIT_DREAMLAND);
        yield break;
    }

    public void CleanUpUI()
    {
        this.director.btnBook.gameObject.SetActive(false);
        this.director.btnDreamland.gameObject.SetActive(false);
        this.director.btnInventory.gameObject.SetActive(false);
        this.director.btn_Interaction.gameObject.SetActive(false);
        this.director.btnShop.gameObject.SetActive(false);
        this.director.shop.transform.GetChild(0).gameObject.SetActive(false);
        this.director.btnMap.gameObject.SetActive(false);
        this.director.dream.gameObject.SetActive(false);

        this.director.ticket.transform.localPosition = new Vector3(-800, 455, 0);
    }
    private Vector3 defaultUITicketPos = new Vector3(-164, 456, 0);

    public void ResetUI()
    {
        this.director.btnBook.gameObject.SetActive(true);
        this.director.btnDreamland.gameObject.SetActive(true);
        this.director.btnInventory.gameObject.SetActive(true);
        this.director.btn_Interaction.gameObject.SetActive(true);
        this.director.btnShop.gameObject.SetActive(true);
        this.director.shop.transform.GetChild(0).gameObject.SetActive(true);
        this.director.btnMap.gameObject.SetActive(true);
        this.director.dream.gameObject.SetActive(true);

        this.director.transform.GetChild(0).GetChild(8).localPosition = this.defaultUITicketPos;
    }

    private void CreateDreamPieces()
    {
        List<int> posIndexes = new List<int>();
        while (posIndexes.Count < totalCount)
        {
            //Debug.LogFormat("pieceTrans length:{0}", this.pieceTranses.Length);
            var num = Random.Range(0, this.pieceTranses.Length);

            if (!posIndexes.Contains(num))
                posIndexes.Add(num);
        }

        var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;

        for (int i = 0; i < totalCount; i++)
        {
            if (myLevel >= 1)
            {
                GameObject pieceGo = null;

                int selectPiece = Random.Range(1, this.dreamPiecesPrefab.Length + 1);
                if (selectPiece == 1)
                {
                    pieceGo = dreamPiecesPrefab[0];
                }
                if (selectPiece == 2)
                {
                    pieceGo = dreamPiecesPrefab[1];
                }
                if (selectPiece == 3)
                {
                    pieceGo = dreamPiecesPrefab[2];
                }
                if (selectPiece == 4)
                {
                    pieceGo = dreamPiecesPrefab[3];
                }
                Instantiate(pieceGo, this.pieceTranses[posIndexes[i]]);

            }
            else
            {
                Instantiate(this.magicPiecePrefab, this.pieceTranses[posIndexes[i]]);
            }
        }
    }
}
