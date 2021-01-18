using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Brain : MonoBehaviour
{
    IA_FSM fsm = null;
    IA_MoveState moveState = null;
    IA_WaitState waitState = null;
    [SerializeField] IA_Movements movements = null;
    [SerializeField] IA_WayPointsSystem wayPointsSystem = null;

    public IA_Movements Movements => movements;
    public IA_WayPointsSystem WayPointsSystem => wayPointsSystem;
    public bool IsValid => movements && wayPointsSystem;

    private void Start() => InitFSM();

    void InitFSM()
    {
        if (!IsValid) return;
        fsm = new IA_FSM();
        moveState = new IA_MoveState();
        waitState = new IA_WaitState();

        moveState.InitBrain(this);
        waitState.InitBrain(this);

        StartCoroutine(fsm.StartFSM(waitState));
    }


}
