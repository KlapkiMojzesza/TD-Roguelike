using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : Projectile
{
    [Header("Settings")]
    [SerializeField] private CannonProjectileLevel _projectileLevel = CannonProjectileLevel.FirstLevel;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _splashRange = 5f;

    [Header("To Attach")]
    [SerializeField] Transform _objectToRotate;
    [SerializeField] private ParticleSystem _explosionParticle;
    [SerializeField] private AudioClip _explosionSound;

    public static event Action<AudioClip> OnExplosion;

    private Vector3 _randomRotation;
    private bool hitSomething = false;

    public override void Create(Transform target, float speed, float damage, int enemyPierce, float slowPercentage, float towerRange)
    {
        base.Create(target, speed, damage, enemyPierce);
        _randomRotation = new Vector3(UnityEngine.Random.Range(0f, 180f), 
                                      UnityEngine.Random.Range(0f, 180f),
                                      UnityEngine.Random.Range(0f, 180f)).normalized * _rotationSpeed;
        hitSomething = false;
    }

    protected override void Update()
    {
        base.Update();
        
        if (_projectileLevel == CannonProjectileLevel.SecondLevel ||
            _projectileLevel == CannonProjectileLevel.ThirdLevel)
        {
            _objectToRotate.Rotate(_randomRotation * Time.deltaTime);
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (hitSomething) return;

        if (collision.collider.CompareTag("Ground") ||
            collision.collider.CompareTag("Obstacle"))
        {
            HandleProjectileHit(null);
            gameObject.SetActive(false);
            _pool.Release(this);
            hitSomething = true;
            return;
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            _target = null;
            collision.gameObject.GetComponent<IDamegeable>().TakeDamage(_damage);
            HandleProjectileHit(collision.collider);
            gameObject.SetActive(false);
            _pool.Release(this);
            hitSomething = true;
        }
    }

    private void HandleProjectileHit(Collider enemyCollider)
    {
        if (_projectileLevel == CannonProjectileLevel.SecondLevel ||
            _projectileLevel == CannonProjectileLevel.ThirdLevel)
        {
            Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, _splashRange, _enemyLayer);
            foreach (Collider enemy in enemiesInRange)
            {
                if (enemy != enemyCollider) enemy.GetComponent<IDamegeable>().TakeDamage(_damage);
            }
        }
        if (_explosionParticle != null) Instantiate(_explosionParticle, transform.position, Quaternion.identity);
        if (_explosionSound != null) OnExplosion?.Invoke(_explosionSound);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _splashRange);
    }
}

public enum CannonProjectileLevel { FirstLevel, SecondLevel, ThirdLevel};
