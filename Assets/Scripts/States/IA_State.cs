using System.Collections;
using System;

public abstract class IA_State
{
    public event Action<IA_State, ITransition> OnEndState = null;
    protected IA_Brain brain = null;
    protected IA_State nextState = null;
    protected ITransition transition = null;
    protected abstract void OnStateEnter();
    protected abstract IEnumerable OnStateUpdate();
    protected abstract void OnStateExit();



    public virtual void InitBrain(IA_Brain _brain)
    {
        brain = _brain;
    }

    public IEnumerable State()
    {
        OnStateEnter();
        yield return OnStateUpdate().GetEnumerator();
        OnStateExit();
        OnEndState?.Invoke(nextState, transition);
        yield break;
    }
}
