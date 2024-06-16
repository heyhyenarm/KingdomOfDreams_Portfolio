using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeChargeTimeInfo
{
    public System.DateTime chestChargeStartTime;
    public bool chestAdOn;
    public FreeChargeTimeInfo(System.DateTime time, bool AdOn)
    {
        this.chestChargeStartTime = time;
        this.chestAdOn = AdOn;
    }
}
