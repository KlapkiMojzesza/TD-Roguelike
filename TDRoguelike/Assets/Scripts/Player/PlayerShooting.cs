using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] GameObject projectilePrefab;

    bool towerSelected = false;
    Vector3 direction;
    float lastFired = 0f;

    private void Start()
    {
        TowerManager.OnTowerSelect += TowerSelected;
        TowerManager.OnTowerDeselect += TowerDeselected;
    }

    private void OnDestroy()
    {
        TowerManager.OnTowerSelect -= TowerSelected;
        TowerManager.OnTowerDeselect -= TowerDeselected;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (CanShoot()) Shoot();
        }
    }

    private bool CanShoot()
    {
        if (!IsMouseOverUI() && !towerSelected) return true;

        return false;
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void Shoot()
    {
        if (Time.time - lastFired > 1 / fireRate)
        {
            lastFired = Time.time;
            GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            PlayerProjectile projectile = projectileObject.GetComponent<PlayerProjectile>();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit groundHit;
            if (Physics.Raycast(ray, out groundHit, Mathf.Infinity, groundLayer))
            {
                direction = (groundHit.point - transform.position);
            }

            projectile.Create(direction, projectileSpeed, playerDamage, pierceThroughEnemiesAmount);

            
        }
    }

    private void TowerDeselected()
    {
        towerSelected = false;
    }

    private void TowerSelected()
    {
        towerSelected = true;
    }
}
