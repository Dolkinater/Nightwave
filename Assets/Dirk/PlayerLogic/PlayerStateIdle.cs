using System;
using UnityEngine;
public class PlayerStateIdle : PlayerStateGrounded
{
	PlayerStateMachine sm;

	public PlayerStateIdle(StateMachine baseSM) : base(baseSM)
	{
		sm = (PlayerStateMachine)baseSM;
    }

    public override void thisStart()
    {
        sm.particleController.CallParticle(PlayerParticleController.ParticleStates.none);
        sm.animator.Play("idle");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (sm.inputController.moveInput != Vector2.zero)
        {
            sm.ChangeState(sm.MoveState);
        }
    }

    public override void thisEnd()
    {
        sm.animator.StopPlayback();
    }
}
