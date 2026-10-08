using System;
using System.Collections;

using UnityEngine;
using UnityEngine.AI;

public class WalkerStateAttack : StatesBase
{
    Walker_EnemySM sm;

    public WalkerStateAttack(StateMachine baseSM) : base(baseSM)
    {
        sm = (Walker_EnemySM)baseSM;
    }

    private const float PunchDuration = 0.3f;
    private const float RecoveryDuration = 0.5f;
    private const float ParryCueDuration = 0.1f;

    private Vector3 punchDirection;
    private float punchSpeed;

    Coroutine attackCoroutine;

    public override void thisStart()
    {
        if (!sm.AgentReady)
        {
            Debug.LogWarning("NavMesh Agent unavailable or unapplied, returning to idle");
            sm.ChangeState(sm.IdleState);
            return;
        }

        punchSpeed = 0f;
        punchDirection = Vector3.zero;

        sm.CallParry();

        attackCoroutine = sm.StartCoroutine(Punch());
    }

    public void MoveAttack(Vector3 displacement)
    {
        if (!sm.AgentReady)
            return;

        if (sm.agent.Raycast(
            sm.agent.nextPosition + displacement,
            out NavMeshHit hit))
        {
            displacement = hit.position - sm.agent.nextPosition;
        }

        sm.agent.Move(displacement);
    }

    IEnumerator Punch()
    {
        float timer = 0;

        while (timer < sm.windUpTime)
        {
            sm.FacePlayer();
            timer += Time.deltaTime;
            yield return null;
        }

        sm.CallParry();

        yield return new WaitForSeconds(ParryCueDuration);

        punchDirection = sm.transform.forward;
        punchDirection.y = 0f;
        punchDirection.Normalize();

        punchSpeed = sm.DistanceToPlayer() * 0.9f;

        timer = 0;

        while (timer < PunchDuration)
        {
            MoveAttack(
                punchDirection * punchSpeed * Time.deltaTime
            );

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(RecoveryDuration);

        sm.ChangeState(sm.IdleState);
    }

    public override void thisEnd()
    {
        if (attackCoroutine != null) 
        {
            sm.StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }
}
