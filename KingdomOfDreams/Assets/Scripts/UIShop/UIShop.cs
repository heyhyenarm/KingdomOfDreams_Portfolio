using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    public Button btnShop;
    public UIChestShop uiChestShop;
    public void Init()
    {
        Debug.Log("UIShop Init");
        DataManager.instance.LoadChestData();
        this.uiChestShop.Init();
    }
    void Start()
    {
        this.uiChestShop.gameObject.SetActive(false);
        this.btnShop.onClick.AddListener(() =>
        {
            Debug.Log("btnShop clicked");
            this.uiChestShop.gameObject.SetActive(true);
        });
    }
}
