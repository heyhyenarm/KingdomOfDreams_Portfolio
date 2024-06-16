using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChestShopCell : MonoBehaviour
{
    public RectTransform rectTrans;

    public string _name;
    public int _num;
    public GameObject _frame;
    public GameObject _Chest;
    public GameObject _btnOpen;
    public GameObject _btnDream;
    public GameObject _btnFreeOff;
    public GameObject _btnAD;
    public GameObject _txtFree;
    public GameObject _txtName;
    public GameObject _txtTimer;
    public GameObject _txtAmount;

    public int price;
    public int pieceAmount;
    public int chestAmount;

    void Start()
    {
        this.rectTrans = GetComponent<RectTransform>();
    }
}
