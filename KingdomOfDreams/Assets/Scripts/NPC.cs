using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public int id;
    public Image img;
    //요구 수량 모으기 완료?
    [HideInInspector]
    public bool isDone = false;
    //부활 완료?
    [HideInInspector]
    public bool isClear = false;

    public Animator anim;

    public void Init(int id)
    {
        this.id = id;
        Debug.LogFormat("npc id:{0}", this.id);
    }
    private void Start()
    {
    }
    private void Update()
    {
        img.transform.LookAt(Camera.main.transform);

        if (isClear)
        {
            this.anim.SetInteger("State", 1);
        }
    }
}
