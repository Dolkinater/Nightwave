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
        sm.animator.Play("c1_lightKick");
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
        float timer = 0;
        while (timer < 0.2f)
        {
            Vector3 movement = sm.transform.forward * sm.heavyAttack_speed + sm.velocity;

            sm.playerController.Move(movement * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        
        timer = 0;
        float finalTimer = 0.2f;
        while (timer < finalTimer)
        {
            Vector3 movement = sm.transform.forward * (sm.heavyAttack_speed * .1f * (Mathf.Clamp01((finalTimer-timer)/finalTimer))) + sm.velocity;

            sm.playerController.Move(movement * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        sm.c1Attack2.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        sm.ChangeState(sm.IdleState);
    }
}
