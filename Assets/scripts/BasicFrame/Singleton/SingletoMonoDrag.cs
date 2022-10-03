using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region Singleton Base Class Inheriting MonoBehaviour that used by dragging
//The scripts that inherit this can use all the methods in MonoBehaviour

//But the scripts need to be manually dragged onto gameObjects
//The object with the script WILL destroy on load. (Otherwise reenter the scene creates duplicates)
//Don't drag it onto two objects, otherwise it would only stay on the last one

#endregion
public class SingletonMonoDrag<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;

    protected void Awake()
    {
        instance = this as T;
    }
}