using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIPopupNickname : MonoBehaviour
{
    public Button btnClose;
    public TMP_InputField inputNickname;
    public Button btnOk;


    public System.Action<string> onClickLogin;

    private void Awake()
    {
        this.btnOk.onClick.AddListener((UnityEngine.Events.UnityAction)(() => {
            string nickname = this.inputNickname.text;

            if (string.IsNullOrEmpty(nickname))
            {
                Debug.LogFormat("<color=cyan>닉네임을 입력해주세요.</color>");
            }
            else
            {
                Debug.LogFormat("Nickname: {0}", nickname);
                InfoManager.instance.PlayerInfo.nickName = nickname;

            }

            EventManager.instance.onTouched();

        }));
    }

    public void Init()
    {

    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

}