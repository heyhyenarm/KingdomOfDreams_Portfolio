using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInfo
{
    public int id;
    public int claim; //0: ȹ����, 1: ȹ����

    public BookInfo(int id, int claim = 0)
    {
        this.id = id;
        this.claim = claim;
    }
}
