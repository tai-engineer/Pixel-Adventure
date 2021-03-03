using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] StateSO _idleState = default;
        [SerializeField] StateSO _walkingState = default;
        [SerializeField] StateSO _jumpAscendingState = default;
        [SerializeField] StateSO _jumpDescendingState = default;

        State _currentState;
        void Start()
        {
            SetInitialState();
        }
        public void OnUpdate()
        {
            _currentState.OnStateUpdate();
        }

        public void OnFixedUpdate()
        {
            _currentState.OnStateFixedUpdate();
        }
        void SetInitialState()
        {
            _currentState = _idleState.GetState();
            _currentState.Awake(this);
            _currentState.OnStateEnter();
        }
        void SetState(State state)
        {
            _currentState.OnStateExit();
            _currentState = state;
            _currentState.Awake(this);
            _currentState.OnStateEnter();
        }
        public void Transition(EnumState state)
        {
            switch(state)
            {
                case EnumState.IDLE:
                    SetState(_idleState.GetState());
                    break;
                case EnumState.WALKING:
                    SetState(_walkingState.GetState());
                    break;
                case EnumState.JUMP_ASCENDING:
                    SetState(_jumpAscendingState.GetState());
                    break;
                case EnumState.JUMP_DESCENDING:
                    SetState(_jumpDescendingState.GetState());
                    break;
            }
        }
        public enum EnumState
        { IDLE, WALKING, JUMP_ASCENDING, JUMP_DESCENDING }
    }
}
