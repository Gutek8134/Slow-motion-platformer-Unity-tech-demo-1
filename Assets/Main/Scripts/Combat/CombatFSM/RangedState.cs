using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedState : State
{
    MainInputActions playerInput;
    Vector3 arrowSpawnOffset = new Vector3(0.5f, 0);
    float slowMotionFactor = 0.1f;
    
    public override void OnEnter(CombatStateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        playerInput = InputManager.playerInput;
        playerInput.Ranged.Enable();
        playerInput.Melee.Disable();
        playerInput.Moving.Disable();

        playerInput.Ranged.RangedOutPause.performed += CancelRangedAttackPlusPause;
        playerInput.Ranged.Shoot.performed += Shoot;

        UIManager.EnterRanged();

        if (InputManager.inputDevice == Device.Gamepad)
        {
            UIManager.hands.transform.rotation = Quaternion.Euler(0, 0, 0);
            UIManager.crosshair.transform.localPosition = Vector2.right * UIManager.distanceToCrosshair;
        }

        TimeManager.StartSlowMotion(slowMotionFactor);
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        //Update crosshair position and hands rotation
        if(InputManager.inputDevice == Device.KeyboardAndMouse)
        {
            UIManager.crosshair.transform.position = InputManager.mousePosition;
        }
        else if (InputManager.inputDevice == Device.Gamepad)
        {
            Vector2 newCrosshairPosition = playerInput.Ranged.ShotDirectionG.ReadValue<Vector2>().normalized * UIManager.distanceToCrosshair;
            if(newCrosshairPosition != Vector2.zero)
                UIManager.crosshair.transform.localPosition = newCrosshairPosition;
        }
        LookAt(UIManager.crosshair.transform);
        
    }

    public override void OnExit()
    {
        base.OnExit();

        playerInput.Ranged.RangedOutPause.performed -= CancelRangedAttackPlusPause;
        playerInput.Ranged.Shoot.performed -= Shoot;

        playerInput.Ranged.Disable();
        playerInput.Melee.Enable();
        playerInput.Moving.Enable();

        UIManager.ExitRanged();

        TimeManager.StopSlowMotion();
    }

    private void CancelRangedAttackPlusPause(InputAction.CallbackContext context)
    {
        CombatManager.CancelRangedAttack();
        stateMachine.SetNextStateToMain();
        //Pause
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (CombatManager.arrowPrefab != null)
        {
            GameObject arrow = Object.Instantiate(CombatManager.arrowPrefab, UIManager.hands.transform.position, UIManager.hands.transform.rotation);
            arrow.transform.Translate(arrowSpawnOffset);
        }

        stateMachine.SetNextStateToMain();
    }

    private void LookAt(Transform target)
    {
        Vector2 direction = target.position - UIManager.hands.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        UIManager.hands.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
