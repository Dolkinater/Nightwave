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

        if (sm.inputController.GetJumpPressed() && sm.playerController.isGrounded)
        {
            sm.ChangeState(sm.JumpState);
            return;
        }

        if (sm.inputController.GetParryPressed()) // parry
        {
            sm.ChangeState(sm.ParryState);
            return;
        }

        sm.PlayerDashCheck(); // dash

        if (sm.inputController.GetSlidePressed()) // slide
        {
            sm.ChangeState(sm.SlideState);
            return;
        }

        int a = (sm.inputController.GetSwapValue()); // character swapping

        if  (a != -1)
        {
            sm.SetCurrentCharacter(a);
        }

        sm.CallAttack(); // character combat
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
