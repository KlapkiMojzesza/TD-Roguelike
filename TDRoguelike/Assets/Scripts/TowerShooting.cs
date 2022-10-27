using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooting : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float fireRate;
    [SerializeField] float towerRange;
    [SerializeField] float bulletSpeed;
    [SerializeField] float damage;

    [Header("To Attach")]
    [SerializeField] Transform firePoint;

    GameObject[] enemies;

    private void Update()
    {
        
    }

    


    /*public GameObject GetClosestEnemy(Vector3 position, float towerRange)
    {
        GameObject closest = null;
        foreach (GameObject enemy in enemies)
        {
            if ()
        }
    }*/

    /*
    public class Equalizer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float fireRate;
    [SerializeField] float bulletSpeed;
    [SerializeField] float damage;

    [Header("To Attach")]
    [SerializeField] GameObjectsPool bulletsPool;
    [SerializeField] GameObject movingParts;
    [SerializeField] Transform firePoint;
    [SerializeField] AudioClip shootSound;

    Animator animator;
    AudioSource audioSource;
    GameManager gameManager;
    GameObject player;
    GameObject[] enemies;
    float lastFired = 0f;

    private void Start()
    {
        var sameTraps = GameObject.FindGameObjectsWithTag("Equalizer");
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        if (sameTraps.Length > 1)
        {
            audioSource.enabled = false;
        }
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = gameManager.GetPlayer();
    }

    private void Update()
    {
        if (gameManager.GetCurrentState() == GameState.CombatPhase)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject target = GetClosestEnemy(GameObject.FindGameObjectWithTag("Player").transform, enemies);
            if (target != null)
            {
                Rotate(target);
                Shoot(target);
            }

        }
    }

    private void Shoot(GameObject target)
    {
        if (Time.time - lastFired > 1 / fireRate)
        {
            animator.SetTrigger("shoot");
            audioSource.PlayOneShot(shootSound);
            lastFired = Time.time;
            var bullet = bulletsPool.GetBullet();
            bullet.GetComponent<Bullet>().SetBulletDamage(damage);
            bullet.transform.rotation = firePoint.transform.rotation;
            bullet.transform.position = firePoint.transform.position;
            bullet.gameObject.SetActive(true);

            bullet.GetComponent<FollowingBullet>().CreateBullet(bulletSpeed, target);  
        }
    }

    private void Rotate(GameObject enemy)
    {
        Vector3 direction = enemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        movingParts.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    GameObject GetClosestEnemy(Transform player, GameObject[] enemies)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 playerPosition = player.position;
        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - playerPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
     */
}
