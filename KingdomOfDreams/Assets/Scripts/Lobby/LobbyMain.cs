using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMain : MonoBehaviour
{
    public UILobbyDirector director;

    //public System.Action<int> onClickStartGame;
    //private int selectedCharacterId;

    public void Init()
    {
        director.Init();
    }

}
