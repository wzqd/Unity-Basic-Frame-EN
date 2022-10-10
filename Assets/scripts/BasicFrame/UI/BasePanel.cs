using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

#region Base class for UI panel
//It is the base class for all UI panels
//All its subclasses can be managed by UIMgr

//Methods are to find components of UI, to show, and to hide panels
#endregion
public class BasePanel : MonoBehaviour
{
    /// <summary>
    ///Dictionary to store all components of UI
    ///Key：name of the component in hierarchy
    ///Value：Component script，List is used to store composite components
    /// </summary>
    private Dictionary<string, List<UIBehaviour>> UIComponentsDict = new Dictionary<string, List<UIBehaviour>>();
    
    /// <summary>
    /// Virtual method, Lifecycle function, Remember to use base() when overriding
    /// </summary>
    protected void Awake()
    {
        //Need to keep base() when overriding
        //Add component into dictionary
        FindChildrenComponent<Button>();
        FindChildrenComponent<Toggle>();
        FindChildrenComponent<Slider>();
        FindChildrenComponent<Text>();
        FindChildrenComponent<Image>();
        FindChildrenComponent<ScrollRect>();
        FindChildrenComponent<InputField>();
        FindChildrenComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Find all the components in its children
    /// </summary>
    private void FindChildrenComponent<T>() where T: UIBehaviour
    {
        T[] UIComponents = GetComponentsInChildren<T>(); //Get all the components in children
        
        foreach (var componentInChildren in UIComponents)//traverse all the components
        {
            string componentName = componentInChildren.gameObject.name; //name of the child component
            if (UIComponentsDict.ContainsKey(componentName))//if it exists(is composite component)
            {
                UIComponentsDict[componentName].Add(componentInChildren);//add component into its list
            }
            else //If it is added for the first time
            {
                UIComponentsDict.Add(componentName, new List<UIBehaviour>() {componentInChildren}); //create a new list
            }
            
            //Listening to events
            if (componentInChildren is Button) //Listener for button
            {
                (componentInChildren as Button).onClick.AddListener((() =>
                {
                    OnClick(componentName); //Add listener of virtual method
                }));
            }
            else if(componentInChildren is Toggle) //Listener for toggle
            {
                (componentInChildren as Toggle).onValueChanged.AddListener(((boolValue) =>
                {
                    OnValueChange(componentName,boolValue); //Add listener of virtual method
                }));
            }
            //-------------------If other components are needed to be listened，add more listener with corresponding virtual method----------------------------
        }
    }

    /// <summary>
    /// Virtual Method, Listener for button click event
    /// </summary>
    /// <param name="buttonName">name of the button</param>
    protected virtual void OnClick(string buttonName)
    {
        //Override this method, listener is added in the above method
        //Use switch case to enter logic for button with different names
    }

    /// <summary>
    /// Virtual Method, Listener for toggle check event
    /// </summary>
    /// <param name="toggleName">name of toggle</param>
    /// <param name="boolValue">bool passed</param>
    protected virtual void OnValueChange(string toggleName, bool boolValue)
    {
        //Override this method, listener is added in the above method
        //Use switch case to enter logic for toggle with different names
    }
    
    
    
    
    /// <summary>
    /// Get component by name
    /// </summary>
    /// <typeparam name="T">Type of the component</typeparam>
    protected T GetUIComponent<T>(string UIComponentName) where T: UIBehaviour
    {
        if (UIComponentsDict.ContainsKey(UIComponentName))
        {
            for (int i = 0; i < UIComponentsDict[UIComponentName].Count; i++)
            {
                if (UIComponentsDict[UIComponentName][i] is T)
                {
                    return UIComponentsDict[UIComponentName][i] as T;
                }
            }
        }
        return null;
    }

    
    
    
    
    /// <summary>
    /// Virtual method, Show the panel
    /// </summary>
    public virtual void Show(){}
    /// <summary>
    /// Virtual method, Destroy the panel
    /// </summary>
    public virtual void Hide(){}

}
