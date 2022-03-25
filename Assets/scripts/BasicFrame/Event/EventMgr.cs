using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
///Empty interface, to encapsulate the event
///----By doing so, you can create events of generic type----
/// </summary>
public interface IEventInfo
{

}

/// <summary>
///Event class (generic type parameter)
/// </summary>
/// <typeparam name="T">generic type, allows the event to pass a parameter</typeparam>
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo( UnityAction<T> action) //Constructor, to add a method of generic parameter in member delegate
    {
        actions += action;
    }
}

/// <summary>
///Event class (without parameter)
/// </summary>
public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action) //Constructor, to add a method in member delegate
    {
        actions += action;
    }
}


#region Event center manager
//The event center is a class based on observer pattern
//It mainly includes functions of adding event listener and trigger event

//When triggering event, you may pass a info parameter of any type for standby usage
//there is also a overload without info parameter
//when adding listener, you also have the two choices
#endregion
public class EventMgr : Singleton<EventMgr>
{
    //key —— name of the event (decided by yourself, can be monsterDied, achievementObtained..)
    //IEventInfo ——  the event (base interface to be specific), the collection of recorded methods
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    ///Add event listener (generic parameter)
    /// </summary>
    /// <param name="name">the according event name</param>
    /// <param name="action">the method you want to add into the event, the method should be one generic parameter and no return value</param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {

        if( eventDic.ContainsKey(name) ) //if there is an according event listener
        {
            (eventDic[name] as EventInfo<T>).actions += action; //add method to existing event
        }
        else //if there is no according event listener
        {
            eventDic.Add(name, new EventInfo<T>( action )); //add new event to the dict using constructor
        }
    }

    /// <summary>
    ///Add event listener (no parameter)
    /// </summary>
    /// <param name="name">the according event name</param>
    /// <param name="action">the method you want to add into the event, method should be no parameter, no return</param>
    public void AddEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))//if there is an according event listener
        {
            (eventDic[name] as EventInfo).actions += action;//add method to existing event
        }
        else //if there is no according event listener
        {
            eventDic.Add(name, new EventInfo(action)); //add new event to the dict using constructor
        }
    }
    
    /// <summary>
    ///Remove event listener (generic parameter)
    /// </summary>
    /// <param name="name">the according event name</param>
    /// <param name="action">the method you want to add into the event, the method should be one generic parameter and no return</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo<T>).actions -= action;
    }

    /// <summary>
    ///Remove event listener (no parameter)
    /// </summary>
    /// <param name="name">the according event name</param>
    /// <param name="action">the method you want to add into the event, method should be no parameter, no return</param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo).actions -= action;
    }

    /// <summary>
    ///Event trigger (generic parameter)
    /// </summary>
    /// <param name="name">event name</param>
    /// <param name="info">the information you want to pass, you can pass a list if there are more than one</param>
    public void EventTrigger<T>(string name, T info)
    {
        //if there is an according listener (if there is no listener, then it means no one cares about the event, and you dont have to trigger)
        if (eventDic.ContainsKey(name))
        {
            if((eventDic[name] as EventInfo<T>).actions != null)
                (eventDic[name] as EventInfo<T>).actions(info); //call the delegate inside event class
        }
    }

    /// <summary>
    ///Event trigger (no parameter)
    /// </summary>
    /// <param name="name">Event name</param>
    public void EventTrigger(string name)
    {
        //if there is an according listener (if there is no listener, then it means no one cares about the event, and you dont have to trigger)
        if (eventDic.ContainsKey(name))
        {
            if ((eventDic[name] as EventInfo).actions != null)
                (eventDic[name] as EventInfo).actions(); //call the delegate inside event class
        }
    }

    /// <summary>
    /// Clear the event center, mainly used when changing scenes
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
