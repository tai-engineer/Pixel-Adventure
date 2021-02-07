using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelAdventure
{
    [CreateAssetMenu(fileName = "WalkingState", menuName = "State Machines/States/Walking State")]
    public class WalkingStateSO : StateSO<WalkingState> 
    { 
        [SerializeField] public float moveSpeed = 3.0f;
    }
    public class WalkingState : State
    {
        PlayerController _player;
        Animator _animator;
        Rigidbody2D _rb;
        StateMachine _stateMachine;

        Vector2 _currentPosition;
        WalkingStateSO _originSO;
        Transform _transform;

        public override void Awake(StateMachine stateMachine)
        {
            _player = stateMachine.GetComponent<PlayerController>();
            _animator = stateMachine.GetComponent<Animator>();
            _rb = stateMachine.GetComponent<Rigidbody2D>();

            _originSO = (WalkingStateSO)base.originSO;
            _stateMachine = stateMachine;
            _transform = stateMachine.gameObject.transform;
        }
        public override void OnStateEnter()
        {
            _animator.SetBool(_player.walkingHash, _player.IsWalking);
        }

        public override void OnStateExit()
        {
            
        }

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
            _currentPosition = _transform.position;
            _player.moveVector.x = _player.moveInput.x * _originSO.moveSpeed;
            //_player.moveVector.y = _player.verticalPull; // Apply gravity
            // moveVector should be zero vector in normal case
            // if external forces apply to character, moveVector will be changed
            _rb.MovePosition(_currentPosition + _player.moveVector * Time.fixedDeltaTime);
            _player.moveVector = Vector2.zero;
        }
    }
}