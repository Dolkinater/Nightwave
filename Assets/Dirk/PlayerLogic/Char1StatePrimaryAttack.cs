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
        Debug.Log("Start");

        float timer = 0;
        while (timer < 0.2f)
        {
            Vector3 movement = sm.transform.forward * sm.lightAttack_speed + sm.velocity;
            Debug.Log("movement = " + movement);
            sm.playerController.Move(movement * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        sm.c1Attack1.SetActive(false); 
        timer = 0f;

        float finalTimer = 0.3f;
        while (timer < finalTimer)
        {
            Vector3 movement = sm.transform.forward * (sm.lightAttack_speed * .1f * (Mathf.Clamp01((finalTimer - timer) / finalTimer))) + sm.velocity;

            sm.playerController.Move(movement * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        if (sm.inputController.GetIsHoldingPrimaryAttack())
        {
            sm.ChangeState(sm.c1_primary);
        }
        else
        {
            sm.ChangeState(sm.IdleState);
        }
    }
}
