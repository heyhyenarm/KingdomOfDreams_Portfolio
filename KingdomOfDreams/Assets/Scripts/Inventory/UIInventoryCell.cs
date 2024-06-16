using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryCell : MonoBehaviour
{
    public Image imgIcon;
    public Text txtName;
    public Text txtAmount;
    public Text txtAuto;

    public void Init(int id, string name, Sprite sprite, int amount)
    {
        var data = InfoManager.instance.IngredientInfos.Find(x => x.id == id);

        this.imgIcon.sprite = sprite;
        this.txtName.text = string.Format("{0}", name);
        this.txtAmount.text = string.Format("{0}", amount);
        if(data.id == 2005 || data.id == 2006 || data.id == 2007)
        {
            this.txtAuto.text = string.Format(" ");
        }
        else
        {
            this.txtAuto.text = string.Format("{0}°³/5ºÐ", data.auto);
        }

    }
}
