using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelAdventure
{
    [CreateAssetMenu(fileName = "WalkingState", menuName = "State Machines/States/Walking State")]
    public class WalkingStateSO : StateSO<WalkingState> 
    { 
        [Range(0f, 30f)]
        [SerializeField] public float moveSpeed = 3.0f;
        [Range(100f, 300f)]
        [SerializeField] public float acceleration = 100.0f;
    }
    public class WalkingState : State
    {
        PlayerController _player;
        Animator _animator;
        StateMachine _stateMachine;
        Protagonist _protagonist;

        WalkingStateSO _originSO;
        Transform _transform;

        float desiredSpeed;
        public override void Awake(StateMachine stateMachine)
        {
            _player = stateMachine.GetComponent<PlayerController>();
            _animator = stateMachine.GetComponent<Animator>();
            _protagonist = stateMachine.GetComponent<Protagonist>();

            _originSO = (WalkingStateSO)base.originSO;
            _stateMachine = stateMachine;
            _transform = stateMachine.gameObject.transform;
        }
        public override void OnStateEnter() => base.OnStateEnter();

        public override void OnStateExit() { }

        public override void OnStateUpdate()
        {
            if (!_player.IsWalking)
                _stateMachine.Transition(StateMachine.EnumState.IDLE);
            //if (_player.JumpInput)
            //    _stateMachine.Transition(StateMachine.EnumState.JUMP_ASCENDING);
            //if (!_player.IsGrounded)
            //    _stateMachine.Transition(StateMachine.EnumState.JUMP_DESCENDING);
        }

        public override void OnStateFixedUpdate()
        {
            GroundHorizontalMovement();
            GroundVerticalMovement();
        }

        protected override void SetAnimations()
        {
            _animator.SetBool(_protagonist.walkingHash, _player.IsWalking);
        }
        void GroundHorizontalMovement()
        {
            desiredSpeed = _player.moveInput.x * _originSO.moveSpeed;
            _player.moveVector.x = Mathf.MoveTowards(_player.moveVector.x, desiredSpeed, _originSO.acceleration * Time.fixedDeltaTime);
        }

        void GroundVerticalMovement()
        {
            // Gravity of physic2D has negative value
            _player.moveVector.y += Physics2D.gravity.y * Time.fixedDeltaTime;
            if(_player.moveVector.y < Physics2D.gravity.y)
            {
                _player.moveVector.y = Physics2D.gravity.y * Time.fixedDeltaTime;
            }
        }
    }
}