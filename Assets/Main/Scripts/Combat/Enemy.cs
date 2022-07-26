using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BehaviourTreeNS;
using Pathfinding;

///<summary>Basic enemy class other should derive from</summary>
[RequireComponent(typeof(Seeker))]
public class Enemy : BehaviourTree, IDamageable
{
    public Stats stats {get; set;}
    ///<value>Health bar object</value>
    [SerializeField] GameObject healthCanvas;
    [SerializeField] Slider healthBar;
    [SerializeField] Image fill;
    [SerializeField] Gradient healthBarColor;
    [SerializeField] int maxHp, damage, armor, armorPen;
    [SerializeField] TMP_Text nameComp;
    [SerializeField] Transform[] patrolPath;
    [SerializeField] LayerMask mask = default;


    #region BTData
    public float speed = 5;
    public float nextWaypointDistance = 0.02f;
    public float repathTime = 0.5f;
    public float lookRange = 1.2f;
    public float chaseDist = 5f;
    public Rigidbody2D rigidbody2d;
    public Seeker seeker;
    #endregion

    void Awake()
    {
        stats = new Stats(maxHp, damage, armor, armorPen, Team.Enemy);
        UpdateHP();
        healthCanvas.SetActive(false);
        rigidbody2d = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
    }

    public void ReceiveDamage(Stats source){
        stats.ReceiveDamage(source);
        UpdateHP();

        if(stats.currentHP <=0){
            Die();
        }
    }

    public void ReceiveDamage(int _damage){
        stats.ReceiveDamage(_damage);
        UpdateHP();

        if(stats.currentHP <=0){
            Die();
        }
    }

    public void UpdateHP(){
        if(!healthCanvas.activeSelf)
            healthCanvas.SetActive(true);
        //If you have some text to display, use it to output data in format:
        //Name: Current HP/Max HP
        if(nameComp != null) nameComp.text = $"{name}: {stats.currentHP}/{stats.maxHp}";

        //Updating fill and color of heath bar
        if(healthBar != null)
        {
            healthBar.value = (float) stats.currentHP/stats.maxHp;
            fill.color = healthBarColor.Evaluate(healthBar.value);
        }
    }

    public void Die(){
        Destroy(gameObject);
    }

    protected override Node SetupTree(){
        return
        new Selector( new List<Node>(){
            new Sequence( new List<Node>(){
                new CheckEnemyInFOV(this, transform, mask),
                new Chase(this, transform),
                }),
            new Patrol(this, transform, patrolPath),
            });
    }
}
