using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class FairyShopMain : MonoBehaviour
{
    public Enums.eSceneType prevScene;
    public FairyShopDirector director;
    public UIStage06Director director06;
    public FairyShopPlayer player;
    public Button btnExit;
    public bool isIn;

    private int nowCharacterId;
    private Material nowMaterial;
    private int nowMatNum;
    private int[] myCharacters = new int[8];

    private void Awake()
    {
        var go = GameObject.Find("UIStage06Director");
        go.SetActive(false);
        this.director06 = go.GetComponent<UIStage06Director>();
        this.director06.map.uiStage.gameObject.SetActive(false);
        this.Init();
    }

    public void Init()
    {
        Debug.Log("FairyShopMain Init");
        this.isIn = true;

        var info = InfoManager.instance.PlayerInfo;
        this.nowCharacterId = info.nowCharacterId;
        for(int i = 0; i < this.player.characters.Length; i++)
        {
            //Debug.Log(i);
            if (this.nowCharacterId == i)
            {
                Debug.LogFormat("same numColor {0}", i);
                this.player.characters[i].SetActive(true);
                this.player.nowCharacter = this.player.characters[i];
                //this.director.characterSlots[i].check.SetActive(true);
                this.director.characterSlots[i].isSelected = true;
                this.director.selectedSlot = this.director.characterSlots[i];
            }
            else this.player.characters[i].SetActive(false);
        }
        var txtMat = string.Format("Materials/{0}", info.nowMatNum);
        this.nowMaterial = Resources.Load<Material>(txtMat);
        //색 포커스
        for (int i = 1; i < this.director.colors.Length; i++) this.director.colors[i].selected.SetActive(false);
        //this.director.colors[0].selected.SetActive(true);
        Debug.LogFormat("matNum:{0}", info.nowMatNum);

        for (int i = 0; i < info.myCharacters.Length; i++)
        {
            //Debug.Log(i);
            myCharacters[i] = info.myCharacters[i];
            Debug.Log(myCharacters[i]);
            if (myCharacters[i] == 1)
            {
                Debug.LogFormat("lock set false {0}", i);
                //this.director.characterSlots[i].lockGo.SetActive(false);
                this.director.characterSlots[i].isAdded = true;
            }
        }
    }
    void Start()
    {
        this.btnExit.onClick.AddListener(() =>
        {
            Debug.Log("btnExit clicked");
            this.director06.gameObject.SetActive(true);
            int[] arr = new int[2];
            arr[0] = this.nowCharacterId;
            arr[1] = this.nowMatNum;
            EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.EXIT_FAIRYSHOP);
            EventDispatcher.instance.SendEvent<int[]>((int)LHMEventType.eEventType.CHARACTER_MATERIAL_CHANGE, arr);

            this.isIn = false;
        });

        for(int i=0;i< this.director.characterSlots.Length; i++)
        {
            var slot = this.director.characterSlots[i];
            //슬롯 클릭
            slot.btnSlot.onClick.AddListener(() =>
            {
                Debug.LogFormat("{0} selected", slot.name);
                //this.director.selectedSlot.check.SetActive(false);
                this.director.FocusInitFalseChar();
                this.director.selectedSlot = slot;
                slot.check.SetActive(true);
                this.player.nowCharacter.SetActive(false);
                this.player.characters[slot.number].SetActive(true);
                this.player.nowCharacter = this.player.characters[slot.number];
                this.nowCharacterId = slot.number;
                InfoManager.instance.PlayerInfo.nowCharacterId = slot.number;
                InfoManager.instance.SavePlayerInfo();
            });
        }
        for (int i = 0; i < this.director.colors.Length; i++)
        {
            int numColor = i;
            //색상 선택
            this.director.colors[numColor].btnColor.onClick.AddListener(() =>
            {
                Debug.Log(numColor);
                Debug.LogFormat("{0} selected", this.director.colors[numColor]);
                this.director.FocusInitFalseColor();
                this.director.colors[numColor].selected.SetActive(true);
                this.nowMatNum = numColor;
                this.nowMaterial = this.director.colors[numColor].mat;
                this.player.nowCharacter.GetComponent<Renderer>().material = this.nowMaterial;
                InfoManager.instance.PlayerInfo.nowMatNum = numColor;
                InfoManager.instance.SavePlayerInfo();
                //EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.MATERIAL_TO_MAIN, numColor);
            });
        }
        //EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.MATERIAL_TO_MAIN, new EventHandler<int>((type, numColor) =>
        //{
        //    Debug.Log("MATERIAL_TO_MAIN");
        //    string matName = string.Format("Mateials/ChangeColor0{0}", numColor);
        //    this.player.GetComponent<Renderer>().material = Resources.Load<Material>(matName);
        //    InfoManager.instance.PlayerInfo.nowMatNum = this.player.GetComponent<Renderer>().material.ToString();
        //    InfoManager.instance.SavePlayerInfo();
        //}));
        //잠긴 캐릭터 구매
        this.director.popup.btnPurchase.onClick.AddListener(() =>
        {
            Debug.LogFormat("{0} 구매 완료", this.director.selectedSlot.name);
            this.UseDream(this.director.selectedSlot.price);
            this.director.selectedSlot.btnDream.gameObject.SetActive(false);
            this.director.selectedSlot.lockGo.SetActive(false);
            this.director.popup.gameObject.SetActive(false);
            this.myCharacters[this.director.selectedSlot.number] = 1;
            InfoManager.instance.PlayerInfo.myCharacters[this.director.selectedSlot.number] = 1;
            InfoManager.instance.SavePlayerInfo();
        });
    }

    private void UseDream(int amount)
    {
        //꿈으로 구매
        InfoManager.instance.DreamAcount(-amount);
        StartCoroutine(this.director.dream.CGetDream(-amount));

        //꿈 업뎃
        this.director.dream.Init();
    }

}
