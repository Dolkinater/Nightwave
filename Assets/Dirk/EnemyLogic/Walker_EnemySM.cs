using System;
using UnityEngine;
public class Walker_EnemySM : EnemyStateMachine
{
    public WalkerStateAttack AttackState { get; private set; }
    public override StatesBase GetAttackState() { return AttackState; }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        AttackState = new WalkerStateAttack(this);
    }

    [Min(0.1f)] public float windUpTime = 0.5f;
    [Min(0)] public int damageDealt = 10;

    public override void InstantiateValues()
    {
        base.InstantiateValues();
    }
}
