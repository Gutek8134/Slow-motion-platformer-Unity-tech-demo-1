using UnityEngine;

public class CombatStateMachine: MonoBehaviour
{
    public string customName;

    private State mainStateType;
    public State CurrentState { get; private set; }//Sometimes you just need to see these things
    private State nextState;

    private MainInputActions playerInput;

    private void Awake()
    {
        //As everywhere, first goes setting up variables
        mainStateType = new IdleCombatState();
        //Set current state to idle so I don't have to deal with null exceptions. ASAP
        SetNextStateToMain();
        while (playerInput == null)
        {
            playerInput = InputManager.playerInput;
        }
    }

//Region for calling update functions
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

    //Listeners are too expansive
    ///<summary>Switches states, simultaneously calling their OnEnter and OnExit functions</summary>
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

//I could use a ToString for that, but whatever
    private void PrintStates()
    {
        Debug.Log("Current: " + CurrentState);
        Debug.Log("Next: " + nextState);
    }
}
