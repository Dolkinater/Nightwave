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

    int punchIndex = 0;

    bool resettingAttack = false; // checks to see if we're going into another punch, at which point it will swap animations

    public override void thisStart()
    {
        actionCoroutine = sm.StartCoroutine(Attack());

        sm.PlayerRotation(true);

        resettingAttack = false;

        switch (punchIndex)
        {
            case 0:
                sm.c1Attack1Right.SetActive(true);
                sm.animator.Play("c1_combo1");
                break;
            case 1:
                sm.c1Attack1Right.SetActive(true);
                sm.animator.Play("c1_combo2");
                break;
            case 2:
                sm.c1Attack1Right.SetActive(true);
                sm.animator.Play("c1_combo3");
                break;
            case 3:
                sm.c1Attack1Left.SetActive(true);
                sm.animator.Play("c1_combo4");
                break;
        }
    }


    public override void thisEnd()
    {
        if (resettingAttack)
        {
            punchIndex++;

            if (punchIndex > 3) { punchIndex = 0; }
        }
        else
        {
            punchIndex = 0;
        }

        if (actionCoroutine != null)
        {
            sm.StopCoroutine(actionCoroutine);
            actionCoroutine = null;
        }

        sm.c1Attack1Left.SetActive(false);
        sm.c1Attack1Right.SetActive(false);
    }

    IEnumerator Attack()
    {
        Debug.Log("Start");

        float timer = 0;
        while (timer < 0.2f)
        {
            Vector3 movement = sm.transform.forward * sm.lightAttack_speed + sm.velocity;
            sm.playerController.Move(movement * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        
        timer = 0f;

        float finalTimer = 0.3f;
        while (timer < finalTimer)
        {
            Vector3 movement = sm.transform.forward * (sm.lightAttack_speed * .1f * (Mathf.Clamp01((finalTimer - timer) / finalTimer))) + sm.velocity;

            sm.playerController.Move(movement * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        sm.c1Attack1Left.SetActive(false);
        sm.c1Attack1Right.SetActive(false);

        yield return new WaitForSeconds (0.1f);

        if (sm.inputController.GetIsHoldingPrimaryAttack())
        {
            resettingAttack = true;
            sm.ChangeState(sm.c1_primary);
        }
        else
        {
            sm.ChangeState(sm.IdleState);
        }
    }
}
