using System.Collections;
using UnityEngine;

public abstract class StateMachineMB : MonoBehaviour
{
    public State CurrentState { get; private set; }
    State _previousState = null;

    bool _inTransition = false;

    public void ChangeState(State newState)
    {
        //ensure we're ready for new state
        if (CurrentState == newState || _inTransition)
            return;

        ChangeStateSequence(newState);
    }

    public void ChangeStateToPrevious()
    {
        if (_previousState != null)
            ChangeState(_previousState);
        else
            Debug.Log("Previous State Null");
    }

    private void ChangeStateSequence(State newState)
    {
        _inTransition = true;
        //begin our exit sequence, to prepare for the new state
        if (CurrentState != null)
            CurrentState.Exit();

        CurrentState = newState;

        //begin  our new Enter Sequence
        if (CurrentState != null)
            CurrentState.Enter();

        _inTransition = false;
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    //pass down update ticks to states, since they won't have a monobehavior
    protected virtual void Update()
    {
        //simulate ticks in states
        if (CurrentState != null && !_inTransition)
            CurrentState.Tick();
    }

    protected virtual void FixedUpdate()
    {
        //simulate ticks in states
        if (CurrentState != null && !_inTransition)
            CurrentState.FixedUpdate();
    }

    protected virtual void OnDestroy()
    {
        CurrentState?.Exit();
    }
}
