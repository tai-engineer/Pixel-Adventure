using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Void Event Channel", menuName ="Events/Void Event Channel")]
public class VoidEventChannelSO : ScriptableObject
{
    public UnityAction eventRaised;

    public void RaiseEvent()
    {
        eventRaised.Invoke();
    }
}
