using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Holder class for combat-related data</summary>
public static class CombatManager
{
    ///<value>Holds player statistics</value>
    public static Stats playerStats
    {
        get => player.GetComponent<ComboCharacter>().playerStats;
    }

    ///<value>Holds the player itself</value>
    public static GameObject player = null;

    ///<value>Used to determine whether player can or cannot enter ranged mode at given moment</value>
    public static bool canEnterRanged;

    ///<value>Prefab for arrow to use in RangedMode, since it cannot be defined there</value>
    public static GameObject arrowPrefab;

    ///<summary>Sets values for the class</summary>
    public static void Instantiate(GameObject _player, GameObject _arrowPrefab)
    {
        player = _player;
        arrowPrefab = _arrowPrefab;
    }

    ///<summary>To be called upon entering an area with combat disabled</summary>
    public static void ExitCombatArea()
    {
        canEnterRanged = false;
        player.GetComponent<CombatStateMachine>().SetNextStateToMain();
        InputManager.playerInput.Melee.Disable();
    }

    ///<summary>To be called upon entering an area where player can attack</summary>
    public static void EnterCombatArea()
    {
        canEnterRanged = true;
        InputManager.playerInput.Melee.Enable();
    }

    ///<summary>Cancels the ranged attack. Made it exterior for rare occasions when player enters a cutscene or an area with combat disabled in Ranged Mode.</summary>
    public static void CancelRangedAttack()
    {
        InputManager.playerInput.Ranged.Disable();
        InputManager.playerInput.Melee.Enable();
        InputManager.playerInput.Moving.Enable();
    }
}
