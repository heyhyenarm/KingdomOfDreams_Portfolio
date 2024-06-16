using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDreamlandDirector : MonoBehaviour
{
    //페이드 아웃
    public Image imgPopup;
    public TMP_Text txtMessage;

    public float fadeSpeed = 0.5f;
    public float currentAlpha = 1f;

    //슬라이더
    public Slider slider;
    public float maxTime = 30f;
    public float currentTime;
    public GameObject timeSlider;

    //처음 조각 갯수 가
    private int cnt0;
    private int cnt1;
    private int cnt2;
    private int cnt3;


    //결과창
    public Image imgResult;
    public TMP_Text txtCnt0;
    public TMP_Text txtCnt1;
    public TMP_Text txtCnt2;
    public TMP_Text txtCnt3;


    public void Init()
    {
        currentTime = 0f;
        slider.minValue = 0f;
        slider.maxValue = maxTime;
        slider.value = currentTime;

        this.cnt0 = InfoManager.instance.DreamPieceInfo.Find(x => x.id == 600).amount;
        this.cnt1 = InfoManager.instance.DreamPieceInfo.Find(x => x.id == 601).amount;
        this.cnt2 = InfoManager.instance.DreamPieceInfo.Find(x => x.id == 602).amount;
        this.cnt3 = InfoManager.instance.DreamPieceInfo.Find(x => x.id == 603).amount;

    }

    private void Update()
    {
        //팝업창 페이드 아웃
        this.FadeOutImage();

        //30초 타이머
        this.Timer();
    }



    public void FadeOutImage()
    {
        this.currentAlpha -= fadeSpeed * Time.deltaTime;

        if (this.currentAlpha <= 0)
        {
            this.imgPopup.gameObject.SetActive(false);
        }

        //알파 값 변경
        Color newColor0 = imgPopup.color;
        newColor0.a = currentAlpha;
        imgPopup.color = newColor0;

        Color newColor1 = txtMessage.color;
        newColor1.a = currentAlpha;
        txtMessage.color = newColor1;
    }

    public void Timer()
    {
        if (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
            slider.value = currentTime;
        }
        else if (currentTime >= maxTime)
        {
            currentTime = 0f;

            EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.END_DREAMLAND_TIME);
        }
    }

    public void PlusPiece()
    {
        int cnt0 = InfoManager.instance.DreamPieceInfo.Find(x => x.id == 600).amount;
        int cnt1 = InfoManager.instance.DreamPieceInfo.Find(x => x.id == 601).amount;
        int cnt2 = InfoManager.instance.DreamPieceInfo.Find(x => x.id == 602).amount;
        int cnt3 = InfoManager.instance.DreamPieceInfo.Find(x => x.id == 603).amount;

        txtCnt0.text = (cnt0 - this.cnt0).ToString();
        txtCnt1.text = (cnt1 - this.cnt1).ToString();
        txtCnt2.text = (cnt2 - this.cnt2).ToString();
        txtCnt3.text = (cnt3 - this.cnt3).ToString();
    }
}
