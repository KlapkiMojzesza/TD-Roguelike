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

    private AudioSource _audioSource;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _playerMaxHealth;
        _healthbarImage.fillAmount = _currentHealth / _playerMaxHealth;

        _audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _healthbarImage.fillAmount = (float)_currentHealth / (float)_playerMaxHealth;
        _audioSource.PlayOneShot(_playerTakeDamageSound);

        if (_currentHealth <= 0)
        {
            HendlePlayerDeath();
        }
    }

    private void HendlePlayerDeath()
    {
        throw new NotImplementedException();
    }
}
