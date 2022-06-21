using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboCharacter : MonoBehaviour
{
    private CombatStateMachine stateMachine;
    [SerializeField] public Collider2D hitbox;
    private MainInputActions playerInput;
    // Start is called before the first frame update
    void Awake()
    {
        stateMachine = GetComponent<CombatStateMachine>();
        while (playerInput == null)
        {
            playerInput = InputManager.playerInput;
        }
        playerInput.Melee.Attack.performed += Attack_performed;
    }

    private void Attack_performed(InputAction.CallbackContext context)
    {
        //Debug.Log("Attack performed");
        if(stateMachine.CurrentState.GetType() == typeof(IdleCombatState))
        {
            //Debug.Log("Setting next state");
            stateMachine.SetNextState(new MeleeEntryState());
        }
    }
}
