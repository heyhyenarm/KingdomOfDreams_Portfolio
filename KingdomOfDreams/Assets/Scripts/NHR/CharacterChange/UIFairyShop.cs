using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFairyShop : MonoBehaviour
{
    public NHRCharacterPlayer player;
    //public NHRUICharacterPlayer uiPlayer;
    public Button btnCharaterChange;
    public Button btnClose;
    public Button btnSelect;
    public GameObject uiCharacterChange;

    //나중에 데이터로 바꾸기
    public List<CharacterSlot> slots;
    public CharacterSlot nowSlot;
    public CharacterSlot preSlot;
    public CharacterSlot savedSlot;

    //color
    public List<CharacterColor> colors;
    public CharacterColor nowColor;
    public CharacterColor preColor;
    public CharacterColor savedColor;

    public System.Action onClickChange;
    public System.Action onClickChangeClose;
    public System.Action onClickSlot;

    void Start()
    {
        this.uiCharacterChange.SetActive(false);
        foreach (CharacterSlot slot in this.slots)
        {
            slot.check.SetActive(false);
        }
        this.savedSlot.check.SetActive(true);
        this.savedColor.selected.SetActive(true);
        //this.selectedColor = nowColor;
        //this.player.savedCharacter = this.player.character;

        this.btnCharaterChange.onClick.AddListener(() =>
        {
            Debug.Log("btnCharaterChange clicked");
            this.uiCharacterChange.SetActive(true);
            this.onClickChange();
        });
        this.btnClose.onClick.AddListener(() =>
        {
            Debug.Log("btnClose clicked");
            this.uiCharacterChange.SetActive(false);

            this.onClickChangeClose();
        });

        foreach(CharacterSlot slot in this.slots)
        {
            slot.btnSlot.onClick.AddListener(() =>
            {
                if (this.savedSlot != slot)
                {
                    this.savedSlot.check.SetActive(false);
                }

                if (this.nowSlot != slot)
                {
                    if (this.nowSlot != null) this.nowSlot.check.SetActive(false);
                    Debug.Log(slot.name);
                    this.nowSlot = slot;
                    slot.check.SetActive(true);

                    this.preSlot = this.nowSlot;

                    this.player.preCharacter = this.player.nowCharacter;
                    this.player.characters[slot.number].SetActive(true);
                    this.player.nowCharacter = this.player.characters[slot.number];
                    this.player.preCharacter.SetActive(false);
                    this.onClickSlot();
                }
            });
            slot.addCharacter = () =>
            {
                Debug.Log("add character director");
                Debug.Log(slot.number);
                this.player.myCharacters.Add(this.player.characters[slot.number]);
                slot.lockGo.SetActive(false);

            };
        }
        this.btnSelect.onClick.AddListener(() =>
        {
            Debug.Log("btnSelect");
            this.savedColor = nowColor;
            this.savedSlot = nowSlot;
            this.player.savedCharacter = this.player.nowCharacter;
        });
    }

}
