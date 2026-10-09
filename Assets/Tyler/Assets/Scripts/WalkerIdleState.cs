public class WalkerIdleState : EnemyBaseState
{
    private readonly WalkerController walker;

    public WalkerIdleState(WalkerController walker)
        : base("Idle")
    {
        this.walker = walker;
    }

    public override void Enter()
    {
        walker.StopMoving();
    }

    public override void UpdateLogic()
    {
        if (walker.HasLineOfSight &&
            walker.DistanceToPlayer <= walker.followingRange)
        {
            walker.ChangeState(walker.FollowState);
        }
    }

    public override void Exit()
    {
    }
}