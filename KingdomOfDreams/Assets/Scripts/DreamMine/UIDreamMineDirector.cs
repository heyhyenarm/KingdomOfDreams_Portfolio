using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDreamMineDirector : MonoBehaviour
{
    private int dreamCount;
    //���̵� �ƿ�
    public Image imgPopup;
    public TMP_Text txtWelcome;

    public float fadeSpeed = 0.5f;
    public float currentAlpha = 1f;

    //�����̴�
    public Slider slider;
    public float maxTime = 30f;
    public float currentTime;
    public GameObject timeSlider;

    //���â
    public Image imgResult;
    public TMP_Text txtCnt;

    public void Init()
    {
        currentTime = 0f;
        slider.minValue = 0f;
        slider.maxValue = maxTime;
        slider.value = currentTime;
        dreamCount = 0;

        EventManager.instance.dreamCount = (cnt) =>
        {
            this.dreamCount++;
        };
    }

    private void Update()
    {
        //�˾�â ���̵� �ƿ�
        this.FadeOutImage();

        //60�� Ÿ�̸�
        this.Timer();
    }

    public void FadeOutImage()
    {
        this.currentAlpha -= fadeSpeed * Time.deltaTime;

        if (this.currentAlpha <= 0)
        {
            this.imgPopup.gameObject.SetActive(false);
        }

        //���� �� ����
        Color newColor0 = imgPopup.color;
        newColor0.a = currentAlpha;
        imgPopup.color = newColor0;

        Color newColor1 = txtWelcome.color;
        newColor1.a = currentAlpha;
        txtWelcome.color = newColor1;
    }

    public void Timer()
    {
        if(currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
            slider.value = currentTime;
        }
        else if(currentTime >= maxTime)
        {
            currentTime = 0f;

            this.PlusDream(this.dreamCount);

            //this.timeSlider.SetActive(false);
        }
    }

    public void PlusDream(int cnt)
    {
        txtCnt.text = string.Format("X {0}", cnt);
        this.imgResult.gameObject.SetActive(true);


        InfoManager.instance.DreamAcount(cnt);

        StartCoroutine(WaitPlusDream());
        //int dreamCnt = 0;
        
        //txtCnt.text = string.Format("X {0}", dreamCnt);

        //Debug.LogFormat("<color=yellow>dreamCnt : {0}</color>", dreamCnt);

        //this.imgResult.gameObject.SetActive(true);

        //Invoke("ComeBack", 2f);

        //new WaitForSeconds(2f);
        //EventDispatcher.instance.SendEvent<Enums.ePortalType>((int)LHMEventType.eEventType.ENTER_PORTAL, Enums.ePortalType.Stage);
    }
    private IEnumerator WaitPlusDream()
    {
        yield return YieldCache.WaitForSeconds(2f);

        EventDispatcher.instance.SendEvent<Enums.ePortalType>((int)LHMEventType.eEventType.ENTER_PORTAL, Enums.ePortalType.Stage);
    }

    //public void ComeBack()
    //{
    //    EventDispatcher.instance.SendEvent<Enums.ePortalType>((int)LHMEventType.eEventType.ENTER_PORTAL, Enums.ePortalType.Stage);
    //}
}
