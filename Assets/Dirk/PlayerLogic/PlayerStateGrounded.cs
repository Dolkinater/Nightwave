using System;
using UnityEngine;

public class PlayerStateGrounded : StatesBase
{
    PlayerStateMachine sm;

    public PlayerStateGrounded(StateMachine baseSM) : base(baseSM)
    {
        sm = (PlayerStateMachine)baseSM;
    }

    public override void thisStart()
    {
        base.thisStart();
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        //input checks

        if (sm.inputController.GetJumpPressed())
        {
            Debug.Log("Go");
            sm.ChangeState(sm.JumpState);
            return;
        }

        sm.PlayerDashCheck();

        if (sm.inputController.GetSlidePressed())
        {
            sm.ChangeState(sm.SlideState);
            return;
        }

        int a = (sm.inputController.GetSwapValue());

        if  (a != -1)
        {
            sm.SetCurrentCharacter(a);
        }

        int b = (sm.inputController.GetAttackValue());

        if (b != -1)
        {
            sm.CallAttack(b);
        }
    }

    public override void thisFixedUpdate()
    {

    }

    public override void thisLateUpdate()
    {

    }

    public override void thisEnd()
    {
    }
}
