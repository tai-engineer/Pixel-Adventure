using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelAdventure
{
    [CreateAssetMenu(fileName = "IdleState", menuName = "State Machines/States/Idle State")]
    public class IdleStateSO : StateSO<IdleState> { }
    public class IdleState : State
    {
        PlayerController _player;
        Animator _animator;
        Rigidbody2D _rb;
        StateMachine _stateMachine;

        Vector2 _currentPosition;
        public override void Awake(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _player = stateMachine.GetComponent<PlayerController>();
            _animator = stateMachine.GetComponent<Animator>();
            _rb = stateMachine.GetComponent<Rigidbody2D>();
        }
        public override void OnStateEnter()
        {
            _player.isWalking = false;
            _player.isAirborne = false;
            _animator.SetBool(_player.walkingHash, false);
            _animator.SetBool(_player.airBorneHash, false);
        }

        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate()
        {
            _player.isWalking = _player.moveVector.x != 0;

            if (_player.isWalking)
                _stateMachine.Transition(StateMachine.EnumState.WALKING);
        }

        public override void OnStateFixedUpdate()
        {
            _currentPosition = _rb.position;
            _player.moveVector = _player.moveInput;
            //_player.moveVector.y = _player.verticalPull; // Apply gravity
            // moveVector should be zero vector in normal case
            // if external forces apply to character, moveVector will be changed
            _rb.MovePosition(_currentPosition + _player.moveVector * Time.fixedDeltaTime);
        }
    }
}