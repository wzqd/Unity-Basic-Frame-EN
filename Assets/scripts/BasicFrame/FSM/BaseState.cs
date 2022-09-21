using System;


#region Base class of state
//Base class of state in FSM

//There are three main methods that will be called when a state is entered, acted, or exited
//Need to override the three virtual methods in subclasses
//********The name of subclasses MUST be the same as the ones in enum, since it is created by reflection********
#endregion
public abstract class BaseState
{
    public E_States StateName;
    private FSM _fsm;
    public virtual void Init(FSM fsm, E_States stateType)
    {
        this._fsm = fsm; //the state machine that this state belongs to
        this.StateName = stateType; //initial state
    }

    /// <summary>
    /// What to do when entering the state
    /// </summary>
    public virtual void Enter(){} 
    /// <summary>
    /// What at do when staying in state every frame
    /// </summary>
    public virtual void Act(){} 
    /// <summary>
    /// What to do when exiting the state
    /// </summary>
    public virtual void Exit(){}
}
