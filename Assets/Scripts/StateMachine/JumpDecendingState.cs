using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure
{
    [CreateAssetMenu(fileName = "JumpDescendingState", menuName = "State Machines/States/JumpDescending State")]
    public class JumpDescendingStateSO : StateSO<JumpDescendingState> { }
    public class JumpDescendingState : State
    {
        PlayerController _player;
        Animator _animator;
        StateMachine _stateMachine;

        public override void Awake(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _player = stateMachine.GetComponent<PlayerController>();
            _animator = stateMachine.GetComponent<Animator>();
        }
        public override void OnStateEnter()
        {

        }

        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate()
        {

        }

        public override void OnStateFixedUpdate()
        {

        }
    }
}