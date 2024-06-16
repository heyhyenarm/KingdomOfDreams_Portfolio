using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using GooglePlayGames;
using Firebase.Auth;
using System;
using Random = UnityEngine.Random;

public class App : MonoBehaviour
{
    public Inventory inventory;
    public Book book;
    public MagicTool MagicTool;


    public enum eSceneType
    {
        App, Pond, Mine, Dungeon, OpeningVideo, Tutorial01, Tutorial02, Tutorial03, Tutorial04, Tutorial05,
        Stage06, Stage07, Stage08, Title, Lobby, Loading, Stage06Start, DreamMine, FairyShop, Dreamland
    }

    private eSceneType state = eSceneType.Title;
    public eSceneType preState;

    public bool isNewbie;

    private int charaterNum;

    private int stageNum;
    private DateTime offlineTime;
    private DateTime ticketTime;
    private DateTime chestTime;
    private float soundVolume;
    private int vibration;
    private int ticketAmount;
    private bool isGive = false;
    private string ExitTimeKey = "ExitTime";
    private string SoundVolume = "SoundVolume";
    private string OtherVolume = "OtherVolume";
    private string VibrationOnOff = "VibrationOnOff";


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);       

        this.Init();

        this.inventory.Init();
        this.book.Init();
        this.MagicTool.Init();
    }
    private void Init()
    {
        //�̺�Ʈ ���
        GPGSManager.instance.onAuthenticate = (success) =>
        {
            Debug.LogFormat("onAuthenticate: {0}", success);

            PlayGamesPlatform.Instance.RequestServerSideAccess(true, (token) =>
            {

                Debug.Log("********* token *********");
                Debug.Log(token);
                Debug.Log("*************************");

                FirebaseAuth auth = FirebaseAuth.DefaultInstance;

                Credential credential = PlayGamesAuthProvider.GetCredential(token);
                auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogFormat("******** task.IsCanceled ********");
                        return;
                    }

                    if (task.IsFaulted)
                    {
                        Debug.LogFormat("******** task.IsFaulted ********");
                        return;
                    }

                    FirebaseUser newUser = task.Result;
                    if (newUser != null)
                    {
                        Debug.LogFormat("[newUser] DisplayName: {0}, UserId: {1}", newUser.DisplayName, newUser.UserId);
                    }
                    else
                    {
                        Debug.Log("newUser is null");
                    }

                    FirebaseUser currentUser = auth.CurrentUser;

                    if (currentUser != null)
                    {
                        Debug.LogFormat("[currentUser] DisplayName: {0}, UserId: {1}", currentUser.DisplayName, currentUser.UserId);
                    }
                    else
                    {
                        Debug.LogFormat("currentUser is null");
                    }
                });

            });

        };
        GPGSManager.instance.onSavedDataToCloud = (success) =>
        {
            Debug.LogFormat("onSavedDataToCloud: {0}", success);
        };
        GPGSManager.instance.onLoadDataFromCloud = (data) =>
        {
            Debug.LogFormat("onLoadDataFromCloud: {0}", data);
        };
        GPGSManager.instance.onErrorHandler = (error) =>
        {
            Debug.LogFormat(error.ToString());
        };

        //���� 
        GPGSManager.instance.Authenticate();

    }

    private void Start()
    {
        
        DataManager.instance.LoadMissionData();
        DataManager.instance.LoadDialogueData();
        DataManager.instance.LoadIngredientData();
        DataManager.instance.LoadBuildingData();
        DataManager.instance.LoadStageData();
        DataManager.instance.LoadChestData();
        DataManager.instance.LoadBookData();
        DataManager.instance.LoadBookItemData();
        DataManager.instance.LoadDreamPieceData();
        DataManager.instance.LoadMagicToolData();
        DataManager.instance.LoadMagicToolLevelData();
        DataManager.instance.LoadNpcData();
        EventManager.instance = new EventManager();

        Debug.Log(Application.persistentDataPath);

        string playerPath = InfoManager.instance.playerPath;

        if (!InfoManager.instance.IsNewbie(playerPath))
        {
            //��������
            Debug.Log("<color=yellow>��������</color>");

            //�÷��̾�
            InfoManager.instance.LoadPlayerInfo();
            //�̼�
            InfoManager.instance.LoadMissionInfo();
            //�κ��丮
            InfoManager.instance.LoadIngredientInfos();
            //��������
            InfoManager.instance.LoadStageInfo();
            //����
            InfoManager.instance.LoadChestInfo();
            //����
            InfoManager.instance.LoadBookInfo();
            //���� ������
            InfoManager.instance.LoadBookItemInfo();
            //���� ����
            InfoManager.instance.LoadDreamPieceInfo();
            //���� ����
            InfoManager.instance.LoadMagicToolInfo();
            //�ǹ�
            InfoManager.instance.LoadBuildingInfos();
            //��
            InfoManager.instance.LoadDreamInfo();
            //���̾�α�
            InfoManager.instance.LoadDialogueInfo();
            //���� �ð�
            InfoManager.instance.LoadChargeTimeInfo();
            //Ƽ��
            InfoManager.instance.LoadTicketInfo();

            this.isNewbie = false;
        }
        else
        {
            //�ű�����
            Debug.Log("<color=yellow>�ű�����</color>");

            InfoManager.instance.PlayerInfo = new PlayerInfo(0);
            InfoManager.instance.PlayerInfo.nowMatNum = 0;
            InfoManager.instance.SavePlayerInfo();
            //====================================================================================
            //�κ��丮 ����
            InfoManager.instance.IngredientInfoInit();

            InfoManager.instance.StageInfoInit();
            InfoManager.instance.ChestInfoInit();

            InfoManager.instance.MissionInfoInit();
            InfoManager.instance.BuildingInfoInit();

            //���� ����
            var bookInfo = new BookInfo(5000);
            InfoManager.instance.BookInfos = new List<BookInfo>();
            InfoManager.instance.BookInfos.Add(bookInfo);
            InfoManager.instance.SaveBookInfo();

            //���� ������ ����
            var bookItemInfo = new BookItemInfo(100);
            InfoManager.instance.BookItemInfos = new List<BookItemInfo>();
            InfoManager.instance.BookItemInfos.Add(bookItemInfo);
            InfoManager.instance.SaveBookItemInfo();

            //���� ���� ����
            var dreamPieceInfo = new DreamPieceInfo(600, 0);
            var dreamPieceInfo1 = new DreamPieceInfo(601, 0);
            var dreamPieceInfo2 = new DreamPieceInfo(602, 0);
            var dreamPieceInfo3 = new DreamPieceInfo(603, 0);

            InfoManager.instance.DreamPieceInfo = new List<DreamPieceInfo>();

            InfoManager.instance.DreamPieceInfo.Add(dreamPieceInfo);
            InfoManager.instance.DreamPieceInfo.Add(dreamPieceInfo1);
            InfoManager.instance.DreamPieceInfo.Add(dreamPieceInfo2);
            InfoManager.instance.DreamPieceInfo.Add(dreamPieceInfo3);

            InfoManager.instance.SaveDreamPieceInfo();

            //���� ���� ����
            var magicToolInfo = new MagicToolInfo(300, 0);
            InfoManager.instance.MagicToolInfo = new List<MagicToolInfo>();
            InfoManager.instance.MagicToolInfo.Add(magicToolInfo);
            InfoManager.instance.SaveMagicToolInfo();

            //�� ����
            var dreamInfo = new DreamInfo(0);
            InfoManager.instance.DreamInfo = new DreamInfo(0);
            InfoManager.instance.SaveDreamInfo();

            var dialogueInfo = new DialogueInfo(10000);
            InfoManager.instance.DialogueInfo = dialogueInfo;
            InfoManager.instance.SaveDialogueInfo();

            //Ƽ�� ����
            InfoManager.instance.TicketInfoInit();
            //�ð� ����
            InfoManager.instance.ChargeTimeInfo = new FreeChargeTimeInfo(System.DateTime.Now, true);
            InfoManager.instance.SaveChargeTimeInfo();

            PlayerPrefs.SetFloat(this.OtherVolume, 1);
            PlayerPrefs.SetFloat(this.SoundVolume, 1);

            this.isNewbie = true;
        }

        this.ChangeScene(this.state);

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.GO_DREAMLAND, new EventHandler((type) => {
            Debug.LogFormat("<color=magenta>go dreamland event, app.state:{0}</color>",this.state);
            this.preState = this.state;
            this.state = eSceneType.Dreamland;
            this.ChangeScene(this.state);       
        }));

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.EXIT_DREAMLAND, new EventHandler((type) => {
            Debug.Log("exit dreamland");
            this.state = this.preState;
            this.ChangeScene(this.state);
        }));


        //��Ż �� ��ȯ
        EventDispatcher.instance.AddEventHandler<Enums.ePortalType>((int)LHMEventType.eEventType.ENTER_PORTAL, new EventHandler<Enums.ePortalType>((type, portalType) =>
        {
            Debug.Log("<color=yellow>��Ż �� ��ȯ �̺�Ʈ ȣ��</color>");
            if (portalType != Enums.ePortalType.Stage)
            {
                //������, ����, ����
                this.preState = this.state;
                this.state = (eSceneType)portalType;

                if(this.state == eSceneType.Dungeon)
                {

                }
                else if(this.state == eSceneType.Pond)
                {

                }
                else if(this.state == eSceneType.Mine)
                {

                    float randomValue = Random.value;

                    if (randomValue <= 0.1f) // 10% Ȯ���� �ޱ������� ����
                    {
                        this.state = eSceneType.DreamMine;
                    }
                }

                Debug.LogFormat("portalType: {0}", portalType);
                Debug.LogFormat("eSceneType: {0}", this.state);
            }
            else
            {
                //�� Ŭ����?
                foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
                {
                    //Ŭ���� ���� �� ����(��Ż ź ��)
                    if (stageInfo.isClear == false)
                    {
                        this.state = (eSceneType)(5 + stageInfo.stage);
                        //LoadingSceneController.Instance.LoadSceneInfo(5 + stageInfo.stage);
                        break;
                    }
                }
                Debug.LogFormat("portalType: {0}", portalType);
                Debug.LogFormat("eSceneType: {0}", this.state);
            }

            this.ChangeScene(this.state);
        }));

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.EXIT_DUNGEON, new EventHandler((type) =>
        {
            Debug.Log("exit dungeon");
            this.state = this.preState;
            this.ChangeScene(this.state);
        }));

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.ENTER_FAIRYSHOP, new EventHandler((type) =>
        {
            Debug.Log("go to fairyShop");
            this.preState = this.state;
            this.ChangeScene(eSceneType.FairyShop);
        }));
        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.EXIT_FAIRYSHOP, new EventHandler((type) =>
        {
            Debug.Log("exit fairyShop");
            this.state = this.preState;
            this.ChangeScene(this.state);
        }));

        EventManager.instance.changeScene = (sceneNum) =>
        {
            this.ChangeScene((eSceneType)(sceneNum + 4));
        };

        // ���� ���� �� ������ ������ ���� �ð��� �ҷ��ɴϴ�.
        if (PlayerPrefs.HasKey(ExitTimeKey))
        {
            string savedTime = PlayerPrefs.GetString(ExitTimeKey);
            DateTime exitTime = DateTime.FromBinary(Convert.ToInt64(savedTime));
            this.offlineTime = exitTime;

            Debug.Log("<color=yellow>���� ���� �ð�: </color>" + offlineTime.ToString());
        }
        // ���� ���� �� ������ ������ setting �� �ҷ�����
        if (PlayerPrefs.HasKey(SoundVolume))
        {
            float soundVolume = PlayerPrefs.GetFloat(SoundVolume);
            this.soundVolume = soundVolume;
            SoundManager.SetVolumeMusic(soundVolume);

            Debug.Log("<color=yellow>���� ���� ũ��: </color>" + soundVolume.ToString());
        }
        if (PlayerPrefs.HasKey(OtherVolume))
        {
            float otherVolume = PlayerPrefs.GetFloat(OtherVolume);
            SoundManager.SetVolumeSFX(otherVolume);

            Debug.Log("<color=yellow>���� ȿ���� ũ��: </color>" + otherVolume.ToString());
        }
        if (PlayerPrefs.HasKey(VibrationOnOff))
        {
            int vibration = PlayerPrefs.GetInt(VibrationOnOff);
            this.vibration = vibration;

            Debug.Log("<color=yellow>���� ���� onoff: </color>" + this.vibration.ToString());
        }


    }
    //�ε� �� ����
    public void ChangeScene(eSceneType sceneType)
    {
        if(sceneType == eSceneType.Title)
        {
            var titleOper = SceneManager.LoadSceneAsync("Title");
            titleOper.completed += (obj) =>
            {
                //���ε尡 �Ϸ�� (�޸𸮿� �� �ö�)
                //�ν��Ͻ��� ���� ���� 
                var titleMain = GameObject.FindObjectOfType<TitleMain>();
                titleMain.Init();

                //titleMain.uiDirector.onTest = () =>
                //{
                //    DataManager.instance.missionDataPath=DataManager.instance.testMisisonDataPath;
                //    DataManager.instance.LoadMissionData();
                //};
                //titleMain.uiDirector.onReal = () =>
                //{
                //    DataManager.instance.missionDataPath = DataManager.instance.realMissionDataPath;
                //    DataManager.instance.LoadMissionData();
                //};

                titleMain.uiDirector.onClick = () =>
                {
                    if (this.isNewbie)
                    {
                        this.state = eSceneType.Lobby;
                        this.ChangeScene(this.state);
                    }
                    else
                    {
                        //eSceneType ���� (������ Ŭ������ ��)
                        //if (InfoManager.instance.StageInfos[7].isClear) this.state = (eSceneType)12;
                        foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
                        {
                            Debug.LogFormat("<color>stageInfo.stage : {0}, stageInfo.isClear : {1}</color>", stageInfo.stage, stageInfo.isClear);

                            if (stageInfo.isClear == false)
                            {
                                Debug.LogFormat("stageInfo.stage : {0}", stageInfo.stage);
                                this.state = (eSceneType)(5 + stageInfo.stage);
                                break;
                            }
                        }
                        Debug.LogFormat("<color>this.state : {0}</color>", this.state);

                        if (this.state == eSceneType.Stage07 || this.state == eSceneType.Stage08) //7,8�������� ������ ���
                        {
                            Debug.Log("<color=yellow>7,8 �������� ����</color>");
                            this.ChangeScene(eSceneType.Stage06);
                            //AutoSceneChange();
                        }
                        else
                        {
                            Debug.Log("<color=yellow>1~6�������� ����</color>");
                            this.ChangeScene(this.state);
                        }
                    }
                };
            };
            return;
        }
        var oper = SceneManager.LoadSceneAsync("LoadingTest");

        oper.completed += (obj) =>
        {
            var loadingTestMain = GameObject.FindObjectOfType<LoadingTestMain>();
            var oper = loadingTestMain.LoadSceneAdditive(sceneType.ToString());

            //loadingTestMain.StartCoroutine(LoadSceneProcess());

            switch (sceneType)
            {
                case eSceneType.Lobby:
                    oper.completed += (obj) =>
                    {
                        //���ε尡 �Ϸ�� (�޸𸮿� �� �ö�)
                        SceneManager.UnloadSceneAsync("LoadingTest");
                        //�ν��Ͻ��� ���� ���� 
                        var lobbyMain = GameObject.FindObjectOfType<LobbyMain>();
                        EventManager.instance.onTouched = () =>
                        {
                            this.state = eSceneType.OpeningVideo;
                            this.ChangeScene(this.state);
                        };
                    };
                    break;

                case eSceneType.OpeningVideo:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        EventManager.instance.playOpening();

                        //������ ������ �Ѿ
                        EventManager.instance.EndOpening = () =>
                        {
                            this.state = eSceneType.Tutorial01;
                            this.ChangeScene(this.state);
                        };

                        ////��ŵ ��ư ������ �Ѿ
                        //EventManager.instance.onClickedSkip = () =>
                        //{
                        //    this.state = eSceneType.Tutorial01;
                        //    this.ChangeScene(this.state);
                        //};
                    };
                    break;

                case eSceneType.Tutorial01:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var tutorial01Main = GameObject.FindObjectOfType<Tutorial01Main>();
                        Debug.Log("Tutorial01Main Init");
                        tutorial01Main.Init();

                        EventManager.instance.onTutorial01Clear = () =>
                        {
                            Debug.LogFormat("Ʃ�丮�� 01 Ŭ����");
                            Debug.LogFormat("InfoManager.instance.StageInfos : {0}", InfoManager.instance.StageInfos);
                            Debug.LogFormat("InfoManager.instance.StageInfos[0] : {0}, InfoManager.instance.StageInfos[0].isClear : ", InfoManager.instance.StageInfos[0]);//, InfoManager.instance.StageInfos[0].isClear);
                            InfoManager.instance.StageInfos[0].isClear = true;
                            InfoManager.instance.SaveStageInfos();

                            this.state = eSceneType.Tutorial02;
                            this.ChangeScene(this.state);
                        };

                    };
                    break;
                case eSceneType.Tutorial02:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var tutorial02Main = GameObject.FindObjectOfType<Tutorial02Main>();
                        tutorial02Main.Init();

                        EventManager.instance.onTutorial02Clear = () =>
                        {
                            Debug.LogFormat("Ʃ�丮�� 02 Ŭ����");
                            Debug.LogFormat("InfoManager.instance.StageInfos : {0}", InfoManager.instance.StageInfos);
                            Debug.LogFormat("InfoManager.instance.StageInfos[0] : {0}, InfoManager.instance.StageInfos[0].isClear : ", InfoManager.instance.StageInfos[1]);//, InfoManager.instance.StageInfos[0].isClear);
                            InfoManager.instance.StageInfos[1].isClear = true;
                            InfoManager.instance.SaveStageInfos();

                            this.state = eSceneType.Tutorial03;
                            this.ChangeScene(this.state);
                        };
                    };
                    break;

                case eSceneType.Tutorial03:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var tutorial03Main = GameObject.FindObjectOfType<Tutorial03Main>();
                        Debug.Log("Tutorial03Main Init");
                        tutorial03Main.Init();

                        EventManager.instance.onTutorial03Clear = () =>
                        {
                            Debug.LogFormat("Ʃ�丮�� 03 Ŭ����");
                            Debug.LogFormat("InfoManager.instance.StageInfos : {0}", InfoManager.instance.StageInfos);
                            Debug.LogFormat("InfoManager.instance.StageInfos[0] : {0}, InfoManager.instance.StageInfos[0].isClear : ", InfoManager.instance.StageInfos[2]);//, InfoManager.instance.StageInfos[0].isClear);
                            InfoManager.instance.StageInfos[2].isClear = true;
                            InfoManager.instance.SaveStageInfos();

                            this.state = eSceneType.Tutorial04;
                            this.ChangeScene(this.state);
                        };
                    };
                    break;

                case eSceneType.Tutorial04:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var tutorial04Main = GameObject.FindObjectOfType<Tutorial04Main>();
                        tutorial04Main.Init();

                        EventManager.instance.onTutorial04Clear = () =>
                        {
                            Debug.LogFormat("Ʃ�丮�� 04 Ŭ����");
                            //Debug.LogFormat("InfoManager.instance.StageInfos : {0}", InfoManager.instance.StageInfos);
                            //Debug.LogFormat("InfoManager.instance.StageInfos[0] : {0}, InfoManager.instance.StageInfos[0].isClear : ", InfoManager.instance.StageInfos[0]);//, InfoManager.instance.StageInfos[0].isClear);
                            InfoManager.instance.StageInfos[3].isClear = true;
                            InfoManager.instance.SaveStageInfos();

                            this.state = eSceneType.Tutorial05;
                            this.ChangeScene(this.state);
                        };
                    };
                    break;

                case eSceneType.Tutorial05:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var tutorial05Main = GameObject.FindObjectOfType<Tutorial05Main>();
                        Debug.Log("Tutorial05Main Init");
                        tutorial05Main.Init();

                        EventManager.instance.onTutorial05Clear = () =>
                        {
                            this.isGive = true; //�������� �ڵ����� �ȹް� üũ

                            Debug.LogFormat("Ʃ�丮�� 05 Ŭ����");
                            Debug.LogFormat("InfoManager.instance.StageInfos : {0}", InfoManager.instance.StageInfos);
                            Debug.LogFormat("InfoManager.instance.StageInfos[0] : {0}, InfoManager.instance.StageInfos[0].isClear : ", InfoManager.instance.StageInfos[4]);//, InfoManager.instance.StageInfos[0].isClear);
                            InfoManager.instance.StageInfos[4].isClear = true;
                            InfoManager.instance.SaveStageInfos();

                            this.state = eSceneType.Stage06Start;
                            this.ChangeScene(this.state);
                        };
                    };
                    break;

                case eSceneType.Stage06Start:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        //4��
                        EventManager.instance.onStage06StartEnd = () =>
                        {
                            this.isGive = true; //�������� �ڵ����� �ȹް� üũ

                            this.state = eSceneType.Stage06;
                            this.ChangeScene(this.state);
                        };
                    };
                    break;
                case eSceneType.Stage06:
                    oper.completed += (obj) =>
                    {
                        var stage06Main = GameObject.FindObjectOfType<Stage06Main>();
                        if(this.state == eSceneType.Stage06)
                        {
                            SceneManager.UnloadSceneAsync("LoadingTest");
                            stage06Main.realStageID = 8005;
                        }
                        else if (this.state == eSceneType.Stage07) //7�������� �����϶�
                        {
                            stage06Main.realStageID = 8006;
                            ChangeScene(this.state);
                        }
                        else if (this.state == eSceneType.Stage08) //8�������� �����϶�
                        {
                            stage06Main.realStageID = 8007;
                            ChangeScene(this.state);
                        }

                        EventManager.instance.onStage06Clear = () =>
                        {
                            Debug.LogFormat("�������� 06 Ŭ����");
                            Debug.LogFormat("InfoManager.instance.StageInfos : {0}", InfoManager.instance.StageInfos);
                            Debug.LogFormat("InfoManager.instance.StageInfos[0] : {0}, InfoManager.instance.StageInfos[0].isClear : ", InfoManager.instance.StageInfos[4]);//, InfoManager.instance.StageInfos[0].isClear);
                            InfoManager.instance.StageInfos[5].isClear = true;
                            InfoManager.instance.SaveStageInfos();

                            this.state = eSceneType.Stage07;
                            this.ChangeScene(this.state);
                        };

                        if (this.isGive == false)
                        {
                            this.isGive = true;
                            //this.OfflineGift();
                            Invoke("OfflineGift", 1f);
                        }
                    };
                    break;

                case eSceneType.Stage07:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var stage07Main = GameObject.FindObjectOfType<Stage07Main>();
                        stage07Main.realStageID = 8006;

                        if (this.state >= (eSceneType)(12)) //8�������� �����϶�
                        {
                            stage07Main.realStageID = 8007;
                        }

                        EventManager.instance.onStage07Clear = () =>
                        {
                            Debug.LogFormat("�������� 07 Ŭ����");
                            InfoManager.instance.StageInfos[6].isClear = true;
                            InfoManager.instance.SaveStageInfos();

                            this.state = eSceneType.Stage08;
                            this.ChangeScene(this.state);

                            //����ü�� �����ִ� �̺�Ʈ ȣ��
                            //EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.SHOW_PRODUCTION_CHAIN);
                        };
                    };
                    break;

                case eSceneType.Dungeon:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var dungeonMain = GameObject.FindObjectOfType<DungeonMain>();
                        dungeonMain.prevScene = this.preState;
                        dungeonMain.Init();
                    };
                    break;

                case eSceneType.FairyShop:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var fairyShopMain = GameObject.FindObjectOfType<FairyShopMain>();
                        fairyShopMain.prevScene = (Enums.eSceneType)this.preState;
                        fairyShopMain.Init();
                    };
                    break;

                case eSceneType.Stage08:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var stage08Main = GameObject.FindObjectOfType<Stage08Main>();
                        stage08Main.realStageID = 8007;

                        EventManager.instance.onStage08Clear = () =>
                        {
                            Debug.LogFormat("�������� 08 Ŭ����");
                            InfoManager.instance.StageInfos[7].isClear = true;
                            InfoManager.instance.SaveStageInfos();
                        };
                    };
                    break;

                case eSceneType.Dreamland:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var dreamlandMain = GameObject.FindObjectOfType<DreamlandMain>();
                        dreamlandMain.prevScene = this.preState;
                        dreamlandMain.Init();
                    };
                    break;

                case eSceneType.Pond:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var pondMain = GameObject.FindObjectOfType<PondMain>();
                    };
                    break;

                case eSceneType.Mine:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var mineMain = GameObject.FindObjectOfType<MineMain>();
                    };
                    break;

                case eSceneType.DreamMine:
                    oper.completed += (obj) =>
                    {
                        SceneManager.UnloadSceneAsync("LoadingTest");

                        var dreamMineMain = GameObject.FindObjectOfType<DreamMineMain>();
                    };
                    break;
            }
        };
    }
    
    public void OfflineGift()
    {
        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.GIVE_OFFLINE_INGREDIENT, this.offlineTime);
    }
    public void AutoSceneChange()
    {
        Debug.Log("<color=yellow>Auto������ ����</color>");
        //6���������� �̵�
        this.ChangeScene(eSceneType.Stage06);

        Invoke("SceneChange", 0.1f);

        //���� �������� ��ȣ ��������
        //int stageNum = 0;
        //foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
        //{
        //    Debug.LogFormat("<color>stageInfo.stage : {0}, stageInfo.isClear : {1}</color>", stageInfo.stage, stageInfo.isClear);
        //    if (stageInfo.isClear == false)
        //    {
        //        //���ϴ°��� stageInfo.num ����
        //        stageNum = stageInfo.stage;
        //        break;
        //    }
        //}
        
        //if(this.state == (eSceneType)(11)) //7�������� �����϶�
        //{
        //    Debug.Log("<color=yellow>7���������� �̵�</color>");

        //    //7���������� �̵�
        //    Invoke("ChangeScene7", 0.1f);
        //}
        //else if(this.state == (eSceneType)(12)) //8�������� �����϶�
        //{
        //    Debug.Log("<color=yellow>8���������� �̵�</color>");

        //    //8���������� �̵�
        //    Invoke("ChangeScene8", 0.1f);
        //}
    }
    public void SceneChange()
    {
        ChangeScene(this.state);
    }
    public void ChangeScene7()
    {
        this.state = eSceneType.Stage07;
        ChangeScene(this.state);
    }

    public void ChangeScene8()
    {
        this.state = eSceneType.Stage08;
        ChangeScene(this.state);
    }

}