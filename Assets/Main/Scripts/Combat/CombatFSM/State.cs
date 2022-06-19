using UnityEngine;
public abstract class State
{
    protected float time { get; set; }
    protected float fixedTime { get; set; }
    protected float lateTime { get; set; }

    public CombatStateMachine stateMachine;
    public virtual void OnEnter(CombatStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

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

    public virtual void OnExit()
    {
        return;
    }

    protected static void Destroy(UnityEngine.Object obj)
    {
        UnityEngine.Object.Destroy(obj);
    }

    protected T GetComponent<T>() where T: Component { return stateMachine.GetComponent<T>(); }

    protected Component GetComponent(System.Type type) { return stateMachine.GetComponent(type); }

    protected Component GetComponent(string type) { return stateMachine.GetComponent(type); }
}
