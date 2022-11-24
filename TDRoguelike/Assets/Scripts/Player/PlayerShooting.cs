using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class PlayerShooting : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float fireRate = 2f;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float playerDamage = 50f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] int pierceThroughEnemiesAmount = 1;

    [Header("To Attach")]
    [SerializeField] Transform firePoint;
    [SerializeField] PlayerProjectile projectilePrefab;

    private ObjectPool<PlayerProjectile> _pool;
    Vector3 direction;
    float lastFired = 0f;

    private void Start()
    {
        _pool = new ObjectPool<PlayerProjectile>(CreateProjectile, OnTakeProjectileFromPool, OnReturnProjectileToPool);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!IsMouseOverUI()) Shoot();
        }
    }

    public void Shoot()
    {
        if (Time.time - lastFired > 1 / fireRate)
        {
            lastFired = Time.time;

            PlayerProjectile projectile = _pool.Get();
            projectile.gameObject.transform.position = firePoint.position;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit groundHit;
            if (Physics.Raycast(ray, out groundHit, Mathf.Infinity, groundLayer))
            {
                direction = (groundHit.point - transform.position);
            }

            projectile.Create(direction, projectileSpeed, playerDamage, pierceThroughEnemiesAmount);    
        }
    }

    PlayerProjectile CreateProjectile()
    {
        var projectile = Instantiate(projectilePrefab);
        projectile.SetPool(_pool);
        return projectile;
    }

    private void OnTakeProjectileFromPool(PlayerProjectile bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnProjectileToPool(PlayerProjectile bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
