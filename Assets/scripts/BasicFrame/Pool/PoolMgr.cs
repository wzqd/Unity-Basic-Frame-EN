using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// PoolData, one pool of the big dictionary
/// </summary>
public class PoolData
{
    //father object in hierarchy 
    public GameObject fatherObj;
    //pool list
    public List<GameObject> poolList;

    /// <summary>
    /// Constructor, create the pool
    /// </summary>
    /// <param name="obj">the first object of the pool</param>
    /// <param name="poolObj">the father object</param>
    public PoolData(GameObject obj, GameObject poolObj)
    {
        //create a father object for the pool, and let the pool as a child of the pool
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform; 
        poolList = new List<GameObject> (){}; //create the pool list
        PushToPool(obj); // deactivate the first object and push it into the list
    }

    /// <summary>
    /// Put object in to the list
    /// </summary>
    /// <param name="obj">the object you want to put in</param>
    public void PushToPool(GameObject obj)
    {
        //deactivate the object
        obj.SetActive(false);
        //add it to the list
        poolList.Add(obj);
        //set it as a child of the father object
        obj.transform.parent = fatherObj.transform;
    }

    /// <summary>
    /// Get the object from the list
    /// </summary>
    /// <returns>the object you want to get</returns>
    public GameObject GetFromPool()
    {
        GameObject obj = poolList[0]; 
        poolList.RemoveAt(0);//get the first object from the list
        
        obj.SetActive(true); //activate the object
        obj.transform.parent = null; //remove its relation with father object

        return obj;
    }
}


#region Object pool manager
//Object pool is used for objects that may be reused for multiply times
//The principle is to deactivate objects instead of destroy them
//and reactivate those instead of instantiate them

//The class mainly includes methods of get objects from the pool and put objects into the pool
//a dictionary acting like a pool
//the dict includes PoolData class which encapsulates a list of objects and father object (in hierarchy window)
#endregion
public class PoolMgr : Singleton<PoolMgr>
{                                                                                                                                                                                                     
    //Pool dictionary
    //string —— name of that kind of object（一个抽屉）
    //PoolData —— encapsulates a list of objects and father object (in hierarchy window)
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    private GameObject poolObj; //the father of all PoolData (in hierarchy)

    /// <summary>
    ///Get object async
    /// </summary>
    /// <param name="name">the name of object you want to get</param>
    /// <param name="AfterGetCallback">the manipulation to the object after asyncLoading</param>
    /// <returns></returns>
    public void GetObjAsync(string name, UnityAction<GameObject> AfterGetCallback)
    {
        //if there is a pool and there are objects in the pool
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            //call the get method of PoolData, activate the object
            AfterGetCallback(poolDic[name].GetFromPool()); //make the object as the parameter of the call back function
        }
        else //if there is no such pool or all the objects are picked out
        {
            //instantiate the object by using async loading
            ResMgr.Instance.LoadAsync<GameObject>(name, (o) =>
            {
                o.name = name; //let the name of the object the same as the pool (the default name contains "clone", which isn't good for managing)
                AfterGetCallback(o); //make the object as the parameter of the call back function
            });
        }
    }

    /// <summary>
    /// Get object sync
    /// </summary>
    /// <param name="name">he name of object you want to get</param>
    /// <returns>the object</returns>
    public GameObject GetObj(string name)
    {
        GameObject obj;
        //if there is a pool and there are objects in the pool
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0) 
        {
            //call the get method of PoolData, activate the object
            obj = poolDic[name].GetFromPool();
        }
        else //if there is no such pool or all the objects are picked out
        {
            //load and instantiate the object
            obj = ResMgr.Instance.Load<GameObject>(name);
            obj.name = name; //make the name the same as the pool
        }
        return obj;
    }
        
        
        
    /// <summary>
    ///Put object back
    ///put object back into the pool, you may write a delayed invoke function to implement it
    /// </summary>
    public void PushObj(string name, GameObject obj)
    {
        if (poolObj == null) //if the father of all PoolData is not created yet
            poolObj = new GameObject("Pool"); //create it

        //if there is a pool
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushToPool(obj); //call push method of PoolData to deactivate the object
        }
        else //if there is no pool
        {
            poolDic.Add(name, new PoolData(obj, poolObj)); //create a pool
        }
    }


    /// <summary>
    /// Clear the whole big pool
    /// Mainly used when changing scenes
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
