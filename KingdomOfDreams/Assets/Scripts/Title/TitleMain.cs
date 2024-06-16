using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMain : MonoBehaviour
{
    public UITitleDirector uiDirector;
    private void OnEnable()
    {
        Debug.Log("[TitleMain] onEnable");
    }
    private void OnDisable()
    {
        Debug.Log("[TitleMain] onDisable");
    }
    private void Awake()
    {
        Debug.Log("[TitleMain] Awake");
    }
    private void Start()
    {
        Debug.Log("[TitleMain] Start");
    }

    public void Init()
    {
        
        Debug.Log("[TitleMain] Init");
        //this.uiDirector.Init();
        //this.uiDirector.FadeIn();
    }
}
