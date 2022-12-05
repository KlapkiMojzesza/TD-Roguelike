using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamegeable
{
    [Header("Settings")]
    [SerializeField] private float _health = 100f;
    public float EnemyStrength = 1f;

    [Header("To Attach")]
    public Transform AimPoint;

    public static event Action<GameObject> OnEnemySpawn;
    public static event Action<GameObject> OnEnemyDeath;

    private void Start()
    {
        OnEnemySpawn?.Invoke(gameObject);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
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
