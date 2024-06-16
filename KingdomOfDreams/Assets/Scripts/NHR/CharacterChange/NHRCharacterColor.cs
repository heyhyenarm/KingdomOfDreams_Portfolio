using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NHRCharacterColor : MonoBehaviour
{
    public Button btnColor;
    public GameObject selected;
    public Material mat;

    void Start()
    {
        this.btnColor = GetComponent<Button>();
        this.selected.SetActive(false);

    }

}
