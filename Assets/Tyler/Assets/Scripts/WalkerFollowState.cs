using UnityEngine;

public class WalkerFollowState : EnemyBaseState
{
    private readonly WalkerController walker;

    private float timeWithoutSight;
    private float attackCheckTimer;

    public WalkerFollowState(WalkerController walker)
        : base("Follow")
    {
        this.walker = walker;
    }

    public override void Enter()
    {
        timeWithoutSight = 0f;
        attackCheckTimer = 0f;

        walker.UpdateFollowMovement();
    }

    public override void UpdateLogic()
    {
        float loseTargetDistance =
            walker.followingRange * walker.loseTargetMultiplier;

        if (walker.DistanceToPlayer > loseTargetDistance)
        {
            walker.ChangeState(walker.IdleState);
            return;
        }

        if (walker.HasLineOfSight)
        {
            timeWithoutSight = 0f;
        }
        else
        {
            timeWithoutSight += Time.deltaTime;

            if (timeWithoutSight >= walker.loseSightDelay)
            {
                walker.ChangeState(walker.IdleState);
                return;
            }
        }

        walker.UpdateFollowMovement();

        if (walker.DistanceToPlayer > walker.attackRange)
        {
            attackCheckTimer = 0f;
            return;
        }

        attackCheckTimer += Time.deltaTime;

        if (attackCheckTimer >= walker.attackCheckTime)
        {
            attackCheckTimer = 0f;

            if (walker.CanStartAttack())
            {
                walker.ChangeState(walker.AttackState);
                return;
            }
        }
    }

    public override void Exit()
    {
        walker.StopMoving();

        timeWithoutSight = 0f;
        attackCheckTimer = 0f;
    }
}