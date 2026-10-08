using System;

public class Shooter_EnemySM : EnemyStateMachine
{
	public ShooterStateShoot ShootState { get; private set; }
    public override StatesBase GetAttackState() { return ShootState; }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        ShootState = new ShooterStateShoot(this);
    }

    public override void InstantiateValues()
    {
        base.InstantiateValues();
    }
}
