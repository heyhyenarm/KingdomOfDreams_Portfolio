using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStageCellBuilder : IUIBuilder
{
    UIStageCell _uiStageCell;

    private RectTransform rectTrans;
    private string _name;
    private int _num;
    private GameObject _btnStageGo;
    private GameObject _playerPosGo;
    private GameObject _lockGo;
    private GameObject _focusGo;
    private GameObject stageGo;

    public UIStageCellBuilder() 
    {
        this.stageGo = new GameObject();
        this.rectTrans = this.stageGo.AddComponent<RectTransform>();
        this._uiStageCell = this.stageGo.AddComponent<UIStageCell>();
    }

    public IUIBuilder SetRectTrans(RectTransform rectTrans)
    {
        this.stageGo.GetComponent<RectTransform>().SetParent(rectTrans);
        return this;
    }

    public UIStageCellBuilder BuildButton()
    {
        this._btnStageGo = new GameObject("btnStage");
        this._btnStageGo.AddComponent<RectTransform>().SetParent(this.rectTrans);
        this._btnStageGo.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        //this._btnStageGo.AddComponent<Image>();
        //this._btnStageGo.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        this._btnStageGo.AddComponent<Button>();
        return this;
    }
    public IUIBuilder SetName(string name)
    {
        this.stageGo.name = name;
        this._name = name;
        return this;
    }
    public IUIBuilder SetNum(int num)
    {
        this._num = num;
        return this;
    }
    public IUIBuilder SetPosition(float x, float y)
    {
        var pos = this.rectTrans.localPosition;
        pos.x = x;
        pos.y = y;
        this.rectTrans.localPosition = new Vector3(x,y,0);
        return this;
    }
    public IUIBuilder SetSprite(Sprite spriteColor)
    {
        var img = this._btnStageGo.AddComponent<Image>();
        img.GetComponent<Image>().sprite = spriteColor;
        img.SetNativeSize();
        img.color = new Color(0.3f, 0.3f, 0.3f);

        return this;
    }

    public IUIBuilder SetSize(float w, float h)
    {
        Debug.LogFormat("SetSize {0},{1}", w, h);
        var rectTrans = this._btnStageGo.GetComponent<RectTransform>();
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        return this;
    }

    public UIStageCellBuilder AddLock(Sprite spriteLock)
    {
        this._lockGo = new GameObject("lockGo");
        this._lockGo.AddComponent<RectTransform>().SetParent(this.rectTrans);
        var rectTrans = this._lockGo.GetComponent<RectTransform>();
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 220);
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 220);
        var img = this._lockGo.AddComponent<Image>();
        img.color = new Color(1, 1, 1, 0);

        rectTrans.localPosition = new Vector2(0, 0);

        var go = new GameObject("icon");
        go.AddComponent<RectTransform>().SetParent(rectTrans);
        var rectTrasnIcon = go.GetComponent<RectTransform>();
        var imgIcon = go.AddComponent<Image>();
        imgIcon.sprite = spriteLock;
        imgIcon.SetNativeSize();
        rectTrasnIcon.localPosition = new Vector2(0, 0);
        rectTrasnIcon.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        return this;
    }
    public UIStageCellBuilder AddFocus(Sprite spriteFocus)
    {
        this._focusGo = new GameObject("focusGo");
        this._focusGo.AddComponent<RectTransform>().SetParent(this.rectTrans);
        var rectTrans = this._focusGo.GetComponent<RectTransform>();
        rectTrans.localPosition = new Vector2(0, -50);
        this._focusGo.SetActive(false);
        var img = this._focusGo.AddComponent<Image>();
        img.sprite = spriteFocus;
        img.SetNativeSize();
        rectTrans.localScale = new Vector3(1f, 1f, 1f);

        return this;
    }

    public UIStageCellBuilder SetPlayerPos()
    {
        this._playerPosGo = new GameObject("PlayerPosGo");
        this._playerPosGo.AddComponent<RectTransform>().SetParent(this.rectTrans);
        this._playerPosGo.GetComponent<RectTransform>().localPosition = new Vector2(0, -130);

        return this;
    }

    public GameObject Build()
    {
        _uiStageCell._name = _name;
        _uiStageCell._num = _num;
        _uiStageCell._btnStageGo = _btnStageGo;
        _uiStageCell._focusGo = _focusGo;
        _uiStageCell._playerPosGo = _playerPosGo;
        _uiStageCell._lockGo = _lockGo;

        return this.stageGo;
    }
}
