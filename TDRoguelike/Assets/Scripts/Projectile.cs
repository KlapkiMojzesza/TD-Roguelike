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
    int pierceThroughEnemiesAmount = 1;

    public void Create(Transform target, float speed, float damage)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        lastKnownDirection = target.position - transform.position;
    }

    private void Update()
    {
        if (target != null)
        {
            lastKnownDirection = target.position - transform.position;
        }

        transform.Translate(lastKnownDirection.normalized * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (pierceThroughEnemiesAmount > 0)
            {
                pierceThroughEnemiesAmount--;
                collision.gameObject.GetComponent<IDamegeable>().TakeDamage(damage);
            }
            if (pierceThroughEnemiesAmount == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
