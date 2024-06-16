using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIChestShopCellBuilder : IUIBuilder
{
    UIChestShopCell _uiChestShopCell;
    private RectTransform rectTrans;
    private string _name;
    private int _num;
    private GameObject _frame;
    private GameObject _Chest;
    private GameObject _btnFreeOn;
    private GameObject _btnDream;
    private GameObject _btnTimer;
    private GameObject _btnAD;
    private GameObject _txtName;
    private GameObject _txtTimer;
    private GameObject _txtAmount;

    private GameObject chestGO;

    public UIChestShopCellBuilder()
    {
        this.chestGO = new GameObject();
        this.rectTrans = this.chestGO.AddComponent<RectTransform>();
        this._uiChestShopCell = this.chestGO.AddComponent<UIChestShopCell>();
    }
    public IUIBuilder SetRectTrans(RectTransform rectTrans)
    {
        this.chestGO.GetComponent<RectTransform>().SetParent(rectTrans);
        return this;
    }
    public IUIBuilder BuildButtonOpen(Sprite sprite, Font font, string txt)
    {
        //btnFreeOn
        this._btnFreeOn = new GameObject("btnOpen");
        var freeRect = this._btnFreeOn.AddComponent<RectTransform>();
        freeRect.SetParent(this.rectTrans);
        freeRect.localPosition = new Vector2(0, -350);
        freeRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 330);
        freeRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);

        var img = this._btnFreeOn.AddComponent<Image>();
        img.sprite = sprite;
        img.color = new Color(0.3f, 0.3f, 0.3f);
        this._btnFreeOn.AddComponent<Button>();

        var txtOpen = new GameObject("txtDream");
        txtOpen.AddComponent<RectTransform>().SetParent(this._btnFreeOn.GetComponent<RectTransform>());
        txtOpen.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);

        var text = txtOpen.AddComponent<Text>();
        text.text = txt;
        text.font = font;

        var txtRect = txtOpen.GetComponent<RectTransform>();
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 330);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 35;

        return this;
    }
    public IUIBuilder BuildButtonPDream(Sprite sprite, Font font, string txt)
    {
        //_btnDream
        this._btnDream = new GameObject("btnDream");
        var dreamRect = this._btnDream.AddComponent<RectTransform>();
        dreamRect.SetParent(this.rectTrans);
        dreamRect.localPosition = new Vector2(0, -350);
        dreamRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 330);
        dreamRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);

        var img = this._btnDream.AddComponent<Image>();
        img.sprite = sprite;
        img.color = new Color(0.3f, 0.3f, 0.3f);
        this._btnDream.AddComponent<Button>();

        var txtDream = new GameObject("txtDream");
        txtDream.AddComponent<RectTransform>().SetParent(this._btnDream.GetComponent<RectTransform>());
        txtDream.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);

        var text = txtDream.AddComponent<Text>();
        text.text = string.Format("{0} ²Þ", txt);
        text.font = font;

        var txtRect = txtDream.GetComponent<RectTransform>();
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 330);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 35;

        return this;
    }
    public UIChestShopCellBuilder AddButtonAd(Sprite sprite, Font font)
    {
        //btnAD
        this._btnAD = new GameObject("btnAD");
        var ADRect = this._btnAD.AddComponent<RectTransform>();
        ADRect.SetParent(this.rectTrans);
        ADRect.localPosition = new Vector2(0, -350);
        ADRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 330);
        ADRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);

        var img = this._btnAD.AddComponent<Image>();
        img.sprite = sprite;
        img.color = new Color(0.3f, 0.3f, 0.3f);
        this._btnAD.AddComponent<Button>();
        

        var txtAd = new GameObject("txtAd");
        var txtRect = txtAd.AddComponent<RectTransform>();
        txtRect.SetParent(this._btnAD.GetComponent<RectTransform>());
        txtRect.localPosition = new Vector2(0, 0);

        var text = txtAd.AddComponent<Text>();
        text.text = "±¤°íº¸±â";
        text.font = font;
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 330);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 35;

        return this;
    }
    public UIChestShopCellBuilder AddButtonTimer(Sprite sprite, Font font)
    {
        //btnFreeOff
        this._btnTimer = new GameObject("btnTimer");
        var timerRect = this._btnTimer.AddComponent<RectTransform>();
        timerRect.SetParent(this.rectTrans);
        timerRect.localPosition = new Vector2(0, -350);
        timerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 330);
        timerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);

        var img = this._btnTimer.AddComponent<Image>();
        img.sprite = sprite;
        this._btnTimer.AddComponent<Button>();
        img.color = new Color(0.3f, 0.3f, 0.3f);

        this._txtTimer = new GameObject("txtTime");
        var txtRect = this._txtTimer.AddComponent<RectTransform>();
        txtRect.SetParent(this._btnTimer.GetComponent<RectTransform>());
        txtRect.localPosition = new Vector2(0, 0);

        var text = this._txtTimer.AddComponent<Text>();
        text.text = "00:00:00";
        text.font = font;
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 330);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 35;

        return this;
    }
    public IUIBuilder SetName(string name)
    {
        this.chestGO.name = name;
        this._name = name;
        this._txtName = new GameObject("txtName");
        var txtRect = this._txtName.AddComponent<RectTransform>();
        txtRect.SetParent(this.rectTrans);
        txtRect.localPosition = new Vector2(0, 200);

        return this;
    }
    public IUIBuilder SetNum(int num)
    {
        this._num = num;
        return this;
    }
    public IUIBuilder SetPosition(float x, float y)
    {
        this.rectTrans.localPosition = new Vector3(0, 0, 0);
        return this;
    }
    public IUIBuilder SetSprite(Sprite spriteColor)
    {
        //frame
        this._frame = new GameObject("imgFrame");
        var frameRect = this._frame.AddComponent<RectTransform>();
        frameRect.SetParent(this.rectTrans);
        frameRect.localPosition = new Vector2(0, 0);

        var img = this._frame.AddComponent<Image>();
        img.GetComponent<Image>().sprite = spriteColor;
        img.color = new Color(0.2f, 0.2f, 0.3f);
        img.SetNativeSize();
        img.type = Image.Type.Sliced;

        frameRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 460);
        frameRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 515);

        return this;
    }
    public IUIBuilder SetSize(float w, float h)
    {
        var rectTrans = this.chestGO.GetComponent<RectTransform>();
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        return this;
    }
    public UIChestShopCellBuilder SetSpriteChest(Sprite sprite)
    {
        //chest
        this._Chest = new GameObject("imgChest");
        var chestRect = this._Chest.AddComponent<RectTransform>();
        chestRect.SetParent(this.rectTrans);
        chestRect.localPosition = new Vector2(0, 0);

        var img = this._Chest.AddComponent<Image>();
        img.GetComponent<Image>().sprite = sprite;
        img.SetNativeSize();

        chestRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 217);
        chestRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 198.1f);

        return this;
    }
    public UIChestShopCellBuilder SetText(Font font)
    {
        var text = this._txtName.AddComponent<Text>();
        text.text = this._name;
        text.font = font;
        var textRect = text.GetComponent<RectTransform>();
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 460);
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 35;

        //textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 515);

        return this;
    }
    public UIChestShopCellBuilder SetTextAmount(Font font, String stringAmount)
    {
        this._txtAmount = new GameObject("txtAmount");
        var txtRect = this._txtAmount.AddComponent<RectTransform>();
        txtRect.SetParent(this.rectTrans);
        txtRect.localPosition = new Vector2(0, -200);
        var text = this._txtAmount.AddComponent<Text>();
        text.text = stringAmount;
        text.font = font;
        var textRect = text.GetComponent<RectTransform>();
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 460);
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 35;

        //textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 515);

        return this;
    }

    public GameObject Build()
    {
        _uiChestShopCell._name = _name;
        _uiChestShopCell._num = _num;
        _uiChestShopCell._frame = _frame;
        _uiChestShopCell._Chest = _Chest;
        _uiChestShopCell._btnOpen = _btnFreeOn;
        _uiChestShopCell._btnFreeOff = _btnTimer;
        _uiChestShopCell._btnAD = _btnAD;
        _uiChestShopCell._btnDream = _btnDream;
        _uiChestShopCell._txtName = _txtName;
        _uiChestShopCell._txtTimer = _txtTimer;
        _uiChestShopCell._txtAmount = _txtAmount;

        return this.chestGO;
    }
}
