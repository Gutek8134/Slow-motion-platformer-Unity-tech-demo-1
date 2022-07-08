using UnityEngine;
///<summary>Basic State class every state derives from</summary>
public abstract class State
{
    protected float time { get; set; }
    protected float fixedTime { get; set; }
    protected float lateTime { get; set; }

    public CombatStateMachine stateMachine;

    //Dependency injection for state machine reference
    public virtual void OnEnter(CombatStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    //These functions count the time from entering the state
    public virtual void OnUpdate()
    {
        time += Time.deltaTime;
    }

    public virtual void OnFixedUpdate()
    {
        fixedTime += Time.fixedDeltaTime;
    }

    public virtual void OnLateUpdate()
    {
        lateTime += Time.deltaTime;
    }

    //TFW you won't make an abstract function
    public virtual void OnExit()
    {
        return;
    }

#region MonoBehaviour essentials

//Shortens Destroy call to look similarly to MB one
    protected static void Destroy(UnityEngine.Object obj)
    {
        UnityEngine.Object.Destroy(obj);
    }

#region GetComponent

//This field sends GC calls to state machine, which gets it from MonoBehaviour,
//a class State can't derive from

    protected T GetComponent<T>() where T: Component { return stateMachine.GetComponent<T>(); }

    protected Component GetComponent(System.Type type) { return stateMachine.GetComponent(type); }

    protected Component GetComponent(string type) { return stateMachine.GetComponent(type); }

#endregion
#endregion
}
