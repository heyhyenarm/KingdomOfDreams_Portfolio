using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBook : MonoBehaviour
{
    public UIBookScrollView scrollview;

    public Button btnClose;

    public Image imgNotice;
    public Text txtNotice;
    public Button btnNoticeClose;
    //public imgExclaimIcon imgExclaim;

    void Start()
    {
        this.btnClose.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });

        this.btnNoticeClose.onClick.AddListener(() =>
        {
            this.imgNotice.gameObject.SetActive(false);
        });
    }
    private void OnDestroy()
    {
        // FarmRow ÆÄ±« ½Ã ÀÌº¥Æ® ÇÚµé·¯ Á¦°Å
        EventDispatcher.instance.RemoveEventHandler<string>((int)LHMEventType.eEventType.CLAIM_BOOK_ITEM, Notice);

    }

    private void AddEvent()
    {
        EventDispatcher.instance.AddEventHandler<string>((int)LHMEventType.eEventType.CLAIM_BOOK_ITEM, Notice);
    }
    public void Init()
    {   
        this.scrollview.Init();

        this.AddEvent();
        //this.imgExclaim.Init();
    }

    public void Notice(short type, string item)
    {
        this.imgNotice.gameObject.SetActive(true);
        this.txtNotice.text = string.Format("{0} È¹µæ!", item);
    }
}
