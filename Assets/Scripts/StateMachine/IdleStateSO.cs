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
        StateMachine _stateMachine;

        public override void Awake(StateMachine stateMachine)
        {
            _player = stateMachine.GetComponent<PlayerController>();
            _animator = stateMachine.GetComponent<Animator>();

            _stateMachine = stateMachine;
        }
        public override void OnStateEnter()
        {
            _animator.SetBool(_player.walkingHash, false);
            _animator.SetBool(_player.airBorneHash, false);
        }

        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate()
        {
            if (_player.IsWalking)
                _stateMachine.Transition(StateMachine.EnumState.WALKING);
            //if (_player.JumpInput)
            //    _stateMachine.Transition(StateMachine.EnumState.JUMP_ASCENDING);
            //if (!_player.IsGrounded)
            //    _stateMachine.Transition(StateMachine.EnumState.JUMP_DESCENDING);
        }

        public override void OnStateFixedUpdate()
        {
            //_player.moveVector.y = _player.verticalPull; // Apply gravity
        }
    }
}