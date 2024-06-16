using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInfo
{
    public int id;
    public bool isClear;

    public MissionInfo(int id, bool isClear = false)
    {
        this.id = id;
        this.isClear = isClear;
    }
}