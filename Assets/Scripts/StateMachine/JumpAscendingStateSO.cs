using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure
{
    [CreateAssetMenu(fileName = "JumpAscendingState", menuName = "State Machines/States/JumpAscending State")]
    public class JumpAscendingStateSO : StateSO<JumpAscendingState>
    {
        public float lowJumpForce = 3f;
        public float highJumpForce = 5f;
        public float holdingTime = .2f;
    }
    public class JumpAscendingState : State
    {
        PlayerController _player;
        Animator _animator;
        StateMachine _stateMachine;
        Rigidbody2D _rb;

        JumpAscendingStateSO _originSO;
        
        Transform _transform;
        Vector2 _currentPosition;

        float _verticalMovement = 0f;
        float _gravityEffect = 0f;

        const float GRAVITY_MULTIPLIER = .6f;
        const float GRAVITY_ADDITIONAL_MULTIPLIER = .03f;

        float _jumpHoldingStart = 0f;
        bool _isHolding = false;
        public override void Awake(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _player = stateMachine.GetComponent<PlayerController>();
            _animator = stateMachine.GetComponent<Animator>();
            _rb = stateMachine.GetComponent<Rigidbody2D>();

            _originSO = (JumpAscendingStateSO)base.originSO;
            _transform = stateMachine.gameObject.transform;
        }
        public override void OnStateEnter()
        {
            _player.IsAirborne = true;
            _animator.SetBool(_player.airBorneHash, _player.IsAirborne);

            _verticalMovement = _originSO.lowJumpForce;

            _gravityEffect = Physics2D.gravity.y * GRAVITY_ADDITIONAL_MULTIPLIER * GRAVITY_MULTIPLIER;

            _jumpHoldingStart = Time.time;
        }

        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate()
        {
            // Apply high jump for only once
            if (Time.time > _jumpHoldingStart + _originSO.holdingTime && _player.JumpInput && !_isHolding)
            {
                _verticalMovement += _originSO.highJumpForce - _originSO.lowJumpForce;
                _isHolding = true;
            }

            if (_player.IsGrounded && !_player.IsWalking)
                _stateMachine.Transition(StateMachine.EnumState.IDLE);
            else if(_player.IsGrounded && _player.IsWalking)
                _stateMachine.Transition(StateMachine.EnumState.WALKING);
        }

        public override void OnStateFixedUpdate()
        {
            _currentPosition = _transform.position;

            // Gravity always has negative value
             _verticalMovement += _gravityEffect;

            _player.moveVector.y = _verticalMovement;

            _rb.MovePosition(_currentPosition + _player.moveVector * Time.fixedDeltaTime);

            _player.moveVector = Vector2.zero;
        }
    }
}