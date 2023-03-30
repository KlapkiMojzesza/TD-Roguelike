using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonProjectile : Projectile
{
    [Header("Settings")]
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private int _enemyHit = 0;

    public override void Create(Transform target, float speed, float damage, int enemyPierce, float slowPercentage, float towerRange)
    {
        base.Create(target, speed, damage, enemyPierce, slowPercentage, towerRange);

        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position,  _towerRange, _enemyLayer);
        foreach (Collider enemy in enemiesInRange)
        {
            enemy.GetComponent<EnemyMovement>().PylonSlowEnemy(_slowPercentage);
            if (_enemyHit > 0)
            {
                enemy.GetComponent<IDamegeable>().TakeDamage(_damage);
                _enemyHit--;
            }
        }
    }

    protected override void Update()
    {
        return;
    }
}
