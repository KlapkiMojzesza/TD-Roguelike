using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private bool _lookAtTarget = false;

    private Transform _target;
    private Vector3 _lastKnownDirection;
    private float _speed;
    private float _damage;
    private int _enemyPierce = 1;

    IObjectPool<Projectile> _pool;

    public void SetPool(IObjectPool<Projectile> pool) => _pool = pool;

    public void Create(Transform target, float speed, float damage, int enemyPierce)
    {
        _target = target;
        _speed = speed;
        _damage = damage;
        _enemyPierce = enemyPierce;
        _lastKnownDirection = target.position - transform.position;
        if (_lookAtTarget) transform.LookAt(_target);
    }

    private void Update()
    {
        if (_target != null)
        {
            _lastKnownDirection = _target.position - transform.position;
            if (Vector3.Distance(transform.position, _target.position) < 0.1f) _target = null;
            if (_lookAtTarget) transform.LookAt(_target);
        }

        transform.Translate(_lastKnownDirection.normalized * _speed * Time.deltaTime, Space.World);

        
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Ground"))
        {
            if (gameObject.activeSelf) _pool.Release(this);
            gameObject.SetActive(false);
            return;
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            _target = null;
            if (_enemyPierce > 0)
            {
                _enemyPierce--;
                collision.gameObject.GetComponent<IDamegeable>().TakeDamage(_damage);
            }
            if (_enemyPierce == 0)
            {
                if (gameObject.activeSelf) _pool.Release(this);
                gameObject.SetActive(false);
            }
        }

    }
}
