using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    public int maxHp;
    public int currentHP;
    public int damage;
    public int armor;
    public int armorPen;

    public Stats(int _maxHP, int _damage, int _armor, int _armorPen)
    {
        maxHp = _maxHP;
        currentHP = maxHp;
        damage = _damage;
        armor = _armor;
        armorPen = _armorPen;
    }

    public Stats(int _maxHP, int _currentHP, int _damage, int _armor, int _armorPen) {
        maxHp = _maxHP;
        currentHP = _currentHP;
        damage = _damage;
        armor = _armor;
        armorPen = _armorPen;
    }

    public void DealDamage(Stats target)
    {
        target.currentHP -= damage - (target.armor - armorPen);
    }

    public void ReceiveDamage(Stats source)
    {
        currentHP = source.damage - (armor - source.armorPen);
    }
}
