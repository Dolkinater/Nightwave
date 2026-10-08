using System;
using UnityEngine;
using System.Collections;
public class Char1StateSpecialAttack : StatesBase
{
    PlayerStateMachine sm;

    public Char1StateSpecialAttack(StateMachine baseSM) : base(baseSM)
    {
        sm = (PlayerStateMachine)baseSM;
    }

    Coroutine actionCoroutine;

    public override void thisStart()
    {
        actionCoroutine = sm.StartCoroutine(Attack());

        sm.particleController.CallParticle(PlayerParticleController.ParticleStates.c1_specialState);
        sm.speed = sm.speed / 2;
        sm.canMove = true;
    }


    public override void thisEnd()
    {
        if (actionCoroutine != null)
        {
            sm.StopCoroutine(actionCoroutine);
            actionCoroutine = null;
        }

        sm.speed = sm.speed * 2;
        sm.canMove = false;
    }

    IEnumerator Attack()
    {
        sm.c1SpAttack.SetActive(true);
        yield return new WaitForSeconds(2f);
        sm.c1SpAttack.SetActive(false);
        sm.ChangeState(sm.IdleState);
    }
}
