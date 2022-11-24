using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class PlayerShooting : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _fireRate = 2f;
    [SerializeField] private float _projectileSpeed = 200f;
    [SerializeField] private float _playerDamage = 50f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private int _pierceThroughEnemiesAmount = 1;

    [Header("To Attach")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private PlayerProjectile _projectilePrefab;

    private ObjectPool<PlayerProjectile> _pool;
    private Vector3 _direction;
    private float _lastFired = 0f;

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
        if (Time.time - _lastFired > 1 / _fireRate)
        {
            _lastFired = Time.time;

            PlayerProjectile projectile = _pool.Get();
            projectile.gameObject.transform.position = _firePoint.position;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit groundHit;
            if (Physics.Raycast(ray, out groundHit, Mathf.Infinity, _groundLayer))
            {
                _direction = (groundHit.point - transform.position);
            }

            projectile.Create(_direction, _projectileSpeed, _playerDamage, _pierceThroughEnemiesAmount);    
        }
    }

    PlayerProjectile CreateProjectile()
    {
        var projectile = Instantiate(_projectilePrefab);
        projectile.SetPool(_pool);
        return projectile;
    }

    private void OnTakeProjectileFromPool(PlayerProjectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void OnReturnProjectileToPool(PlayerProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
