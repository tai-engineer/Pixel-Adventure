using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Make attached game object can use states
/// </summary>
public class StateMachine : MonoBehaviour
{
    public IdleState _idleState;
    public RunState _runState;
    public JumpState _jumpState;

    State _currentState;
    void Awake()
    {
        Initialize();
    }

    void Update()
    {
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
    }
    public void TransitionToState(State newState)
    {
        _currentState.StateExit();
        _currentState = newState;
        _currentState.StateEnter();
    }
}
