using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure
{
    [CreateAssetMenu(fileName = "JumpAscendingState", menuName = "State Machines/States/JumpAscending State")]
    public class JumpAscendingStateSO : StateSO<JumpAscendingState>
    {
        public float jumpForce = 4f;
        [Tooltip("Extra force when holding jump")]
        [Range(0.1f, 0.4f)] public float additionalForce = 0.1f;
        [Range(0.1f, 0.5f)] public float holdingTime = .3f;
    }
    public class JumpAscendingState : State
    {
        PlayerController _player;
        Animator _animator;
        StateMachine _stateMachine;
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
        bool _hasHighJump = false;
        bool _isHolding = false;
        public override void Awake(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _player = stateMachine.GetComponent<PlayerController>();
            _animator = stateMachine.GetComponent<Animator>();
            _protagonist = stateMachine.GetComponent<Protagonist>();

            _originSO = (JumpAscendingStateSO)base.originSO;
            _transform = stateMachine.gameObject.transform;
        }
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            _player.IsAirborne = true;

            _verticalMovement = _originSO.jumpForce;

            _gravityEffect = Physics2D.gravity.y * GRAVITY_ADDITIONAL_MULTIPLIER * GRAVITY_MULTIPLIER;

            _jumpHoldingTime = _originSO.holdingTime;
        }

        public override void OnStateExit() { }

        public override void OnStateUpdate()
        {
            // Apply high jump when holding jump button
            if(_player.JumpInput && !_hasHighJump)
            {
                if (_jumpHoldingTime > 0)
                {
                    _jumpHoldingTime -= Time.deltaTime; 
                }
                else
                {
                    _isHolding = true;
                    _additionalForce = _originSO.additionalForce;
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

            if(_isHolding)
            {
                _verticalMovement += _additionalForce;
                _hasHighJump = true;
                _isHolding = false;
            }
            // Gravity always has negative value
            _verticalMovement += _gravityEffect;

            _player.moveVector.y = _verticalMovement;
        }

        protected override void SetAnimations()
        {
            _animator.SetBool(_protagonist.airBorneHash, _player.IsAirborne);
        }
    }
}