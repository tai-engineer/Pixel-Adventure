using UnityEngine;

public class FallState : State
{
    public FallState(StateMachine stateMachine) : base(stateMachine) { }

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

        _animation.Fall.SetValue(_animator, true);
    }

    public override void StateExit()
    {
        _animation.Fall.SetValue(_animator, false);
    }

    public override void StateUpdate()
    {
        _characterController.AirborneVerticalMovement();
        _characterController.AirborneHorizontalMovement();
        _characterController.CheckGrounded();
        _characterController.CheckWallCollided();
    }

    public override void TransitionEvaluate()
    {
        if(_characterController.IsGrounded)
        {
            TransitionToState(stateMachine.IdleState);
        }
        else if (_characterController.IsWallCollided)
        {
            TransitionToState(stateMachine.WallSlideState);
        }
    }
}
