<<<<<<< Updated upstream
using UnityEngine;

public class Jumping : BaseState
{
    private MovementSM _sm;
    private bool grounded;
    public Jumping(MovementSM stateMachine) : base("Jumping", stateMachine)
    {
        _sm = (MovementSM)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();


    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (grounded)
        {
            stateMachine.ChangeState(_sm.idleState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        grounded = _sm.characterController.isGrounded;
    }
}
=======
using UnityEngine;

public class Jumping : BaseState
{
    private MovementSM _sm;
    private bool grounded;
    public Jumping(MovementSM stateMachine) : base("Jumping", stateMachine)
    {
        _sm = (MovementSM)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();


    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (grounded)
        {
            stateMachine.ChangeState(_sm.idleState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        grounded = _sm.characterController.isGrounded;
    }
}
>>>>>>> Stashed changes
