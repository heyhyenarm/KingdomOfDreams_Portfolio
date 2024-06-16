using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, IState
{
    private Player player;

    public AttackState(Player player)
    {
        this.player = player;
        this.player.mono.OnAnimationComplete = () =>
        {
            Debug.Log("anim 끝");
            this.player.mono.PlayAnimation(Enums.ePlayerState.Sword_Idle);
        };

    }
    public void DoAction()
    {
        Debug.Log("공격상태 doaction");
        this.player.mono.PlayAnimation(Enums.ePlayerState.Attacking);
        this.player.mono.StartCoroutine("AttackingAnim");

    }
    public void SwitchState(IState state)
    {
        this.player.State = state;
    }

    private void StunReaction()
    {
        Debug.LogFormat("플레이어 기절");
        this.player.mono.PlayAnimation(Enums.ePlayerState.Attacking_Stun);
        this.player.mono.StartCoroutine("StunAnim");
        

    }

    private void ReturnHome()
    {
        //원래는 마을 씬으로 전환
        EventManager.instance.onPlain();
        this.transform.position = Vector3.zero;
    }

}
