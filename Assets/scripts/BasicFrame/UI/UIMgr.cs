using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Enum of panel layer
/// </summary>
public enum E_PanelLayer
{
    Bot,
    Mid,
    Top
}

#region UI Manager
//It is used to manage all UI panels that inherits BasePanel Script
//It automatically loads preset Canvas and EventSystem. The Canvas includes three layers

//Main methods are to show and hide panel (need to set layer when showing panel)
//Other methods are to get currently showing panel, to get Layer object, and a static method to add customised listener
#endregion
public class UIMgr : Singleton<UIMgr>
{
    /// <summary>
    /// Dictionary of panels
    /// Key：Name of the panel
    /// Value：Base panel script
    /// </summary>
    private Dictionary<string, BasePanel> panelDict = new Dictionary<string, BasePanel>();

    /// <summary>
    ///Public canvas object, the only canvas in the scene
    /// </summary>
    public RectTransform canvas;
    
    private Transform bot; //Bottom layer
    private Transform mid; //Middle layer
    private Transform top; //Top layer
    
    /// <summary>
    /// Constructor, loading Canvas and EventSystem
    /// </summary>
    public UIMgr()
    {
        GameObject obj = ResMgr.Instance.Load<GameObject>("UI/Canvas"); //Dynamically load Canvas prefab
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj); //Keep it when changing scenes

        //Get three layers
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");

        obj = ResMgr.Instance.Load<GameObject>("UI/EventSystem"); //Dynamically load EventSystem
        GameObject.DontDestroyOnLoad(obj); //Keep it when changing scenes
    }

    /// <summary>
    /// Get one layer
    /// </summary>
    /// <param name="panelLayer">Name of the layer</param>
    /// <returns></returns>
    public Transform GetPanelLayer(E_PanelLayer panelLayer)
    {
        switch (panelLayer)
        {
            case E_PanelLayer.Bot:
                return bot;
            case E_PanelLayer.Mid:
                return mid;
            case E_PanelLayer.Top:
                return top;
        }
        return null;
    }
    
    
    /// <summary>
    /// Show panel
    /// </summary>
    /// <param name="panelName">Name of the panel</param>
    /// <param name="panelLayer">Panel Layer Bot Mid Top</param>
    /// <param name="afterShown">Function called after panel shown</param>
    /// <typeparam name="T">Panel class</typeparam>
    public void ShowPanel<T>(string panelName, E_PanelLayer panelLayer, UnityAction<T> afterShown = null) where T: BasePanel
    {
        if (panelDict.ContainsKey(panelName)) //If the script is already in dictionary（prevent double loading）
        {
            panelDict[panelName].Show(); //execute show method in BasePanel class
            
            if (afterShown != null)
                afterShown(panelDict[panelName] as T); //If callback function passed, execute logic after the panel shown
            return; //exit the function to avoid repeated logic
        }
        
        
        //Load panel asynchronously
        ResMgr.Instance.LoadAsync<GameObject>("UI/" + panelName, (panel) => //参数为面板obj
        {
            //logic after loading
            
            Transform currPanel = bot; //default layer is bot
            switch (panelLayer)
            {
                case E_PanelLayer.Mid: 
                    currPanel = mid; //set it to mid
                    break;
                case E_PanelLayer.Top:
                    currPanel = top; //set it to top
                    break;
            }

            panel.transform.SetParent(currPanel); //set panel to be child object of selected layer
            
            //Set position and scale to default 
            panel.transform.localPosition = Vector3.zero;
            panel.transform.localScale = Vector3.one;
            
            (panel.transform as RectTransform).offsetMax= Vector2.zero;
            (panel.transform as RectTransform).offsetMin= Vector2.zero;
            
            T panelComponent = panel.GetComponent<T>(); //Get the script on the component

            panelComponent.Show(); //Execute show method in BasePanel class
            
            if (afterShown != null)
                afterShown(panelComponent); //If callback function passed, execute logic after the panel shown
            panelDict.Add(panelName, panelComponent); //Add script to dictionary
        });
    }
    
    /// <summary>
    /// Hide panel
    /// </summary>
    /// <param name="panelName">Name of the panel</param>
    public void HidePanel(string panelName)
    {
        if (panelDict.ContainsKey(panelName))
        {
            panelDict[panelName].Hide(); //Execute hide method in BasePanel class
            GameObject.Destroy(panelDict[panelName].gameObject); //Destroy the shown panel
            panelDict.Remove(panelName); //Remove it from the dictionary
        }
    }

    /// <summary>
    ///Get currently shown panel
    /// </summary>
    /// <param name="panelName">name of the panel</param>
    /// <typeparam name="T">Type of the panel</typeparam>
    public T GetPanel<T>(string panelName) where T: BasePanel
    {
        if (panelDict.ContainsKey(panelName))
            return panelDict[panelName] as T;
        return null;
    }

    
    /// <summary>
    /// Add customized listener of UI component
    /// </summary>
    /// <param name="component">UI component script，Get it using GetControl() method</param>
    /// <param name="triggerType">Type of the event</param>
    /// <param name="eventFunc">Responding method of the event</param>
    public static void AddCustomEventListener(UIBehaviour component, EventTriggerType triggerType, UnityAction<BaseEventData> eventFunc)
    {
        EventTrigger trigger = component.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = component.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = triggerType;
        entry.callback.AddListener(eventFunc);
    }
    
}
