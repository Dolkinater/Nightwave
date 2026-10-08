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

    public override void thisStart()
    {
        sm.playerController.height = 1;
        slideDirection = sm.transform.forward;

        Debug.Log("Slide");

        time = 2f;


        sm.particleController.CallParticle(PlayerParticleController.ParticleStates.slideState);
    }

    public override void thisUpdate()
    {
        time -= Time.deltaTime;

        if (time <= 0)
        {
            sm.ChangeState(sm.IdleState);
        }

        sm.playerController.Move(slideDirection * 15f * Time.deltaTime);
    }

    public override void thisEnd()
    {
        sm.playerController.height = 2;
    }
}
