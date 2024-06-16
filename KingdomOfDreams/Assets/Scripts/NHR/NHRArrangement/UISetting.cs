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

    ////다시 시작 테스트 버튼
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
        //    // 폴더가 존재하는지 확인합니다.
        //    if (directory.exists(sourcefolderpath0))
        //    {
        //        // 폴더 내의 모든 json 파일을 가져옵니다.
        //        string[] jsonfiles = directory.getfiles(sourcefolderpath0, "*.json");

        //        // 각 json 파일을 순회하며 변경 작업을 수행합니다.
        //        foreach (string sourcefilepath in jsonfiles)
        //        {
        //            // 파일명을 추출합니다.
        //            string filename = path.getfilename(sourcefilepath);

        //            // 변경할 파일의 경로 설정
        //            string destinationfilepath = path.combine(application.datapath, folderpath, filename);

        //            // 파일을 새로운 경로로 복사합니다.
        //            file.copy(sourcefilepath, destinationfilepath, true);
        //        }


        //        debug.log("json 파일들이 변경되었습니다.");
        //        if (appinstance != null)
        //        {
        //            destroy(appinstance);
        //        }
        //        scenemanager.loadscene("app");
        //    }
        //    else
        //    {
        //        debug.log("폴더가 존재하지 않습니다.");
        //    }
        //});
        //this.btntest07.onclick.addlistener(() =>
        //{
        //    // 폴더가 존재하는지 확인합니다.
        //    if (directory.exists(sourcefolderpath1))
        //    {
        //        // 폴더 내의 모든 json 파일을 가져옵니다.
        //        string[] jsonfiles = directory.getfiles(sourcefolderpath1, "*.json");

        //        // 각 json 파일을 순회하며 변경 작업을 수행합니다.
        //        foreach (string sourcefilepath in jsonfiles)
        //        {
        //            // 파일명을 추출합니다.
        //            string filename = path.getfilename(sourcefilepath);

        //            // 변경할 파일의 경로 설정
        //            string destinationfilepath = path.combine(application.datapath, folderpath, filename);

        //            // 파일을 새로운 경로로 복사합니다.
        //            file.copy(sourcefilepath, destinationfilepath, true);
        //        }


        //        debug.log("json 파일들이 변경되었습니다.");
        //        scenemanager.loadscene("app");
        //    }
        //    else
        //    {
        //        debug.log("폴더가 존재하지 않습니다.");
        //    }
        //});
        //this.btntest08.onclick.addlistener(() =>
        //{
        //    // 폴더가 존재하는지 확인합니다.
        //    if (directory.exists(sourcefolderpath2))
        //    {
        //        // 폴더 내의 모든 json 파일을 가져옵니다.
        //        string[] jsonfiles = directory.getfiles(sourcefolderpath2, "*.json");

        //        // 각 json 파일을 순회하며 변경 작업을 수행합니다.
        //        foreach (string sourcefilepath in jsonfiles)
        //        {
        //            // 파일명을 추출합니다.
        //            string filename = path.getfilename(sourcefilepath);

        //            // 변경할 파일의 경로 설정
        //            string destinationfilepath = path.combine(application.datapath, folderpath, filename);

        //            // 파일을 새로운 경로로 복사합니다.
        //            file.copy(sourcefilepath, destinationfilepath, true);
        //        }


        //        debug.log("json 파일들이 변경되었습니다.");
        //        scenemanager.loadscene("app");
        //    }
        //    else
        //    {
        //        debug.log("폴더가 존재하지 않습니다.");
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
        //            Debug.Log("파일 삭제 완료: " + filePath);
        //        }

        //        Debug.Log("폴더 내 모든 파일 삭제 완료.");
        //        SceneManager.LoadScene("App");

        //    }
        //    else
        //    {
        //        Debug.LogWarning("폴더를 찾을 수 없습니다: " + folderPath);
        //    }
        //});

        //클라우드 저장 
        this.btnDiscord.onClick.AddListener(() =>
        {
            Application.OpenURL("https://discord.gg/eVYC4MQDRe");
        });
        ////클라우드 저장 
        //this.btnDiscord.onClick.AddListener(() =>
        //{
        //    Debug.LogFormat("IsAuthenticate: {0}", GPGSManager.instance.IsAuthenticate());
        //    Firebase.Analytics.FirebaseAnalytics.LogEvent(GameEnums.eAnalyticsEventType.save_to_cloud.ToString());
        //    GPGSManager.instance.SaveDataToCloud("Hello World!");
        //});

        //클라우드 불러오기  
        this.btnTMI.onClick.AddListener(() =>
        {
            //this.tmiGo.SetActive(true);
        });
        ////클라우드 불러오기  
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
