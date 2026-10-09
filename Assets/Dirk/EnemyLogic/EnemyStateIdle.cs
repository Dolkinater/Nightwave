using System;
using UnityEngine;

public class EnemyStateIdle : StatesBase
{
    EnemyStateMachine sm;

    public EnemyStateIdle(StateMachine baseSM) : base(baseSM)
    {
        sm = (EnemyStateMachine)baseSM;
    }

    public override void thisStart()
    {
        base.thisStart();
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (sm.HasLineOfSight() &&
            sm.DistanceToPlayer() <= sm.detectionDistance)
        {
            sm.ChangeState(sm.FollowingState);
        }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();
    }

    public override void thisEnd()
    {
        base.thisEnd();
    }
}
