using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _playerMaxHealth = 100;

    [Header("To Attach")]
    [SerializeField] private Image _healthbarImage;
    [SerializeField] private AudioClip _playerTakeDamageSound;
    [SerializeField] private ParticleSystem _hitParticle;

    public static event Action OnPlayerDeath;

    private AudioSource _audioSource;
    private int _currentHealth;
    private bool isAlive = true;

    public static GameObject PlayerInstance;

    private void Awake()
    {
        if (PlayerInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            PlayerInstance = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        LevelLoaderManager.OnMainMenuLoading += DestoryPlayer;
        LevelLoaderManager.OnSceneLoaded += MovePlayerToSpawnPoint;

        _currentHealth = _playerMaxHealth;
        _healthbarImage.fillAmount = _currentHealth / _playerMaxHealth;

        _audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        if (PlayerInstance != this.gameObject) return;

        LevelLoaderManager.OnMainMenuLoading -= DestoryPlayer;
        LevelLoaderManager.OnSceneLoaded -= MovePlayerToSpawnPoint;
    }

    private void MovePlayerToSpawnPoint()
    {
        gameObject.SetActive(false);
        PlayerBase playerBase = (PlayerBase)FindObjectOfType(typeof(PlayerBase));
        if (playerBase == null) return;
        transform.position = playerBase.GetPlayerSpawnPoint();
        gameObject.SetActive(true);
    }

    private void DestoryPlayer()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;

        _currentHealth -= damage;
        _healthbarImage.fillAmount = (float)_currentHealth / (float)_playerMaxHealth;
        _audioSource.PlayOneShot(_playerTakeDamageSound);
        if (_hitParticle != null) _hitParticle.Play();

        if (_currentHealth <= 0)
        {
            HandlePlayerDeath();
        }
    }

    private void HandlePlayerDeath()
    {
        OnPlayerDeath?.Invoke();
        isAlive = false;
    }

    public void EndGame()
    {
        //this is called from player death animation event
        PlayerBase playerBase = (PlayerBase)FindObjectOfType(typeof(PlayerBase));
        playerBase.DestroyPlayerBase();
    }
}
