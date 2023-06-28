using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private GameObject _particle;

    private float _speed;
    private float _damage;
    private int _pierceThroughEnemiesAmount;
    private Vector3 _direction;
    private bool _hitSomething = false;
    private GameObject _projectileParticle;

    IObjectPool<PlayerProjectile> _pool;

    public void SetPool(IObjectPool<PlayerProjectile> pool) => _pool = pool;

    public void Create(Vector3 direction, float speed, float damage, int pierceThroughEnemiesAmount)
    {
        _direction = new Vector3(direction.x, 0f, direction.z).normalized;
        _speed = speed;
        _damage = damage;
        _pierceThroughEnemiesAmount = pierceThroughEnemiesAmount;
        _hitSomething = false;
        _projectileParticle = Instantiate(_particle, transform.position, Quaternion.identity);
        _projectileParticle.transform.parent = this.gameObject.transform;

        //create particle system
        //set parent
    }

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hitSomething) return;

        if (collision.collider.CompareTag("Obstacle"))
        {
            _projectileParticle.GetComponent<ParticleSystem>().Stop();
            _projectileParticle.transform.parent = null;

            _pool.Release(this);
            //relese particle parent
            _hitSomething = true;
            return;
        }
        if (collision.collider.CompareTag("Enemy"))
        {
            if (_pierceThroughEnemiesAmount > 0)
            {
                _pierceThroughEnemiesAmount--;
                collision.gameObject.GetComponent<IDamegeable>().TakeDamage(_damage);
                collision.gameObject.GetComponent<EnemyHealth>().DamagedByPlayer = true;
            }
            if (_pierceThroughEnemiesAmount == 0)
            {
                _projectileParticle.GetComponent<ParticleSystem>().Stop();
                _projectileParticle.transform.parent = null;

                _pool.Release(this);
                //relese particle parent
                _pierceThroughEnemiesAmount--;
                _hitSomething = true;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Wall"))
        {
            _projectileParticle.GetComponent<ParticleSystem>().Stop();
            _projectileParticle.transform.parent = null;
            _pool.Release(this);
        }
            //relese particle parent
        }
}
