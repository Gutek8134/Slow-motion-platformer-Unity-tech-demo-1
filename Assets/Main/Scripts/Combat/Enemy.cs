using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour, IDamageable
{
    public Stats stats {get; set;}
    public GameObject HealthBar {get; set;}
    [SerializeField] int maxHp, damage, armor, armorPen;
    [SerializeField] TMP_Text nameComp;
    void Awake()
    {
        stats = new Stats(maxHp, damage, armor, armorPen, Team.Enemy);
        UpdateHP();
    }

    public void ReceiveDamage(Stats source){
        source.DealDamage(stats);
        UpdateHP();
    }

    public void ReceiveDamage(int _damage){
        stats.ReceiveDamage(_damage);
        UpdateHP();
    }

    public void UpdateHP(){
        if(nameComp != null) nameComp.text = $"{name}: {stats.currentHP}/{stats.maxHp}";
    }
}
