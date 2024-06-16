using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NHRCharacterSlot : MonoBehaviour
{
    public int number;
    public Button btnSlot;
    public Button btnDream;
    public GameObject lockGo;
    public GameObject check;
    public bool isAdded = false;

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
        }

        this.btnDream.onClick.AddListener(() =>
        {
            Debug.Log("btnDream clicked");
            this.HaveCharacter();
            this.addCharacter();
        });
    }

    public void HaveCharacter()
    {
        this.lockGo.SetActive(false);
        this.btnDream.gameObject.SetActive(false);
    }
}
