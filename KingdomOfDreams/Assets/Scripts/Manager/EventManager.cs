using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public Action onTouched;
    public Action EndOpening;
    public Action<int> onGetIngredient;
    public Action<int> onMissionUpdate;
    public Action<MissionData> onAchieved;
    public Action<int> onMissionClearNPC;
    public Action onTutorial01ClearUI;
    public Action onTutorial02ClearUI;
    public Action onTutorial03ClearUI;
    public Action onTutorial04ClearUI;
    public Action onTutorial05ClearUI;
    public Action onStage06ClearUI;
    public Action onStage07ClearUI;
    public Action onStage08ClearUI;
    public Action onTutorial01Clear;
    public Action onTutorial02Clear;
    public Action onTutorial03Clear;
    public Action onTutorial04Clear;
    public Action onTutorial05Clear;
    public Action onStage06Clear;
    public Action onStage07Clear;
    public Action onStage08Clear;
    public Action onStage06StartEnd;
    public Action useTicket;
    public Action updateChest;
    public Action<int> getDream;

    public Action<int> changeScene;
    public Action<int> onUIPlayerMoveDone;

    //���� 6�������� ����
    public Action onPoison;
    public Action onStun;
    public Action onDungeon;
    public Action onPlain;
    public Action onHit;

    //������
    public Action onClickedSkip;
    public Action playOpening;

    //���� Ŭ����
    public Action<int> blackToColor;

    //���� Ŭ���� �� npc �̵�
    public Action<int> npcMove;

    //�� ����
    public Action<int> dreamCount;

    public EventManager()
    {

    }

    public void Start()
    {
        //instance = this;
        //Debug.Log(instance);
    }
}