using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IA_FSM
{
    public event Action OnFSMStateEnter = null;
    public event Action OnFSMStateUpdate = null;
    public event Action OnFSMStateExit = null;
    public event Action OnFSMTransition = null;
    public event Action OnFSMStop = null;
    public event Action<string> OnStateChange = null;

    IA_State state = null, nextState = null;
    bool enabled = true;
    ITransition nextTransition = null;

    public IA_State CurrentState
    {
        get => state;
        set
        {
            state = value;
            OnStateChange?.Invoke(state.GetType().ToString());
            state.OnEndState += SetNextState;
            OnFSMStateEnter?.Invoke();
        }
    }

    void SetNextState(IA_State _next, ITransition _transition)
    {
        nextState = _next;
        nextTransition = _transition;
    }

    public IEnumerator StartFSM(IA_State _firstState)
    {
        CurrentState = _firstState;
        while (enabled)
        {
            yield return state.State().GetEnumerator();
            OnFSMStateExit?.Invoke();
            if(nextTransition == null)
            {
                OnFSMStop?.Invoke();
                yield break;
            }

            state.OnEndState -= SetNextState;
            if(nextState == null)
            {
                OnFSMStop?.Invoke();
                yield break;
            }

            OnFSMTransition?.Invoke();
            yield return nextTransition.Enter();
            yield return nextTransition.Exit();
            CurrentState = nextState;
            nextState = null;
            nextTransition = null;
        }
    }
}
