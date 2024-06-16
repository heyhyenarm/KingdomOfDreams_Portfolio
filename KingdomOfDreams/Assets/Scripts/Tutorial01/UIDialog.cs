using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialog : MonoBehaviour
{
    public Text dialog;

    //데이터 테이블 연동
    public void UpdateDialog(string str)
    {
        this.dialog.text = str;
        this.gameObject.SetActive(true);
        StartCoroutine(WaitTime());
    }
    //시간 기다리면 대화창 사라짐
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(3f);

        this.gameObject.SetActive(false);
    }
}
