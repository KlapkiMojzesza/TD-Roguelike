using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooting : MonoBehaviour
{
    [Header("Settings")]
    public float towerFireRate = 1f;
    public float towerRange = 50f;
    [SerializeField] float projectileSpeed = 200f;
    public float towerDamage = 10f;
    public TargetPriority targetPriority = TargetPriority.First;

    [Header("To Attach")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectilePrefab;

    [SerializeField] List<GameObject> aliveEnemies = new List<GameObject>();

    Transform target;
    float fireCountdown = 0;

    private void Start()
    {
        EnemyHealth.OnEnemySpawn += AddEnemyToList;
        EnemyHealth.OnEnemyDeath += RemoveEnemyFromList;
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

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / towerFireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private Transform GetFirstEnemy(List<GameObject> enemies)
    {
        Transform FirstEnemy = null;
        int mostAheadEnemyIndex = 100000000;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= towerRange)
            {
                int enemySpawnIndex = enemy.GetComponent<EnemyHealth>().enemyID;
                if (enemySpawnIndex >= mostAheadEnemyIndex) continue;
                mostAheadEnemyIndex = enemySpawnIndex;
                FirstEnemy = enemy.transform;
            }
        }
        return FirstEnemy;
    }

    private Transform GetLastEnemy(List<GameObject> enemies)
    {
        Transform LastEnemy = null;
        int lastEnemyIndex = 0;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= towerRange)
            {
                int enemySpawnIndex = enemy.GetComponent<EnemyHealth>().enemyID;
                if (enemySpawnIndex < lastEnemyIndex) continue;
                lastEnemyIndex = enemySpawnIndex;
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
            if (distanceToEnemy <= towerRange)
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
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= towerRange)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }
        return closestEnemy;
    }

    private void Shoot()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Create(target.gameObject.GetComponent<EnemyHealth>().aimPoint, projectileSpeed, towerDamage);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerRange);
    }

}

public enum TargetPriority{First = 0, Last = 1, Strongest = 2, Closest = 3}
