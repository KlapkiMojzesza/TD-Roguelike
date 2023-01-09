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
    [SerializeField] private Transform _specialRangeVisual;
    [SerializeField] private Animator _specialVisualAnimator;
    [SerializeField] private AudioClip _specialSound;

    private AudioSource _audioSource;
    private float _timer;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _specialBar.SetActive(true);
        _specialRangeVisual.localScale = new Vector3(_specialRange, _specialRange, 1f);
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
        _specialVisualAnimator.SetTrigger("special");
        _audioSource.PlayOneShot(_specialSound);

        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, _specialRange, enemyLayers);
        foreach (Collider enemy in enemiesInRange)
        {
            enemy.GetComponent<EnemyMovement>().UpgradeSpeed(_speedBoostPercentage);
            enemy.gameObject.GetComponent<Animator>().speed *= _speedBoostPercentage;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _specialRange);
    }
}
