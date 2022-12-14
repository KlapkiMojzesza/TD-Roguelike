using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TowerShooting : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _rotationOffset;

    [Header("To Attach")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _rotatingParts;

    private IUpgradeable _towerUpgrades;
    private Animator _animator;
    private List<GameObject> _aliveEnemies = new List<GameObject>();
    private TowerScriptableObject _towerData;
    private ObjectPool<Projectile> _pool;
    private Transform _target;
    private TargetPriority _targetPriority = TargetPriority.First;
    private float _fireCountdown = 0;

    private void Start()
    {
        EnemyHealth.OnEnemySpawn += AddEnemyToList;
        EnemyHealth.OnEnemyDeath += RemoveEnemyFromList;

        _towerUpgrades = GetComponent<IUpgradeable>();
        _animator = GetComponent<Animator>();

        _towerData = GetComponent<Tower>().TowerData;
        _pool = new ObjectPool<Projectile>(CreateProjectile, OnTakeProjectileFromPool, OnReturnProjectileToPool);

        InvokeRepeating("UpdateTarget", 0f, 0.01f);
    }

    private void OnDestroy()
    {
        EnemyHealth.OnEnemySpawn -= AddEnemyToList;
        EnemyHealth.OnEnemyDeath -= RemoveEnemyFromList;
    }

    private void UpdateTarget()
    {

        switch (_targetPriority)
        {
            case TargetPriority.First:
                _target = GetFirstEnemy(_aliveEnemies);
                break;
            case TargetPriority.Last:
                _target = GetLastEnemy(_aliveEnemies);
                break;
            case TargetPriority.Strongest:
                _target = GetStrongestEnemy(_aliveEnemies);
                break;
            case TargetPriority.Closest:
                _target = GetClosestEnemy(_aliveEnemies);
                break;
        }
    }

    private void Update()
    {
        _fireCountdown -= Time.deltaTime;

        if (_target == null) return;

        RotateToTarget();

        if (_fireCountdown <= 0f)
        {
            Shoot();
            _fireCountdown = 1f / _towerData.TowerFireRate + _towerUpgrades.GetBonusFireRate();
        }

    }

    private void RotateToTarget()
    {
        Vector3 lookDirection = _target.position - transform.position;
        float angle = Mathf.Atan2(lookDirection.x, lookDirection.z) *
                      Mathf.Rad2Deg + _rotationOffset;

        _rotatingParts.transform.rotation = Quaternion.Lerp(_rotatingParts.transform.rotation,
                                                           Quaternion.Euler(0, angle, 0), 
                                                           0.05f);
    }

    private Transform GetFirstEnemy(List<GameObject> enemies)
    {
        Transform firstEnemy = null;

        int currentWaypoint = -1;
        float distanceToWaypoint = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= _towerData.TowerRange + _towerUpgrades.GetBonusRange())
            {
                int enemyCurrentWaypoint = enemy.GetComponent<EnemyMovement>().GetCurrentWaypoint();
                float enemyDistanceToWaypoint = enemy.GetComponent<EnemyMovement>().GetDistanceToNextWaypoint();

                if (enemyCurrentWaypoint < currentWaypoint) continue;
                if (enemyDistanceToWaypoint >= distanceToWaypoint) continue;

                currentWaypoint = enemyCurrentWaypoint;
                distanceToWaypoint = enemyDistanceToWaypoint;

                firstEnemy = enemy.transform;
            }
        }
        return firstEnemy;
    }

    private Transform GetLastEnemy(List<GameObject> enemies)
    {
        Transform lastEnemy = null;

        int currentWaypoint = 10000;
        float distanceToWaypoint = 0;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= _towerData.TowerRange + _towerUpgrades.GetBonusRange())
            {
                int enemyCurrentWaypoint = enemy.GetComponent<EnemyMovement>().GetCurrentWaypoint();

                if (enemyCurrentWaypoint < currentWaypoint)
                {
                    currentWaypoint = enemyCurrentWaypoint;

                    float enemyDistanceToWaypoint = enemy.GetComponent<EnemyMovement>().GetDistanceToNextWaypoint();
                    distanceToWaypoint = enemyDistanceToWaypoint;

                    lastEnemy = enemy.transform;
                }
                else if (enemyCurrentWaypoint == currentWaypoint)
                {
                    float enemyDistanceToWaypoint = enemy.GetComponent<EnemyMovement>().GetDistanceToNextWaypoint();
                    if (enemyDistanceToWaypoint > distanceToWaypoint)
                    {
                        distanceToWaypoint = enemyDistanceToWaypoint;
                        lastEnemy = enemy.transform;
                    }
                }
                else continue;   
            }
        }
        return lastEnemy;
    }

    private Transform GetStrongestEnemy(List<GameObject> enemies)
    {
        Transform strongestEnemy = null;
        float strongestEnemyStrength = 0;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= _towerData.TowerRange + _towerUpgrades.GetBonusRange())
            {
                float enemyStrength = enemy.GetComponent<EnemyHealth>().EnemyStrength;
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
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= _towerData.TowerRange + _towerUpgrades.GetBonusRange())
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }
        return closestEnemy;
    }

    private void Shoot()
    {
        Projectile projectile = _pool.Get();
        projectile.gameObject.transform.position = _firePoint.position;

        if (projectile != null)
        {
            projectile.Create(_target.gameObject.GetComponent<EnemyHealth>().AimPoint,
                              _towerData.ProjectileSpeed, 
                              _towerData.TowerDamage + _towerUpgrades.GetBonusDamage(),
                              _towerData.TowerEnemyPierce + _towerUpgrades.GetBonusPierce());
        }

        _animator.SetTrigger("shoot");
    }

    private void RemoveEnemyFromList(GameObject enemy)
    {
        _aliveEnemies.Remove(enemy);
    }

    private void AddEnemyToList(GameObject enemy)
    {
        _aliveEnemies.Add(enemy);
    }

    public void TowerTargetPrioritySwitch(int index)
    {
        switch(index)
        {
            case 0:
                _targetPriority = TargetPriority.First;
                break;
            case 1:
                _targetPriority = TargetPriority.Strongest;
                break;
            case 2:
                _targetPriority = TargetPriority.Closest;
                break;
            case 3:
                _targetPriority = TargetPriority.Last;
                break;
        }
    }

    private Projectile CreateProjectile()
    {
        var projectile = Instantiate(_towerData.ProjectilePrefab);
        projectile.SetPool(_pool);
        return projectile;
    }

    private void OnTakeProjectileFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void OnReturnProjectileToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _towerData.TowerRange);
    }*/
    
}

public interface IUpgradeable
{
    //getters for TowerShooting
    public float GetBonusDamage();
    public float GetBonusRange();
    public float GetBonusFireRate();
    public int GetBonusPierce();
}

public enum TargetPriority{First = 0, Last = 1, Strongest = 2, Closest = 3}
