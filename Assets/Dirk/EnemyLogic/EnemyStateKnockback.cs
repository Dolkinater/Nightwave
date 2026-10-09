using System;
using UnityEngine;
public class EnemyStateKnockback : StatesBase
{
    EnemyStateMachine sm;

    public EnemyStateKnockback(StateMachine baseSM) : base(baseSM)
    {
        sm = (EnemyStateMachine)baseSM;
    }

    private Vector3 direction;
    private float force;
    private float duration;
    private float elapsedTime;

    public void Configure(Vector3 knockbackDirection, float knockbackForce, float knockbackDuration)
    {
        direction = knockbackDirection;
        force = knockbackForce;
        duration = knockbackDuration;
    }

    public override void thisStart()
    {
        base.thisStart();
        elapsedTime = 0f;
        sm.GetAnimator().Play("knockback");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        UpdateKnockback();

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
        {
            sm.ChangeState(sm.IdleState);
            return;
        }
    }

    public void UpdateKnockback()
    {
        Vector3 displacement = direction * force * Time.deltaTime;

        if (!sm.AgentReady)
            return;

        if (sm.agent.Raycast(
            sm.agent.nextPosition + displacement,
            out UnityEngine.AI.NavMeshHit hit))
        {
            displacement = hit.position - sm.agent.nextPosition;
        }

        sm.agent.Move(displacement);
    }

    public override void thisEnd()
    {
        base.thisEnd();

        Debug.Log(" end knockbac = " + elapsedTime);
    }
}
