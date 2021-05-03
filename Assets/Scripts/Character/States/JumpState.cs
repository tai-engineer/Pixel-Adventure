using UnityEngine;

public class JumpState : State
{
    public JumpState(StateMachine stateMachine) : base(stateMachine) { }

    CharacterController _characterController;
    CharacterStatsSO _characterStats;
    CharacterAnimation _animation;
    Animator _animator;

    public override void StateEnter()
    {
        _characterController = stateMachine.GetComponent<CharacterController>();
        _animator = stateMachine.GetComponent<Animator>();
        _animation = stateMachine.GetComponent<CharacterAnimation>();

        _characterStats = _characterController.Stats;

        _characterController.SetJumpHeight(_characterStats.JumpHeight);
        _animation.Jump.SetValue(_animator, true);
    }

    public override void StateExit()
    {
        _animation.Jump.SetValue(_animator, false);
        _animation.Fall.SetValue(_animator, false);
    }

    public override void StateUpdate()
    {
        _characterController.AirborneVerticalMovement();
        _characterController.CheckGrounded();
        if(_characterController.IsFalling)
        {
            _animation.Fall.SetValue(_animator, true);
        }
    }

    public override void TransitionEvaluate()
    {
        if(_characterController.IsGrounded)
        {
            TransitionToState(stateMachine.IdleState);
        }
    }
}
