using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FairyDirector : MonoBehaviour
{
    public Button btnFairy;

    void Start()
    {
        this.btnFairy.onClick.AddListener(() =>
        {
            Debug.Log("btnFairy clicked");
        });
    }
}
