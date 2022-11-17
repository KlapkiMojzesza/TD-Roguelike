using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooting : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float rotationOffset;

    [Header("To Attach")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject rotationParts;

    List<GameObject> aliveEnemies = new List<GameObject>();
    TowerScriptableObject towerData;
    Transform target;
    TargetPriority targetPriority = TargetPriority.First;
    float fireCountdown = 0;

    private void Start()
    {
        EnemyHealth.OnEnemySpawn += AddEnemyToList;
        EnemyHealth.OnEnemyDeath += RemoveEnemyFromList;
        towerData = GetComponent<Tower>().towerData;
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    private void OnDestroy()
    {
        EnemyHealth.OnEnemySpawn -= AddEnemyToList;
        EnemyHealth.OnEnemyDeath -= RemoveEnemyFromList;
    }

    private void UpdateTarget()
    {
        switch (targetPriority)
        {
            case TargetPriority.First:
                target = GetFirstEnemy(aliveEnemies);
                break;
            case TargetPriority.Last:
                target = GetLastEnemy(aliveEnemies);
                break;
            case TargetPriority.Strongest:
                target = GetStrongestEnemy(aliveEnemies);
                break;
            case TargetPriority.Closest:
                target = GetClosestEnemy(aliveEnemies);
                break;
        }
    }

    private void Update()
    {
        if (target == null) return;

        RotateToTarget();

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / towerData.towerFireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void RotateToTarget()
    {
        Vector3 lookDirection = target.position - transform.position;
        float angle = Mathf.Atan2(lookDirection.x, lookDirection.z) *
                      Mathf.Rad2Deg + rotationOffset;

        rotationParts.transform.rotation = Quaternion.Lerp(rotationParts.transform.rotation,
                                                           Quaternion.Euler(0, angle, 0), 
                                                           0.05f);
    }

    private Transform GetFirstEnemy(List<GameObject> enemies)
    {
        Transform FirstEnemy = null;

        int currentWaypoint = -1;
        float distanceToWaypoint = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= towerData.towerRange)
            {
                int enemyCurrentWaypoint = enemy.GetComponent<EnemyMovement>().GetCurrentWaypoint();
                float enemyDistanceToWaypoint = enemy.GetComponent<EnemyMovement>().GetDistanceToNextWaypoint();

                if (enemyCurrentWaypoint < currentWaypoint) continue;
                if (enemyDistanceToWaypoint >= distanceToWaypoint) continue;

                currentWaypoint = enemyCurrentWaypoint;
                distanceToWaypoint = enemyDistanceToWaypoint;

                FirstEnemy = enemy.transform;
            }
        }
        return FirstEnemy;
    }

    private Transform GetLastEnemy(List<GameObject> enemies)
    {
        Transform LastEnemy = null;

        int currentWaypoint = 10000;
        float distanceToWaypoint = 0;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= towerData.towerRange)
            {
                int enemyCurrentWaypoint = enemy.GetComponent<EnemyMovement>().GetCurrentWaypoint();
                float enemyDistanceToWaypoint = enemy.GetComponent<EnemyMovement>().GetDistanceToNextWaypoint();

                if (enemyCurrentWaypoint > currentWaypoint) continue;
                if (enemyDistanceToWaypoint <= distanceToWaypoint) continue;

                currentWaypoint = enemyCurrentWaypoint;
                distanceToWaypoint = enemyDistanceToWaypoint;

                LastEnemy = enemy.transform;
            }
        }
        return LastEnemy;
    }

    private Transform GetStrongestEnemy(List<GameObject> enemies)
    {
        Transform strongestEnemy = null;
        float strongestEnemyStrength = 0;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= towerData.towerRange)
            {
                float enemyStrength = enemy.GetComponent<EnemyHealth>().enemyStrength;
                if (enemyStrength <= strongestEnemyStrength) continue;
                strongestEnemyStrength = enemyStrength;
                strongestEnemy = enemy.transform;
            }
        }
        return strongestEnemy;
    }

    private Transform GetClosestEnemy(List<GameObject> enemies)
    {
        Transform closestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= towerData.towerRange)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }
        return closestEnemy;
    }

    private void Shoot()
    {
        GameObject projectileObject = Instantiate(towerData.projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Create(target.gameObject.GetComponent<EnemyHealth>().aimPoint,
                              towerData.projectileSpeed, 
                              towerData.towerDamage,
                              towerData.towerEnemyPierce);
        }
    }

    private void RemoveEnemyFromList(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);
    }

    private void AddEnemyToList(GameObject enemy)
    {
        aliveEnemies.Add(enemy);
    }

    public void TowerTargetPrioritySwitch(int index)
    {
        switch(index)
        {
            case 0:
                targetPriority = TargetPriority.First;
                break;
            case 1:
                targetPriority = TargetPriority.Strongest;
                break;
            case 2:
                targetPriority = TargetPriority.Closest;
                break;
            case 3:
                targetPriority = TargetPriority.Last;
                break;
        }
    }

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerData.towerRange);
    }*/

}

public enum TargetPriority{First = 0, Last = 1, Strongest = 2, Closest = 3}
