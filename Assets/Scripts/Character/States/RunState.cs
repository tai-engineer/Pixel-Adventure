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
        _desiredSpeed = _characterController.moveInput.x * _characterStats.MaxSpeed;
        _characterController.moveVector.x = Mathf.MoveTowards(_characterController.moveVector.x, _desiredSpeed, _characterStats.MaxAcceleration * Time.deltaTime);
    }
}
