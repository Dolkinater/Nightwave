using System;
using UnityEngine;
public class PlayerStateSlide : StatesBase
{
    PlayerStateMachine sm;

    public PlayerStateSlide(StateMachine baseSM) : base(baseSM)
    {
        sm = (PlayerStateMachine)baseSM;
    }

    float time;

    Vector3 slideDirection;

    bool exitSlide;
    bool jumpOutOfSlide;

    public override void thisStart()
    {
        sm.playerController.height = 1;
        slideDirection = sm.transform.forward;

        time = 2f;

        exitSlide = false;
        jumpOutOfSlide = false;

        sm.particleController.CallParticle(PlayerParticleController.ParticleStates.slideState);

        sm.animator.Play("slide");
    }

    public override void thisUpdate()
    {
        if (sm.inputController.GetJumpPressed()) { exitSlide = true; jumpOutOfSlide = true; }

        time -= Time.deltaTime;

        if (time <= 0)
        {
            exitSlide = true;
        }

        if (exitSlide) // if the player either ends the slide by jumping or waiting the timer out, then the game checks if there's nothing above before returning to standing
        {
            bool val = false;

            val = Physics.Linecast(sm.transform.position, sm.transform.position + Vector3.up * 1, LayerMask.GetMask("Walls"));
            val = Physics.Linecast(sm.transform.position + (Vector3.left * .5f), sm.transform.position + (Vector3.left * .5f) + Vector3.up * 1, LayerMask.GetMask("Walls"));
            val = Physics.Linecast(sm.transform.position + (Vector3.right * .5f), sm.transform.position + (Vector3.right * .5f) + Vector3.up * 1, LayerMask.GetMask("Walls"));
            val = Physics.Linecast(sm.transform.position + (Vector3.forward * .5f), sm.transform.position + (Vector3.forward * .5f) + Vector3.up * 1, LayerMask.GetMask("Walls"));
            val = Physics.Linecast(sm.transform.position + (Vector3.back * .5f), sm.transform.position + (Vector3.back * .5f) + Vector3.up * 1, LayerMask.GetMask("Walls"));

            if (val)
            {
                if (jumpOutOfSlide) { jumpOutOfSlide = false; } // if the user attempted to jump out of the slide while something is above, the jump will be disabled
            }
            else // nothing above this, return to standing
            {
                sm.ChangeState(jumpOutOfSlide ? sm.JumpState : sm.IdleState); // if the user has attempted to jump out of the slide, the state will transition to jumping, otherwise it will transition to idle
            }

        }

        sm.playerController.Move(slideDirection * 15f * Time.deltaTime);
    }

    public override void thisEnd()
    {
        sm.animator.StopPlayback();
        sm.playerController.height = 2;
    }
}
