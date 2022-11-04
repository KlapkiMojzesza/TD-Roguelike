using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamegeable
{
    [Header("Settings")]
    [SerializeField] float health;
    public float enemyStrength = 1f;

    [Header("To Attach")]
    public Transform aimPoint;

    public int enemyID;

    public static event Action<GameObject> OnEnemySpawn;
    public static event Action<GameObject> OnEnemyDeath;

    private void Start()
    {
        OnEnemySpawn?.Invoke(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        OnEnemyDeath?.Invoke(gameObject);
    }
}

public interface IDamegeable
{
    void TakeDamage(float damage);
}
