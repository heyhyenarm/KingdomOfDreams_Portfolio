using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UITicket : MonoBehaviour
{
    public TMP_Text txtTime;
    public TMP_Text txtTicket;

    public Button btnUseTicket;

    private int ticket = 5;
    private int maxTicket = 5;
    private int ticketTime = 7200;
    private bool chargeOn;

    private DateTime chargeStartTime;

    public void Init()
    {
        Debug.Log("ticket Init");
        this.ticket = InfoManager.instance.GetTicketInfo().ticketAmount;
        this.chargeStartTime = InfoManager.instance.GetTicketInfo().ticketChargeStartTime;
    }

    void Start()
    {
        Debug.Log("<color=magenta>uiticket start</color>");
        this.Init();
        if (this.ticket != 5) this.chargeOn = true;

        this.btnUseTicket.onClick.AddListener(() =>
        {
            Debug.LogFormat("<color=magenta>btnUseTicket click, ticket count:{0}</color>", this.ticket);
            if (this.ticket > 0)
            {
                Debug.Log("btnUseTicket clicked, ticket-1");
                if (this.chargeOn == false) this.chargeStartTime = DateTime.Now;
                this.chargeOn = true;
                this.ticket--;
                InfoManager.instance.UseTicket(this.chargeStartTime);
                //StartCoroutine(this.CticketCharge());
                Debug.LogFormat("<color=green>this.chargeStartTime:{0}</color>", this.chargeStartTime);

                EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.GO_DREAMLAND);
            }
        });

    }

    private void Update()
    {
        //Debug.LogFormat("<color>charge on:{0}</color>", this.chargeOn);
        if (this.chargeOn)
        {
            //Debug.Log("<color>charge on</color>");
            TimeSpan timeCal = DateTime.Now - this.chargeStartTime;
            int timeSeconds = (int)timeCal.TotalSeconds;
            var time = this.ticketTime - timeSeconds;
            int hour = time / 3600;
            int min = (time % 3600) / 60;
            int sec = (time % 3600) % 60;
            this.txtTime.text = String.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
            Debug.LogFormat("<color>time{0}, timeseconds : {1}, info time : {2}, start : {3}</color>", time, timeSeconds, InfoManager.instance.GetTicketInfo().ticketChargeStartTime, this.chargeStartTime);

            if (time <= 0)
            {
                if ((ticketTime - time) > ticketTime)
                {
                    int n = 0;
                    while (timeSeconds > ticketTime && this.ticket < 5)
                    {
                        n++;
                        timeSeconds -= ticketTime;
                        this.ticket++;
                        if (this.ticket == 5)
                        {
                            this.txtTime.text = "00:00:00";
                            Debug.Log("charge off");
                            this.chargeOn = false;
                            return;
                        }
                    }
                }
                else
                {
                    this.chargeStartTime = DateTime.Now;
                    this.ticket++;
                    if (this.ticket == 5) return;
                }
            }
        }

        this.txtTicket.text = String.Format("{0}/{1}", this.ticket, this.maxTicket);
    }

}
