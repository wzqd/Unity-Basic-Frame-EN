using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Singleton Base Class
//To simplify the usage of singleton pattern
//Classes who inherit this singleton base class will be a singleton class
//Use   classname.instance.method()   to call the methods in singleton classes
//----methods MonoBehavior will be unavailable after inheritance----
//----this singleton mode uses lazy loading, ignoring multiply threading conditions----
#endregion

public class Singleton<T> where T:new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
        if (instance == null)
                instance = new T();
        return instance;
        }
    }
}

