using System;
using System.Collections;
using UnityEngine;

public class Char1StateSecondaryAttack : StatesBase
{
    PlayerStateMachine sm;

    public Char1StateSecondaryAttack(StateMachine baseSM) : base(baseSM)
    {
        sm = (PlayerStateMachine)baseSM;
    }

    Coroutine actionCoroutine;

    public override void thisStart()
    {
        actionCoroutine = sm.StartCoroutine(Attack());
        
        sm.c1Attack2.SetActive(true);
    }


    public override void thisEnd()
    {
        if (actionCoroutine != null)
        {
            sm.StopCoroutine(actionCoroutine);
            actionCoroutine = null;
        }
        
        sm.c1Attack2.SetActive(false);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.2f);
        sm.c1Attack2.SetActive(false);
        yield return new WaitForSeconds(0.8f);
        sm.ChangeState(sm.IdleState);
    }
}
