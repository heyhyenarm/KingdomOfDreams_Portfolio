using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITitleDirector : MonoBehaviour
{
    public Button btn;
    public Action onClick;

    //Setting
    public Button btnSetting;
    public UISetting setting;

    public TMP_Text txtVersion;

    public Button btnTest;
    public Button btnReal;
    public Action onTest;
    public Action onReal;

    void Start()
    {
        this.txtVersion.text = string.Format("version {0}",Application.version);

        this.btn.onClick.AddListener(() =>
        {
            this.onClick();
        });
        this.btnSetting.onClick.AddListener(() =>
        {
            Debug.Log("setting clicked");
            this.setting.gameObject.SetActive(true);
        });

        //this.btnTest.onClick.AddListener(() => {
        //    this.onTest();
        //    this.onClick();
        //});
        //this.btnReal.onClick.AddListener(() => {
        //    this.onReal();
        //    this.onClick();
        //});

    }

    private void OnApplicationQuit()
    {
        Debug.Log("<color=green>설정 저장시작</color>");
        // 게임 종료 시 현재 사운드 크기, 진동 저장
        float soundVolume = this.setting.musicVolume;
        float otherVolume = this.setting.otherVolume;

        // 1 : on, -1 : off
        int vibtationOnOff = this.setting.vibration;

        PlayerPrefs.SetFloat(this.setting.SoundVolume, soundVolume);
        PlayerPrefs.SetFloat(this.setting.OtherVolume, otherVolume);
        PlayerPrefs.SetInt(this.setting.VibrationOnOff, vibtationOnOff);
        PlayerPrefs.Save();

        Debug.Log("종료 사운드 크기: " + soundVolume.ToString());
        Debug.Log("종료 효과음 크기: " + otherVolume.ToString());
        Debug.Log("종료 진동 OnOff: " + vibtationOnOff.ToString());
    }

}
