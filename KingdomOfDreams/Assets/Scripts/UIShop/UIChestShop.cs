using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIChestShop : MonoBehaviour
{
    private int chestAmount = 3;
    public GameObject contents;
    public Button btnClose;

    public UIChestShopCell[] chestCells;
    public UIChestShopCell purchaseChest;
    public UIChestPopup popup;
    public GameObject alert;
    public GameObject alertNoAd;
    public Button reusult;
    public TMP_Text[] txtPieces;

    //무료 상자
    public bool onFree = true;
    public bool onAD = true;
    //public bool onOff = false;
    private int freeChargeTime = 28800;
    public DateTime startTime;
    private int purchaseChestAmount;

    public Image imgNew;

    public string ChargeChestTime = "ChargeChestTime";

    //public GoogleAdMobController adController;

    private bool isBuild = false;

    //public void OnDestroy()
    //{
    //    EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.USE_DREAM, -this.purchaseChest.price);

    //}

    public void Init()
    {
        if(this.isBuild==false) this.chestCells = new UIChestShopCell[this.chestAmount];
        var contentsRect = this.contents.GetComponent<RectTransform>();
        this.ChestShopBuild(this.chestAmount, contentsRect);
        this.chestInit();
    }

    public void chestInit()
    {
        //rewardChestId=401
        Debug.Log("chestInit");
        //var ad = this.chestCells[0]._btnAD.GetComponent<Button>();
        //Debug.LogFormat("<color=yellow>chestCells[0]:{0}, btnAD:{1}, ad:{2}</color>", this.chestCells[0], this.chestCells[0]._btnAD.GetComponent<Button>(), ad);
        //ad.onClick.AddListener(LoadAd);

        for (int i = 0; i < this.chestCells.Length; i++)
        {
            ChestInfo chestInfo = InfoManager.instance.GetChestInfo(i + 100);
            this.chestCells[i].chestAmount = chestInfo.amount;
            var txt = this.chestCells[i]._txtAmount.GetComponent<Text>();
            //txt.text = chestInfo.amount.ToString();
            if (i == 0) txt.text = "8시간마다 무료";
            else txt.text = String.Format("{0}개 보유", chestInfo.amount);
            if (chestInfo.amount > 0) this.chestCells[i]._btnOpen.SetActive(true);
            else this.chestCells[i]._btnOpen.SetActive(false);
        }

        if (this.chestCells[0].chestAmount == 0)
        {
            this.onFree = false;
            this.imgNew.gameObject.SetActive(false);
            if (InfoManager.instance.ChargeTimeInfo.chestAdOn)
            {
                this.chestCells[0]._btnAD.SetActive(true);
                this.onAD = true;
                EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.AD_REWARD_LOAD);
            }
            else
            {
                this.chestCells[0]._btnAD.SetActive(false);
                this.onAD = false;
            }
            this.start = InfoManager.instance.GetChargeTime();
        }
    }
    private void Start()
    {
        this.popup.gameObject.SetActive(false);
        this.alert.SetActive(false);
        this.alertNoAd.SetActive(false);
        //this.chestInit();

        this.btnClose.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });
        this.reusult.onClick.AddListener(() =>
        {
            this.reusult.gameObject.SetActive(false);
        });
        //for (int i = 0; i < chestAmount; i++)
        //{
        //    this.chestCells[i]
        //}
        foreach(var cell in this.chestCells)
        {
            var btnOpen = cell._btnOpen.GetComponent<Button>();
            btnOpen.onClick.AddListener(() =>
            {
                this.purchaseChest = cell;
                this.popup.gameObject.SetActive(true);
                this.popup.txtChestName.text = cell._txtName.GetComponent<Text>().text;
                this.popup.txtPrice.text = "Open";
                this.popup.txtPrice.text = "open";
                this.popup.txtAmount.text = cell.pieceAmount.ToString();
                Debug.LogFormat("<color=yellow>price : {0}</color>", cell.price);
                this.chestInit();
            });

            if (cell.price == 0)
            {
                var btnAD = cell._btnAD.GetComponent<Button>();
                btnAD.onClick.AddListener(() =>
                {
                    this.purchaseChest = cell;
                    this.popup.gameObject.SetActive(true);
                    this.popup.txtChestName.text = cell._txtName.GetComponent<Text>().text;
                    this.popup.txtPrice.text = "AD";
                    this.popup.txtAmount.text = cell.pieceAmount.ToString();
                });
            }

            if (cell._btnDream != null)
            {
                var btnDream = cell._btnDream.GetComponent<Button>();
                btnDream.onClick.AddListener(() =>
                {
                    this.purchaseChest = cell;
                    this.popup.gameObject.SetActive(true);
                    this.popup.txtChestName.text = cell._txtName.GetComponent<Text>().text;
                    this.popup.txtPrice.text = cell.price.ToString();
                    this.popup.txtAmount.text = cell.pieceAmount.ToString();
                    Debug.LogFormat("<color=yellow>price : {0}</color>", cell.price);
                    this.chestInit();
                });

            }
        }

        this.popup.onPurchase = () =>
        {
            Debug.LogFormat("use dream {0}", this.purchaseChest.price);
            if(this.purchaseChest.price==0)
            {
                if (this.purchaseChest._btnOpen.activeSelf)
                {
                    this.purchaseChest._btnOpen.SetActive(false);
                    this.startTime = System.DateTime.Now;
                    InfoManager.instance.GetChestInfo(100).amount--;
                    InfoManager.instance.SaveChestInfos();
                    InfoManager.instance.RefreshChargeTimeInfo(System.DateTime.Now, this.onAD);
                    this.onFree = false;
                    this.start = System.DateTime.Now;
                    //StartCoroutine(this.CTimer(this.purchaseChest._txtTimer.GetComponent<Text>()));
                    this.RandomPiece(this.purchaseChest.pieceAmount);
                    this.reusult.gameObject.SetActive(true);
                }
                else if (this.purchaseChest._btnAD.activeSelf)
                {
                    Debug.Log("<color=red>광고 재생</color>");
                    this.purchaseChest._btnAD.SetActive(false);
                    //EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.AD_REWARD_PLAY);
                    this.onAD = false;
                    InfoManager.instance.ChargeTimeInfo.chestAdOn = false;
                    InfoManager.instance.SaveChargeTimeInfo();
                    StartCoroutine(this.ChestNoAdAlert());
                }
            }
            else
            {
                if(this.purchaseChest.chestAmount != 0)
                {
                    Debug.LogFormat("<color=red>상자 차감{0}</color>", this.purchaseChest._num);
                    var id = this.purchaseChest._num + 100;
                    this.purchaseChest.chestAmount--;
                    this.purchaseChest._txtAmount.GetComponent<Text>().text = String.Format("{0}개 보유", this.purchaseChest.chestAmount);
                    InfoManager.instance.GetChestInfo(id).amount--;
                    InfoManager.instance.SaveChestInfos();
                    Debug.LogFormat("<color=red>상자 info amount {0}</color>", InfoManager.instance.GetChestInfo(id).amount);

                    this.RandomPiece(this.purchaseChest.pieceAmount);
                    this.reusult.gameObject.SetActive(true);
                }
                else
                {
                    //드림 차감
                    var dream = InfoManager.instance.GetDreamInfo();
                    if (dream >= this.purchaseChest.price)
                    {
                        EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.USE_DREAM, this.purchaseChest.price);
                        this.RandomPiece(this.purchaseChest.pieceAmount);
                        this.reusult.gameObject.SetActive(true);
                    }
                    else StartCoroutine(this.ChestAlert());
                }
                this.chestInit();
            }
            this.popup.gameObject.SetActive(false);
        };
    }
    private IEnumerator ChestAlert()
    {
        this.alert.SetActive(true);

        yield return new WaitForSeconds(2f);

        this.alert.SetActive(false);

    }
    private IEnumerator ChestNoAdAlert()
    {
        this.alertNoAd.SetActive(true);

        yield return new WaitForSeconds(2f);

        this.alertNoAd.SetActive(false);

    }

    private void ChestShopBuild(int chestAmount, RectTransform rectTrans)
    {
        var chestIconAtlas = AtlasManager.instance.GetAtlasByName("chestIcon");
        Debug.Log("<color=blue>ChestShopBuild</color>");
        for (int i = 0; i < chestAmount; i++)
        {
            Debug.LogFormat("<color=yellow>{0} build start</color>", i);
            var chestData = DataManager.instance.GetChestData(100 + i);
            Debug.LogFormat("chestData.name : {0}, id : {1}, prefab_name : {2}, piece_count : {3}, type : {4}", 
                chestData.name, chestData.id, chestData.prefab_name, chestData.piece_count, chestData.type);

            ChestInfo chestInfo = InfoManager.instance.GetChestInfo(i + 100);

            UIChestShopCellBuilder builder = new UIChestShopCellBuilder();
            builder.SetRectTrans(rectTrans);
            var font = Resources.Load<Font>("Fonts/Healthset Bold");
            if (chestData.type == 0)
            {
                //무료상자라면
                builder.AddButtonTimer(chestIconAtlas.GetSprite("Btn_Rectangle02_Dark"), font);
                builder.AddButtonAd(chestIconAtlas.GetSprite("Btn_Rectangle02_Sky"), font);
            }
            else
            {
                //유료상자라면
                string txtDream = chestData.price.ToString();
                builder.BuildButtonPDream(chestIconAtlas.GetSprite("Btn_Rectangle02_Dark"), font, txtDream);
            }
            string txtOpen = "Open";

            builder.BuildButtonOpen(chestIconAtlas.GetSprite("Btn_Rectangle02_Green"), font, txtOpen);
            builder.SetNum(i);
            builder.SetPosition(0, 0);
            builder.SetSprite(chestIconAtlas.GetSprite("Frame_ListFrame01_White1"));
            builder.SetSize(515, 540);
            builder.SetSpriteChest(chestIconAtlas.GetSprite(chestData.prefab_name));
            builder.SetName(chestData.name);
            builder.SetText(font);

            if (i > 0)
            {
                var txtAmount = String.Format("{0}개 보유", chestInfo.amount);
                builder.SetTextAmount(font, txtAmount);
            }
            else builder.SetTextAmount(font, "8시간마다 무료");

            if (this.chestCells[i] != null) Destroy(this.chestCells[i].gameObject);
            this.chestCells[i] = builder.Build().GetComponent<UIChestShopCell>();

            this.chestCells[i].price = chestData.price;
            this.chestCells[i].pieceAmount = chestData.piece_count;
            this.chestCells[i].chestAmount = chestInfo.amount;

            Debug.LogFormat("<color=yellow>Chest{0} build done</color>", i);
            this.isBuild = true;
        }
    }
    public string RandomPiece(int pieceCount)
    {
        Debug.Log("RandomPiece");
        int[] piece = new int[4];
        for (int i = 0; i < pieceCount; i++)
        {
            var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;
            var rand = UnityEngine.Random.Range(0, 4);

            if (myLevel >= 1) //마법도구가 있을때
            {
            }

            if (myLevel == 0)  //마법도구가 없을때
            {
                rand = 0;
            }
            if (rand == 0)
            {
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 600);
                piece[0]++;
            }
            else if (rand == 1)
            {
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 601);
                piece[1]++;
            }
            else if (rand == 2)
            {
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 602);
                piece[2]++;

            }
            else
            {
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 603);
                piece[3]++;

            }
        }
        this.txtPieces[0].text = string.Format("x{0}", piece[0]);
        this.txtPieces[1].text = string.Format("x{0}", piece[1]);
        this.txtPieces[2].text = string.Format("x{0}", piece[2]);
        this.txtPieces[3].text = string.Format("x{0}", piece[3]);
        return string.Format("<color=red>piece1 : {0}, piece2 : {1}, piece3 : {2}, piece4 : {3}</color>", piece[0], piece[1], piece[2], piece[3]);
    }
    DateTime start; 

    private void Update()
    {
        if (this.onFree == false)
        {
            this.imgNew.gameObject.SetActive(false);
            TimeSpan timeSpan;
            int timeseconds;

            timeSpan = DateTime.Now - start;
            timeseconds = (int)timeSpan.TotalSeconds;
            var txt = this.chestCells[0]._txtTimer.GetComponent<Text>();

            var time = this.freeChargeTime - timeseconds;
            int hour = time / 3600;
            int min = (time % 3600) / 60;
            int sec = (time % 3600) % 60;

            txt.text = string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
            //Debug.LogFormat("<color>time{0}, timeseconds : {1}, info time : {2}, start : {3}</color>", time, timeseconds, InfoManager.instance.GetChargeTime(), this.start);
            if (time <= 0)
            {
                this.chestCells[0]._btnOpen.SetActive(true);
                this.chestCells[0]._btnAD.SetActive(true);
                InfoManager.instance.GetChestInfo(100).amount++;
                InfoManager.instance.SaveChestInfos();
                InfoManager.instance.ChargeTimeInfo.chestAdOn = true;
                InfoManager.instance.SaveChargeTimeInfo();
                this.onAD = true;
                this.onFree = true;
            }

        }
        else this.imgNew.gameObject.SetActive(true);

    }
    //private IEnumerator CTimer(Text txt)
    //{
    //    TimeSpan timeSpan;
    //    int timeseconds;
    //    DateTime start = InfoManager.instance.GetChargeTime();
    //    timeSpan = DateTime.Now - start;
    //    timeseconds = timeSpan.Seconds;

    //    while (this.onFree==false)
    //    {
    //        timeSpan = DateTime.Now - start;
    //        timeseconds = timeSpan.Seconds;
    //        var time = this.freeChargeTime - timeseconds;
    //        int hour = time / 3600;
    //        int min = (time % 3600) / 60;
    //        int sec = (time % 3600) % 60;

    //        txt.text = string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
    //        if (txt.text == "00:00:00")
    //        {
    //            this.chestCells[0]._btnOpen.SetActive(true);
    //            this.chestCells[0]._btnAD.SetActive(true);
    //            InfoManager.instance.GetChestInfo(100).amount++;
    //            InfoManager.instance.SaveChestInfos();
    //            this.onFree = true;
    //        }

    //        yield return null;
    //    }
    //}
}
