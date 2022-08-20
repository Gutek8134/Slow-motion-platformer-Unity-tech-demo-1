using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

///<summary>This class allows player to attack in sequences. Crazy, right?</summary>
[RequireComponent(typeof(CombatStateMachine))]
public class ComboCharacter : MonoBehaviour
{
    public static bool lives { get; private set; } = true;
    private CombatStateMachine stateMachine;

    [SerializeField]
    public Collider2D hitbox; //Seems unused, but that takes probably something of a megabyte, so I'll leave it here as it is
    private MainInputActions playerInput;
    public Stats playerStats;

    [SerializeField]
    TMP_Text nameComp;

    [SerializeField]
    Slider healthBar;

    public Collider2D collider2d;

    void Start()
    {
        UpdateHP();
        //Initializing values, yay!
        CombatManager.EnterCombatArea();
        stateMachine = GetComponent<CombatStateMachine>();
        while (playerInput == null)
        {
            playerInput = InputManager.playerInput;
        }

        //Lambda functions, because I won't delete them
        playerInput.Melee.Attack.performed += context =>
        {
            //Debug.Log("Attack performed");
            if (stateMachine.CurrentState.GetType() == typeof(IdleCombatState))
            {
                //Debug.Log("Setting next state to Melee");
                stateMachine.SetNextState(new MeleeEntryState()); //If you are not attacking, start attacking
            }
        };

        playerInput.AlwaysActive.RangedModeSwitch.performed += context =>
        {
            if (!InputManager.isRanged)
            {
                //You can'e enter ranged mode while attacking
                if (
                    stateMachine.CurrentState.GetType() == typeof(IdleCombatState)
                    && CombatManager.canEnterRanged
                )
                {
                    //Debug.Log("Setting next state to Ranged");
                    stateMachine.SetNextState(new RangedState());
                }
                else
                {
                    Debug.Log(
                        "Cannot enter ranged mode here"
                            + stateMachine.CurrentState
                            + CombatManager.canEnterRanged
                    );
                }
            }
            else
            {
                stateMachine.SetNextStateToMain();
            }

            //This line explains the whole idea of function
            InputManager.isRanged = !InputManager.isRanged;
        };
    }

    public void ReceiveDamage(Stats enemyStats)
    {
        playerStats.ReceiveDamage(enemyStats);
        UpdateHP();

        if (playerStats.currentHP <= 0)
        {
            Die();
        }
    }

    public void UpdateHP()
    {
        //If you have some text to display, use it to output data in format:
        //Name: Current HP/Max HP
        if (nameComp != null)
            nameComp.text = $"HP: {playerStats.currentHP}/{playerStats.maxHp}";

        //Updating fill and color of heath bar
        if (healthBar != null)
        {
            healthBar.value = (float)playerStats.currentHP / playerStats.maxHp;
        }

        if (playerStats.currentHP <= 0)
            Die();
    }

    private void Die()
    {
        ComboCharacter.lives = false;
        Destroy(gameObject);
        UIManager.PlayerDied();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(collider2d.bounds.center, collider2d.bounds.size);
    }
}
