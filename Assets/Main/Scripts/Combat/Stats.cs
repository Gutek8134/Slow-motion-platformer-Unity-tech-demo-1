using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Enum for entity groups</summary>
public enum Team : sbyte
{
    None,
    Neutral,
    Player,
    Enemy
}

///<summary>Holds statistics for every entity, required in classes contracting <c>IDamageable</c></summary>
///<remarks>Not made a struct to be able to catch errors in other classes by <c>null</c> comparison</remarks>
[System.Serializable]
public class Stats
{
    public int maxHp,
        currentHP,
        damage,
        armor,
        armorPen;
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

    public Stats(int _maxHP, int _currentHP, int _damage, int _armor, int _armorPen, Team _team)
    {
        maxHp = _maxHP;
        currentHP = _currentHP;
        damage = _damage;
        armor = _armor;
        armorPen = _armorPen;
        team = _team;
    }

    public void DealDamage(Stats target)
    {
        target.ReceiveDamage(this);
    }

    public void ReceiveDamage(Stats source)
    {
        currentHP -= (int)Mathf.Clamp(
            source.damage - (armor - source.armorPen),
            1f,
            float.MaxValue
        );
        currentHP = Mathf.Clamp(currentHP, 0, int.MaxValue);
    }

    public void ReceiveDamage(int _damage)
    {
        currentHP -= (int)Mathf.Clamp((_damage - armor), 1f, float.MaxValue);
        currentHP = Mathf.Clamp(currentHP, 0, int.MaxValue);
    }

    public void Heal(int _value)
    {
        currentHP += _value;
        if (currentHP > maxHp)
            currentHP = maxHp;
    }

    ///<returns>A string with all of the instances properties </returns>
    public override string ToString()
    {
        return $"Max HP: {maxHp}\nCurrent HP: {currentHP}\nDamage: {damage}\nArmor: {armor}\nArmor penetration: {armorPen}\nTeam: {team}";
    }
}
