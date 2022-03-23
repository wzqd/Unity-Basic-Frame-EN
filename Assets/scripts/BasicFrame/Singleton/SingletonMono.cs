using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Singleton Base Class Inheriting MonoBehavior
//Classes who inherit this class will be singleton class, and also have access to methods in MonoBehavior
//----this singleton mode is lazy loading, ignoring multiply threading conditions----
#endregion

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if( instance == null )
            {
                GameObject obj = new GameObject {name = typeof(T) + "_Singleton"};
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            } 
            return instance;
        }
    }
}
