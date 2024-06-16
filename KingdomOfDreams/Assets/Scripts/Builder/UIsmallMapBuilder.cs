//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEditor.SceneManagement;
//using UnityEngine;
//using UnityEngine.UI;


//public class UIsmallMapBuilder : IUIBuilder
//{

//    //private GameObject StageGo;
//    private string _name;
//    private int _num;
//    private Button _btnStageGo;
//    private Transform _trans;
//    private GameObject _playerPosGo;
//    private Image _imgColor;
//    private Image _imgBlack;
//    private GameObject _lockGo;
//    private GameObject _focusGo;

//    private UIStageCell _uiStageCell;
//    public UIsmallMapBuilder() 
//    {
//        _uiStageCell = new UIStageCell();
//    }

//    public IUIBuilder BuildButtonOpen()
//    {
//        var go = new GameObject();
//        this._btnStageGo = go.AddComponent<Button>();
//        return this;
//    }
//    public IUIBuilder SetName(string name)
//    {
//        this._name = name;
//        return this;
//    }
//    public IUIBuilder SetNum(int num)
//    {
//        this._num = num;
//        return this;
//    }
//    public IUIBuilder SetPosition(float x, float y)
//    {
//        this._trans.position = new Vector2(x, y);
//        return this;
//    }
//    public IUIBuilder SetSize(float w, float h)
//    {
//        this._trans.localScale = new Vector2(w, h);
//        return this;
//    }
//    public IUIBuilder SetSprite(Sprite spriteColor)
//    {
//        this._imgColor.sprite = spriteColor;
//        this._imgBlack.sprite = spriteColor;
//        this._imgBlack.color = new Color(0.3f, 0.3f, 0.3f);
//        return this;
//    }
//    public UIsmallMapBuilder AddLock(Sprite spriteLock)
//    {

//        this._lockGo = new GameObject("lockGo");
//        this._lockGo.AddComponent<Image>();
//        this._lockGo.GetComponent<Image>().sprite = spriteLock;
//        return this;
//    }
//    public UIsmallMapBuilder AddFocus(Sprite spriteFocus)
//    {
//        this._focusGo = new GameObject("focusGo");
//        this._focusGo.SetActive(false);
//        this._focusGo.AddComponent<Image>();
//        this._focusGo.GetComponent<Image>().sprite = spriteFocus;
//        return this;
//    }
//    public UIsmallMapBuilder SetPlayerPos()
//    {
//        this._playerPosGo = new GameObject();
//        this._playerPosGo.transform.position = new Vector2(0, -165f);
//        return this;
//    }

//    public UIStageCell Build()
//    {
//        return _uiStageCell;
//    }
//}
