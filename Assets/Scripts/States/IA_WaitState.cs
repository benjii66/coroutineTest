using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_WaitState : IA_State
{
    protected override void OnStateEnter()
    {
        Debug.Log("Enter wait state");

        brain.Movements.SetTarget(brain.WayPointsSystem.PickPoint());
    }

    protected override void OnStateExit()
    {
        IA_MoveState _patterState = new IA_MoveState();
        _patterState.InitBrain(brain);
        nextState = _patterState;
        transition = new IA_MoveToWait(); 
    }

    protected override IEnumerable OnStateUpdate()
    {
        float _timer = Random.Range(1, 5);
        Debug.Log($"timer : {_timer}");
        yield return new WaitForSeconds(_timer);
    }
}
