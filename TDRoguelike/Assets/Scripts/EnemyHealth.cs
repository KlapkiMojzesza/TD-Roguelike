using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamegeable
{
    [Header("Settings")]
    [SerializeField] float health;

    [Header("To Attach")]
    public Transform aimPoint;

    public static event Action<GameObject> OnEnemyDeath;

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
