using UnityEngine;

public class BaseState
{
    public string name;
    protected StateMachine_Deprecated stateMachine;
    public BaseState(string name, StateMachine_Deprecated stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }
    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }

}