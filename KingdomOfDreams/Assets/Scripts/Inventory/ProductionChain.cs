using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionChain : MonoBehaviour
{
    public Image imgLock;
    public GameObject imgItems;

    public void Start()
    {
        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.SHOW_PRODUCTION_CHAIN, ShowProductionChain);
    }
    private void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.SHOW_PRODUCTION_CHAIN, ShowProductionChain);
    }

    private void ShowProductionChain(short type)
    {
        this.imgLock.gameObject.SetActive(false);
        this.imgItems.SetActive(true);
    }
}
