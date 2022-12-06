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

    [Header("To Attach")]
    [SerializeField] private Transform _healthbarCanvas;
    [SerializeField] private Image _healthbarImage;
    public Transform AimPoint;

    public static event Action<GameObject> OnEnemySpawn;
    public static event Action<GameObject> OnEnemyDeath;

    private Camera _camera;
    private float _currentHealth;

    private void Start()
    {
        OnEnemySpawn?.Invoke(gameObject);

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
        if (!_healthbarCanvas.gameObject.activeSelf) _healthbarCanvas.gameObject.SetActive(true);

        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }
        UpdateHealthbar(_maxHealth, _currentHealth);
    }

    private void OnDestroy()
    {
        OnEnemyDeath?.Invoke(gameObject);
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
