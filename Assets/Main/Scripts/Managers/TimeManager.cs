using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeManager
{
    private static float originalFixedDeltaTime;
    public static void Instantiate()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;//This Prevents slow motion in Game Editor.
    }
    public static void StartSlowMotion(float slowDownFactor)
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * originalFixedDeltaTime;//Adjust Physics - Slowdown by the same factor as TimeScale
    }

    public static void StopSlowMotion()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }
}
