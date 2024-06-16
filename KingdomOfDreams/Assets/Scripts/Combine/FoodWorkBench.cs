using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 
    FoodWorkBench : MonoBehaviour
{
    public Text txtItem1;
    public Text txtItem2;
    public Text txtWant;
    public Button btnPlus;
    public Button btnMinus;
    public Button btnMax;
    public Button btnMake;
    public Button btnClose;

    private int amountItem1;
    private int amountItem2;
    private int numItem1;
    private int numItem2;
    private int numWant = 0;
    void Start()
    {
        //텍스트 초기화
        this.amountItem1 = InfoManager.instance.IngredientInfos.Find(x => x.id == 2001).amount;
        this.amountItem2 = InfoManager.instance.IngredientInfos.Find(x => x.id == 2002).amount;

        this.txtItem1.text = string.Format("{0} / {1}", numItem1, amountItem1);
        this.txtItem2.text = string.Format("{0} / {1}", numItem2, amountItem2);

        this.txtWant.text = numWant.ToString();

        //버튼 초기화
        this.btnClose.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });

        this.btnPlus.onClick.AddListener(() =>
        {
            this.numWant += 1;
            this.Calcuate();
        });

        this.btnMinus.onClick.AddListener(() =>
        {
            if (numWant > 0)
            {
                this.numWant -= 1;
                this.Calcuate();
            }
        });

        this.btnMake.onClick.AddListener(() =>
        {
            if (numWant <= this.amountItem1 && numWant <= this.amountItem2/2)
            {
                this.Make(numWant);
                this.Calcuate();
                EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.REFRESH_UI_INVENTORY);
            }
            else
            {
                Debug.Log("재료가 부족함");
            }
        });

        this.btnMax.onClick.AddListener(() =>
        {
            this.MaxCalculate();
            this.Calcuate();
        });
    }
    public void Init()
    {
        Debug.Log("<color=blue>식료품점 조합대 Init</color>");
    }
    private void Make(int num)
    {
        for (int i = 0; i < num; i++)
        {
            //아이템 얻기
            EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.COMBINE_INGREDIENT, 2005);
        }

        //아이템 차감
        InfoManager.instance.IngredientInfos.Find(x => x.id == 2001).amount -= numItem1;
        InfoManager.instance.IngredientInfos.Find(x => x.id == 2002).amount -= numItem2;
        //수 초기화
        this.numWant = 0;
    }
    private void Calcuate()
    {
        this.numItem1 = 1 * numWant;
        this.numItem2 = 2 * numWant;

        this.amountItem1 = InfoManager.instance.IngredientInfos.Find(x => x.id == 2001).amount;
        this.amountItem2 = InfoManager.instance.IngredientInfos.Find(x => x.id == 2002).amount;

        this.txtItem1.text = string.Format("{0} / {1}", numItem1, amountItem1);
        this.txtItem2.text = string.Format("{0} / {1}", numItem2, amountItem2);
        this.txtWant.text = numWant.ToString();
    }
    private void MaxCalculate()
    {
        var max = Mathf.Min(amountItem1, amountItem2/2);
        this.numWant = max;
    }
}
