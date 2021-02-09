using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure
{
    [CreateAssetMenu(fileName = "JumpAscendingState", menuName = "State Machines/States/JumpAscending State")]
    public class JumpAscendingStateSO : StateSO<JumpAscendingState>
    {
        public float JumpForce = 4f;
        public float holdingTime = .3f;
    }
    public class JumpAscendingState : State
    {
        PlayerController _player;
        Animator _animator;
        StateMachine _stateMachine;
        Rigidbody2D _rb;
        Protagonist _protagonist;

        JumpAscendingStateSO _originSO;
        
        Transform _transform;
        Vector2 _currentPosition;

        float _verticalMovement = 0f;
        float _gravityEffect = 0f;

        const float GRAVITY_MULTIPLIER = .6f;
        const float GRAVITY_ADDITIONAL_MULTIPLIER = .03f;

        float _jumpHoldingTime = 0f;
        float _additionalForce = 0f;
        public override void Awake(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _player = stateMachine.GetComponent<PlayerController>();
            _animator = stateMachine.GetComponent<Animator>();
            _rb = stateMachine.GetComponent<Rigidbody2D>();
            _protagonist = stateMachine.GetComponent<Protagonist>();

            _originSO = (JumpAscendingStateSO)base.originSO;
            _transform = stateMachine.gameObject.transform;
        }
        public override void OnStateEnter()
        {
            _player.IsAirborne = true;
            _animator.SetBool(_protagonist.airBorneHash, _player.IsAirborne);

            _verticalMovement = _originSO.JumpForce;

            _gravityEffect = Physics2D.gravity.y * GRAVITY_ADDITIONAL_MULTIPLIER * GRAVITY_MULTIPLIER;

            _jumpHoldingTime = _originSO.holdingTime;
        }

        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate()
        {
            // Apply high jump when holding jump button
            if(_player.JumpInput)
            {
                if (_jumpHoldingTime > 0)
                {
                    _jumpHoldingTime -= Time.deltaTime; 
                }
                else
                {
                    _additionalForce = 0.15f;
                }
            }

            if (_player.IsGrounded && !_player.IsWalking)
                _stateMachine.Transition(StateMachine.EnumState.IDLE);
            else if(_player.IsGrounded && _player.IsWalking)
                _stateMachine.Transition(StateMachine.EnumState.WALKING);
        }

        public override void OnStateFixedUpdate()
        {
            _currentPosition = _transform.position;

            _additionalForce = _additionalForce > 0 ? _additionalForce - Time.fixedDeltaTime : 0f; 
            Debug.Log("_additionalForce: " + _additionalForce);
            _verticalMovement += _additionalForce;
            // Gravity always has negative value
            _verticalMovement += _gravityEffect;

            _player.moveVector.y = _verticalMovement;

            _rb.MovePosition(_currentPosition + _player.moveVector * Time.fixedDeltaTime);

            _player.moveVector = Vector2.zero;
        }
    }
}