using UnityEngine;

public abstract class State
{
    protected StateMachine stateMachine;

    public State(StateMachine stateMachine) => this.stateMachine = stateMachine;

    public abstract void StateEnter();
    public abstract void StateUpdate();
    public abstract void StateExit();
    public abstract void TransitionEvaluate();
    protected void TransitionToState(State state)
    {
        stateMachine.TransitionToState(state);
    }

}
