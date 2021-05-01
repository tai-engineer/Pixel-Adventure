using UnityEngine;

public class JumpState : State
{
    public JumpState(StateMachine stateMachine) : base(stateMachine) { }

    CharacterController _characterController;
    CharacterStatsSO _characterStats;

    float _initialJumpForce;
    float _jumpForce;
    public override void StateEnter()
    {
        _characterController = stateMachine.GetComponent<CharacterController>();
        _characterStats = _characterController.Stats;

        _initialJumpForce = _characterStats.JumpForce;
        _jumpForce = _initialJumpForce;
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate()
    {
        _jumpForce -= _characterController.Gravity * Time.deltaTime;
        _jumpForce = Mathf.Clamp(_jumpForce, -_initialJumpForce, _initialJumpForce);

        _characterController.moveVector.y = _jumpForce;
    }
}
