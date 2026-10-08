using System;
using UnityEngine;
public class PlayerStateJump : StatesBase
{
    PlayerStateMachine sm;

    public PlayerStateJump(StateMachine baseSM) : base(baseSM)
    {
        sm = (PlayerStateMachine)baseSM;
    }

    public override void thisStart()
    {
        Debug.Log("Jump");
        sm.velocity.y = Mathf.Sqrt(sm.jumpHeight * -2f * sm.gravity);
        sm.canMove = true;

        sm.particleController.CallParticle(PlayerParticleController.ParticleStates.jumpState);
    }

    public override void thisUpdate()
    {

    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        if (sm.playerController.isGrounded)
        {
            sm.ChangeState(sm.IdleState);
        }

        sm.PlayerDashCheck();
    }

    public override void thisLateUpdate()
    {

    }

    public override void thisEnd()
    {
        sm.canMove = false;
    }
}