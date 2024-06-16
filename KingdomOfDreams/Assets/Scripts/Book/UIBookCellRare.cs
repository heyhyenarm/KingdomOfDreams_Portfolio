using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBookCellRare : MonoBehaviour
{
    public Image imgRare1;
    public Image imgRare2;
    public Button btnClaim;
    public Image deem1;
    public Image deem2;
    public Image deem3;
    public Image deem4;
    public Image imgThema;
    public Image imgReward;

    private bool isClaim = false;
    private BookData fieldData;
    private int stageNum;

    void Start()
    {
        this.btnClaim.interactable = isClaim;

        this.btnClaim.onClick.AddListener(() =>
        {
            Debug.LogFormat("보상 {0}개 받기", fieldData.reward_count);

            //북 인포 보상 획득으로 변경
            var claimData = InfoManager.instance.BookInfos.Find(x => x.id == fieldData.id);
            claimData.claim = 1;

            //자동공급 보상 지급
            var dataTable = DataManager.instance.GetBookDatas().Find(x => x.id == fieldData.id);
            if (dataTable.id == 5002 || dataTable.id == 5005 || dataTable.id == 5008 || dataTable.id == 5011 || dataTable.id == 5014)
            {
                var itemInfo = InfoManager.instance.IngredientInfos.Find(x => x.id == dataTable.reward_id);
                if(itemInfo.auto == 5)
                {
                    itemInfo.auto += dataTable.reward_count;
                }
                else if(itemInfo.auto == 1)
                {
                    itemInfo.auto = dataTable.reward_count;

                }
                InfoManager.instance.SaveIngredientInfos();
                EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.REFRESH_UI_INVENTORY);
                EventDispatcher.instance.SendEvent<string>((int)LHMEventType.eEventType.CLAIM_BOOK_ITEM, "자동공급 15배 증가");

            }
            else if(dataTable.id == 5003 || dataTable.id == 5006 || dataTable.id == 5009 || dataTable.id == 50012 || dataTable.id == 50015)
            {
                //현재 스테이지 받아오기
                foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
                {
                    Debug.LogFormat("<color>stageInfo.stage : {0}, stageInfo.isClear : {1}</color>", stageInfo.stage, stageInfo.isClear);
                    if (stageInfo.isClear == false)
                    {
                        //원하는곳에 stageInfo.num 저장
                        this.stageNum = stageInfo.stage;
                        break;
                    }
                }

                //꿈 보상 받기
                switch (this.stageNum)
                {
                    case 4:
                        var mainGo5 = GameObject.FindObjectOfType<Tutorial05Main>();
                        mainGo5.GetDream(600);
                        break;
                    case 5:
                        var mainGo6 = GameObject.FindObjectOfType<Stage06Main>();
                        mainGo6.GetDream(600);
                        break;
                    case 6:
                        var mainGo7 = GameObject.FindObjectOfType<Stage07Main>();
                        mainGo7.GetDream(600);
                        break;
                    case 7:
                        var mainGo8 = GameObject.FindObjectOfType<Stage08Main>();
                        mainGo8.GetDream(600);
                        break;
                }

                InfoManager.instance.SaveBookInfo();
                EventDispatcher.instance.SendEvent<string>((int)LHMEventType.eEventType.CLAIM_BOOK_ITEM, "꿈 600개");

            }

            InfoManager.instance.SaveBookInfo();

            this.isClaim = false;
            this.btnClaim.interactable = isClaim;


        });
    }

    public void Init(BookData data)
    {
        this.fieldData = data;

        var atlas = AtlasManager.instance.GetAtlasByName("book");

        BookItemData rareItem1 = DataManager.instance.dicBookItemData[data.rare1_id];
        BookItemData rareItem2 = DataManager.instance.dicBookItemData[data.rare2_id];

        this.imgRare1.sprite = atlas.GetSprite(rareItem1.sprite_name);
        this.imgRare2.sprite = atlas.GetSprite(rareItem2.sprite_name);

        //행동 UI
        switch (data.action)
        {
            case 2:
                this.imgThema.sprite = atlas.GetSprite("logging");
                this.imgReward.sprite = atlas.GetSprite("equip_shield_wood");
                break;
            case 3:
                this.imgThema.sprite = atlas.GetSprite("fishing");
                this.imgReward.sprite = atlas.GetSprite("equip_fish");
                break;
            case 4:
                this.imgThema.sprite = atlas.GetSprite("gathering");
                this.imgReward.sprite = atlas.GetSprite("cabbage");
                break;
            case 5:
                this.imgThema.sprite = atlas.GetSprite("mining");
                this.imgReward.sprite = atlas.GetSprite("equip_stone");
                break;
            case 6:
                this.imgThema.sprite = atlas.GetSprite("hunting");
                this.imgReward.sprite = atlas.GetSprite("icon_itemicon_meat");
                break;

        }

        //아이템 UI
        for (int i = 0; i < InfoManager.instance.BookItemInfos.Count; i++)
        {
            var info = InfoManager.instance.BookItemInfos[i];

            if (info.id == data.rare1_id && info.exist == 1)
            {
                this.deem1.gameObject.SetActive(false);
            }
            else if (info.id == data.rare2_id && info.exist == 1)
            {
                this.deem2.gameObject.SetActive(false);
            }
        }

        //보상, Claim버튼 UI
        var infoRare1 = InfoManager.instance.BookItemInfos.Find(x => x.id == data.rare1_id);
        var infoRare2 = InfoManager.instance.BookItemInfos.Find(x => x.id == data.rare2_id);
        if (infoRare1 != null && infoRare2 != null && infoRare1.exist == 1 && infoRare2.exist == 1)
        {
            this.deem3.gameObject.SetActive(false);
            this.deem4.gameObject.SetActive(false);

            var foundInfo = InfoManager.instance.BookInfos.Find(x => x.id == data.id);
            if (foundInfo == null)
            {
                var TbookInfo = new BookInfo(data.id);
                InfoManager.instance.BookInfos.Add(TbookInfo);
                InfoManager.instance.SaveBookInfo();
            }

            if (InfoManager.instance.BookInfos.Find(x => x.id == data.id).claim != 1)
            {
                this.isClaim = true;
                this.btnClaim.interactable = isClaim;
            }
        }
    }

}
