using UnityEngine.Events;
using System;
namespace PA.Events
{
    [Serializable] public class IntEvent: UnityEvent<int> { }
    [Serializable] public class FloatEvent: UnityEvent<float> { }
    [Serializable] public class VoidEvent: UnityEvent { }
}