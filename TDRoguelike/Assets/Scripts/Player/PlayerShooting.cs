using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = .5f;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float playerDamage = 50f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] int pierceThroughEnemiesAmount = 1;

    Vector3 direction;

    float lastFired = 0f;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
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
}
