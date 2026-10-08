using UnityEngine;

public abstract class EnemyStateMachine : MonoBehaviour
{
    protected EnemyBaseState currentState;

    [Header("State - View During Play")]
    [SerializeField] private string currentStateName;

    protected virtual void Start()
    {
        ChangeState(GetInitialState());
    }

    protected virtual void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateLogic();
        }
    }

    protected abstract EnemyBaseState GetInitialState();

    public void ChangeState(EnemyBaseState newState)
    {
        if (newState == null || newState == currentState)
            return;

        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentStateName = currentState.Name;

        currentState.Enter();
    }
}