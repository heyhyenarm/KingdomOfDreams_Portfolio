using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicToolInfo
{
    public int id;
    public int level;

    public MagicToolInfo(int id, int level = 1)
    {
        this.id = id;
        this.level = level;
    }
}
