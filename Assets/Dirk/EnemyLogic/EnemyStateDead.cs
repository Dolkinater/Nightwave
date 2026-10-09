using UnityEngine;

public class EnemyStateDead : StatesBase
{
    EnemyStateMachine sm;

    public EnemyStateDead(StateMachine baseSM) : base(baseSM)
    {
        sm = (EnemyStateMachine)baseSM;
    }

    public override void thisStart()
    {
        base.thisStart(); Object.Destroy(sm.gameObject);
    }
}
