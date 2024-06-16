using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dreamland : MonoBehaviour
{
    enum HSYEnum
    {
        None=-1,
        End_Time
    }

    public GameObject[] pieces;
    public Transform[] pieceTranses;
    private int totalCount=20;

    public Text txtRemainTime;
    private float totalTime = 5f;
    private float delta;

    private void Start()
    {
        EventDispatcher.instance.AddEventHandler((int)Dreamland.HSYEnum.End_Time,new EventHandler((type)=>{
            txtRemainTime.text = "시간 끝!";
            //이전 씬으로 전환하기
        }));

        this.CreateRandomDreamPieces();
        this.txtRemainTime.text = "시작!";
        StartCoroutine(this.CoCheckTime());
    }

    private IEnumerator CoCheckTime()
    {
        this.delta = totalTime;
        while (delta>0)
        {
            txtRemainTime.text = string.Format("{0:0.0}",delta);
            delta -= Time.deltaTime;          
            yield return null;
        }
        EventDispatcher.instance.SendEvent((int)Dreamland.HSYEnum.End_Time);
        yield break;
    }

    private void CreateRandomDreamPieces()
    {
        List<int> posIndexes = new List<int>();
        while (posIndexes.Count < totalCount)
        {
            Debug.LogFormat("pieceTrans length:{0}", this.pieceTranses.Length);
            var num = Random.Range(0, this.pieceTranses.Length);

            if (!posIndexes.Contains(num))
                posIndexes.Add(num);
        }

        for (int i = 0; i < totalCount; i++)
        {
            GameObject pieceGo = null;
            int selectPiece = Random.Range(1, this.pieces.Length + 1);
            if (selectPiece == 1)
            {
                pieceGo = pieces[0];
            }
            if (selectPiece == 2)
            {
                pieceGo = pieces[1];
            }
            if (selectPiece == 3)
            {
                pieceGo = pieces[2];
            }
            if (selectPiece == 4)
            {
                pieceGo = pieces[3];
            }

            Instantiate(pieceGo, this.pieceTranses[posIndexes[i]]);
        }
    }
}
