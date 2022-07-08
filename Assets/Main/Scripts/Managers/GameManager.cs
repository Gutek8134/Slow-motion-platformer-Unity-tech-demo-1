using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

///<summary>Initializes other (static) managers and calls their Behaviour-related functions</summary>
public class GameManager : MonoBehaviour
{
    ///<value>Ranged Mode-related GameObjects</value>
    [SerializeField] GameObject hands, crosshair;
    ///<value>Value used for adjusting distance between hands and crosshair</value>
    [SerializeField] float distanceToCrosshair = 2;
    ///<value>Reference to currently used camera</value>
    [SerializeField] new Camera camera;

    ///<value>Combat Manager related GameObjects</value>
    [SerializeField] GameObject player, arrowPrefab;
    
    ///<value>Player's stats</value>
    //TODO: change it to read Stats from JSON
    [SerializeField] int maxHp, damage, armor, armorPen;
    
    void Awake()
    {
        //At first, make sure the process won't be interrupted by changing scenes
        DontDestroyOnLoad(this);
        //Secondly, call other Managers' methods for setting their values
        TimeManager.Instantiate();
        InputManager.Instantiate(camera);
        CombatManager.Instantiate(player, new Stats(maxHp, damage, armor, armorPen, Team.Player), arrowPrefab);
        UIManager.Instantiate();

        //UIManager is treated specially, because it may have way too many values to set in one function call
        UIManager.hands = hands;
        UIManager.crosshair = crosshair;
        UIManager.distanceToCrosshair = distanceToCrosshair;

        //Turn hands and crosshair visibility off, as they are only needed in Ranged Mode
        UIManager.ExitRanged();
    }

    private void Update()
    {
        //Call Update-related functions
        InputManager.UpdateMousePosition();
    }
}
