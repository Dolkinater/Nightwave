using UnityEngine;

public class MovementSM : StateMachine_Deprecated
{
    [HideInInspector]
    public Idle idleState;
    [HideInInspector]
    public Moving movingState;
    [HideInInspector]
    public Jumping jumpingState;
    [HideInInspector]
    public Sliding slidingState;
    public CharacterController characterController;

    private void Awake()
    {
        idleState = new Idle(this);
        movingState = new Moving(this);
        jumpingState = new Jumping(this);
        slidingState = new Sliding(this);
    }
    protected override BaseState GetInitialState()
    {
        return idleState;
    }
}
