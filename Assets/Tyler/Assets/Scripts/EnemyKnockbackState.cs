using UnityEngine;

public class EnemyKnockbackState : EnemyBaseState
{
    private readonly EnemyTracking enemy;

    private Vector3 direction;
    private float force;
    private float duration;
    private float elapsedTime;

    public EnemyKnockbackState(EnemyTracking enemy)
        : base("Knockback")
    {
        this.enemy = enemy;
    }

    public void Configure(
        Vector3 knockbackDirection,
        float knockbackForce,
        float knockbackDuration)
    {
        direction = knockbackDirection;
        force = knockbackForce;
        duration = knockbackDuration;

        elapsedTime = 0f;
    }

    public override void Enter()
    {
        enemy.StopMoving();
        elapsedTime = 0f;
    }

    public override void UpdateLogic()
    {
        float remainingTime = duration - elapsedTime;
        float stepTime = Mathf.Min(Time.deltaTime, remainingTime);

        Vector3 displacement = direction * force * stepTime;
        enemy.MoveKnockback(displacement);

        elapsedTime += stepTime;

        if (elapsedTime >= duration)
        {
            enemy.ChangeState(enemy.IdleState);
            return;
        }
    }

    public override void Exit()
    {
        enemy.StopMoving();
        elapsedTime = 0f;
    }
}