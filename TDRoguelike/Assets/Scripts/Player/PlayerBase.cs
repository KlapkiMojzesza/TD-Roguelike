using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerBase : MonoBehaviour
{
    [SerializeField] private float _startBaseHealth = 100;

    private float _baseHealth;

    public static event Action OnBaseDestroyed;
    public static event Action<float> OnBaseTakeDamage;

    private void Start()
    {
        EnemyMovement.OnEnemyPathCompleate += TakeDamage;
        _baseHealth = _startBaseHealth;
        TakeDamage(0); //tower manager set player base health start value
    }

    private void OnDestroy()
    {
        EnemyMovement.OnEnemyPathCompleate -= TakeDamage;
    }

    public void TakeDamage(float damage)
    {
        _baseHealth -= damage;
        if (_baseHealth <= 0)
        {
            _baseHealth = 0f;
            DestroyPlayerBase();
        }

        OnBaseTakeDamage?.Invoke(_baseHealth);
    }

    private void DestroyPlayerBase()
    {
        OnBaseDestroyed?.Invoke();
        _baseHealth = _startBaseHealth;
    }
}
