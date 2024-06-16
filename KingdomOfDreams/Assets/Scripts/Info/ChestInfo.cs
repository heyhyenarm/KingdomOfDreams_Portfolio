using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInfo
{
    public int chestId;
    public int amount;

    public ChestInfo(int id, int amount)
    {
        this.chestId = id;
        this.amount = amount;
    }
}
