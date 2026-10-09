using UnityEngine;

public class PlayerStateParry : StatesBase
{
    PlayerStateMachine sm;

    public PlayerStateParry(StateMachine baseSM) : base(baseSM)
    {
        sm = (PlayerStateMachine)baseSM;
    }

    float time;

    public override void thisStart()
    {
        sm.health.SetIsParrying(true);

        time = sm.parryWindow;
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        time -= Time.deltaTime;

        if (time <= 0)
        {
            sm.ChangeState(sm.MoveState);
        }
    }

    public override void thisEnd()
    {
        sm.health.SetIsParrying(false);
    }
}
