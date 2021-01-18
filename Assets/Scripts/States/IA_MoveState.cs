using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_MoveState : IA_State
{
    protected override void OnStateEnter()
    {
        Debug.Log("Enter move state");
    }

    protected override void OnStateExit()
    {
        IA_WaitState _tmp = new IA_WaitState();
        _tmp.InitBrain(brain);
        nextState = _tmp;
        transition = new IA_MoveToWait();
    }

    protected override IEnumerable OnStateUpdate()
    {
        while (!brain.Movements.IsAtPosition)
        {
            brain.Movements.MoveTo();
            yield return null;
        }
    }
}
