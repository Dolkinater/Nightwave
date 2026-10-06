using UnityEngine;

public class Idle : Grounded
{
    private float _horizontalInput;
    private float _verticalInput;
    public Idle(MovementSM stateMachine) : base("Idle", stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _horizontalInput = 0f;
        _verticalInput = 0f;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(_horizontalInput) > Mathf.Epsilon || Mathf.Abs(_verticalInput) > Mathf.Epsilon)
        {
            stateMachine.ChangeState(((MovementSM)stateMachine).movingState);
        }
    }
}
