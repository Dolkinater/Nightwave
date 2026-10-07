using UnityEngine;

public class EnemyFollowingState : EnemyBaseState
{
    private readonly EnemyTracking enemy;
    private float timeWithoutSight;

    public EnemyFollowingState(EnemyTracking enemy)
        : base("Following")
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        timeWithoutSight = 0f;
        enemy.FollowPlayer();
    }

    public override void UpdateLogic()
    {
        float loseTargetDistance =
            enemy.detectionDistance * enemy.loseTargetMultiplier;

        if (enemy.DistanceToPlayer > loseTargetDistance)
        {
            enemy.ChangeState(enemy.IdleState);
            return;
        }

        if (enemy.HasLineOfSight)
        {
            timeWithoutSight = 0f;
        }
        else
        {
            timeWithoutSight += Time.deltaTime;

            if (timeWithoutSight >= enemy.loseSightDelay)
            {
                enemy.ChangeState(enemy.IdleState);
                return;
            }
        }

        enemy.FollowPlayer();
    }

    public override void Exit()
    {
        timeWithoutSight = 0f;
        enemy.StopMoving();
    }
}