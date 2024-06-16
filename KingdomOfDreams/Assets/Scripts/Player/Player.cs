using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public IState State { get; set; }

    public PlayerMono mono;

    //»ý¼ºÀÚ 
    public Player(GameObject prefab)
    {
        var go = GameObject.Instantiate(prefab);
        this.mono = go.GetComponent<PlayerMono>();
    }
    public void DoAction()
    {
        State.DoAction();
    }
    public void SwitchState(IState state)
    {
        State.SwitchState(state);
    }
}
