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
    [SerializeField] private Transform _playerSpawnPoint; 
    [SerializeField] private AudioClip _gameOverSound;
    [SerializeField] private TMP_Text _healthAmountText;
    [SerializeField] private GameObject _playerBaseCanvas;

    private AudioSource _audioSource;
    private float _currentBaseHealth;
    private bool gameIsOver = false;

    public static event Action OnBaseDestroyed;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        EnemyMovement.OnEnemyPathCompleate += TakeDamage;

        _currentBaseHealth = _startBaseHealth;
        UpdatePlayerBaseHealthUI(_currentBaseHealth);

        PauseManager.OnGamePaused += OnPaused;
        PauseManager.OnGameResumed += OnResumed;
    }

    private void OnDestroy()
    {
        EnemyMovement.OnEnemyPathCompleate -= TakeDamage;
        PauseManager.OnGamePaused -= OnPaused;
        PauseManager.OnGameResumed -= OnResumed;
    }

    private void OnPaused()
    {
        _playerBaseCanvas.SetActive(false);
    }

    private void OnResumed()
    {
        _playerBaseCanvas.SetActive(true);
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

    public void DestroyPlayerBase()
    {
        if (gameIsOver) return;

        _audioSource.Stop();
        _audioSource.PlayOneShot(_gameOverSound);
        
        gameIsOver = true;
        OnBaseDestroyed?.Invoke();
    }

    private void UpdatePlayerBaseHealthUI(float value)
    {
        _healthAmountText.text = value.ToString();
    }

    public Vector3 GetPlayerSpawnPoint()
    {
        return _playerSpawnPoint.position;
    }
}
