using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Holder class for everything UI-related</summary>
public static class UIManager
{
    public static GameObject hands,
        crosshair;

    ///<value> Information at <see cref="GameManager.distanceToCrosshair"/></value>
    public static float distanceToCrosshair;

    public static void Instantiate() { }

    ///<summary>Manages UI aspect of entering Ranged Mode</summary>
    public static void EnterRanged()
    {
        hands.SetActive(true);
        crosshair.SetActive(true);
    }

    ///<summary>Manages UI aspect of exiting Ranged Mode</summary>
    public static void ExitRanged()
    {
        hands.SetActive(false);
        crosshair.SetActive(false);
    }

    public static void PlayerDied() { }
}
