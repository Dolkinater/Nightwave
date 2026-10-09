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
        sm.velocity.y = Mathf.Sqrt(sm.jumpHeight * -2f * sm.gravity);
        sm.canMove = true;

        sm.particleController.CallParticle(PlayerParticleController.ParticleStates.jumpState);
        sm.animator.Play("jump");
    }

    public override void thisUpdate()
    {
        sm.CallAttack();
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        if (sm.playerController.isGrounded && sm.velocity.y < 0)
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
        sm.animator.StopPlayback();
        sm.canMove = false;
    }
}