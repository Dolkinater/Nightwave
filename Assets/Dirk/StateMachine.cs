using UnityEngine;

public class StateMachine : MonoBehaviour
{
    StatesBase currentState;
    public StatesBase GetCurrentState() { return currentState; }
    public void ChangeState(StatesBase state)
    {
        if (!canChangeStates || this == null) { return; }

        OnChangeState(currentState, state);

        currentState.thisEnd();
        currentState = state;
        currentState.thisStart();
    }
    public virtual void OnChangeState(StatesBase currentState, StatesBase nextState) { }

    bool canChangeStates;
    public bool GetCanChangeStates() { return canChangeStates; }
    public void SetCanChangeStates(bool a) { canChangeStates = a; }

    public virtual void InstantiateComponents() { }
    public virtual void InstantiateStates() { }
    public virtual void InstantiateValues() { SetCanChangeStates(true); }

    public virtual void StartFunctions() { }
    public virtual void OnEnableFunctions() { }
    public virtual void OnDisableFunctions() { }
    public virtual StatesBase GetInitialState() { return null; }
    public virtual StatesBase GetDeathState() { this.SetCanChangeStates(false); return null; }

    private void Awake()
    {
        InstantiateComponents();
        InstantiateStates();
        InstantiateValues();
    }

    private void Start()
    {
        StartFunctions();

        if (GetInitialState() != null) { currentState = GetInitialState(); }

        currentState?.thisStart();
    }

    private void Update()
    {
        UpdateFunctions();
        currentState?.thisUpdate();
    }

    public virtual void UpdateFunctions() { }

    private void FixedUpdate()
    {
        currentState?.thisFixedUpdate();
    }
    private void LateUpdate()
    {
        currentState?.thisLateUpdate();
    }

    private void OnEnable()
    {
        OnEnableFunctions();
    }

    private void OnDisable()
    {
        OnDisableFunctions();
    }
}
