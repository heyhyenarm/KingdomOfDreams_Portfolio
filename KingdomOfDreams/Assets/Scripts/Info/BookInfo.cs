using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInfo
{
    public int id;
    public int claim; //0: È¹µæÀü, 1: È¹µæÈÄ

    public BookInfo(int id, int claim = 0)
    {
        this.id = id;
        this.claim = claim;
    }
}
