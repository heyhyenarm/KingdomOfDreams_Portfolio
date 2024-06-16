using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMagicTool : MonoBehaviour
{
    public Button btnClose;
    public UIPieces pieces;
    //테스트 버튼
    public Button btnTest1;
    public Button btnTest2;
    public Button btnTest3;

    public Button btnMagicTool;
    public Image imgExclaimIcon;
    public Text txtDetail;

    void Start()
    {
        //초기 UI 설정
        if (InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level == 0)
        {
            this.pieces.deem.gameObject.SetActive(true);
            this.pieces.txtLevel.gameObject.SetActive(false);
        }
        else
        {
            this.pieces.deem.gameObject.SetActive(false);
            this.pieces.txtLevel.gameObject.SetActive(true);
            this.pieces.txtLevel.text = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level.ToString();
        }

        this.TxtDetailRefresh(); //텍스트 UI 설정
        this.CheckMagicToolLevel(); //마법도구 레벨 체크하여 버튼 활성화, 느낌표 표시

        this.btnMagicTool.onClick.AddListener(() =>
        {

            this.imgExclaimIcon.gameObject.SetActive(false);
            this.pieces.deem.gameObject.SetActive(false);

            this.pieces.ResetUIPiece();

            MagicToolInfo info = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300);
            if (info != null && info.level == 0)
            {
                //이벤트 전송 
                EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.CREATED_MAGIC_TOOL);
                this.TxtDetailRefresh(); //텍스트 UI 갱신
            }
            else
            {
                EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.UPGRADE_MAGIC_TOOL);
                this.TxtDetailRefresh(); //텍스트 UI 갱신
            }

            this.pieces.Refresh();
            this.pieces.txtLevel.gameObject.SetActive(true);
            this.pieces.txtLevel.text = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level.ToString();
            this.btnMagicTool.GetComponent<Button>().interactable = false;

            InfoManager.instance.SaveDreamPieceInfo();

            this.CheckMagicToolLevel();

        });

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.CHECK_MAGICTOOL_LEVEL, new EventHandler((type) =>
        {
            this.CheckMagicToolLevel();

        }));

        

        //----------------테스터------------------------------------------------------------------------

        this.btnTest1.onClick.AddListener(() =>
        {
            for (int i = 0; i < 147; i++)
            {
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 600);


                this.CheckMagicToolLevel();

                InfoManager.instance.SaveDreamPieceInfo();
            }
        });

        this.btnTest2.onClick.AddListener(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 600);
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 601);
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 602);
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 603);

                this.CheckMagicToolLevel();

                InfoManager.instance.SaveDreamPieceInfo();
            }
        });

        this.btnTest3.onClick.AddListener(() =>
        {
            for (int i = 0; i < 1; i++)
            {
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 600);
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 601);
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 602);
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 603);

                this.CheckMagicToolLevel();

                InfoManager.instance.SaveDreamPieceInfo();
            }
        });

        //-------------------------------------------------------------------------------------------------
        this.btnClose.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });

    }

    public void Init()
    {
        this.pieces.Init();
    }

    public void CheckMagicToolLevel()
    {
        var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;
        var data = DataManager.instance.GetMagicToolLevelDatas().Find(x => x.level == myLevel + 1);

        if(data != null)
        {
            if (InfoManager.instance.DreamPieceInfo.Find(x => x.id == 600).amount >= data.magic_piece_require
                    && InfoManager.instance.DreamPieceInfo.Find(x => x.id == 601).amount >= data.speed_piece_require
                    && InfoManager.instance.DreamPieceInfo.Find(x => x.id == 602).amount >= data.detox_piece_require
                    && InfoManager.instance.DreamPieceInfo.Find(x => x.id == 603).amount >= data.wisdom_piece_require)
            {
                this.btnMagicTool.GetComponent<Button>().interactable = true;
                this.imgExclaimIcon.gameObject.SetActive(true);
            }

        }
    }

    public void TxtDetailRefresh()
    {
        var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;
        var data = DataManager.instance.GetMagicToolLevelDatas().Find(x => x.level == myLevel);
        if(data != null)
        {
            this.txtDetail.text = string.Format("마법도구 레벨: {0}단계\n\n효능\n\n마법: 획득량 {1}개 증가\n\n신속: 이동 속도 {2} % 증가\n\n정화: 벌, 독 양배추 피해 {3} % 감소\n\n지혜: 도감 레어 아이템 획득 가능 획득 확률 {4} % 증가"
                 , myLevel, data.add_magic_property, data.add_speed_property, data.add_detox_property, data.add_wisdom_property);
        }
    }
}
