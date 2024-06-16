using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellingState : IState
{
    private Player player;
    public FellingState(Player player)
    {
        this.player = player;
        this.player.mono.OnAnimationComplete = () =>
        {
            this.player.mono.PlayAnimation(Enums.ePlayerState.TwoHandEquipe_Idle);
        };
    }
    public void DoAction()
    {
        this.player.mono.PlayAnimation(Enums.ePlayerState.Felling);
        //¹ú¸ñ
        this.player.mono.StartCoroutine("FellingAnim");
    }
    public void SwitchState(IState state)
    {
        this.player.State = state;
    }
}
