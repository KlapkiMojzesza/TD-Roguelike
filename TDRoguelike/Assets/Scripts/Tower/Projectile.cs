using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Projectile : MonoBehaviour
{
    protected Transform _target;
    protected Vector3 _lastKnownDirection;
    protected float _speed;
    protected float _damage;
    protected int _enemyPierce = 1;
    protected float _slowPercentage = 0;
    protected float _towerRange = 1f;

    protected IObjectPool<Projectile> _pool;

    public void SetPool(IObjectPool<Projectile> pool) => _pool = pool;

    public virtual void Create(Transform target,
                               float speed,
                               float damage,
                               int enemyPierce,
                               float slowPercentage = 1f,
                               float towerRange = 1f)
    {
        _target = target;
        _speed = speed;
        _damage = damage;
        _enemyPierce = enemyPierce;
        _slowPercentage = slowPercentage;
        _towerRange = towerRange;
        _lastKnownDirection = target.position - transform.position;
    }

    protected virtual void Update()
    {
        if (_target != null)
        {
            _lastKnownDirection = _target.position - transform.position;
            if (Vector3.Distance(transform.position, _target.position) < 0.1f) _target = null;
        }

        transform.Translate(_lastKnownDirection.normalized * _speed * Time.deltaTime, Space.World);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //do stuff;
    }
}
