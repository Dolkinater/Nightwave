public abstract class EnemyBaseState
{
    public string Name { get; }

    protected EnemyBaseState(string name)
    {
        Name = name;
    }

    public virtual void Enter() { }

    public virtual void UpdateLogic() { }

    public virtual void Exit() { }
}