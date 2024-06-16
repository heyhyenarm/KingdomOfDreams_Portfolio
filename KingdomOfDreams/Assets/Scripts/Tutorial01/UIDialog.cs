using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialog : MonoBehaviour
{
    public Text dialog;

    //������ ���̺� ����
    public void UpdateDialog(string str)
    {
        this.dialog.text = str;
        this.gameObject.SetActive(true);
        StartCoroutine(WaitTime());
    }
    //�ð� ��ٸ��� ��ȭâ �����
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(3f);

        this.gameObject.SetActive(false);
    }
}
