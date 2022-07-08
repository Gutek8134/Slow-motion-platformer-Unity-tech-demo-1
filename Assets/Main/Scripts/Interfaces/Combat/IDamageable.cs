using UnityEngine;
using UnityEngine.UI;

///<summary>Implement in everything that can take damage</summary>
public interface IDamageable
{
    ///<value>Damageable object's stats</value>
    Stats stats {get; set;}

    ///<value>Health bar object</value>
    //TODO: Change it to slider?
    GameObject HealthBar {get; set;}

    ///<summary>For managing UI aspect of receiving damage</summary>
    void ReceiveDamage(Stats source);

    ///<summary>For managing UI aspect of receiving damage</summary>
    void ReceiveDamage(int damage);

    ///<summary>For updating Health bar</summary>
    void UpdateHP();
    
}
