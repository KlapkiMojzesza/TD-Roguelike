using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class PlayerShooting : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _fireRate = 2f;
    [SerializeField] private float _projectileSpeed = 200f;
    [SerializeField] private float _playerDamage = 50f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private int _pierceThroughEnemiesAmount = 1;

    [Header("To Attach")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private PlayerProjectile _projectilePrefab;
    [SerializeField] private GameObject _projectileVisual;
    [SerializeField] private AudioClip[] _shootSounds;

    private ObjectPool<PlayerProjectile> _pool;
    private Vector3 _direction;
    private float _lastFired = 0f;
    private bool _towerSelected = false;
    private Animator _animator;
    private AudioSource _audioSource;

    private void Start()
    {
        TowerManager.OnTowerSelectedToPlace += TowerSelected;
        TowerManager.OnTowerDeselect += TowerDeselect;

        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _pool = new ObjectPool<PlayerProjectile>(CreateProjectile, OnTakeProjectileFromPool, OnReturnProjectileToPool);
    }

    private void OnDestroy()
    {
        TowerManager.OnTowerSelectedToPlace -= TowerSelected;
        TowerManager.OnTowerDeselect -= TowerDeselect;
    }

    private void Update()
    {
        if (CanShoot() && !_projectileVisual.activeSelf) _projectileVisual.SetActive(true);

        if (Input.GetMouseButton(0)) //change to inputsystem later
        {
            if (!IsMouseOverUI() && !_towerSelected && Time.timeScale != 0) Shoot();
        }
    }

    public void Shoot()
    {
        if (Time.time - _lastFired > 1 / _fireRate)
        {
            _lastFired = Time.time;
            _animator.SetTrigger("shoot");
            _projectileVisual.SetActive(false);
            _audioSource.PlayOneShot(_shootSounds[UnityEngine.Random.Range(0, _shootSounds.Length - 1)]);

            PlayerProjectile projectile = _pool.Get();
            projectile.gameObject.transform.position = _firePoint.position;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit groundHit;
            if (Physics.Raycast(ray, out groundHit, Mathf.Infinity, _groundLayer))
            {
                _direction = (groundHit.point - transform.position);
            }

            projectile.Create(_direction, _projectileSpeed, _playerDamage, _pierceThroughEnemiesAmount);    
        }
    }

    PlayerProjectile CreateProjectile()
    {
        var projectile = Instantiate(_projectilePrefab);
        projectile.SetPool(_pool);
        return projectile;
    }

    private bool CanShoot()
    {
        return Time.time - _lastFired > 1 / _fireRate;
    }
    private void OnTakeProjectileFromPool(PlayerProjectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void OnReturnProjectileToPool(PlayerProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void TowerDeselect()
    {
        Invoke("ChangeTowerSelection", 0.2f);
    }

    private void ChangeTowerSelection()
    {
        _towerSelected = false;
    }

    private void TowerSelected(Tower selectedTower)
    {
        _towerSelected = true;
    }
}
