using System;
using System.Collections;
using UnityEngine;
public class Char1StatePrimaryAttack : StatesBase
{
    PlayerStateMachine sm;

    public Char1StatePrimaryAttack(StateMachine baseSM) : base(baseSM)
    {
        sm = (PlayerStateMachine)baseSM;
    }

    Coroutine actionCoroutine;

    public override void thisStart()
    {
        actionCoroutine = sm.StartCoroutine(Attack());
        
        sm.c1Attack1.SetActive(true);
    }


    public override void thisEnd()
    {
        if (actionCoroutine != null)
        {
            sm.StopCoroutine(actionCoroutine);
            actionCoroutine = null;
        }
        
        sm.c1Attack1.SetActive(false);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.7f);
        sm.c1Attack1.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        sm.ChangeState(sm.IdleState);
    }
}
