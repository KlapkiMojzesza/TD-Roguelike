using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerBase : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _startBaseHealth = 100;

    [Header("To Attach")]
    [SerializeField] private AudioClip _gameOverSound;
    [SerializeField] private TMP_Text _healthAmountText;

    private AudioSource _audioSource;
    private float _currentBaseHealth;

    public static event Action OnBaseDestroyed;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        EnemyMovement.OnEnemyPathCompleate += TakeDamage;

        _currentBaseHealth = _startBaseHealth;
        UpdatePlayerBaseHealthUI(_currentBaseHealth);
    }

    private void OnDestroy()
    {
        EnemyMovement.OnEnemyPathCompleate -= TakeDamage;
    }

    public void TakeDamage(float damage)
    {
        _currentBaseHealth -= damage;
        if (_currentBaseHealth <= 0)
        {
            _currentBaseHealth = 0f;
            DestroyPlayerBase();
        }
        UpdatePlayerBaseHealthUI(_currentBaseHealth);
    }

    private void DestroyPlayerBase()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(_gameOverSound);
        OnBaseDestroyed?.Invoke();
        _currentBaseHealth = _startBaseHealth;
    }

    private void UpdatePlayerBaseHealthUI(float value)
    {
        _healthAmountText.text = value.ToString();
    }
}
