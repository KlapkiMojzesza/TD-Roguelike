﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    bool mouseOnUI = false;
    bool towerSelected = false;
    Vector3 direction;
    float lastFired = 0f;

    private void Start()
    {
        //UIManager.OnMouseEnterUI += MouseOnUI;
        //UIManager.OnMouseEnterUI += MouseNotOnUI;
        TowerManager.OnTowerSelect += TowerSelected;
        TowerManager.OnTowerDeselect += TowerDeselected;
        TowerManager.OnMouseButtonEnter += MouseOnUI;
        TowerManager.OnMouseButtonExit += MouseNotOnUI;
    }

    private void OnDestroy()
    {
        TowerManager.OnTowerSelect -= TowerSelected;
        TowerManager.OnTowerDeselect -= TowerDeselected;
        TowerManager.OnMouseButtonEnter -= MouseOnUI;
        TowerManager.OnMouseButtonExit -= MouseNotOnUI;
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
        if (!mouseOnUI && !towerSelected) return true;

        return false;
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

    private void MouseOnUI()
    {
        mouseOnUI = true;
    }

    private void MouseNotOnUI()
    {
        mouseOnUI = false;
    }

}
