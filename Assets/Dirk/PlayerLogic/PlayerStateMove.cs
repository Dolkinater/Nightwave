using System;
using UnityEngine;
public class PlayerStateMove : PlayerStateGrounded
{
    PlayerStateMachine sm;

    public PlayerStateMove(StateMachine baseSM) : base(baseSM)
    {
        sm = (PlayerStateMachine)baseSM;
    }

    public override void thisStart()
    {
        base.thisStart();
        sm.canMove = true; Debug.Log("Move");
        sm.particleController.CallParticle(PlayerParticleController.ParticleStates.walkState);
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (sm.inputController.moveInput == Vector2.zero) 
        {
            sm.ChangeState(sm.IdleState);
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
        sm.canMove = false;
    }
}
