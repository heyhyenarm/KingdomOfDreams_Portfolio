using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIStage : MonoBehaviour
{
    public int stageAmount = 8;

    public RectTransform rectTrans;
    public Button btnBack;
    public Sprite spriteLock;
    public Sprite spriteFocus;
    public GameObject[] uiStageCells;
    public UIStagePlayer uiStagePlayer;

    public UIStageCell nowStage;
    public bool isClearMoving = false;

    public System.Action onClickBack;

    public void Init()
    {
        this.rectTrans = GetComponent<RectTransform>();
        this.uiStageCells = new GameObject[stageAmount];
        Debug.Log("<color=blue>UIStage Init</color>");
        this.StageBuild(this.stageAmount, this.rectTrans);
        //this.nowStage = this.uiStageCells[0].GetComponent<UIStageCell>();
        //foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
        //{
        //    Debug.LogFormat("<color>stageInfo.stage : {0}, stageInfo.isClear : {1}</color>", stageInfo.stage, stageInfo.isClear);
        //    if (stageInfo.isClear == false)
        //    {
        //        Debug.LogFormat("stageInfo.stage : {0}", stageInfo.stage);
        //        this.nowStage = this.uiStageCells[(int)stageInfo.stage].GetComponent<UIStageCell>();
        //
        //        break;
        //    }
        //}
        for(int i = 0; i < InfoManager.instance.StageInfos.Count; i++)
        {
            var stageInfo = InfoManager.instance.StageInfos[i];
            Debug.LogFormat("<color>stageInfo.stage : {0}, stageInfo.isClear : {1}</color>", stageInfo.stage, stageInfo.isClear);

            this.uiStageCells[i].GetComponent<UIStageCell>()._lockGo.SetActive(false);
            if (stageInfo.isClear == false)
            {
                Debug.LogFormat("stageInfo.stage : {0}", stageInfo.stage);
                this.nowStage = this.uiStageCells[stageInfo.stage].GetComponent<UIStageCell>();

                break;
            }
            this.uiStageCells[i].GetComponent<UIStageCell>()._btnStageGo.GetComponent<Image>().color=new Color(1,1, 1, 1);
        }

        this.uiStagePlayer.transform.position = this.nowStage.GetComponent<UIStageCell>()._playerPosGo.transform.position;
        this.uiStagePlayer.GetComponent<RectTransform>().SetAsLastSibling();
        this.nowStage._lockGo.SetActive(false);
    }
    void Start()
    {
        this.nowStage._focusGo.SetActive(true);

        this.btnBack.onClick.AddListener(() =>
        {
            Debug.Log("btnBack clicked");
            this.onClickBack();
        });

        foreach (GameObject uiStageCellGo in this.uiStageCells)
        {
            var uiStageCell = uiStageCellGo.GetComponent<UIStageCell>();
            if (uiStageCell.isClear) uiStageCell._lockGo.SetActive(false);

            //일단 막아둠
            //uiStageCell._btnStageGo.GetComponent<Button>().onClick.AddListener(() =>
            //{
            //    Debug.LogFormat("{0} clicked", uiStageCell._num);
            //    if (!uiStagePlayer.isMoving)
            //    {
            //        this.SelectStage(uiStageCell);

            //        //yield return new WaitForSeconds(2f);
            //    }
            //    if (!this.isClearMoving) EventManager.instance.onUIPlayerMoveDone(uiStageCell._num);
            //});
        }

        EventManager.instance.onUIPlayerMoveDone = (stageNum) =>
        {
            new WaitForSeconds(8f);
            EventManager.instance.changeScene(stageNum);
        };
    }

    public void SelectStage(UIStageCell selectStage)
    {
        Debug.LogFormat("{0} selected", selectStage);
        this.nowStage._focusGo.SetActive(false);
        selectStage._focusGo.SetActive(true);
        Debug.LogFormat("targetStageCell : {0}", selectStage);
        this.uiStagePlayer.MoveOn(selectStage._playerPosGo.transform);
        this.nowStage = selectStage;
    }

    private void StageBuild(int stageAmount, RectTransform rectTrans)
    {
        var stageIconAtlas = AtlasManager.instance.GetAtlasByName("stageIcon");
        for (int i = 0; i < stageAmount; i++)
        {
            //Debug.LogFormat("<color=yellow>{0} build start</color>", i);
            var stageData = DataManager.instance.GetStageData(8000 + i);
            UIStageCellBuilder builder = new UIStageCellBuilder();
            builder.SetRectTrans(rectTrans);
            builder.AddFocus(this.spriteFocus);

            builder.BuildButton();
            builder.SetName(string.Format("Stage{0}", stageData.num));
            builder.SetNum(stageData.num);
            builder.SetPosition(stageData.ui_position[0], stageData.ui_position[1]);
            builder.SetSprite(stageIconAtlas.GetSprite(stageData.ui_sprite_name));

            //나중에 에셋 확정되면 다시 설정
            //builder.SetSize(stageData.ui_size[0], stageData.ui_size[1]);
            builder.SetSize(200, 200);

            builder.AddLock(this.spriteLock);
            builder.SetPlayerPos();
            this.uiStageCells[i] = builder.Build();

            Debug.LogFormat("<color=yellow>Stage{0} build done</color>", i);
        }
    }
}
