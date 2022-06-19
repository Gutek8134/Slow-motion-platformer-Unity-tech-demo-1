using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatController : MonoBehaviour
{
    [SerializeField] Transform pointOfDamage;
    private MainInputActions playerInput;
    BoxCollider2D boxCollider;
    [SerializeField] Vector3[] normalHitAreas = new Vector3[] { new Vector3(0.34f, 0.7f), new Vector3(0.79f, 0.28f) };
    [SerializeField] Vector3[] normalHitOffsets = new Vector3[] {new Vector3(), new Vector3(0.09f, 0.24f), new Vector3(0.09f, -0.21f)};
    SpriteRenderer spriteRenderer;
    bool flipped = false;
    Action lastActionPerformed;
    SidescrollerController sidescrollerController;

    void Awake()
    {
        sidescrollerController = GetComponent<SidescrollerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerInput = new MainInputActions();
        playerInput.Melee.Enable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (pointOfDamage != null)
        {

            Gizmos.DrawWireCube(pointOfDamage.position, normalHitAreas[0]);
            Gizmos.DrawWireCube(pointOfDamage.position + normalHitOffsets[1], normalHitAreas[1]);
            Gizmos.DrawWireCube(pointOfDamage.position + normalHitOffsets[2], normalHitAreas[1]);
        }
    }
    private void Update()
    {
        if((spriteRenderer.flipX && !flipped)||(!spriteRenderer.flipX && flipped))
        {
            FlipAttackZones();
        }
    }

    private void FlipAttackZones()
    {
        for (int i = 0; i < normalHitAreas.Length; ++i)
        {
            normalHitAreas[i].x = -normalHitAreas[i].x;
        }
        for (int i = 0; i < normalHitOffsets.Length; ++i)
        {
            normalHitOffsets[i].x = -normalHitOffsets[i].x;
        }
        pointOfDamage.localPosition = new Vector3(-pointOfDamage.localPosition.x, pointOfDamage.localPosition.y, pointOfDamage.localPosition.z);
        flipped = !flipped;
    }

    private void Attack()
    {
        switch (lastActionPerformed)
        {
            default:
                StartCoroutine(Attack1());
                break;
            case Action.Attack:
                StartCoroutine(Attack2());
                break;
            case Action.Attack2:
                StartCoroutine(Attack3());
                break;
        }

    }
    
    //First attack in three-hit combo
    private IEnumerator Attack1()
    {
        //This is a place for triggering attack animation
        yield return new WaitForSeconds(5);
    }

    //Second attack in three-hit combo
    private IEnumerator Attack2()
    {
        //This is a place for triggering attack animation
        yield return new WaitForSeconds(5);
    }

    //Third attack in three-hit combo
    private IEnumerator Attack3()
    {
        //This is a place for triggering attack animation
        yield return new WaitForSeconds(5);
    }

    private void SwitchCombatMode()
    {
        if (playerInput.Melee.enabled)
        {
            playerInput.Melee.Disable();
            //Some animation?
            playerInput.Ranged.Enable();
        }
        else
        {
            playerInput.Ranged.Disable();
            //Some animation? Timer?
            playerInput.Melee.Enable();
        }
    }

    private void EnterNoCombatZone()
    {
        if (playerInput.Ranged.enabled)
        {
            SwitchCombatMode();
        }
        playerInput.Melee.Disable();
    }

    private void ExitNoCombatZone()
    {
        playerInput.Melee.Enable();
    }

    private enum Action
    {
        Dash,
        Attack,
        Attack2,
        Attack3,
        Jump
    }
}
