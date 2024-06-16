using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamMineMain : MonoBehaviour
{
    public GameObject prefab; // ������ ������
    //public float spawnRadius = 10f; // �������� ������ �ݰ�

    private List<Vector3> positions = new List<Vector3>(); // ������ ��ġ�� ������ ����Ʈ

    public UIDreamMineDirector dreamDirector;


    [SerializeField]
    private UIStage06Director director;
    public GameObject playerPrefab;
    public CinemachineVirtualCamera followCam;

    public int stageID;

    private bool isClicked;

    private Player player;

    //�Ҹ� ����
    private float musicVolume;
    private float otherVolume;
    //�÷��̾� ������ҽ�
    AudioSource audioSource;

    private void Awake()
    {
        var go = GameObject.Find("UIStage06Director");
        go.SetActive(true);
        this.director = go.GetComponent<UIStage06Director>();
    }

    void Start()
    {
        this.Init();
        dreamDirector.Init();
        SpawnPrefabs();

        var dreamMine_SC = SoundManager.GetSoundConnectionForThisLevel("DreamMine");
        SoundManager.PlayConnection(dreamMine_SC);
    }
    public void Init()
    {
        //�÷��̾�, UI ����
        this.player = new Player(this.playerPrefab);
        this.player.State = new NormalState(this.player);

        this.player.mono.Init();

        this.player.mono.transform.position = new Vector3(-20.9f, 0f, 5.92f);

        this.audioSource = this.player.mono.GetComponent<AudioSource>();
        this.audioSource.volume = PlayerPrefs.GetFloat("OtherVolume"); ;

        this.player.mono.joystick = this.director.joystick;
        this.followCam.Follow = this.player.mono.transform;
        this.followCam.LookAt = this.player.mono.transform;

        //��ȣ�ۿ� ��ư Ŭ��
        this.director.btn_Interaction.onClick.AddListener(() => {
            this.isClicked = true;
            switch (this.player.mono.location)
            {
                case Enums.ePlayerLocation.DreamMine:
                    this.player.State = new DreamMiningState(this.player);
                    break;
            }
            Debug.LogFormat("IState:{0}", this.player.State);
            this.player.DoAction();

            Invoke("IsClicked", 2f);
        });

        //�̼� Ŭ���� �̺�Ʈ
        EventManager.instance.onAchieved = (data) => {

        };

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

    }
    public void IsClicked()
    {
        this.isClicked = false;
    }

    private void SpawnPrefabs()
    {
        for (int i = 0; i < 50; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition();
            Quaternion spawnRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            GameObject newPrefab = Instantiate(prefab, spawnPosition, spawnRotation);
            positions.Add(spawnPosition);
        }
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


    private Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        bool isOverlap = true;

        // �ߺ����� �ʴ� ��ġ ã��
        while (isOverlap)
        {
            float xPos = Random.Range(-10.85f, 9.73f);
            float zPos = Random.Range(5.07f, -5.79f);
            spawnPosition = new Vector3(xPos, 0f, zPos);

            isOverlap = false;
            foreach (Vector3 pos in positions)
            {
                if (Vector3.Distance(spawnPosition, pos) < prefab.transform.localScale.x)
                {
                    isOverlap = true;
                    break;
                }
            }
        }
        return spawnPosition;
    }
}
