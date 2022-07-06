using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage;
    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision");
        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Triggered");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Entities") || collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                damageable.ReceiveDamage(damage);
            }
            Destroy(gameObject);
        }
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
