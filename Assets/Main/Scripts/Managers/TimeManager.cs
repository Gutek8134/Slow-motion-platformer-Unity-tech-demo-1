using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Holder class for internal game's time and speed data and functions</summary>
public static class TimeManager
{

    private static float originalFixedDeltaTime;
    ///<summary>Stores original FDT for later usage</summary>
    public static void Instantiate()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;//This Prevents slow motion in Game Editor.
    }
    ///<summary>Slows down game time by given factor</summary>
    ///<remarks>The lower the factor, the slower the time</remarks>
    //TODO: maybe create some enum for this thing?
    public static void StartSlowMotion(float slowDownFactor)
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * originalFixedDeltaTime;//Adjust Physics - Slowdown by the same factor as TimeScale
    }
    ///<summary>Set the time's flow to default</summary>
    //TODO: setting up everything so it can go well with custom values
    public static void StopSlowMotion()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }
}
