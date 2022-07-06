using UnityEngine;
using UnityEngine.UI;

public interface IDamageable
{
    Stats stats {get; set;}
    void ReceiveDamage(Stats source);
    void ReceiveDamage(int damage);
    void UpdateHP();
    GameObject HealthBar {get; set;}
}
