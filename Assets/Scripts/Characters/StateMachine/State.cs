using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure
{
    public class State
    {
        public StateSO originSO { get; set; }
        public virtual void Awake(StateMachine stateMachine) { }
        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
        public virtual void OnStateUpdate() { }
        public virtual void OnStateFixedUpdate() { }
    }
}