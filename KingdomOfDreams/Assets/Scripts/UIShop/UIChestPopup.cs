using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIChestPopup : MonoBehaviour
{
    public Button btnPurchase;
    public Button btnClose;
    public TMP_Text txtPrice;
    public TMP_Text txtChestName;
    public TMP_Text txtAmount;

    public Action onPurchase;

    void Start()
    {
        this.btnPurchase.onClick.AddListener(() =>
        {
            Debug.LogFormat("<color=yellow>puchase {0}</color>", this.txtChestName);
            this.onPurchase();
        });
        this.btnClose.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

}
