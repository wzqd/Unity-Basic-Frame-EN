using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


#region Resource Load Manager
//It tweaks the original load and loadAsync methods in Resources class
//The new methods directly use the resources after loading in to memory space

//if gameObject is loaded, it is directly instantiated
//loadAsync require a method to manipulate the loaded resource
#endregion
public class ResMgr : Singleton<ResMgr>
{
    /// <summary>
    /// Better load method
    /// ----Make sure the resource is under Resources file----
    /// </summary>
    /// <param name="name">name of the resource</param>
    /// <typeparam name="T">Generic type, the type to be loaded</typeparam>
    /// <returns>return the resource, instantiate it if it is gameObject</returns>
    public T Load<T>(string name) where T:Object
    {
        T res = Resources.Load<T>(name);
        
        if (res is GameObject) 
            return GameObject.Instantiate(res); // instantiate the gameObject
        else 
            return res; //for other resources that cannot be instantiated, return them
    }
    
    /// <summary>
    /// Better loadAsync method
    /// </summary>
    /// <param name="name">name of the resource</param>
    /// <param name="AfterLoadCallback">call back function, manipulation for resource after async loading, goes in to coroutine</param>
    /// <typeparam name="T">type of the resource</typeparam>
    public void LoadAsync<T>(string name, UnityAction<T> AfterLoadCallback) where T:Object
    {
        //start coroutine using public mono manager
        MonoMgr.Instance.StartCoroutine(LoadAsyncCoroutine(name, AfterLoadCallback));
    }

    /// <summary>
    /// The real coroutine method
    /// to be used in LoadAsync
    /// </summary>
    /// <param name="name">name of the resource</param>
    /// <param name="AfterLoadCallback">call back function, manipulation for resource after async loading, parameter is the resource, generic is resource type</param>
    /// <typeparam name="T">type of the resource</typeparam>
    /// <returns>yield return, use call back function to manipulate the returned resource</returns>
    private IEnumerator LoadAsyncCoroutine<T>(string name, UnityAction<T> AfterLoadCallback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        if (r.asset is GameObject)
            AfterLoadCallback(GameObject.Instantiate(r.asset) as T); //make the resource as the parameter of call back function
        else
            AfterLoadCallback(r.asset as T); //make the resource as the parameter of call back function
    }
}
