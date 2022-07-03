using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject hands, crosshair;
    [SerializeField] new Camera camera;
    [SerializeField] GameObject player, arrowPrefab;
    [SerializeField] int maxHp, damage, armor, armorPen;
    [SerializeField] float distanceToCrosshair = 2;
    void Awake()
    {
        DontDestroyOnLoad(this);
        TimeManager.Instantiate();
        InputManager.Instantiate(camera);
        CombatManager.Instantiate(player, new Stats(maxHp, damage, armor, armorPen, Team.Player), arrowPrefab);
        UIManager.Instantiate();

        UIManager.hands = hands;
        UIManager.crosshair = crosshair;
        UIManager.distanceToCrosshair = distanceToCrosshair;

        UIManager.ExitRanged();
    }

    private void Update()
    {
        InputManager.UpdateMousePosition();
    }
}
