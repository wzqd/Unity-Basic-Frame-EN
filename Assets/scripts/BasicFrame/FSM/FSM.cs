using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



#region Base class of FSM
//Base class of any FSM

//The only public method is to switch state. You can set an initial state
//The Update function can be overriden in order to add logical about when to change state
//Need to add state in the AllState enum,
//********The name of subclasses MUST be the same as the ones in enum********
#endregion
public abstract class FSM: MonoBehaviour
{
    private Dictionary<int,BaseState> stateDict = new Dictionary<int, BaseState>();
    public E_States CurrentStateType { get; set; } = E_States.Default;//state enum
    private BaseState currentState;


    /// <summary>
    /// Virtual method, Lifecycle method, Remember to use base() when overriding
    /// </summary>
    protected virtual void Update()
    {
        if (currentState != null)
        {
            currentState.Act(); //call corresponding method that execute every frame
        }
    }

    /// <summary>
    ///Switch state
    /// </summary>
    /// <param name="newState">the state you want to enter</param>
    /// <param name="allowReenter">if allowing to reenter the state</param>
    public void ChangeState(E_States newState, bool allowReenter = false)
    {

        if (!allowReenter && newState == CurrentStateType) return; //return if reenter or dont allow reenter
        if(currentState != null) 
            currentState.Exit(); //call the method that execute when exiting

        currentState = GetStateObj(newState); //get new state
        CurrentStateType = currentState.StateName;
        currentState.Enter(); //call the method that execute when entering
        

    }

    /// <summary>
    ///Get new state from the list
    /// </summary>
    /// <param name="stateType">corresponding enum</param>
    /// <returns>return state, for changing state</returns>
    private BaseState GetStateObj(E_States stateType) //boxing problem
    {
        int key = (int) stateType;
        if (stateDict.ContainsKey(key)) 
            return stateDict[key]; //if already in dictionary, return it

        //*********MUST be the same name as in enum，otherwise creating empty objects*********
        BaseState state = Activator.CreateInstance(Type.GetType(stateType.ToString())!) as BaseState; //if not in dictionary, create one
        state.Init(this,stateType); //initialize it using enum parameter
        stateDict.Add((int)stateType, state); //add it into dictionary
        return state;
    }

    /// <summary>
    /// clear dictionary
    /// </summary>
    public void ClearAllStates()
    {
        stateDict.Clear();
    }
}
