using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonONOFF : MonoBehaviour
{
    public Button btnONOFF;
    public GameObject On;
    public GameObject Off;
    public bool isOn = true;

    private void Start()
    {

    }
    public void BtnOn()
    {
        this.On.SetActive(true);
        this.Off.SetActive(false);
        Vibration.isOn = true;
        Debug.Log("진동 on");
        //Vibration.Vibrate();
    }
    public void BtnOff()
    {
        this.On.SetActive(false);
        this.Off.SetActive(true);
        Vibration.isOn = false;
        //Vibration.Vibrate();
        Debug.Log("진동 off");
    }
}
