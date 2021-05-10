using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Float Event Channel", menuName ="Events/Float Event Channel")]
public class FloatEventChannelSO : ScriptableObject
{
    public UnityAction<float> eventRaised;

    public void RaiseEvent(float value)
    {
        eventRaised.Invoke(value);
    }
}
