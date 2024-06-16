using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NHRCharaterChangeMain : MonoBehaviour
{
    public NHRUICharacterChangeDirector uiDirector;
    public NHRCharacterPlayer player;
    public Camera camPlayer;

    private Renderer playerRen;

    void Start()
    {
        this.camPlayer.gameObject.SetActive(false);
        this.playerRen = this.player.nowCharacter.GetComponent<Renderer>();
        //this.uiDirector.colors[0].selected.SetActive(true);
        this.uiDirector.nowColor = this.uiDirector.colors[0];

        this.uiDirector.onClickChange = () =>
        {
            Debug.Log("onClickChange clicked");
            this.uiDirector.savedColor = this.uiDirector.nowColor;
            this.player.savedCharacter = this.player.nowCharacter;
            //this.uiDirector.savedSlot.check.SetActive(true);

            this.camPlayer.gameObject.SetActive(true);
        };
        this.uiDirector.onClickChangeClose = () =>
        {
            Debug.Log("onClickChangeClose clicked");
            Debug.LogFormat("nowColor:{0}, selectedColor:{1}", this.uiDirector.nowColor, this.uiDirector.savedColor);
            this.playerRen.material = this.uiDirector.savedColor.mat;
            Debug.LogFormat("character:{0}, savedCharacter:{1}", this.player.nowCharacter, this.player.savedCharacter);
            this.player.nowCharacter.SetActive(false);
            this.player.savedCharacter.SetActive(true);
            this.player.nowCharacter = this.player.savedCharacter;

            this.uiDirector.nowColor = this.uiDirector.savedColor;

            foreach (CharacterSlot slot in this.uiDirector.slots)
            {
                slot.check.SetActive(false);
            }
            foreach (CharacterColor color in this.uiDirector.colors)
            {
                color.selected.SetActive(false);
            }
            //this.uiDirector.nowSlot = this.uiDirector.savedSlot;
            //this.uiDirector.preSlot = this.uiDirector.savedSlot;
            this.uiDirector.savedSlot.check.SetActive(true);
            this.uiDirector.savedColor.selected.SetActive(true);

            this.camPlayer.gameObject.SetActive(false);
        };

        foreach (CharacterColor color in this.uiDirector.colors)
        {
            color.btnColor.onClick.AddListener(() =>
            {
                Debug.Log(color.name);
                if (this.uiDirector.nowColor != color)
                {
                    if (this.uiDirector.nowColor != null) this.uiDirector.nowColor.selected.SetActive(false);
                    this.uiDirector.nowColor = color;
                    color.selected.SetActive(true);
                    this.playerRen.material = color.mat;
                }

            });
        }
        this.uiDirector.onClickSlot = () =>
        {
            Debug.Log("onClickSlot");
            this.uiDirector.nowColor.selected.SetActive(false);
            this.playerRen = this.player.nowCharacter.GetComponent<Renderer>();

            this.uiDirector.nowColor = this.uiDirector.colors[0];
            this.uiDirector.colors[0].selected.SetActive(true);
            this.playerRen.material = this.uiDirector.colors[0].mat;
        };

    }

}
