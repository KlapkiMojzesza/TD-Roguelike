using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    float speed;
    float damage;
    Vector3 lastKnownDirection;
    int enemyPierce = 1;

    public void Create(Transform target, float speed, float damage, int enemyPierce)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.enemyPierce = enemyPierce;
        lastKnownDirection = target.position - transform.position;
    }

    private void Update()
    {
        if (target != null)
        {
            lastKnownDirection = target.position - transform.position;
            if (Vector3.Distance(transform.position, target.position) > 0.01f) target = null;
        }

        transform.Translate(lastKnownDirection.normalized * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (enemyPierce > 0)
            {
                enemyPierce--;
                collision.gameObject.GetComponent<IDamegeable>().TakeDamage(damage);
            }
            if (enemyPierce == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
