using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyEffects))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _damageToPlayerBase = 25f;
    [SerializeField] private int _damageToPlayer = 10;
    [SerializeField] private float _speed = 50f;
    [SerializeField] private float _enemyVariantSpeedMultiplier = 0f;
    [SerializeField] private float _rotateSpeed = 0.05f;
    [SerializeField] private float _lookDirectionOffset = 90f;

    [Header("To Attach")]
    [SerializeField] private ParticleSystem _footstepParticle;

    public static event Action<float> OnEnemyPathCompleate;

    private WaypointManager _waypointManager;
    private float _distanceToNextWaypoint = 0f;
    private int _currentWaypoint;
    private Animator _animator;
    private float _currentSlowPercentage = 0;

    private void Start()
    {
        _waypointManager = (WaypointManager)FindObjectOfType(typeof(WaypointManager));
        _animator = GetComponent<Animator>();

        _speed *= (100 + _enemyVariantSpeedMultiplier) / 100;
        _animator.speed *= (100 + _enemyVariantSpeedMultiplier) / 100;
    }

    private void Update()
    {
        RotateTowardsNextWaypoint();
        MoveTowardsNextWaypoint();
    }

    private void MoveTowardsNextWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, _waypointManager.Waypoints[_currentWaypoint].position, _speed * Time.deltaTime);

        _distanceToNextWaypoint = Vector3.Distance(transform.position, _waypointManager.Waypoints[_currentWaypoint].position);

        if (_distanceToNextWaypoint < 0.01f)
        {
            if (_currentWaypoint < _waypointManager.Waypoints.Length - 1)
            {
                _currentWaypoint++;
                _distanceToNextWaypoint = Vector3.Distance(transform.position, _waypointManager.Waypoints[_currentWaypoint].position);
            }
            else
            {
                OnEnemyPathCompleate?.Invoke(_damageToPlayerBase);
                Destroy(gameObject);
            }
        }
    }

    private void RotateTowardsNextWaypoint()
    {
        Vector3 lookDirection = _waypointManager.Waypoints[_currentWaypoint].position - transform.position;
        float angle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg - _lookDirectionOffset;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), Time.deltaTime / _rotateSpeed);
    }

    public int GetCurrentWaypoint()
    {
        return _currentWaypoint;
    }

    public float GetDistanceToNextWaypoint()
    {
        return _distanceToNextWaypoint;
    }

    //Minotaur Special
    public void UpgradeSpeed(float amount)
    {
        _speed *= (100 + amount) / 100;
        _animator.speed *= (100 + amount) / 100;
    }

    //Pylon Slow Effect
    public void PylonSlowEnemy(float slowPercentage)
    {
        if (slowPercentage > _currentSlowPercentage)
        {
            _speed /= (100 - _currentSlowPercentage) / 100;
            _animator.speed /= (100 - _currentSlowPercentage) / 100;

            _currentSlowPercentage = slowPercentage;

            _speed *= (100 - _currentSlowPercentage) / 100;
            _animator.speed *= (100 - _currentSlowPercentage) / 100;
        }
    }

    public void PylonRemoveSlow()
    {
        _speed /= (100 - _currentSlowPercentage) / 100;
        _animator.speed /= (100 - _currentSlowPercentage) / 100;
    }

    public int GetDamageToPlayer()
    {
        return _damageToPlayer;
    }

    //From animation
    public void Footstep()
    {
        _footstepParticle.Play();
    }
}
