using UnityEngine;

public class JumpState : State
{
    public JumpState(StateMachine stateMachine) : base(stateMachine) { }

    CharacterController _characterController;
    CharacterStatsSO _characterStats;

    float _initialJumpForce;
    public override void StateEnter()
    {
        _characterController = stateMachine.GetComponent<CharacterController>();
        _characterStats = _characterController.Stats;

        _initialJumpForce = _characterStats.JumpHeight;
        _characterController.moveVector.y = _initialJumpForce;
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate()
    {
        _characterController.AirborneVerticalMovement();
        _characterController.CheckGrounded();
    }

    public override void TransitionEvaluate()
    {
        if(_characterController.IsGrounded && _characterController.GettingMoveInput)
        {
            TransitionToState(stateMachine.RunState);
        }
        else if(_characterController.IsGrounded && !_characterController.GettingMoveInput)
        {
            TransitionToState(stateMachine.IdleState);
        }
    }
}
