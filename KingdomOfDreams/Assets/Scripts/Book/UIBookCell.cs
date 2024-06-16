using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBookCell : MonoBehaviour
{
    public Image imgRare;
    public Image imgNormal;
    public Button btnClaim;
    public Image deem1;
    public Image deem2;
    public Image deem3;
    public Image deem4;
    public Image imgThema;

    private int stageNum;
    private bool isClaim = false;

    void Start()
    {
        this.btnClaim.interactable = isClaim;

        this.btnClaim.onClick.AddListener(() =>
        {
            Debug.LogFormat("꿈 {0}개 획득", DataManager.instance.GetBookDatas().Find(x => x.id == 5000).reward_count);

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
                    mainGo5.GetDream(30);
                    break;
                case 5:
                    var mainGo6 = GameObject.FindObjectOfType<Stage06Main>();
                    mainGo6.GetDream(30);
                    break;
                case 6:
                    var mainGo7 = GameObject.FindObjectOfType<Stage07Main>();
                    mainGo7.GetDream(30);
                    break;
                case 7:
                    var mainGo8 = GameObject.FindObjectOfType<Stage08Main>();
                    mainGo8.GetDream(30);
                    break;
            }

            var claimData = InfoManager.instance.BookInfos.Find(x => x.id == 5000);
            claimData.claim = 1;

            InfoManager.instance.SaveBookInfo();

            this.isClaim = false;
            this.btnClaim.interactable = isClaim;

            EventDispatcher.instance.SendEvent<string>((int)LHMEventType.eEventType.CLAIM_BOOK_ITEM, "꿈 30개");
        });

    }
    public void Init(BookData data)
    {
        var atlas = AtlasManager.instance.GetAtlasByName("book");

        BookItemData rareItem = DataManager.instance.dicBookItemData[data.rare1_id];
        BookItemData normalItem = DataManager.instance.dicBookItemData[data.normal1_id];

        this.imgRare.sprite = atlas.GetSprite(rareItem.sprite_name);
        this.imgNormal.sprite = atlas.GetSprite(normalItem.sprite_name);
        this.imgThema.sprite = atlas.GetSprite("tutorial");

        for (int i = 0; i < InfoManager.instance.BookItemInfos.Count; i++)
        {
            var info = InfoManager.instance.BookItemInfos[i];
            if (info.id == data.normal1_id && info.exist == 1)
            {
                this.deem2.gameObject.SetActive(false);
            }

            if (info.id == data.rare1_id && info.exist == 1)
            {
                this.deem1.gameObject.SetActive(false);
                this.deem3.gameObject.SetActive(false);
                this.deem4.gameObject.SetActive(false);

                if (InfoManager.instance.BookInfos.Find(x => x.id == 5000).claim != 1)
                {
                    this.isClaim = true;
                    this.btnClaim.interactable = isClaim;
                }

            }

        }

    }

}
