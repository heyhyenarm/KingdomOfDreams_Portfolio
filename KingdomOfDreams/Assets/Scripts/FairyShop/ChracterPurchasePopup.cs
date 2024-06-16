using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChracterPurchasePopup : MonoBehaviour
{
    public TMP_Text txtPrice;
    public Button btnPurchase;
    public Button btnClose;

    void Start()
    {
        this.btnClose.onClick.AddListener(() =>
        {
            Debug.Log("popup close");
            this.gameObject.SetActive(false);
        });
    }
}
