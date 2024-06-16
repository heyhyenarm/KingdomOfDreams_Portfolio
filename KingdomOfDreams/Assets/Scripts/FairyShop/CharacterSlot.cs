using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    public int number;
    public int price;
    public Button btnSlot;
    public Button btnDream;
    public GameObject lockGo;
    public GameObject check;
    public bool isAdded = false;
    public bool isSelected = false;

    public System.Action addCharacter;

    void Start()
    {
        this.check.SetActive(false);
        if (this.isAdded == false)
        {
            this.lockGo.SetActive(true);
            this.btnDream.gameObject.SetActive(true);
        }
        else
        {
            this.lockGo.SetActive(false);
            this.btnDream.gameObject.SetActive(false);
        }
        if (this.isSelected)
        {
            this.check.SetActive(true);
        }
    }
}
