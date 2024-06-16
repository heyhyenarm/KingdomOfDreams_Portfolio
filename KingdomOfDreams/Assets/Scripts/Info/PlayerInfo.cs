using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public int nowCharacterId;
    public string nickName;
    public int[] myCharacters = new int[8];
    public int nowMatNum;

    public PlayerInfo(int id, string nickName = "null")
    {
        this.nowCharacterId = id;
        this.nickName = nickName;
    }
}
