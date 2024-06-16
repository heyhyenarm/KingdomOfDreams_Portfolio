using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void DoAction();
    void SwitchState(IState state);
}