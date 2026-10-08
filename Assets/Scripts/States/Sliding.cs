using UnityEngine;

public class Sliding : BaseState
{
    private MovementSM _sm;
    private float _horizontalInput;
    private float _verticalInput;
    public Sliding(MovementSM stateMachine) : base("Sliding", stateMachine)
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
       
        
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        if (!Input.GetKey(KeyCode.LeftControl))
        {
            if (Mathf.Abs(_horizontalInput) < Mathf.Epsilon && Mathf.Abs(_verticalInput) < Mathf.Epsilon)
            {
                stateMachine.ChangeState(((MovementSM)stateMachine).idleState);
            }
            if (Mathf.Abs(_horizontalInput) > Mathf.Epsilon || Mathf.Abs(_verticalInput) > Mathf.Epsilon)
            {
                stateMachine.ChangeState(((MovementSM)stateMachine).movingState);
            }
        }

    }
}
