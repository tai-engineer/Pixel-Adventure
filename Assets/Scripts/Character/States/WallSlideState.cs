using UnityEngine;
using System;
public class WallSlideState : State
{
    public WallSlideState(StateMachine stateMachine) : base(stateMachine) { }

    CharacterController _characterController;
    CharacterStatsSO _characterStats;
    CharacterAnimation _animation;
    Animator _animator;

    Vector2 _wallJumpdirection = Vector2.zero;
    public override void StateEnter()
    {
        _characterController = stateMachine.GetComponent<CharacterController>();
        _animator = stateMachine.GetComponent<Animator>();
        _animation = stateMachine.GetComponent<CharacterAnimation>();

        _characterStats = _characterController.Stats;

        _characterController.ResetMoveVector();
        _animation.WallSlide.SetValue(_animator, true);

        _wallJumpdirection.x = -_characterController.FaceDirection.x;
    }

    public override void StateExit() 
    { 
        _animation.WallSlide.SetValue(_animator, false);
    }

    public override void StateUpdate()
    {
        WallSlide();

        _characterController.CheckCeiling();
        _characterController.CheckWallCollided();
        _characterController.CheckGrounded();
    }
    public override void TransitionEvaluate()
    {
        if (_characterController.IsGrounded)
        {
            TransitionToState(stateMachine.IdleState);
        }
        else if (_characterController.JumpInput)
        {
            WallJump();
            TransitionToState(stateMachine.JumpState);
        }
        else if(!_characterController.IsWallCollided)
        {
            TransitionToState(stateMachine.FallState);
        }
    }

    void WallSlide()
    {
        // Horizontal
        _characterController.GroundHorizontalMovement();
        // Vertical
        _characterController.moveVector.y = Mathf.MoveTowards(_characterController.moveVector.y, _characterStats.MaxFallingForce, _characterStats.WallSlideSpeed * Time.deltaTime);
    }

    void WallJump()
    {
        _characterController.SetJumpHeight(_characterStats.WallJumpHeight);
        _characterController.SetHorizontalDistance(_characterStats.WallJumpDistance * _wallJumpdirection.x);
    }
}
