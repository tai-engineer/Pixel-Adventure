using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Int Event Channel", menuName ="Events/Int Event Channel")]
public class IntEventChannelSO : ScriptableObject
{
    public UnityAction<int> eventRaised;

    public void RaiseEvent(int value)
    {
        eventRaised.Invoke(value);
    }
}
