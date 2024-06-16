using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamMiningState : MonoBehaviour, IState
{
    private Player player;
    public DreamMiningState(Player player)
    {
        this.player = player;
        this.player.mono.OnAnimationComplete = () =>
        {
            Debug.Log("anim ³¡");
            this.player.mono.PlayAnimation(Enums.ePlayerState.TwoHandEquipe_Idle);
        };
    }
    public void DoAction()
    {
        this.player.mono.PlayAnimation(Enums.ePlayerState.Mining);

        this.player.mono.StartCoroutine("DreamMiningAnim");
    }
    public void SwitchState(IState state)
    {
        this.player.State = state;
    }
}
