using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#region Timing Manager
//Timing manager more conveniently times for a function by using coroutine

//StartFuncTimer method counts down for a function, and do something after the count down
//StartTimer straightly counts down for certain time, and do something after the count down 
#endregion
public class TimeMgr : Singleton<TimeMgr>
{
    /// <summary>
    /// Count down for a function
    /// </summary>
    /// <param name="TimeToWait">Time between two functions in seconds</param>
    /// <param name="TimeFunc">Function to be timed</param>
    /// <param name="AfterTime">Function to be called after count down</param>
    public Coroutine StartFuncTimer(float TimeToWait, UnityAction TimeFunc, UnityAction AfterTime)
    {
        Coroutine timerCoroutine = MonoMgr.Instance.StartCoroutine(FuncTimerCoroutine(TimeToWait, TimeFunc, AfterTime));
        return timerCoroutine; //return coroutine for stop
    }

    /// <summary>
    /// Directly count down
    /// </summary>
    /// <param name="TimeToWait">Time of count down in seconds</param>
    /// <param name="AfterTime">Function to be called after count down</param>
    public Coroutine StartTimer(float TimeToWait, UnityAction AfterTime)
    {
        Coroutine timerCoroutine = MonoMgr.Instance.StartCoroutine(TimerCoroutine(TimeToWait, AfterTime));
        return timerCoroutine; //return coroutine for stop
    }

    /// <summary>
    /// Stop count down
    /// stop coroutine in case error happens
    /// </summary>
    /// <param name="coroutineToStop">count down coroutine to be stopped</param>
    public void StopTimer(Coroutine coroutineToStop)
    {
        MonoMgr.Instance.StopCoroutine(coroutineToStop);
    }
    
    
    private IEnumerator TimerCoroutine(float TimeToWait, UnityAction AfterTime) //coroutine for direct count down
    {
        yield return new WaitForSeconds(TimeToWait);
        AfterTime();
    }
    
    private IEnumerator FuncTimerCoroutine(float TimeToWait, UnityAction TimeFunc, UnityAction AfterTime) //coroutine for function count down
    {
        TimeFunc();
        yield return new WaitForSeconds(TimeToWait);
        AfterTime();
    }
}
