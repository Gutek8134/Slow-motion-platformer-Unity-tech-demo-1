using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team: sbyte
{
    None,
    Neutral,
    Player,
    Enemy
}

[System.Serializable]
public class Stats
{
    public int maxHp;
    public int currentHP;
    public int damage;
    public int armor;
    public int armorPen;
    public Team team;

    public Stats(int _maxHP, int _damage, int _armor, int _armorPen, Team _team)
    {
        maxHp = _maxHP;
        currentHP = maxHp;
        damage = _damage;
        armor = _armor;
        armorPen = _armorPen;
        team = _team;
    }

    public Stats(int _maxHP, int _currentHP, int _damage, int _armor, int _armorPen, Team _team) {
        maxHp = _maxHP;
        currentHP = _currentHP;
        damage = _damage;
        armor = _armor;
        armorPen = _armorPen;
        team = _team;
    }

    public void DealDamage(Stats target)
    {
        target.currentHP -= (int)Mathf.Clamp(damage - (target.armor - armorPen), 0f, float.MaxValue);
        
    }

    public void ReceiveDamage(Stats source)
    {
        currentHP -= (int)Mathf.Clamp(source.damage - (armor - source.armorPen), 0f, float.MaxValue);
    }

    public void ReceiveDamage(int damage)
    {
        currentHP -= (int)Mathf.Clamp((damage - armor), 0f, float.MaxValue);
    }

    public override string ToString()
    {
        return $"Max HP: {maxHp}\nCurrent HP: {currentHP}\nDamage: {damage}\nArmor: {armor}\nArmor penetration: {armorPen}\nTeam: {team}";
    }
}
