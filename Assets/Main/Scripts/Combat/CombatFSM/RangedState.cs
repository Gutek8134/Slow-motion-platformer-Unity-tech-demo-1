using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedState : State
{
    MainInputActions playerInput;
    Vector3 arrowSpawnOffset = new Vector3(0.5f, 0);
    float slowMotionFactor = 0.1f;//Should probably move it to GM
    
    public override void OnEnter(CombatStateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        //Get this input class
        playerInput = InputManager.playerInput;
        //... and freeze player's movement. No cheating with slo mo!
        playerInput.Ranged.Enable();
        playerInput.Melee.Disable();
        playerInput.Moving.Disable();

        //Setting those functions so they'll be actually firing when needed
        playerInput.Ranged.RangedOutPause.performed += CancelRangedAttackPlusPause;
        playerInput.Ranged.Shoot.performed += Shoot;

        //UI handling
        UIManager.EnterRanged();

        //Crosshair has to start somewhere and not all people will change direction right off the bat
        if (InputManager.inputDevice == Device.Gamepad)
        {
            UIManager.hands.transform.rotation = Quaternion.Euler(0, 0, 0);
            UIManager.crosshair.transform.localPosition = Vector2.right * UIManager.distanceToCrosshair;
        }

        //Slow down time and start to moonwalk
        TimeManager.StartSlowMotion(slowMotionFactor);
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        //Update crosshair position and hands rotation
        if(InputManager.inputDevice == Device.KeyboardAndMouse)
        {
            UIManager.crosshair.transform.position = InputManager.mousePosition;//To mouse current position
        }
        else if (InputManager.inputDevice == Device.Gamepad)
        {   
            //Or at set distance from hands
            Vector2 newCrosshairPosition = playerInput.Ranged.ShotDirectionG.ReadValue<Vector2>().normalized * UIManager.distanceToCrosshair;
            if(newCrosshairPosition != Vector2.zero)
                UIManager.crosshair.transform.localPosition = newCrosshairPosition;
        }
        //Look where you're aiming, idiot!
        LookAt(UIManager.crosshair.transform);
        
    }

    public override void OnExit()
    {
        base.OnExit();

        //It's time for cleaning functions that were added
        playerInput.Ranged.RangedOutPause.performed -= CancelRangedAttackPlusPause;
        playerInput.Ranged.Shoot.performed -= Shoot;

        //You couldn't move? Now you can!
        playerInput.Ranged.Disable();
        playerInput.Melee.Enable();
        playerInput.Moving.Enable();

        //Some UI handling so it doesn't look all weird
        UIManager.ExitRanged();

        //And getting time back right
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
        //If arrow prefab is set, spawn it
        if (CombatManager.arrowPrefab != null)
        {
            GameObject arrow = Object.Instantiate(CombatManager.arrowPrefab, UIManager.hands.transform.position, UIManager.hands.transform.rotation);
            arrow.transform.Translate(arrowSpawnOffset);
        }

        //Get back to doing nothing
        stateMachine.SetNextStateToMain();
    }

    ///<summary>Makes the player character look at the target</summary>
    //TODO: Maybe move it somewhere where it can be accessed easier
    private void LookAt(Transform target)
    {
        Vector2 direction = target.position - UIManager.hands.transform.position;//Where to look at
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//Black math magic
        UIManager.hands.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);//The act of rotating itself
    }
}
