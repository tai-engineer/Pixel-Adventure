﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Make attached game object can use states
/// </summary>
public class StateMachine : MonoBehaviour
{
    IdleState _idleState;
    RunState _runState;
    JumpState _jumpState;

    State _currentState;

    public IdleState IdleState { get { return _idleState; } }
    public RunState RunState { get { return _runState; } }
    public JumpState JumpState { get { return _jumpState; } }
    public State CurrentState { get { return _currentState; } }

#if UNITY_EDITOR
    public StateMachineDebugger _debugger;
#endif
    void Awake()
    {
        Initialize();
#if UNITY_EDITOR
        _debugger = new StateMachineDebugger(this);
#endif
    }

    void Update()
    {
        _currentState.TransitionEvaluate();
        _currentState.StateUpdate();
    }
    void Initialize()
    {
        _idleState = new IdleState(this);
        _runState = new RunState(this);
        _jumpState = new JumpState(this);

        SetDefaultState(_idleState);
    }

    void SetDefaultState(State defaultState)
    {
        _currentState = defaultState;
        _currentState.StateEnter();
    }
    public void TransitionToState(State newState)
    {
        _currentState.StateExit();
        _currentState = newState;
#if UNITY_EDITOR
        _debugger.TransitionEvaluate(newState.ToString());
#endif
        _currentState.StateEnter();
    }
}
