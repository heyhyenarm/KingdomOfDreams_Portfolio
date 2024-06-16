using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imgExclaimIcon : MonoBehaviour
{
    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler<int>((int)LHMEventType.eEventType.EXCLAIM_ICON_BOOK_ITEM, onExclaimEvent);
    }
    private void Start()
    {

    }
    public void Init()
    {
        Debug.LogFormat("<color=yellow>´À³¦Ç¥ Init</color>");
        EventDispatcher.instance.AddEventHandler<int>((int)LHMEventType.eEventType.EXCLAIM_ICON_BOOK_ITEM, onExclaimEvent);

    }

    private void onExclaimEvent(short type, int a)
    {
        if (a == 0)
        {
            this.gameObject.SetActive(false);

        }
        else if (a == 1)
        {
            this.gameObject.SetActive(true);

        }
    }
}
