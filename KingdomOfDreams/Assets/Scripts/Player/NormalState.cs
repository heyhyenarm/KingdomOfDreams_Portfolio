using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : IState
{
    private Player player;

    public NormalState(Player player)
    {
        this.player = player;
    }

    public void DoAction()
    {
        //할일이 없다 
    }

    public void SwitchState(IState state)
    {
        player.State = state;
    }
}
