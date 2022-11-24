using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerBase : MonoBehaviour
{
    [SerializeField] private float _startBaseHealth = 100;
    [SerializeField] private TMP_Text _baseHealthText;

    private float _baseHealth;
    public static event Action OnBaseDestroyed;

    private void Start()
    {
        EnemyMovement.OnEnemyPathCompleate += TakeDamage;
        _baseHealth = _startBaseHealth;
        _baseHealthText.text = _baseHealth.ToString();
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

        _baseHealthText.text = _baseHealth.ToString();
    }

    private void DestroyPlayerBase()
    {
        OnBaseDestroyed?.Invoke();
        _baseHealth = _startBaseHealth;
        _baseHealthText.text = _baseHealth.ToString();
    }
}
