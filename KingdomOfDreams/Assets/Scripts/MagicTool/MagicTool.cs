using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTool : MonoBehaviour
{
    private int magicToolLevel;
    void Start()
    {   
        if(InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level >= 1 ) //�������� ���� ��������
        {
            this.SpeedUp();
        }
    }
    public void Init()
    {
        Debug.Log("�������� Init");

        //�������� ����, ���׷��̵� �̺�Ʈó��
        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.CREATED_MAGIC_TOOL, new EventHandler((type) =>
        {
            this.CreateMagicTool();
        }));

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.UPGRADE_MAGIC_TOOL, new EventHandler((type) =>
        {
            this.UpgradeMagicTool();
        }));

        //���� ���� ȹ�� �̺�Ʈó��
        EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, new EventHandler<int>((type, a) =>
        {
            this.GetDreamPiece(a);

            EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.CHECK_MAGICTOOL_LEVEL);

            InfoManager.instance.SaveDreamPieceInfo();
        }));

    }

    public void GetDreamPiece(int num)
    {
        var data = DataManager.instance.GetDreamPieceData(num);

        var id = data.id;
        var foundInfo = InfoManager.instance.DreamPieceInfo.Find(x => x.id == id);

        if (foundInfo == null)
        {
            DreamPieceInfo info = new DreamPieceInfo(id, 1);
            InfoManager.instance.DreamPieceInfo.Add(info);
        }
        else
        {
            foundInfo.amount++;
        }

        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.REFRESH_UI_MAGICTOOL);

    }

    public void CreateMagicTool()
    {
        Debug.Log("<color=yellow>���� ���� ����</color>");
        var magicPiece = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300);
        if (magicPiece != null)
        {
            magicPiece.level = 1;
        }

        var speedPiece = new MagicToolInfo(310);
        InfoManager.instance.MagicToolInfo.Add(speedPiece);
        var detoxPiece = new MagicToolInfo(320);
        InfoManager.instance.MagicToolInfo.Add(detoxPiece);
        var wisdomPiece = new MagicToolInfo(330);
        InfoManager.instance.MagicToolInfo.Add(wisdomPiece);

        InfoManager.instance.SaveMagicToolInfo();

        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.CREATE_MAGIC_CIRCLE);

        SpeedUp();
    }

    public void UpgradeMagicTool()
    {
        var magicPiece = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300);
        if (magicPiece != null)
        {
            magicPiece.level++;
        }

        var speedPiece = InfoManager.instance.MagicToolInfo.Find(x => x.id == 310);
        if (speedPiece != null)
        {
            speedPiece.level++;
        }

        var detoxPiece = InfoManager.instance.MagicToolInfo.Find(x => x.id == 320);
        if (detoxPiece != null)
        {
            detoxPiece.level++;
        }

        var widsdomPiece = InfoManager.instance.MagicToolInfo.Find(x => x.id == 330);
        if (widsdomPiece != null)
        {
            widsdomPiece.level++;
        }

        InfoManager.instance.SaveMagicToolInfo();

        Debug.LogFormat("<color=yellow>�������� {0}�ܰ� ���׷��̵�</color>", InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level);

        SpeedUp();
    }

    //��ȭ�ϱ�
    public void Detox()
    {

    }
    //�̼������ϱ�
    public void SpeedUp()
    {
        //�������� ������ ���� �̼� ��ȭ
        var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level; //�������� ���� ��������
        this.magicToolLevel = myLevel;

        if (myLevel >= 1) //1���� �̻��̸�
        {
            var data = DataManager.instance.GetMagicToolLevelDatas().Find(x => x.level == myLevel); //���� ������ ���

            var player = GameObject.FindObjectOfType<PlayerMono>();

            player.speed = 5 * data.add_speed_property; //���� �ӵ� * �̼Ӵɷ�

            var farming = GameObject.FindObjectOfType<Farming>();
            if (farming != null)
            {
                farming.GetOriginalValues();
                farming.SetPoisonDuration(data.add_detox_property);
            }

        }
        
    }

}
