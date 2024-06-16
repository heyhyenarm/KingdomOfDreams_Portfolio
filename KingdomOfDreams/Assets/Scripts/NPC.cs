using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public int id;
    public Image img;
    //�䱸 ���� ������ �Ϸ�?
    [HideInInspector]
    public bool isDone = false;
    //��Ȱ �Ϸ�?
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
