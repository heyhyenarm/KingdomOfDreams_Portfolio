using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingState : IState
{
    private Player player;

    public FishingState(Player player)
    {
        this.player = player;
        this.player.mono.OnAnimationComplete = () =>
        {
            Debug.Log("anim 끝");
            this.player.mono.PlayAnimation(Enums.ePlayerState.OneHandEquipe_Idle);
            //마법도구 착용?
        };
    }
    public void DoAction()
    {
        this.player.mono.PlayAnimation(Enums.ePlayerState.Fishing);

        this.player.mono.StartCoroutine("FishingAnim");
    }
    public void SwitchState(IState state)
    {
        this.player.State = state;
    }
}
