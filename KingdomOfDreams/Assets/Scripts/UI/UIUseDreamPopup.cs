using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUseDreamPopup : MonoBehaviour
{
    private int id;

    public Button dim;
    public Button btn_close;
    public Button btn_use;
    public TMP_Text txt_price;
    private System.DateTime buildStartTime;
    private int buildTime;
    [HideInInspector]
    public int price;
    private void Update()
    {
        var time = System.DateTime.Now - this.buildStartTime;
        var remaintime = this.buildTime - 3600 * time.Hours - 60 * time.Minutes - time.Seconds;

        if (remaintime <= 0) this.gameObject.SetActive(false);
        else if (remaintime % 20 == 0) this.price = remaintime / 20;
        else if (remaintime % 20 != 0) this.price = remaintime / 20 + 1;

        this.txt_price.text = string.Format("{0:#,###}", this.price);
    }
    public void PriceUpdate(BuildingData data)
    {
        this.id = data.id;
        this.buildTime = data.build_time;

        var info = InfoManager.instance.BuildingInfos.Find(x => x.id == data.id);
        this.buildStartTime = info.buildStartTime;
    }
    public int ClosePopup()
    {
        this.gameObject.SetActive(false);

        return this.id;
    }
}
