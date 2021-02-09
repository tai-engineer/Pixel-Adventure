using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelAdventure
{
    public abstract class StateSO : ScriptableObject
    {
        public abstract State GetState();
    }

    public class StateSO<T>: StateSO where T: State, new()
    {
        public override State GetState()
        {
            T state = new T();
            state.originSO = this;
            return state;
        }
    }
}