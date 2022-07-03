using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CombatStateMachine))]
public class ComboCharacter : MonoBehaviour
{
    private CombatStateMachine stateMachine;
    [SerializeField] public Collider2D hitbox;
    private MainInputActions playerInput;
    public Stats playerStats;

    // Start is called before the first frame update
    void Awake()
    {
        CombatManager.EnterCombatArea();
        stateMachine = GetComponent<CombatStateMachine>();
        while (playerInput == null)
        {
            playerInput = InputManager.playerInput;
        }

        playerInput.Melee.Attack.performed += context => {
            //Debug.Log("Attack performed");
            if (stateMachine.CurrentState.GetType() == typeof(IdleCombatState))
            {
                //Debug.Log("Setting next state to Melee");
                stateMachine.SetNextState(new MeleeEntryState());
            }
        };

        playerInput.AlwaysActive.RangedModeSwitch.performed += context =>
        {
            if (!InputManager.isRanged)
            {
                if (stateMachine.CurrentState.GetType() == typeof(IdleCombatState) && CombatManager.canEnterRanged)
                {
                    //Debug.Log("Setting next state to Ranged");
                    stateMachine.SetNextState(new RangedState());
                }
                else
                {
                    Debug.Log("Cannot enter ranged mode here");
                }
            }
            else
            {
                stateMachine.SetNextStateToMain();
            }
            InputManager.isRanged = !InputManager.isRanged;
        };
    }
}
