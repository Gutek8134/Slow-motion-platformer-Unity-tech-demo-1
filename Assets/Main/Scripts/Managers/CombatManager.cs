using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatManager
{
    public static Stats playerStats { get; private set; }
    public static GameObject player = null;
    public static bool canEnterRanged;
    public static GameObject arrowPrefab;

    public static void Instantiate(GameObject _player, Stats stats, GameObject _arrowPrefab)
    {
        player = _player;
        playerStats = stats;
        arrowPrefab = _arrowPrefab;
    }

    public static void ExitCombatArea()
    {
        canEnterRanged = false;
        player.GetComponent<CombatStateMachine>().SetNextStateToMain();
        InputManager.playerInput.Melee.Disable();
    }

    public static void EnterCombatArea()
    {
        canEnterRanged = true;
        InputManager.playerInput.Melee.Enable();
    }

    public static void CancelRangedAttack()
    {
        InputManager.playerInput.Ranged.Disable();
        InputManager.playerInput.Melee.Enable();
        InputManager.playerInput.Moving.Enable();
    }
}
