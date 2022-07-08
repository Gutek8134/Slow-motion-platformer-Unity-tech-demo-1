using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    ///<value>Speed at which arrow traverses</value>
    [SerializeField] float speed;
    ///<value>Base damage arrow inflicts</value>
    [SerializeField] int damage;

    private void Update()
    {
        //Move arrow forward
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision");
        
        //If arrow hits a collider (although it shouldn't), destroys itself
        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Triggered");

        //If arrow hits an entity or environment element, it destroys itself
        if (collision.gameObject.layer == LayerMask.NameToLayer("Entities") || collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            //In case that thing is damageable, it is dealt damage
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                damageable.ReceiveDamage(damage);
            }
            Destroy(gameObject);
        }

        //Random debug code
        /*
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("hit ground");
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Debug.Log("hit walls");
        }*/
    }
}
