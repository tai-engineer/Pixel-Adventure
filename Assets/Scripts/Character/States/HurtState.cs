using UnityEngine;
/// <summary>
/// This state has special behaviour
/// It can only be entered via OnHit event handler
/// </summary>
public class HurtState : State
{
    public HurtState(StateMachine stateMachine) : base(stateMachine) { }

    CharacterController _characterController;
    CharacterAnimation _animation;
    Animator _animator;

    Vector2 _inverseDir;
    float _fallBackSpeed = 5f;
    public override void StateEnter()
    {
        GetReferences();

        Init();

        _characterController.StartFading();
    }

    public override void StateExit()
    {
        _characterController.ResetMoveVector();
    }

    public override void StateUpdate()
    {
        FallBack();
    }

    public override void TransitionEvaluate()
    {
        if(Mathf.Approximately(_characterController.moveVector.x, 0f) && Mathf.Approximately(_characterController.moveVector.y, 0f))
        {
            TransitionToState(stateMachine.IdleState);
        }
    }

    void GetReferences()
    {
        _characterController = stateMachine.GetComponent<CharacterController>();
        _animator = stateMachine.GetComponent<Animator>();
        _animation = stateMachine.GetComponent<CharacterAnimation>();
    }
    void Init()
    {
        _inverseDir = _characterController.Direction * -1;

        _characterController.ResetMoveVector();

        if (!Mathf.Approximately(_inverseDir.x, 0f))
            _characterController.moveVector.x = _characterController.Stats.FallBackDistance * _inverseDir.x;

        if (!Mathf.Approximately(_inverseDir.y, 0f))
            _characterController.moveVector.y = _characterController.Stats.FallBackDistance * _inverseDir.y;

        _animation.Hurt.SetValue(_animator);
    }
    void FallBack()
    {
        if (!Mathf.Approximately(_characterController.moveVector.x, 0f))
            _characterController.moveVector.x = Mathf.MoveTowards(_characterController.moveVector.x, 0f, Time.deltaTime * _fallBackSpeed);
        
        if (!Mathf.Approximately(_characterController.moveVector.y, 0f))
            _characterController.moveVector.y = Mathf.MoveTowards(_characterController.moveVector.y, 0f, Time.deltaTime * _fallBackSpeed);
    }
}
