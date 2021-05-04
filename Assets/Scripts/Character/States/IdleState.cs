using UnityEngine;
using System;
public class IdleState : State
{
    public IdleState(StateMachine stateMachine) : base(stateMachine) { }

    CharacterController _characterController;
    public override void StateEnter()
    {
        _characterController = stateMachine.GetComponent<CharacterController>();

        ResetMoveVector();
    }

    public override void StateExit() { }

    public override void StateUpdate()
    {
        _characterController.GroundVerticalMovement();
        _characterController.CheckGrounded();
    }
    public override void TransitionEvaluate()
    {
        if(_characterController.GettingMoveInput)
        {
            TransitionToState(stateMachine.RunState);
        }
        else if(_characterController.JumpInput)
        {
            TransitionToState(stateMachine.JumpState);
        }
    }

    void ResetMoveVector()
    {
        _characterController.moveVector = Vector2.zero;
    }
}
