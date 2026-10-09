using System;
using System.Collections;
using UnityEngine;
public class PlayerStateDash : StatesBase
{
    PlayerStateMachine sm;

    public PlayerStateDash(StateMachine baseSM) : base(baseSM)
    {
        sm = (PlayerStateMachine)baseSM;
    }

    Coroutine dashCoroutine;
    float gravityValue;
    public override void thisStart()
    {
        gravityValue = sm.gravity;
        sm.gravity = 0f;
        sm.velocity.y = 0f;

        dashCoroutine = sm.StartCoroutine(DashCoroutine());

        sm.particleController.CallParticle(PlayerParticleController.ParticleStates.dashState);
        sm.animator.Play("dash");
    }

    public override void thisEnd()
    {
        if (dashCoroutine != null)
        {
            sm.StopCoroutine(dashCoroutine);
            dashCoroutine = null;
        }
        sm.animator.StopPlayback();

        sm.gravity = gravityValue;
        sm.velocity.y = -12;
    }

    public bool groundedDash;
    public bool jumpDash;

    public bool jumpDashAirborneFalling = false;

    IEnumerator DashCoroutine()
    {
        jumpDash = false;
        groundedDash = false;

        bool noMovementDash = false;
        float fakeMoveInput = 0f;

        if (sm.inputController.moveInput == Vector2.zero)
        {
            fakeMoveInput = 1f;
            noMovementDash = true;
        }

        if (sm.playerController.isGrounded)
        {
            groundedDash = true;
        }

        //Vector3 moveDirection = new Vector3(sm.inputController.moveInput.x, 0, noMovementDash ? fakeMoveInput : sm.inputController.moveInput.y);
        Vector3 moveRotation = sm.transform.forward;//sm.transform.TransformDirection(moveDirection);

        float startTime = Time.time;

        while (Time.time < startTime + sm.dashTime)
        {
            if (groundedDash && sm.inputController.GetJumpPressed())
            {
                groundedDash = false;
                jumpDash = true;
                sm.dashMomentum *= 2f;
                break;
            }

            sm.playerController.Move(moveRotation * sm.dashSpeed * Time.deltaTime);
            sm.dashMomentum = moveRotation * sm.dashSpeed;
            yield return null;
        }


        sm.ChangeState(jumpDash ? sm.JumpState : sm.IdleState);
    }
}
