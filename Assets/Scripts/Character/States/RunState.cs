using UnityEngine;

public class RunState : State
{
    public RunState(StateMachine stateMachine) : base(stateMachine) { }

    CharacterController _characterController;
    CharacterStatsSO _characterStats;

    float _desiredSpeed;
    public override void StateEnter()
    {
        _characterController = stateMachine.GetComponent<CharacterController>();
        _characterStats = _characterController.Stats;
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate()
    {
        _characterController.GroundHorizontalMovement();
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
