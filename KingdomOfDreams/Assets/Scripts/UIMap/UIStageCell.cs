using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStageCell : MonoBehaviour
{
    public RectTransform rectTrans;

    public string _name;
    public int _num;
    public GameObject _btnStageGo;
    public GameObject _focusGo;
    public GameObject _playerPosGo;
    public GameObject _lockGo;

    public bool isClear = false;

    private void Start()
    {
        this.rectTrans = GetComponent<RectTransform>();
    }
}

