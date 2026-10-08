using UnityEngine;

public class StatesBase
{
    StateMachine system;

    public StatesBase(StateMachine _system)
    {
        system = _system;
    }

    public virtual void thisStart()
    {
    }

    public virtual void thisUpdate()
    {

    }

    public virtual void thisFixedUpdate()
    {

    }

    public virtual void thisLateUpdate()
    {

    }

    public virtual void thisEnd()
    {
    }

    public virtual void SwitchStateFunctions() // if the next state is different from the current's definition, this will occur
    {
        return;
    }
}
