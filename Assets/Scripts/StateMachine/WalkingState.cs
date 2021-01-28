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
        public override void Awake(StateMachine stateMachine)
        {
            _originSO = (WalkingStateSO)base.originSO;

            _stateMachine = stateMachine;
            _player = stateMachine.GetComponent<PlayerController>();
            _animator = stateMachine.GetComponent<Animator>();
            _rb = stateMachine.GetComponent<Rigidbody2D>();
        }
        public override void OnStateEnter()
        {
            _animator.SetBool(_player.walkingHash, _player.isWalking);
        }

        public override void OnStateExit()
        {
            _player.isWalking = false;
        }

        public override void OnStateUpdate()
        {
            _player.isWalking = _player.moveVector.x != 0;

            if (!_player.isWalking)
                _stateMachine.Transition(StateMachine.EnumState.IDLE);
        }

        public override void OnStateFixedUpdate()
        {
            _currentPosition = _rb.position;
            _player.moveVector.x = _player.moveInput.x * _originSO.moveSpeed;
            //_player.moveVector.y = _player.verticalPull; // Apply gravity
            // moveVector should be zero vector in normal case
            // if external forces apply to character, moveVector will be changed
            _rb.MovePosition(_currentPosition + _player.moveVector * Time.fixedDeltaTime);
        }
    }
}