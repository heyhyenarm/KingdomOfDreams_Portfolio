using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBookScrollView : MonoBehaviour
{
    public Transform contentTrans;
    public GameObject bookCellGo;
    public GameObject bookNormalCellGo;
    public GameObject bookRareCellGo;

    public void Init()
    {
        List<BookData> list = DataManager.instance.GetBookDatas();

        foreach (BookData data in list)
        {
            switch (data.reward_count)
            {
                case 30:
                    this.AddBookCell(data);
                    break;

                case 5:
                    this.AddBookNormalCell(data);
                    break;

                case 15:
                    this.AddBookRareCell(data);
                    break;

                case 600:
                    this.AddBookRareCell(data);
                    break;

            }
        }

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.REFRESH_UI_BOOK, new EventHandler((type) =>
        {
            this.Refresh();
        }));
    }

    public void AddBookCell(BookData data)
    {
        var go = Instantiate(this.bookCellGo, this.contentTrans);
        UIBookCell bookCell = go.GetComponent<UIBookCell>();
        bookCell.Init(data);
    }

    public void AddBookNormalCell(BookData data)
    {
        var go = Instantiate(this.bookNormalCellGo, this.contentTrans);
        UIBookCellNormal bookNormalCell = go.GetComponent<UIBookCellNormal>();
        bookNormalCell.Init(data);
    }

    public void AddBookRareCell(BookData data)
    {
        var go = Instantiate(this.bookRareCellGo, this.contentTrans);
        UIBookCellRare bookRareCell = go.GetComponent<UIBookCellRare>();
        bookRareCell.Init(data);
    }

    public void Refresh()
    {   
        if(this.contentTrans != null)
        {
            foreach (Transform child in this.contentTrans)
            {
                Destroy(child.gameObject);
            }

            List<BookData> list = DataManager.instance.GetBookDatas();

            foreach (BookData data in list)
            {
                switch (data.reward_count)
                {
                    case 30:
                        this.AddBookCell(data);
                        break;

                    case 5:
                        this.AddBookNormalCell(data);
                        break;

                    case 15:
                        this.AddBookRareCell(data);
                        break;

                    case 600:
                        this.AddBookRareCell(data);
                        break;

                }
            }

        }

    }
}
