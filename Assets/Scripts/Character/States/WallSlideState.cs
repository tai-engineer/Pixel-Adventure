using UnityEngine;
using System;
public class WallSlideState : State
{
    public WallSlideState(StateMachine stateMachine) : base(stateMachine) { }

    CharacterController _characterController;
    CharacterStatsSO _characterStats;
    CharacterAnimation _animation;
    Animator _animator;

    bool _isSliding;
    bool _isWallJumping;
    Vector2 _direction = Vector2.zero;
    public override void StateEnter()
    {
        _characterController = stateMachine.GetComponent<CharacterController>();
        _animator = stateMachine.GetComponent<Animator>();
        _animation = stateMachine.GetComponent<CharacterAnimation>();

        _characterStats = _characterController.Stats;

        _characterController.ResetMoveVector();
        _animation.WallSlide.SetValue(_animator, true);

        _isSliding = true;
        _isWallJumping = false;
        _direction.x = -_characterController.FaceDirection.x;
    }

    public override void StateExit() 
    { 
        _animation.WallSlide.SetValue(_animator, false);
    }

    public override void StateUpdate()
    {
        if(_characterController.JumpInput && _isSliding)
        {
            _characterController.SetJumpHeight(_characterStats.WallJumpHeight);
            _characterController.moveVector.x = _characterStats.WallJumpDistance * _direction.x;

            _isSliding = false;
            _isWallJumping = true;
            _animation.WallSlide.SetValue(_animator, false);
        }

        WallSlide();
        WallJump();

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
        //else if(!_characterController.IsWallCollided)
        //{
        //    TransitionToState(stateMachine.JumpState);
        //}
    }

    void WallSlide()
    {
        if (_isSliding)
        {
            // Horizontal
            _characterController.GroundHorizontalMovement();
            // Vertical
            _characterController.moveVector.y = Mathf.MoveTowards(_characterController.moveVector.y, _characterStats.MaxFallingForce, _characterStats.WallSlideSpeed * Time.deltaTime); 
        }
    }

    void WallJump()
    {
        if (_isWallJumping)
        {
            // Horizontal
            _characterController.moveVector.x = Mathf.MoveTowards(_characterController.moveVector.x, 0f, _characterStats.MaxAcceleration * Time.deltaTime);
            // Vertical
            _characterController.AirborneVerticalMovement(); 
        }
    }
}
