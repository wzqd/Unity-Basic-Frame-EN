using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

#region Public MonoBehavior Class
//This class allows other classes who dont inherit MonoBehavior to use the methods in mono
//The class itself inherits singleton with mono
//Therefore it can use methods like lifecycle functions and coroutines

// ----for lifecycle functions, use addUpdateListener() to use----
#endregion

public class MonoMgr : SingletonMono<MonoMgr>
{
    private event UnityAction updateEvent; // lifecycle event to hold methods

    void Update()
    {
        if (updateEvent != null) //if there is methods in event
            updateEvent(); //call those methods in its own Update lifecycle function
    }
    
    /// <summary>
    /// add update listener 
    /// </summary>
    /// <param name="func">function needed to be in Update</param>
    public void AddUpdateListener(UnityAction func) 
    {
        updateEvent += func;
    }
    
    /// <summary>
    /// remove update listener
    /// </summary>
    /// <param name="func">function needed to be in Update</param>
    public void RemoveUpdateListener(UnityAction func)
    {
        updateEvent -= func;
    }
}    
