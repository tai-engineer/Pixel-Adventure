using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure
{
    public abstract class State
    {
        public StateSO originSO { get; set; }
        public abstract void Awake(StateMachine stateMachine);
        public virtual void OnStateEnter() => SetAnimations();
        public abstract void OnStateExit();
        public abstract void OnStateUpdate();
        public abstract void OnStateFixedUpdate();

        protected abstract void SetAnimations();
    }
}