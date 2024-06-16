using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOffline : MonoBehaviour
{
    public Button btnClose;
    public Text txtBody;
    public Text txtCount;
    void Start()
    {
        this.btnClose.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });

    }

    public void Init()
    {
        Debug.Log("<color=pink>오프라인 Init</color>");
        EventDispatcher.instance.AddEventHandler<TimeSpan>((int)LHMEventType.eEventType.SHOW_OFFLINE_GIFT_EXPLAIN, this.OfflineGift);

    }
    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler<TimeSpan>((int)LHMEventType.eEventType.SHOW_OFFLINE_GIFT_EXPLAIN, this.OfflineGift);
    }

    void OfflineGift(short type, TimeSpan offlineTime)
    {
        int maxGiftCount;
        this.gameObject.SetActive(true);

        Debug.LogFormat("<color=yellow>offline 시간: {0}</color>", offlineTime);

        TimeSpan twoHours = new TimeSpan(2, 0, 0);
        TimeSpan fiveMinutes = new TimeSpan(0, 5, 0);
        if (offlineTime >= twoHours)
        {
            this.txtBody.text = string.Format("오프라인 수익\n\n {0} 시간 ", 2);
            maxGiftCount = (int)(twoHours.TotalMinutes / 5);
        }
        else if(offlineTime < fiveMinutes)
        {
            this.txtBody.text = string.Format("오프라인 수익\n\n{0}시간 {1}분\n보상 미지급", offlineTime.Hours, offlineTime.Minutes);
            maxGiftCount = 0;

        }
        else
        {
            this.txtBody.text = string.Format("오프라인 수익\n\n{0}시간 {1}분", offlineTime.Hours, offlineTime.Minutes);
            maxGiftCount = (int)(offlineTime.TotalMinutes / 5);

        }

        this.txtCount.text = string.Format("X{0}", maxGiftCount);

    }

}
