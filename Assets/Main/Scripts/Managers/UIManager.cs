using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIManager
{
    public static GameObject hands;
    public static GameObject crosshair;
    public static float distanceToCrosshair;
    public static void Instantiate()
    {

    }

    public static void EnterRanged()
    {
        hands.SetActive(true);
        crosshair.SetActive(true);
    }

    public static void ExitRanged()
    {
        hands.SetActive(false);
        crosshair.SetActive(false);
    }
}
