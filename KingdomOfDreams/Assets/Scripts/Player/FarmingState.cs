using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingState : MonoBehaviour, IState
{
    private Player player;

    public FarmingState(Player player)
    {
        this.player = player;
        this.player.mono.OnAnimationComplete = () =>
        {
            this.player.mono.PlayAnimation(Enums.ePlayerState.TwoHandEquipe_Idle);
        };

    }
    public void DoAction()
    {
        this.player.mono.PlayAnimation(Enums.ePlayerState.Farming);
        this.player.mono.StartCoroutine("FarmingAnim");
    }

    public void SwitchState(IState state)
    {
        this.player.State = state;
    }
}