public class EnemyIdleState : EnemyBaseState
{
    private readonly EnemyTracking enemy;

    public EnemyIdleState(EnemyTracking enemy) : base("Idle")
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        enemy.StopMoving();
    }

    public override void UpdateLogic()
    {
        if (enemy.HasLineOfSight &&
            enemy.DistanceToPlayer < enemy.detectionDistance)
        {
            enemy.ChangeState(enemy.FollowingState);
        }
    }

    public override void Exit()
    {
    }
}