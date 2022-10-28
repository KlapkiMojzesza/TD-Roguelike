using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    float speed;
    float damage;
    Vector3 targetPosition;
    Vector3 lastKnownDirection;
    int eniemiesHitAmount = 1;

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
            targetPosition = target.position;

            lastKnownDirection = targetPosition - transform.position;

            /*if (lastKnownDirection.magnitude <= speed * Time.deltaTime)
            {
                HitTarget();
                return;
            }*/
        }

        transform.Translate(lastKnownDirection.normalized * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (eniemiesHitAmount <= 0) return;
            collision.gameObject.GetComponent<IDamegeable>().TakeDamage(damage);
            eniemiesHitAmount--;
            Destroy(gameObject);
            return;
        }
    }

    private void HitTarget()
    {
        if (target != null) target.GetComponent<IDamegeable>().TakeDamage(damage);

        Destroy(gameObject);
        return;
    }
}
