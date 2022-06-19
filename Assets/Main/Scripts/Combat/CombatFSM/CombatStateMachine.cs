using UnityEngine;

public class CombatStateMachine: MonoBehaviour
{
    public string customName;
    private State mainStateType;
    public State CurrentState { get; private set; }
    private State nextState;
    private MainInputActions playerInput;

    private void Awake()
    {
        mainStateType = new IdleCombatState();
        SetNextStateToMain();
        while (playerInput == null)
        {
            playerInput = InputManager.playerInput;
        }
    }

    #region Updates
    private void Update()
    {
        if (nextState != null)
        {
            SetState(nextState);
            nextState = null;
        }

        if (CurrentState != null)
            CurrentState.OnUpdate();
        //PrintStates();
    }
    private void LateUpdate()
    {
        if(CurrentState!= null)
            CurrentState.OnLateUpdate();
    }

    private void FixedUpdate()
    {
        if (CurrentState != null)
            CurrentState.OnFixedUpdate();
    }
    #endregion Updates

    private void SetState(State _nextState)
    {
        if (CurrentState != null)
            CurrentState.OnExit();
        CurrentState = _nextState;
        CurrentState.OnEnter(this);
    }

    public void SetNextState(State _nextState)
    {
        if (_nextState != null)
            nextState = _nextState;
    }

    public void SetNextStateToMain()
    {
        if (mainStateType != null)
            nextState = mainStateType;
    }

    private void PrintStates()
    {
        Debug.Log("c: " + CurrentState);
        Debug.Log("n: " + nextState);
    }
}
