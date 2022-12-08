using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerProjectile : MonoBehaviour
{
    private float _speed;
    private float _damage;
    private int _pierceThroughEnemiesAmount;
    private Vector3 _direction;

    IObjectPool<PlayerProjectile> _pool;

    public void SetPool(IObjectPool<PlayerProjectile> pool) => _pool = pool;

    public void Create(Vector3 direction, float speed, float damage, int pierceThroughEnemiesAmount)
    {
        _direction = new Vector3(direction.x, 0f, direction.z).normalized;
        _speed = speed;
        _damage = damage;
        _pierceThroughEnemiesAmount = pierceThroughEnemiesAmount;
    }

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (_pierceThroughEnemiesAmount > 0)
            {
                _pierceThroughEnemiesAmount--;
                collision.gameObject.GetComponent<IDamegeable>().TakeDamage(_damage);
            }
            if (_pierceThroughEnemiesAmount == 0)
            {
                _pool.Release(this);
                _pierceThroughEnemiesAmount--;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Wall")) _pool.Release(this);
    }
}
