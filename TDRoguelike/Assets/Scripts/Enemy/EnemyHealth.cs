using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamegeable
{
    [Header("Settings")]
    [SerializeField] private float _maxHealth = 100f;
    public float EnemyStrength = 1f;
    public int _experienceDrop = 10;

    [Header("To Attach")]
    [SerializeField] private Transform _healthbarCanvas;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private Image _healthbarImage;
    public Transform AimPoint;
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private ParticleSystem _deathParticle;
    [SerializeField] private Transform _deathParticleSpawn;

    [Header("Audio")]
    [SerializeField] private AudioSource _enemyHitAudioSource;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] public AudioClip StartWaveSound;
    [SerializeField] public AudioClip HitSound;
    [SerializeField] public AudioClip DeathSound;

    public static event Action<EnemyHealth> OnEnemySpawn;
    public static event Action<EnemyHealth> OnEnemyDeath;
    public static event Action<EnemyHealth> OnEnemyKilled;

    [HideInInspector] public bool DamagedByPlayer = false;
    private Camera _camera;
    private float _currentHealth;

    private void Start()
    {
        OnEnemySpawn?.Invoke(this);

        _camera = Camera.main;

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
        if (damage <= 0) return;
        if (!_healthBar.activeSelf) _healthBar.SetActive(true);

        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            if (_deathParticle != null) Instantiate(_deathParticle, _deathParticleSpawn.position, Quaternion.identity);
            OnEnemyKilled?.Invoke(this);
            Destroy(gameObject);
            return;
        }

        if (_hitParticle != null) _hitParticle.Play();
        if (HitSound != null) _enemyHitAudioSource.PlayOneShot(HitSound);
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
