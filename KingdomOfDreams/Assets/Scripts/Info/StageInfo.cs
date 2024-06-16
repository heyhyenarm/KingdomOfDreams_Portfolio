using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
    public int stage;
    public bool isClear;

    public StageInfo(int num, bool isClear)
    {
        this.stage = num;
        this.isClear = isClear;
    }
}
