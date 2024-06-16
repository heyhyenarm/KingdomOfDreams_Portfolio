using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo
{
    public int id;
    public bool isDone = false;
    public bool isComplete = true;
    public bool endTime = false;
    public bool isBuild = false;
    public System.DateTime buildStartTime = System.DateTime.Now;
    public BuildingInfo(int id)
    {
        this.id = id;
    }
    public void BuildingInfoInit()
    {
        this.isDone = false;
        this.isComplete = true;
        this.endTime = false;
        this.isBuild = false;
        this.buildStartTime = System.DateTime.Now;
    }
}
