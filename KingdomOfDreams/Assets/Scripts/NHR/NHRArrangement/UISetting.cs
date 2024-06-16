using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
using System.IO;

public class UISetting : MonoBehaviour
{
    public Button dim;
    public Button btnClose;
    public ButtonONOFF uiButtonOnOff;
    public Button btnDiscord;
    public Button btnTMI;
    public Slider soundSlider;
    public Slider soundOtherSlider;
    //public GameObject tmiGo;
    //public AudioManager audioManager;

    public System.Action onClickVibration;

    public float musicVolume = 1;
    public float otherVolume = 1;
    public int vibration = 1;
    public string SoundVolume = "SoundVolume";
    public string OtherVolume = "OtherVolume";
    public string VibrationOnOff = "VibrationOnOff";

    ////�ٽ� ���� �׽�Ʈ ��ư
    //public Button btnRestart;
    //public Button btnTest06;
    //public Button btnTest07;
    //public Button btnTest08;
    //private string sourceFolderPath0 = @"C:\workspace\svn-notsame\Dev\Client\KingdomOfDreams\Assets\Resources\Datas\test06";
    //private string sourceFolderPath1 = @"C:\workspace\svn-notsame\Dev\Client\KingdomOfDreams\Assets\Resources\Datas\test07";
    //private string sourceFolderPath2 = @"C:\workspace\svn-notsame\Dev\Client\KingdomOfDreams\Assets\Resources\Datas\test08";
    //private App appInstance;


    private void Awake()
    {
        //this.audioManager = Camera.main.GetComponent<AudioManager>();
        this.musicVolume = PlayerPrefs.GetFloat(SoundVolume);
        this.otherVolume = PlayerPrefs.GetFloat(OtherVolume);
        this.soundSlider.value = this.musicVolume;
        this.soundOtherSlider.value = this.otherVolume;
        SoundManager.SetVolumeMusic(this.musicVolume);
        SoundManager.SetVolumeSFX(this.otherVolume);
        this.vibration = PlayerPrefs.GetInt(VibrationOnOff);
        //this.tmiGo.SetActive(false);
        if (this.vibration == 1)
        {
            this.uiButtonOnOff.BtnOn();
            this.uiButtonOnOff.isOn = true;
        }
        else
        {
            this.uiButtonOnOff.BtnOff();
            this.uiButtonOnOff.isOn = false;
        }
    }
    void Start()
    {
        this.dim.onClick.AddListener(() =>
        {
            Debug.Log("dim clicked");
            this.gameObject.SetActive(false);
        });
        this.btnClose.onClick.AddListener(() =>
        {
            Debug.Log("btnClose clicked");
            this.gameObject.SetActive(false);
        });
        this.uiButtonOnOff.btnONOFF.onClick.AddListener(() =>
        {
            Debug.Log("btnVibration clicked");
            if (this.uiButtonOnOff.isOn)
            {
                this.uiButtonOnOff.BtnOff();
                this.uiButtonOnOff.isOn = false;
                this.vibration = -1;
                PlayerPrefs.SetInt(this.VibrationOnOff, this.vibration);
            }
            else
            {
                this.uiButtonOnOff.BtnOn();
                this.uiButtonOnOff.isOn = true;
                this.vibration = 1;
                PlayerPrefs.SetInt(this.VibrationOnOff, this.vibration);
                Handheld.Vibrate();
            }
        });

        //this.btntest06.onclick.addlistener(() =>
        //{
        //    // ������ �����ϴ��� Ȯ���մϴ�.
        //    if (directory.exists(sourcefolderpath0))
        //    {
        //        // ���� ���� ��� json ������ �����ɴϴ�.
        //        string[] jsonfiles = directory.getfiles(sourcefolderpath0, "*.json");

        //        // �� json ������ ��ȸ�ϸ� ���� �۾��� �����մϴ�.
        //        foreach (string sourcefilepath in jsonfiles)
        //        {
        //            // ���ϸ��� �����մϴ�.
        //            string filename = path.getfilename(sourcefilepath);

        //            // ������ ������ ��� ����
        //            string destinationfilepath = path.combine(application.datapath, folderpath, filename);

        //            // ������ ���ο� ��η� �����մϴ�.
        //            file.copy(sourcefilepath, destinationfilepath, true);
        //        }


        //        debug.log("json ���ϵ��� ����Ǿ����ϴ�.");
        //        if (appinstance != null)
        //        {
        //            destroy(appinstance);
        //        }
        //        scenemanager.loadscene("app");
        //    }
        //    else
        //    {
        //        debug.log("������ �������� �ʽ��ϴ�.");
        //    }
        //});
        //this.btntest07.onclick.addlistener(() =>
        //{
        //    // ������ �����ϴ��� Ȯ���մϴ�.
        //    if (directory.exists(sourcefolderpath1))
        //    {
        //        // ���� ���� ��� json ������ �����ɴϴ�.
        //        string[] jsonfiles = directory.getfiles(sourcefolderpath1, "*.json");

        //        // �� json ������ ��ȸ�ϸ� ���� �۾��� �����մϴ�.
        //        foreach (string sourcefilepath in jsonfiles)
        //        {
        //            // ���ϸ��� �����մϴ�.
        //            string filename = path.getfilename(sourcefilepath);

        //            // ������ ������ ��� ����
        //            string destinationfilepath = path.combine(application.datapath, folderpath, filename);

        //            // ������ ���ο� ��η� �����մϴ�.
        //            file.copy(sourcefilepath, destinationfilepath, true);
        //        }


        //        debug.log("json ���ϵ��� ����Ǿ����ϴ�.");
        //        scenemanager.loadscene("app");
        //    }
        //    else
        //    {
        //        debug.log("������ �������� �ʽ��ϴ�.");
        //    }
        //});
        //this.btntest08.onclick.addlistener(() =>
        //{
        //    // ������ �����ϴ��� Ȯ���մϴ�.
        //    if (directory.exists(sourcefolderpath2))
        //    {
        //        // ���� ���� ��� json ������ �����ɴϴ�.
        //        string[] jsonfiles = directory.getfiles(sourcefolderpath2, "*.json");

        //        // �� json ������ ��ȸ�ϸ� ���� �۾��� �����մϴ�.
        //        foreach (string sourcefilepath in jsonfiles)
        //        {
        //            // ���ϸ��� �����մϴ�.
        //            string filename = path.getfilename(sourcefilepath);

        //            // ������ ������ ��� ����
        //            string destinationfilepath = path.combine(application.datapath, folderpath, filename);

        //            // ������ ���ο� ��η� �����մϴ�.
        //            file.copy(sourcefilepath, destinationfilepath, true);
        //        }


        //        debug.log("json ���ϵ��� ����Ǿ����ϴ�.");
        //        scenemanager.loadscene("app");
        //    }
        //    else
        //    {
        //        debug.log("������ �������� �ʽ��ϴ�.");
        //    }
        //});

        //this.btnRestart.onClick.AddListener(() =>
        //{
        //    string folderPath = Application.persistentDataPath;

        //    if (Directory.Exists(folderPath))
        //    {
        //        string[] fileNames = Directory.GetFiles(folderPath);

        //        foreach (string filePath in fileNames)
        //        {
        //            File.Delete(filePath);
        //            Debug.Log("���� ���� �Ϸ�: " + filePath);
        //        }

        //        Debug.Log("���� �� ��� ���� ���� �Ϸ�.");
        //        SceneManager.LoadScene("App");

        //    }
        //    else
        //    {
        //        Debug.LogWarning("������ ã�� �� �����ϴ�: " + folderPath);
        //    }
        //});

        //Ŭ���� ���� 
        this.btnDiscord.onClick.AddListener(() =>
        {
            Application.OpenURL("https://discord.gg/eVYC4MQDRe");
        });
        ////Ŭ���� ���� 
        //this.btnDiscord.onClick.AddListener(() =>
        //{
        //    Debug.LogFormat("IsAuthenticate: {0}", GPGSManager.instance.IsAuthenticate());
        //    Firebase.Analytics.FirebaseAnalytics.LogEvent(GameEnums.eAnalyticsEventType.save_to_cloud.ToString());
        //    GPGSManager.instance.SaveDataToCloud("Hello World!");
        //});

        //Ŭ���� �ҷ�����  
        this.btnTMI.onClick.AddListener(() =>
        {
            //this.tmiGo.SetActive(true);
        });
        ////Ŭ���� �ҷ�����  
        //this.btnTMI.onClick.AddListener(() =>
        //{
        //    Debug.LogFormat("IsAuthenticate: {0}", GPGSManager.instance.IsAuthenticate());
        //    Firebase.Analytics.FirebaseAnalytics.LogEvent(GameEnums.eAnalyticsEventType.load_from_cloud.ToString());
        //    GPGSManager.instance.LoadDataFromCloud();
        //});

        this.soundSlider.onValueChanged.AddListener(OnSliderValueChanged);
        this.soundOtherSlider.onValueChanged.AddListener(OnSliderOTHERValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        SoundManager.SetVolumeMusic(value);
        this.musicVolume = value;
        Debug.LogFormat("volume : {0}", value);
        PlayerPrefs.SetFloat(this.SoundVolume, value);
        EventDispatcher.instance.SendEvent<float>((int)LHMEventType.eEventType.CHANGE_MUSIC_VOLUME, value);
    }
    private void OnSliderOTHERValueChanged(float value)
    {
        SoundManager.SetVolumeSFX(value);
        this.otherVolume = value;
        PlayerPrefs.SetFloat(this.OtherVolume, value);
        Debug.LogFormat("volume : {0}", value);
        EventDispatcher.instance.SendEvent<float>((int)LHMEventType.eEventType.CHANGE_OTHERS_VOLUME, value);
    }

}
