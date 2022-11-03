using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooting : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float fireRate = 1f;
    [SerializeField] float towerRange = 50f;
    [SerializeField] float projectileSpeed = 200f;
    [SerializeField] float towerDamage = 10f;
    [SerializeField] string enemyTag = "Enemy";

    [Header("To Attach")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectilePrefab;

    Transform target;
    float fireCountdown = 0;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        target = GetClosestEnemy(enemies);
    }

    private Transform GetClosestEnemy(GameObject[] enemies)
    {
        Transform closestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
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

    private void Update()
    {
        if (target == null) return;

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerRange);
    }

}
