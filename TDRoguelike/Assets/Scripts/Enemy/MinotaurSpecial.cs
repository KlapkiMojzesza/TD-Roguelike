using System;
using UnityEngine;
using UnityEngine.UI;

public class MinotaurSpecial : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speedBoostPercentage = 1.1f;
    [SerializeField] private float _specialCooldown = 3f;
    [SerializeField] private float _specialRange = 10f;
    [SerializeField] LayerMask enemyLayers;

    [Header("To Attach")]
    [SerializeField] private GameObject _specialBar;
    [SerializeField] private Image _specialImage;
    [SerializeField] private AudioClip _specialSound;
    [SerializeField] private ParticleSystem _minotaurSpecialParticle;

    private AudioSource _audioSource;
    private float _timer;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _specialBar.SetActive(true);
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _specialCooldown)
        {
            PerformSpecial();
            _timer = 0;
        }
        _specialImage.fillAmount = _timer / _specialCooldown;
    }

    private void PerformSpecial()
    {
        _minotaurSpecialParticle.Play();
        _audioSource.PlayOneShot(_specialSound);

        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, _specialRange, enemyLayers);
        foreach (Collider enemy in enemiesInRange)
        {
            enemy.GetComponent<EnemyEffects>().MinotaurSpeedEnemy(enemy.gameObject, _speedBoostPercentage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _specialRange);
    }
}
