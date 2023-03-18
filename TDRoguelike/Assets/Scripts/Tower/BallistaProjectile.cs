using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaProjectile : Projectile
{
    public override void Create(Transform target, float speed, float damage, int enemyPierce, float slowPercentage, float towerRange)
    {
        base.Create(target, speed, damage, enemyPierce);
        transform.LookAt(target);
    }
    protected override void Update()
    {
        base.Update();
        if (_target != null) transform.LookAt(_target);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") ||
            collision.collider.CompareTag("Obstacle"))
        {
            if (gameObject.activeSelf) _pool.Release(this);
            gameObject.SetActive(false);
            return;
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            _target = null;
            if (_enemyPierce > 0)
            {
                _enemyPierce--;
                collision.gameObject.GetComponent<IDamegeable>().TakeDamage(_damage);
            }
            if (_enemyPierce == 0)
            {
                if (gameObject.activeSelf) _pool.Release(this);
                gameObject.SetActive(false);
            }
        }
    }
}
