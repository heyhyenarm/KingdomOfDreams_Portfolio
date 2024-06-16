using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoExplain : MonoBehaviour
{
    public Button btnClose;
    void Init()
    {
        this.btnClose.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);

        });
    }

}
