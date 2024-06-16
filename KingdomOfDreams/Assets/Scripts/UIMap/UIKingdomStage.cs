using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKingdomStage : MonoBehaviour
{
    public Button btnBack;
    public System.Action onClickBack;

    void Start()
    {
        this.btnBack.onClick.AddListener(() =>
        {
            Debug.Log("UIKingdom btnBack Clicked");
            this.onClickBack();
        });
    }


}
