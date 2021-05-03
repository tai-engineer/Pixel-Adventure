#if UNITY_EDITOR
using System;
using UnityEngine;
using System.Text;
[Serializable]
public class StateMachineDebugger
{
    [SerializeField] bool showDebugLog = false;
    public string _currentState;
    StringBuilder _logBuilder;
    StateMachine _stateMachine;

    const string SHARP_ARROR = "\u27A4";
    /// <summary>
    /// Must be called after StateMachine.Initialize
    /// </summary>
    public StateMachineDebugger(StateMachine stateMachine)
    {
        _logBuilder = new StringBuilder();
        _stateMachine = stateMachine;
        _currentState = stateMachine.CurrentState.ToString();
    }

    public void TransitionEvaluate(string targetState)
    {
        _logBuilder.Clear();
        _logBuilder.AppendLine($"{_stateMachine.gameObject.name} state changed");
        _logBuilder.AppendLine($"{_currentState}   {SHARP_ARROR}   {targetState}");

        PrintDebugLog();
        _currentState = targetState;
    }
    void PrintDebugLog()
    {
        _logBuilder.AppendLine();
        _logBuilder.Append("------------------");
        if (showDebugLog)
        {
            Debug.Log(_logBuilder.ToString()); 
        }
    }
}
#endif
