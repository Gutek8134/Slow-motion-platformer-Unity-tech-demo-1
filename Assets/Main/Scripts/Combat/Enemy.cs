using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Stats stats;
    [SerializeField] int maxHp, damage, armor, armorPen;
    void Awake()
    {
        stats = new Stats(maxHp, damage, armor, armorPen, Team.Enemy);
    }
}
