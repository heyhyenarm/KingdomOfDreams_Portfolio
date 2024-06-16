using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    public UIDialogue dialogue;

    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler<int>((int)LHMEventType.eEventType.GET_BOOK_ITEM, this.GetBookItemEventHandler);

    }

    public void Init()
    {
        EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.GET_BOOK_ITEM, this.GetBookItemEventHandler);

    }
    private void GetBookItemEventHandler(short type, int a)
    {
        this.GetBookItem(a);

        if(a == 6001)
        {
            this.FindDialogue();

            var data = DataManager.instance.GetDialogueData(10096);
            InfoManager.instance.DialogueInfo.id = 10096;
            this.dialogue.gameObject.SetActive(true);
            this.dialogue.Init(data);
        }
    }

    public void GetBookItem(int num)
    {
        var data = DataManager.instance.GetBookItemData(num);

        Debug.Log("도감 아이템 |" + data.name + "| 획득");

        var id = data.id;
        var foundInfo = InfoManager.instance.BookItemInfos.Find(x => x.id == id);

        if (foundInfo == null)
        {
            BookItemInfo info = new BookItemInfo(id, 1);
            InfoManager.instance.BookItemInfos.Add(info);

            InfoManager.instance.SaveBookItemInfo();

            //BookUI refresh 이벤트 호출
            EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.REFRESH_UI_BOOK);
            //느낌표 표시 이벤트 호출
            EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.EXCLAIM_ICON_BOOK_ITEM, 1);

        }
        else
        {
            return;
        }

    }

    public void FindDialogue()
    {
        this.dialogue = GameObject.FindObjectOfType<UITutorial05Director>()
            .transform.GetChild(0).GetChild(19).gameObject.GetComponent<UIDialogue>();

        //this.dialogue = dialogue;
    }
}
