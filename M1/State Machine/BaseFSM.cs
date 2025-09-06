using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public abstract class BaseFSM<SEnum, State, FSM> : MonoBehaviour
    where SEnum : struct, Enum
    where State : BaseFSMState<SEnum, State, FSM>
    where FSM : BaseFSM<SEnum, State, FSM>
{
    private SEnum _currentState;
    public SEnum CurrentState => _currentState;
    private State _currentStateInstance;
    public State CurrentStateInstance => _currentStateInstance;

    protected abstract SEnum InitialState { get; }

    protected virtual void Start()
    {
        ChangeStateTo(InitialState);
    }

    protected virtual void Update()
    {
        var nextState = _currentStateInstance.GetNextState();

        var same = EqualityComparer<SEnum>.Default.Equals(_currentState, nextState);
        
        if (same)
        {
            _currentStateInstance.OnUpdate();
        }
        else
        {
            ChangeStateTo(nextState);
        }
    }

    protected virtual void FixedUpdate()
    { 
        _currentStateInstance?.OnFixedUpdate();
    }

    protected virtual void OnStateChange(State current, State next)
    {
        Debug.Log($"StateChange: {current?.GetType()}, next: {next?.GetType()}");
    }

    protected void ChangeStateTo(SEnum next)
    {
        var previousState = _currentStateInstance;
        previousState?.OnExit();
        
        _currentState = next;
        _currentStateInstance = GetStateInstance(next);
        
        OnStateChange(previousState, _currentStateInstance);
        _currentStateInstance.Enter(this as FSM);
    }

    protected abstract State GetStateInstance(SEnum stateId);

    public void AnimationFinished()
    {
        _currentStateInstance?.OnAnimationEnd();
    }

    public void AnimationEvent(string eventName)
    {
        _currentStateInstance?.OnAnimationEvent(eventName);
    }
}

public abstract class BaseFSMState<SEnum, State, FSM>
    where SEnum : struct, Enum
    where State : BaseFSMState<SEnum, State, FSM>
    where FSM : BaseFSM<SEnum, State, FSM>
{
    private FSM _currentFSM;
    protected FSM Machine => _currentFSM;

    public void Enter(FSM fsm)
    {
        _currentFSM = fsm;
        OnEnter();
    }

    protected virtual void OnEnter()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnAnimationEvent(string eventName)
    {
    }

    public virtual void OnAnimationEnd()
    {
    }

    public abstract SEnum GetNextState();
}