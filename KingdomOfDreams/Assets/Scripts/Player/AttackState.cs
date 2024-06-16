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
            Debug.Log("anim ��");
            this.player.mono.PlayAnimation(Enums.ePlayerState.Sword_Idle);
        };

    }
    public void DoAction()
    {
        Debug.Log("���ݻ��� doaction");
        this.player.mono.PlayAnimation(Enums.ePlayerState.Attacking);
        this.player.mono.StartCoroutine("AttackingAnim");

    }
    public void SwitchState(IState state)
    {
        this.player.State = state;
    }

    private void StunReaction()
    {
        Debug.LogFormat("�÷��̾� ����");
        this.player.mono.PlayAnimation(Enums.ePlayerState.Attacking_Stun);
        this.player.mono.StartCoroutine("StunAnim");
        

    }

    private void ReturnHome()
    {
        //������ ���� ������ ��ȯ
        EventManager.instance.onPlain();
        this.transform.position = Vector3.zero;
    }

}
