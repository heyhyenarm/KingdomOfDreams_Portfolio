using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningState : MonoBehaviour, IState
{
    private Player player;
    public MiningState(Player player)
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

        this.player.mono.StartCoroutine("MiningAnim");
    }
    public void SwitchState(IState state)
    {
        this.player.State = state;
    }
}
