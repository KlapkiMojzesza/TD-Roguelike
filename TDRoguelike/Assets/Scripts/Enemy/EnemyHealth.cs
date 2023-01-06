﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamegeable
{
    [Header("Settings")]
    [SerializeField] private float _maxHealth = 100f;
    public float EnemyStrength = 1f;

    [Header("To Attach")]
    [SerializeField] private Transform _healthbarCanvas;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private Image _healthbarImage;
    public Transform AimPoint;

    [Header("Audio")]
    [SerializeField] public AudioClip StartWaveSound;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] public AudioClip DeathSound;

    public static event Action<EnemyHealth> OnEnemySpawn;
    public static event Action<EnemyHealth> OnEnemyDeath;

    private AudioSource _audioSource;
    private Camera _camera;
    private float _currentHealth;

    private void Start()
    {
        OnEnemySpawn?.Invoke(this);

        _camera = Camera.main;
        _audioSource = GetComponent<AudioSource>();

        _currentHealth = _maxHealth;
        UpdateHealthbar(_maxHealth, _currentHealth);
    }

    private void Update()
    {
        RotateHealthbarToCamera();
    }

    private void RotateHealthbarToCamera()
    {
        Vector3 healthbarLookRotation = _healthbarCanvas.position - _camera.transform.position;
        healthbarLookRotation.x = 0;
        _healthbarCanvas.rotation = Quaternion.LookRotation(healthbarLookRotation);
    }

    public void TakeDamage(float damage)
    {
        if (!_healthBar.activeSelf) _healthBar.SetActive(true);

        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            //if (DeathSound != null) _audioSource.PlayOneShot(DeathSound);
            Destroy(gameObject);
            return;
        }
        else if (_hitSound != null) _audioSource.PlayOneShot(_hitSound);


        UpdateHealthbar(_maxHealth, _currentHealth);
    }

    private void OnDestroy()
    {
        OnEnemyDeath?.Invoke(this);
    }

    public void UpdateHealthbar(float maxHealth, float currentHealth)
    {
        _healthbarImage.fillAmount = currentHealth / maxHealth;
    }
}

public interface IDamegeable
{
    void TakeDamage(float damage);
}
