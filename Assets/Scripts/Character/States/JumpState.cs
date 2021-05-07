using UnityEngine;

public class JumpState : State
{
    public JumpState(StateMachine stateMachine) : base(stateMachine) { }

    CharacterController _characterController;
    CharacterStatsSO _characterStats;
    CharacterAnimation _animation;
    Animator _animator;

    bool _canDoubleJump;
    public override void StateEnter()
    {
        _characterController = stateMachine.GetComponent<CharacterController>();
        _animator = stateMachine.GetComponent<Animator>();
        _animation = stateMachine.GetComponent<CharacterAnimation>();

        _characterStats = _characterController.Stats;

        _characterController.SetJumpHeight(_characterStats.JumpHeight);
        _animation.Jump.SetValue(_animator, true);

        _canDoubleJump = false;
    }

    public override void StateExit()
    {
        _animation.Jump.SetValue(_animator, false);
    }

    public override void StateUpdate()
    {
        _characterController.AirborneVerticalMovement();
        _characterController.AirborneHorizontalMovement();
        _characterController.CheckCeiling();

        if (TryDoubleJump())
        {
            if (_characterController.JumpInput)
            {
                DoubleJump();
            }
        }
    }

    public override void TransitionEvaluate()
    {
        if(_characterController.IsFalling)
        {
            TransitionToState(stateMachine.FallState);
        }
    }
    bool TryDoubleJump()
    {
        if (!_characterController.JumpInput && !_canDoubleJump)
        {
           _canDoubleJump = true;
        }

        return _canDoubleJump;
    }

    void DoubleJump()
    {
        _characterController.SetJumpHeight(_characterStats.DoubleJumpHeight);
        _animation.DoubleJump.SetValue(_animator);
    }
}
