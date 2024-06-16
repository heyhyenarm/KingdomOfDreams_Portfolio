using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDream : MonoBehaviour
{
    public TMP_Text txtDream;
    public TMP_Text txtGet;
    private int dreamAmount;

    public void Init()
    {
        Debug.Log("dream Init");
        this.dreamAmount = InfoManager.instance.GetDreamInfo();
        this.txtDream.text = this.dreamAmount.ToString();
    }
    public IEnumerator CGetDream(int amount)
    {
        Debug.LogFormat("<colot=yellow>Get Dream {0}", amount);
        this.txtGet.gameObject.SetActive(true);
        if(amount>0) this.txtGet.text = string.Format("+{0}", amount);
        else this.txtGet.text = string.Format("{0}", amount);

        yield return new WaitForSeconds(2f);
        this.txtGet.gameObject.SetActive(false);
    }

    void Start()
    {
        this.txtGet.gameObject.SetActive(false);
    }

}
