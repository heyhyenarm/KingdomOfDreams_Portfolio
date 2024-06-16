using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIInventory : MonoBehaviour
{
    public UIInventoryScroll scrollview;
    //public ProductionChain productionChain;
    public Image imgLock;
    public GameObject imgItems;
    public Button btnClose;

    private int stageNum;
    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.SHOW_PRODUCTION_CHAIN, EventShowProductionChain);
    }
    public void Start()
    {   
        this.btnClose.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });

        foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
        {
            Debug.LogFormat("<color>stageInfo.stage : {0}, stageInfo.isClear : {1}</color>", stageInfo.stage, stageInfo.isClear);
            if (stageInfo.isClear == false)
            {
                //원하는곳에 stageInfo.num 저장
                this.stageNum = stageInfo.stage;
                break;
            }
        }

        if(this.stageNum >= 7)
        {
            this.imgLock.gameObject.SetActive(false);
            this.imgItems.SetActive(true);
        }

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.SHOW_PRODUCTION_CHAIN, EventShowProductionChain);

    }

    public void Init()
    {
        this.scrollview.Init();
    }

    public void EventShowProductionChain(short type)
    {
        this.imgLock.gameObject.SetActive(false);
        this.imgItems.SetActive(true);

    }
}
