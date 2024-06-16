using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class FairyShopDirector : MonoBehaviour
{
    public UIDream dream;
    public TMP_Text txtTitle;
    public Image imgTitle;
    public Button btnUp;
    public Button btnDown;
    public CharacterSlot[] characterSlots;
    public CharacterColor[] colors;
    public ChracterPurchasePopup popup;

    private float time = 0;
    public CharacterSlot selectedSlot;

    private void Start()
    {
        this.dream.Init();
        this.txtTitle.alpha = 0;
        this.imgTitle.color = new Color(1, 1, 1, 0);
        this.popup.gameObject.SetActive(false);
        StartCoroutine(this.CTitle());

        foreach (var slot in this.characterSlots)
        {
            slot.btnDream.onClick.AddListener(() =>
            {
                Debug.LogFormat("{0} selected", slot.name);
                //this.selectedSlot.check.SetActive(false);
                this.popup.gameObject.SetActive(true);
                this.popup.txtPrice.text = slot.price.ToString();
                this.selectedSlot = slot;
                foreach (var color in this.colors)
                {
                    color.selected.SetActive(false);
                    this.colors[0].selected.SetActive(true);
                }
            });
        }

    }
    public void FocusInitFalseChar()
    {
        for(int i = 0; i < this.characterSlots.Length; i++)
        {
            this.characterSlots[i].check.SetActive(false);
        }
    }
    public void FocusInitFalseColor()
    {
        for(int i = 0; i < this.colors.Length; i++)
        {
            this.colors[i].selected.SetActive(false);
        }
    }

    private IEnumerator CTitle()
    {
        Debug.Log("<color=green>CTitle</color>");
        while (true)
        {
            if (this.txtTitle.alpha == 1) break;
            var alphaTxt = Mathf.Lerp(0f, 1f, this.time);
            var alphaImg = Mathf.Lerp(0f, 0.2f, this.time);
            this.txtTitle.alpha = alphaTxt;
            this.imgTitle.color = new Color(1, 1, 1, alphaImg);
            this.time += Time.deltaTime;

            yield return null;
        }

        yield return YieldCache.WaitForSeconds(1f);
        this.time = 0;

        while (true) 
        {
            if (this.txtTitle.alpha == 0) break;
            var alphaTxt = Mathf.Lerp(1f, 0f, this.time);
            var alphaImg = Mathf.Lerp(0.2f, 0f, this.time);
            this.txtTitle.alpha = alphaTxt;
            this.imgTitle.color = new Color(1, 1, 1, alphaImg);
            this.time += Time.deltaTime;

            yield return null;
        }
    }

}
