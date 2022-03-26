using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Input manager
//Input manager is based on event center and public mono manager
//By using listeners, you can control any gameObjects

//You dont have to call any methods of this script.
//Instead, you need to add the listeners of "KeyIsPressed", "KeyIsReleased", and "KeyIsHeld"

//If you want to add new controls, just add new key value pairs inside the dict
//(I may add methods to read from files, not implemented yet)
#endregion
public class InputMgr : Singleton<InputMgr>
{
    public Dictionary<string, KeyCode> KeySet = new Dictionary<string, KeyCode>() //dict of all the controls
    {
        {"up", KeyCode.W},
        {"down",KeyCode.S},
        {"left", KeyCode.A},
        {"right", KeyCode.D},
        {"jump",KeyCode.K},
        {"dash", KeyCode.L},
        {"attack", KeyCode.J}
    };

    private bool isSwitchOn = false; //flag to open the global check
    public InputMgr() //Constructor, uses public mono manager to open Update function
    {
        MonoMgr.Instance.AddUpdateListener(InputUpdate);
    }

    private void InputUpdate() //The logic in update method
    {
        if (isSwitchOn != true) return;
        CheckKey(KeySet["up"]);
        CheckKey(KeySet["down"]);
        CheckKey(KeySet["left"]);
        CheckKey(KeySet["right"]);
        CheckKey(KeySet["jump"]);
        CheckKey(KeySet["dash"]);
        CheckKey(KeySet["attack"]);
        
    }
    
    /// <summary>
    ///Change key sets
    /// </summary>
    /// <param name="act">the action you want to change</param>
    /// <param name="newKey">the new key you want to change to</param>
    public void ChangeKey(string act, KeyCode newKey)
    {
         KeySet[act] = newKey;
    }
    
    private void CheckKey(KeyCode key) //check if key is pressed or released, only trigger event
    {
        if (Input.GetKeyDown(key)) //press key
        {
            EventMgr.Instance.EventTrigger("KeyIsPressed", key);
        }
        if (Input.GetKeyUp(key)) //release key
        {
            EventMgr.Instance.EventTrigger("KeyIsReleased", key);
        }

        if (Input.GetKey(key)) //hold key
        {
            EventMgr.Instance.EventTrigger("KeyIsHeld", key);
        }
    }

    /// <summary>
    ///Open or close global check
    /// </summary>
    /// <param name="state">open or close</param>
    public void SwitchAllButtons(bool state)
    {
        isSwitchOn = state;
    }
}



    