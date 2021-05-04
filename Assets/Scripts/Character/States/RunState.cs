using UnityEngine;

public class RunState : State
{
    public RunState(StateMachine stateMachine) : base(stateMachine) { }

    CharacterController _characterController;
    CharacterAnimation _animation;
    Animator _animator;

    float _desiredSpeed;
    public override void StateEnter()
    {
        _characterController = stateMachine.GetComponent<CharacterController>();
        _animator = stateMachine.GetComponent<Animator>();
        _animation = stateMachine.GetComponent<CharacterAnimation>();

        _animation.Run.SetValue(_animator, true);
    }

    public override void StateExit()
    {
        _animation.Run.SetValue(_animator, false);
    }

    public override void StateUpdate()
    {
        _characterController.GroundHorizontalMovement();
        _characterController.GroundVerticalMovement();
        _characterController.CheckGrounded();
    }

    public override void TransitionEvaluate()
    {
        if(!_characterController.IsMoving)
        {
            TransitionToState(stateMachine.IdleState);
        }
        else if(_characterController.JumpInput)
        {
            TransitionToState(stateMachine.JumpState);
        }
    }
}
